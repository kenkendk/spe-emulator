﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator
{
    public class SPU
    {
        #region Enumerations
        /// <summary>
        /// A list of documented channel alias
        /// </summary>
        public enum Channels
        {
            /// <summary>
            /// Read event status with mask applied 
            /// </summary>
            SPU_RdEventStat = 0,
            /// <summary>
            /// Write event mask 
            /// </summary>
            SPU_WrEventMask = 1,
            /// <summary>
            /// Write end of event processing 
            /// </summary>
            SPU_WrEventAck = 2,
            /// <summary>
            /// Signal notification 1
            /// </summary>
            SPU_RdSigNotify1 = 3,
            /// <summary>
            /// Signal notification 2 
            /// </summary>
            SPU_RdSigNotify2 = 4,
            /// <summary>
            /// Write decrementer count 
            /// </summary>
            SPU_WrDec = 7,
            /// <summary>
            /// Read decrementer count
            /// </summary>
            SPU_RdDec = 8,
            /// <summary>
            /// Read event mask 
            /// </summary>
            SPU_RdEventMask = 11,
            /// <summary>
            /// Read SPU run status 
            /// </summary>
            SPU_RdMachStat = 13,
            /// <summary>
            /// Write SPU machine state save/restore register 0 (SRR0) 
            /// </summary>
            SPU_WrSRR0 = 14,
            /// <summary>
            /// Read SPU machine state save/restore register 0 (SRR0)
            /// </summary>
            SPU_RdSRR0 = 15,
            /// <summary>
            /// Write outbound mailbox contents 
            /// </summary>
            SPU_WrOutMbox = 28,
            /// <summary>
            /// Read inbound mailbox contents 
            /// </summary>
            SPU_RdInMbox = 29,
            /// <summary>
            /// Write outbound interrupt mailbox contents (interrupting PPU) 
            /// </summary>
            SPU_WrOutIntrMbox = 30
        }

        #endregion

        private SPEProcessor m_spe;
        private OpCodes.OpCodeParser m_parser;

        private Register[] m_registers;
        private Register m_SRR0;

        private uint m_pc;
        private volatile bool m_doRun = true;

        private uint m_codeSize = 0;

        private Dictionary<Type, System.Reflection.MethodInfo> m_executionFunctions;

        private System.Threading.Thread m_thread;
        private object m_lock = new object();
        private System.Threading.ManualResetEvent m_event;

        public SPU(SPEProcessor spe, uint registercount = 128)
        {
            m_spe = spe;
            m_registers = new Register[registercount];
            for (int i = 0; i < m_registers.Length; i++)
                m_registers[i] = new Register();
            m_parser = new OpCodes.OpCodeParser();

            m_event = new System.Threading.ManualResetEvent(true);

            m_executionFunctions =
                this.GetType().GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Where(x => x.Name == "Execute" && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType != typeof(OpCodes.Bases.Instruction))
                .ToDictionary(x => x.GetParameters()[0].ParameterType);
#if xxDEBUG
            var funcs = m_executionFunctions.Keys.Select(x => x.Name).ToList();

            OpCodes.Bases.Mnemonic n;
            foreach(var x in funcs)
                if (!Enum.TryParse<OpCodes.Bases.Mnemonic>(x, out n))
                    throw new Exception(string.Format("Invalid instruction {0}, does not match a mnemonic", x));

            foreach(OpCodes.Bases.Mnemonic m in Enum.GetValues(typeof(OpCodes.Bases.Mnemonic)))
                if (!funcs.Exists(x => string.Equals(x, m.ToString(), StringComparison.InvariantCultureIgnoreCase)))
                    throw new Exception(string.Format("The instruction {0} cannot be executed because no method supports it", m.ToString()));
#endif
        }

        public void Run()
        {
            lock (m_lock)
            {
                if (m_thread == null || m_thread.ThreadState == System.Threading.ThreadState.Stopped || m_thread.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    m_thread = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadRun));
                    m_thread.Start();
                }
            }
        }

        public void Stop()
        {
            m_doRun = false;
            m_event.Set();
        }

        public void Pause()
        {
            m_event.Reset();
        }

        public void Resume()
        {
            m_event.Set();
        }

        private void ThreadRun()
        {
            m_registers[3].Word = 0x0a0a0a0; //spu-id
            m_registers[4].Word = 0x0e0e0e0; //argp
            m_registers[5].Word = 0x0c0c0c0; //envp

            //This seems to be handled by the loader in an ELF file
            /*m_registers[1].Word = 0x3ffd0; //TODO: Must follow configureable LS size

            CopyToLS(new RegisterValue(0), 0x3fff0); //Backpointer = NULL
            CopyToLS(new RegisterValue(0), 0x3ffe0); //Register save area
            CopyToLS(new RegisterValue(0x3fff0), 0x3ffd0); //Backpointer to bottom frame
            */

            bool inCodeSegment = PC < m_codeSize;

            while (m_doRun)
            {
                try
                {
                    m_event.WaitOne();
                    if (!m_doRun)
                        break;

                    if (PC < m_codeSize != inCodeSegment)
                    {
                        inCodeSegment = PC < m_codeSize;
                        m_spe.RaiseWarning(SPEWarning.ExecuteDataArea, inCodeSegment ? "The execution is now back in the code segment" : "The execution is now in the DATA segment");
                    }

                    SPEEmulator.OpCodes.Bases.Instruction op = m_parser.FindCode(m_spe.LS, PC);

                    m_spe.RaiseInstructionExecuting(string.Format("0x{0:x4}: {1}", PC, op.ToString()));

                    try
                    {
                        Execute(op);
                    }
                    catch (Exception ex)
                    {
                        m_spe.RaiseMissingMethodError(string.Format("{0} is not implemented: {1}", op.Mnemonic, ex.ToString()));
                        break;
                    }
                }
                catch (Exception ex)
                {
                    m_spe.RaiseInvalidOpCodeError(ex.ToString());
                    break;
                }
            }

            m_spe.Stop();
        }

        /// <summary>
        /// Gets or sets the Program Counter (PC)
        /// </summary>
        public uint PC
        {
            get { return m_pc; }
            set { m_pc = value; }
        }

        /// <summary>
        /// Gets or sets the size of the code region, used to issue warnings when attempting to mix data and execution
        /// </summary>
        public uint CodeSize
        {
            get { return m_codeSize; }
            set { m_codeSize = value; }
        }

        /// <summary>
        /// Expands an immediate value to 32 bits
        /// </summary>
        /// <param name="x">The instruction with the immediate value to expand</param>
        /// <param name="shift">The number of bits the value is shifted left</param>
        /// <returns>The 32bit sign extended value as an unsigned int</returns>
        private uint RepLeftBit(OpCodes.Bases.RI7 x, int shift)
        {
            return (((x.I7 & 0x40) != 0 ? 0xffffff80 : 0x00000000) | x.I7) << shift;
        }

        /// <summary>
        /// Expands an immediate value to 32 bits
        /// </summary>
        /// <param name="x">The instruction with the immediate value to expand</param>
        /// <param name="shift">The number of bits the value is shifted left</param>
        /// <returns>The 32bit sign extended value as an unsigned int</returns>
        private uint RepLeftBit(OpCodes.Bases.RI8 x, int shift)
        {
            return (((x.I8 & 0x80) != 0 ? 0xffffff00 : 0x00000000) | x.I8) << shift;
        }

        /// <summary>
        /// Expands an immediate value to 32 bits
        /// </summary>
        /// <param name="x">The instruction with the immediate value to expand</param>
        /// <param name="shift">The number of bits the value is shifted left</param>
        /// <returns>The 32bit sign extended value as an unsigned int</returns>
        private uint RepLeftBit(OpCodes.Bases.RI10 x, int shift)
        {
            return (((x.I10 & 0x200) != 0 ? 0xfffffc00 : 0x00000000) | x.I10) << shift;
        }

        /// <summary>
        /// Expands an immediate value to 32 bits
        /// </summary>
        /// <param name="x">The instruction with the immediate value to expand</param>
        /// <param name="shift">The number of bits the value is shifted left</param>
        /// <returns>The 32bit sign extended value as an unsigned int</returns>
        private uint RepLeftBit(OpCodes.Bases.RI16 x, int shift)
        {
            return (((x.I16 & 0x8000) != 0 ? 0xffff0000 : 0x00000000) | x.I16) << shift;
        }

        /// <summary>
        /// Expands an immediate value to 32 bits
        /// </summary>
        /// <param name="x">The instruction with the immediate value to expand</param>
        /// <param name="shift">The number of bits the value is shifted left</param>
        /// <returns>The 32bit sign extended value as an unsigned int</returns>
        private uint RepLeftBit(OpCodes.Bases.RI18 x, int shift)
        {
            return (((x.I18 & 0x20000) != 0 ? 0xfffc0000 : 0x00000000) | x.I18) << shift;
        }

        private void Execute(OpCodes.Bases.Instruction i)
        {
            this.PC += 4;
            uint next = this.PC;

            System.Reflection.MethodInfo mi;
            if (!m_executionFunctions.TryGetValue(i.GetType(), out mi) || mi == null)
                throw new Exception(string.Format("Unable to execute instruction of type {0}", i.Mnemonic));
            mi.Invoke(this, new object[] { i });
        }

        #region Memory Access
        private void CopyToLS(RegisterValue v, long lsOffset)
        {
            if ((lsOffset & (~0xf)) != lsOffset)
                m_spe.RaiseWarning(SPEWarning.UnalignedMemoryAccess, string.Format("Writing unaligned address 0x{0:x8} -> 0x{1:x8}", lsOffset, lsOffset & (~0xf)));
            if ((lsOffset & m_spe.LSLR) != lsOffset)
                m_spe.RaiseWarning(SPEWarning.WrappedMemoryAccess, string.Format("Writing wrapped address 0x{0:x8} -> 0x{1:x8}", lsOffset, lsOffset & m_spe.LSLR));

            lsOffset = lsOffset & m_spe.LSLR & (~0xf);

            if (m_codeSize > 0 && lsOffset < m_codeSize)
                m_spe.RaiseWarning(SPEWarning.ReadCodeArea, string.Format("Writing code address 0x{0:x8}", lsOffset));

            Array.Copy(v.Value, 0, m_spe.LS, lsOffset, 16);
        }

        private void CopyToLS(Register r, long lsOffset)
        {
            CopyToLS(r.Value, lsOffset);
        }

        private void CopyFromLS(Register r, long lsOffset)
        {
            if ((lsOffset & (~0xf)) != lsOffset)
                m_spe.RaiseWarning(SPEWarning.UnalignedMemoryAccess, string.Format("Reading unaligned address 0x{0:x8} -> 0x{1:x8}", lsOffset, lsOffset & (~0xf)));
            if ((lsOffset & m_spe.LSLR) != lsOffset)
                m_spe.RaiseWarning(SPEWarning.WrappedMemoryAccess, string.Format("Reading wrapped address 0x{0:x8} -> 0x{1:x8}", lsOffset, lsOffset & m_spe.LSLR));

            lsOffset = lsOffset & m_spe.LSLR & (~0xf);

            if (m_codeSize > 0 && lsOffset < m_codeSize)
                m_spe.RaiseWarning(SPEWarning.ReadCodeArea, string.Format("Reading code address 0x{0:x8}", lsOffset));

            Array.Copy(m_spe.LS, lsOffset, r.Value.Value, 0, 16);
        }
        #endregion

        #region ALUs
        /// <summary>
        /// Performs an operation on a set of registers, one byte at a time
        /// </summary>
        /// <param name="a">Register operand A</param>
        /// <param name="b">Register operand B</param>
        /// <param name="c">Register operand C</param>
        /// <param name="exec">The function to execute, is given the paramters a,b,c and carry value</param>
        /// <returns>The calculated value</returns>
        private RegisterValue ALUByte(RegisterValue a, RegisterValue b, RegisterValue c, Func<byte, byte, byte, byte, uint> exec)
        {
            RegisterValue x = new RegisterValue();
            uint carry = 0;

            for (int j = 15; j >= 0; j--)
            {
                carry = exec(
                    a == null ? (byte)0 : a.Value[j]
                    ,
                    b == null ? (byte)0 : b.Value[j]
                    ,
                    c == null ? (byte)0 : c.Value[j]
                    ,
                    (byte)(carry >> 8)
                    );

                x.Value[j] = (byte)(carry & 0xff);
            }

            return x;
        }

        /// <summary>
        /// Performs an operation on a set of registers, one halfword at a time
        /// </summary>
        /// <param name="a">Register operand A</param>
        /// <param name="b">Register operand B</param>
        /// <param name="c">Register operand C</param>
        /// <param name="exec">The function to execute, is given the paramters a,b,c and carry value</param>
        /// <returns>The calculated value</returns>
        private RegisterValue ALUHalfWord(RegisterValue a, RegisterValue b, RegisterValue c, Func<ushort, ushort, ushort, ushort, uint> exec)
        {
            RegisterValue x = new RegisterValue();
            ulong carry = 0;

            for (int j = 14; j >= 0; j -= 2)
            {
                carry = exec(
                    a == null ? (ushort)0 : (ushort)((a.Value[j] << 8) | a.Value[j + 1])
                    ,
                    b == null ? (ushort)0 : (ushort)((b.Value[j] << 8) | b.Value[j + 1])
                    ,
                    c == null ? (ushort)0 : (ushort)((c.Value[j] << 8) | c.Value[j + 1])
                    ,
                    (ushort)(carry >> 16)
                    );

                x.Value[j] = (byte)((carry >> 8) & 0xff);
                x.Value[j + 1] = (byte)(carry & 0xff);

            }

            return x;
        }

        /// <summary>
        /// Performs an operation on a set of registers, one word at a time
        /// </summary>
        /// <param name="a">Register operand A</param>
        /// <param name="b">Register operand B</param>
        /// <param name="c">Register operand C</param>
        /// <param name="exec">The function to execute, is given the paramters a,b,c and carry value</param>
        /// <returns>The calculated value</returns>
        private RegisterValue ALUWord(RegisterValue a, RegisterValue b, RegisterValue c, Func<uint, uint, uint, uint, ulong> exec)
        {
            RegisterValue x = new RegisterValue();
            ulong carry = 0;
            for (int j = 12; j >= 0; j -= 4)
            {
                carry =
                    exec(
                        a == null ? 0u : (((uint)a.Value[j] << 24) | ((uint)a.Value[j + 1] << 16) | ((uint)a.Value[j + 2] << 8) | (uint)a.Value[j + 3])
                        ,
                        b == null ? 0u : (((uint)b.Value[j] << 24) | ((uint)b.Value[j + 1] << 16) | ((uint)b.Value[j + 2] << 8) | (uint)b.Value[j + 3])
                        ,
                        c == null ? 0u : (((uint)c.Value[j] << 24) | ((uint)c.Value[j + 1] << 16) | ((uint)c.Value[j + 2] << 8) | (uint)c.Value[j + 3])
                        ,
                        (uint)(carry >> 32)
                    );

                x.Value[j] = (byte)((carry >> 24) & 0xff);
                x.Value[j + 1] = (byte)((carry >> 16) & 0xff);
                x.Value[j + 2] = (byte)((carry >> 8) & 0xff);
                x.Value[j + 3] = (byte)(carry & 0xff);
            }

            return x;
        }

        /// <summary>
        /// Performs an operation on a set of registers, one double word at a time
        /// </summary>
        /// <param name="a">Register operand A</param>
        /// <param name="b">Register operand B</param>
        /// <param name="c">Register operand C</param>
        /// <param name="exec">The function to execute, is given the paramters a,b,c and carry value</param>
        /// <returns>The calculated value</returns>
        private RegisterValue ALUDoubleWord(RegisterValue a, RegisterValue b, RegisterValue c, Func<ulong, ulong, ulong, ulong, ulong> exec)
        {
            RegisterValue x = new RegisterValue();
            ulong carry = 0;
            for (int j = 8; j >= 0; j -= 8)
            {
                carry =
                    exec(
                        a == null ? 0u : (((ulong)a.Value[j] << 56) | ((ulong)a.Value[j + 1] << 48) | ((ulong)a.Value[j + 2] << 40) | ((ulong)a.Value[j + 3] << 32) | ((ulong)a.Value[j + 4] << 24) | ((ulong)a.Value[j + 5] << 16) | ((ulong)a.Value[j + 6] << 8) | (ulong)a.Value[j + 7])
                        ,
                        b == null ? 0u : (((ulong)b.Value[j] << 56) | ((ulong)b.Value[j + 1] << 48) | ((ulong)b.Value[j + 2] << 40) | ((ulong)b.Value[j + 3] << 32) | ((ulong)b.Value[j + 4] << 24) | ((ulong)b.Value[j + 5] << 16) | ((ulong)b.Value[j + 6] << 8) | (ulong)b.Value[j + 7])
                        ,
                        c == null ? 0u : (((ulong)c.Value[j] << 56) | ((ulong)c.Value[j + 1] << 48) | ((ulong)c.Value[j + 2] << 40) | ((ulong)c.Value[j + 3] << 32) | ((ulong)c.Value[j + 4] << 24) | ((ulong)c.Value[j + 5] << 16) | ((ulong)c.Value[j + 6] << 8) | (ulong)c.Value[j + 7])
                        ,
                        (ulong)(carry >> 64)
                    );

                x.Value[j] = (byte)((carry >> 56) & 0xff);
                x.Value[j + 1] = (byte)((carry >> 48) & 0xff);
                x.Value[j + 2] = (byte)((carry >> 40) & 0xff);
                x.Value[j + 3] = (byte)((carry >> 32) & 0xff);
                x.Value[j + 4] = (byte)((carry >> 24) & 0xff);
                x.Value[j + 5] = (byte)((carry >> 16) & 0xff);
                x.Value[j + 6] = (byte)((carry >> 8) & 0xff);
                x.Value[j + 7] = (byte)(carry & 0xff);
            }

            return x;
        }
        #endregion

        #region Memory—Load/Store Instructions
        private void Execute(OpCodes.lqd i)
        {
            CopyFromLS(m_registers[i.RT], ((int)RepLeftBit(i, 4) + (int)m_registers[i.RA].Word));
        }

        private void Execute(OpCodes.lqx i)
        {
            CopyFromLS(m_registers[i.RT], (m_registers[i.RB].Word + m_registers[i.RA].Word));
        }

        private void Execute(OpCodes.lqa i)
        {
            CopyFromLS(m_registers[i.RT], RepLeftBit(i, 2));
        }

        private void Execute(OpCodes.lqr i)
        {
            CopyFromLS(m_registers[i.RT], (RepLeftBit(i, 2) + (PC - 4)));
        }

        private void Execute(OpCodes.stqd i)
        {
            CopyToLS(m_registers[i.RT], (int)RepLeftBit(i, 4) + (int)m_registers[i.RA].Word);
        }

        private void Execute(OpCodes.stqx i)
        {
            CopyToLS(m_registers[i.RT], m_registers[i.RA].Word + m_registers[i.RB].Word);
        }

        private void Execute(OpCodes.stqa i)
        {
            CopyToLS(m_registers[i.RT], RepLeftBit(i, 2));
        }

        private void Execute(OpCodes.stqr i)
        {
            CopyToLS(m_registers[i.RT], RepLeftBit(i, 2) + (PC - 4));
        }

        private void Execute(OpCodes.cbd i)
        {
            uint index = (uint)((int)RepLeftBit(i, 0) + (int)m_registers[i.RA].Word) & 0xf;
            m_registers[i.RT].Value.high = 0x1011121314151617u;
            m_registers[i.RT].Value.low = 0x18191A1B1C1D1E1Fu;
            m_registers[i.RT].Value.Value[index] = 0x03;
        }

        private void Execute(OpCodes.cbx i)
        {
            uint index = (m_registers[i.RB].Word + m_registers[i.RA].Word) & 0xf;
            m_registers[i.RT].Value.high = 0x1011121314151617u;
            m_registers[i.RT].Value.low = 0x18191A1B1C1D1E1Fu;
            m_registers[i.RT].Value.Value[index] = 0x03;
        }

        private void Execute(OpCodes.chd i)
        {
            uint index = (uint)((int)RepLeftBit(i, 0) + (int)m_registers[i.RA].Word) & 0xe;
            m_registers[i.RT].Value.high = 0x1011121314151617u;
            m_registers[i.RT].Value.low = 0x18191A1B1C1D1E1Fu;
            m_registers[i.RT].Value.Value[index] = 0x02;
            m_registers[i.RT].Value.Value[index + 1] = 0x03;
        }

        private void Execute(OpCodes.chx i)
        {
            uint index = (m_registers[i.RB].Word + m_registers[i.RA].Word) & 0xe;
            m_registers[i.RT].Value.high = 0x1011121314151617u;
            m_registers[i.RT].Value.low = 0x18191A1B1C1D1E1Fu;
            m_registers[i.RT].Value.Value[index] = 0x02;
            m_registers[i.RT].Value.Value[index + 1] = 0x03;
        }

        private void Execute(OpCodes.cwd i)
        {
            uint index = (uint)((int)RepLeftBit(i, 0) + (int)m_registers[i.RA].Word) & 0xc; 
            m_registers[i.RT].Value.high = 0x1011121314151617u;
            m_registers[i.RT].Value.low = 0x18191A1B1C1D1E1Fu;
            m_registers[i.RT].Value.Value[index] = 0x00;
            m_registers[i.RT].Value.Value[index + 1] = 0x01;
            m_registers[i.RT].Value.Value[index + 2] = 0x02;
            m_registers[i.RT].Value.Value[index + 3] = 0x03;
        }

        private void Execute(OpCodes.cwx i)
        {
            uint index = (m_registers[i.RB].Word + m_registers[i.RA].Word) & 0xc;
            m_registers[i.RT].Value.high = 0x1011121314151617u;
            m_registers[i.RT].Value.low = 0x18191A1B1C1D1E1Fu;
            m_registers[i.RT].Value.Value[index] = 0x00;
            m_registers[i.RT].Value.Value[index + 1] = 0x01;
            m_registers[i.RT].Value.Value[index + 2] = 0x02;
            m_registers[i.RT].Value.Value[index + 3] = 0x03;
        }

        private void Execute(OpCodes.cdd i)
        {
            uint index = (uint)((int)RepLeftBit(i, 0) + (int)m_registers[i.RA].Word) & 0x8;
            m_registers[i.RT].Value.high = 0x1011121314151617u;
            m_registers[i.RT].Value.low = 0x18191A1B1C1D1E1Fu;
            m_registers[i.RT].Value.Value[index] = 0x00;
            m_registers[i.RT].Value.Value[index + 1] = 0x01;
            m_registers[i.RT].Value.Value[index + 2] = 0x02;
            m_registers[i.RT].Value.Value[index + 3] = 0x03;
            m_registers[i.RT].Value.Value[index + 4] = 0x04;
            m_registers[i.RT].Value.Value[index + 5] = 0x05;
            m_registers[i.RT].Value.Value[index + 6] = 0x06;
            m_registers[i.RT].Value.Value[index + 7] = 0x07;
        }

        private void Execute(OpCodes.cdx i)
        {
            uint index = (m_registers[i.RB].Word + m_registers[i.RA].Word) & 0x8;
            m_registers[i.RT].Value.high = 0x1011121314151617u;
            m_registers[i.RT].Value.low = 0x18191A1B1C1D1E1Fu;
            m_registers[i.RT].Value.Value[index] = 0x00;
            m_registers[i.RT].Value.Value[index + 1] = 0x01;
            m_registers[i.RT].Value.Value[index + 2] = 0x02;
            m_registers[i.RT].Value.Value[index + 3] = 0x03;
            m_registers[i.RT].Value.Value[index + 4] = 0x04;
            m_registers[i.RT].Value.Value[index + 5] = 0x05;
            m_registers[i.RT].Value.Value[index + 6] = 0x06;
            m_registers[i.RT].Value.Value[index + 7] = 0x07;
        }
        #endregion

        #region Constant-Formation Instructions
        private void Execute(OpCodes.ilh i)
        {
            m_registers[i.RT].Value = ALUHalfWord(null, null, null, (a, b, c, carry) => i.I16);
        }

        private void Execute(OpCodes.ilhu i)
        {
            m_registers[i.RT].Value = ALUWord(null, null, null, (a, b, c, carry) => i.I16 << 16);
        }

        private void Execute(OpCodes.il i)
        {
            uint v = RepLeftBit(i, 0);
            m_registers[i.RT].Value = ALUWord(null, null, null, (a, b, c, carry) => v);
        }

        private void Execute(OpCodes.ila i)
        {
            m_registers[i.RT].Value = ALUWord(null, null, null, (a, b, c, carry) => i.I18);
        }

        private void Execute(OpCodes.iohl i)
        {
            m_registers[i.RT].Value = ALUWord(null, null, null, (a, b, c, carry) => i.I16);
        }

        private void Execute(OpCodes.fsmbi i)
        {
            for (int j = 0; j < 15; j++)
                m_registers[i.RT].Value.Value[j] = (byte)(((i.I16 >> j) & 0x1) == 0 ? 0x00 : 0xff);
        }
        #endregion

        #region Integer and Logical Instructions
        private void Execute(OpCodes.ah i)
        {
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => (uint)(a + b));
        }

        private void Execute(OpCodes.ahi i)
        {
            ushort s = (ushort)(RepLeftBit(i, 0) & 0xffff);
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (uint)(a + s));
        }

        private void Execute(OpCodes.a i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => (ulong)(a + b));
        }

        private void Execute(OpCodes.ai i)
        {
            uint t = RepLeftBit(i, 0);
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (ulong)(a + t));
        }

        private void Execute(OpCodes.sfh i)
        {
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => (uint)(b + ~a + 1));
        }

        private void Execute(OpCodes.sfhi i)
        {
            ushort t = (ushort)(RepLeftBit(i, 0) & 0xffff);
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (uint)(t + ~a + 1));
        }

        private void Execute(OpCodes.sf i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => b + ~a + 1);
        }

        private void Execute(OpCodes.sfi i)
        {
            uint t = RepLeftBit(i, 0);
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (uint)(t + ~a + 1));
        }

        private void Execute(OpCodes.addx i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, t, carry) => a + b + (t & 0x1));
        }

        private void Execute(OpCodes.cg i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => ((ulong)a + (ulong)b) > 0xffffffffu ? 0x1u : 0x0u);
        }

        private void Execute(OpCodes.cgx i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, t, carry) => ((ulong)a + (ulong)b) + (t & 0x1) > 0xffffffffu ? 1u : 0u);
        }

        private void Execute(OpCodes.sfx i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, t, carry) => b + ~a + (t & 0x1));
        }

        private void Execute(OpCodes.bg i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => b >= a ? 1u : 0u);
        }

        private void Execute(OpCodes.bgx i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, t, carry) =>
            {
                if ((t & 0x1) == 0x1)
                    return b >= a ? 1u : 0u;
                else
                    return b > a ? 1u : 0u;
            });
        }

        private void Execute(OpCodes.mpy i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => { 
                uint tmp = ((a & 0xffff) * (b & 0xffff));
                return ((a & 0x8000) != (b & 0x8000)) ? (~tmp + 1) : tmp;
            });
        }

        private void Execute(OpCodes.mpyu i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => ((a & 0xffff) * (b & 0xffff)));
        }

        private void Execute(OpCodes.mpyi i)
        {
            ushort t = (ushort)(RepLeftBit(i, 0) & 0xffff);
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) =>
            {
                var tmp = ((a & 0xffff) * t);
                return ((a & 0x8000) != (b & 0x8000)) ? (~tmp + 1) : tmp;
            });
        }

        private void Execute(OpCodes.mpyui i)
        {
            ushort t = (ushort)(RepLeftBit(i, 0) & 0xffff);
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (a & 0xffff) * t);
        }

        private void Execute(OpCodes.mpya i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) =>
            {
                uint tmp = ((a & 0xffff) * (b & 0xffff));
                tmp = ((a & 0x8000) != (b & 0x8000)) ? (~tmp + 1) : tmp;
                return tmp + c;
            });
        }

        private void Execute(OpCodes.mpyh i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => {
                var tmp = a >> 16;
                var tmp2 = b & 0xffff;
                var tmp3 = tmp * tmp2;

                tmp3 = ((tmp & 0x8000) != (tmp2 & 0x8000)) ? (~tmp3 + 1) : tmp3;
                return tmp3 << 16;
            });

        }

        private void Execute(OpCodes.mpys i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => {
                var tmp = ((a & 0xffff) * (b & 0xffff));
                tmp = ((a & 0x8000) != (b & 0x8000)) ? (~tmp + 1) : tmp;

                tmp = tmp >> 16;

                if ((tmp & 0x8000) != 0)
                    return tmp | 0xffff0000;
                else
                    return tmp;
            });
        }

        private void Execute(OpCodes.mpyhh i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => {
                var tmp = ((a >> 16) * (b >> 16));
                tmp = ((a & 0x8000) != (b & 0x8000)) ? (~tmp + 1) : tmp;
                return tmp;
            });
        }

        private void Execute(OpCodes.mpyhha i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, t, carry) =>
            {
                var tmp = ((a >> 16) * (b >> 16));
                tmp = ((a & 0x8000) != (b & 0x8000)) ? (~tmp + 1) : tmp;
                tmp += t;
                return tmp;
            });
        }

        private void Execute(OpCodes.mpyhhu i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => ((a >> 16) * (b >> 16)));
        }

        private void Execute(OpCodes.mpyhhau i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, t, carry) => ((a >> 16) * (b >> 16)) + t);
        }

        private void Execute(OpCodes.clz i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => {
                uint x = 0x80000000;
                for (int j = 0; j < 32; j++)
                    if ((a & x) != 0)
                        return (uint)j;
                    else
                        x = x >> 1;
                
                return 32;
            });
        }

        private void Execute(OpCodes.cntb i)
        {
            m_registers[i.RT].Value = ALUByte(m_registers[i.RA].Value, null, null, (a, b, c, carry) =>
            {
                uint x = 0x80;
                uint res = 0;
                for (int j = 0; j < 8; j++)
                {
                    if ((a & x) != 0)
                        res++;
                    x = x >> 1;
                }

                return res;
            });
        }

        private void Execute(OpCodes.fsmb i)
        {
            uint pref = m_registers[i.RA].Word;
            uint mask = 0x8000;

            m_registers[i.RT].Value = ALUByte(m_registers[i.RA].Value, null, null, (a, b, c, carry) => {
                uint res = (pref & mask) == 0 ? 0x00u : 0xffu;
                mask = mask >> 1;
                return res;
            });
        }

        private void Execute(OpCodes.fsmh i)
        {
            uint pref = m_registers[i.RA].Word & 0xff;
            uint mask = 0x80;

            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) =>
            {
                uint res = (pref & mask) == 0 ? 0x0000u : 0xffffu;
                mask = mask >> 1;
                return res;
            });
        }

        private void Execute(OpCodes.fsm i)
        {
            uint pref = m_registers[i.RA].Word & 0x0f;
            uint mask = 0x08;

            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) =>
            {
                uint res = (pref & mask) == 0 ? 0x00000000u : 0xffffffffu;
                mask = mask >> 1;
                return res;
            });
        }

        private void Execute(OpCodes.gbb i)
        {
            uint res = 0;
            uint mask = 0x8000;

            ALUByte(m_registers[i.RA].Value, null, null, (a, b, c, carry) =>
            {
                if ((a & 0x1) != 0)
                    res |= mask;
                mask = mask << 1;
                return 0;
            });

            m_registers[i.RT].Value = new RegisterValue(res);
        }


        private void Execute(OpCodes.gbh i)
        {
            uint res = 0;
            uint mask = 0x80;

            ALUHalfWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) =>
            {
                if ((a & 0x1) != 0)
                    res |= mask;
                mask = mask << 1;
                return 0;
            });

            m_registers[i.RT].Value = new RegisterValue(res);
        }

        private void Execute(OpCodes.gb i)
        {
            uint res = 0;
            uint mask = 0x08;

            ALUWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) =>
            {
                if ((a & 0x1) != 0)
                    res |= mask;
                mask = mask << 1;
                return 0;
            });

            m_registers[i.RT].Value = new RegisterValue(res);
        }

        private void Execute(OpCodes.avgb i)
        {
            m_registers[i.RT].Value = ALUByte(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => ((uint)a + (uint)b + 1u) >> 1);
        }

        private void Execute(OpCodes.absdb i)
        {
            m_registers[i.RT].Value = ALUByte(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => (uint)((b > a) ? b - a : a - b));
        }

        private void Execute(OpCodes.sumb i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value,null, (a, b, c, carry) =>
            {
                uint tmpb = (b & 0xff) + ((b >> 8) & 0xff) + ((b >> 16) & 0xff) + ((b >> 24) & 0xff);
                uint tmpa = (a & 0xff) + ((a >> 8) & 0xff) + ((a >> 16) & 0xff) + ((a >> 24) & 0xff);

                return (tmpb << 16) |  tmpa;
            });
        }

        private void Execute(OpCodes.xsbh i)
        {
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (uint)((a & 0x80) != 0 ? 0xff00 : 0x0000 | (a & 0xff)));
        }

        private void Execute(OpCodes.xshw i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (a & 0x8000) != 0 ? 0xffff0000 : 0x00000000 | (a & 0xffff));
        }

        private void Execute(OpCodes.xswd i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (a & 0x80000000) != 0 ? 0xffffffff00000000 : 0x0000000000000000 | (a & 0xffffffffu));
        }
    
        private void Execute(OpCodes.and i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => a & b);
        }

        private void Execute(OpCodes.andc i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => a & ~b);
        }

        private void Execute(OpCodes.andbi i)
        {
            uint tmp = (i.I10 & 0xffu);
            uint bbbb = tmp | (tmp << 8) | (tmp << 16) | (tmp << 24);
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (uint)(a & bbbb));
        }

        private void Execute(OpCodes.andhi i)
        {
            ushort t = (ushort)(RepLeftBit(i, 0) & 0xffff);
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (uint)(a & t));
        }

        private void Execute(OpCodes.andi i)
        {
            uint t = RepLeftBit(i, 0);
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (uint)(a & t));
        }

        private void Execute(OpCodes.or i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => a | b);
        }

        private void Execute(OpCodes.orc i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => a | ~b);
        }

        private void Execute(OpCodes.orbi i)
        {
            uint tmp = (i.I10 & 0xffu);
            uint bbbb = tmp | (tmp << 8) | (tmp << 16) | (tmp << 24);
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (uint)(a | bbbb));
        }

        private void Execute(OpCodes.orhi i)
        {
            ushort t = (ushort)(RepLeftBit(i, 0) & 0xffff);
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (uint)(a | t));
        }

        private void Execute(OpCodes.ori i)
        {
            uint t = RepLeftBit(i, 0);
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (uint)(a | t));
        }

        private void Execute(OpCodes.orx i)
        {
            m_registers[i.RT].Value = new RegisterValue(0);
            m_registers[i.RT].Value.Value[0] = (byte)(m_registers[i.RA].Value.Value[0] | m_registers[i.RA].Value.Value[4] | m_registers[i.RA].Value.Value[8] | m_registers[i.RA].Value.Value[12]);
            m_registers[i.RT].Value.Value[1] = (byte)(m_registers[i.RA].Value.Value[1] | m_registers[i.RA].Value.Value[5] | m_registers[i.RA].Value.Value[9] | m_registers[i.RA].Value.Value[13]);
            m_registers[i.RT].Value.Value[2] = (byte)(m_registers[i.RA].Value.Value[2] | m_registers[i.RA].Value.Value[6] | m_registers[i.RA].Value.Value[10] | m_registers[i.RA].Value.Value[14]);
            m_registers[i.RT].Value.Value[3] = (byte)(m_registers[i.RA].Value.Value[3] | m_registers[i.RA].Value.Value[7] | m_registers[i.RA].Value.Value[11] | m_registers[i.RA].Value.Value[15]);
        }

        private void Execute(OpCodes.xor i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => a ^ b);
        }

        private void Execute(OpCodes.xorbi i)
        {
            uint tmp = (i.I10 & 0xffu);
            uint bbbb = tmp | (tmp << 8) | (tmp << 16) | (tmp << 24);
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (uint)(a ^ bbbb));
        }

        private void Execute(OpCodes.xorhi i)
        {
            ushort t = (ushort)(RepLeftBit(i, 0) & 0xffff);
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (uint)(a ^ t));
        }

        private void Execute(OpCodes.xori i)
        {
            uint t = RepLeftBit(i, 0);
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (uint)(a ^ t));
        }

        private void Execute(OpCodes.nand i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => ~(a & b));
        }

        private void Execute(OpCodes.nor i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => ~(a | b));
        }

        private void Execute(OpCodes.eqv i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => (a ^ (~b)));
        }

        private void Execute(OpCodes.selb i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RC].Value, (a, b, c, carry) => ((c & b) | ((~c) & a)));
        }

        private void Execute(OpCodes.shufb i)
        {
            byte[] rconcat = new byte[32];
            Array.Copy(m_registers[i.RA].Value.Value, rconcat, 16);
            Array.Copy(m_registers[i.RB].Value.Value, 0, rconcat, 16, 16);

            m_registers[i.RT].Value = ALUByte(null, null, m_registers[i.RC].Value, (a, b, c, carry) =>
            {
                if ((c & 0xc0) == 0x80)
                    return 0x00;
                else if ((c & 0xe0) == 0xc0)
                    return 0xff;
                else if ((c & 0xe0) == 0xe0)
                    return 0x80;
                else
                    return rconcat[c & 0x1f];
            });
        }
        #endregion

        #region Shift and Rotate Instructions

        private void Execute(OpCodes.shlh i)
        {
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => (uint)(a << (int)(b & 0x1f)));
        }

        private void Execute(OpCodes.shlhi i)
        {
            uint shift = i.I7 & 0x1f;
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (uint)(a << (int)shift));
        }

        private void Execute(OpCodes.shl i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => a << (int)(b & 0x3f));
        }

        private void Execute(OpCodes.shli i)
        {
            uint shift = i.I7 & 0x3f;
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => a << (int)shift);
        }

        /*
        private void Execute(OpCodes.shlqbi i)
        {
        }

        private void Execute(OpCodes.shlqbii i)
        {
        }
         */

        private void Execute(OpCodes.shlqby i)
        {
            uint s = m_registers[i.RB].Word & 0xf;

            RegisterValue tmp = new RegisterValue(0);

            for (int b = 0; b < 15; b++)
                if (b + s < 16)
                    tmp.Value[b] = m_registers[i.RA].Value.Value[b + s];
                else
                    tmp.Value[b] = 0;

            m_registers[i.RT].Value = tmp;

            throw new Exception("Not reviewed");
        }

        private void Execute(OpCodes.shlqbyi i)
        {
            uint s = i.I7 & 0x1f;

            RegisterValue tmp = new RegisterValue(0);

            for (int b = 0; b < 15; b++)
                if (b + s < 16)
                    tmp.Value[b] = m_registers[i.RA].Value.Value[b + s];
                else
                    tmp.Value[b] = 0;

            m_registers[i.RT].Value = tmp;
        }

        private void Execute(OpCodes.shlqbybi i)
        {
            uint s = m_registers[i.RB].Word & 0xf8;

            RegisterValue tmp = new RegisterValue(0);

            for (int b = 0; b < 15; b++)
                if (b + s < 16)
                    tmp.Value[b] = m_registers[i.RA].Value.Value[b + s];
                else
                    tmp.Value[b] = 0;

            m_registers[i.RT].Value = tmp;

            throw new Exception("Not reviewed");
        }

        /*
        private void Execute(OpCodes.roth i)
        {
        }

        private void Execute(OpCodes.rothi i)
        {
        }

        private void Execute(OpCodes.rot i)
        {
        }

        private void Execute(OpCodes.roti i)
        {
        }
         */ 
        

        private void Execute(OpCodes.rotqby i)
        {
            uint count = m_registers[i.RB].Word & 0x0f;
            if (count == 0)
            {
                m_registers[i.RT].Value = new RegisterValue(m_registers[i.RA].Value.high, m_registers[i.RA].Value.low);
            }
            else
            {
                RegisterValue tmp = new RegisterValue(0);

                uint byteShift = count;

                for (int j = 0; j < 15; j++)
                    tmp.Value[j] = (byte)(m_registers[i.RA].Value.Value[(j + byteShift) % 16]);

                m_registers[i.RT].Value = tmp;
            }
        }

        private void Execute(OpCodes.rotqbyi i)
        {
            uint count = i.I7 & 0x0f;
            if (count == 0)
            {
                m_registers[i.RT].Value = new RegisterValue(m_registers[i.RA].Value.high, m_registers[i.RA].Value.low);
            }
            else
            {
                RegisterValue tmp = new RegisterValue(0);

                uint byteShift = count;

                for (int j = 0; j < 15; j++)
                    tmp.Value[j] = (byte)(m_registers[i.RA].Value.Value[(j + byteShift) % 16]);

                m_registers[i.RT].Value = tmp;
            }
        }
        /*
        private void Execute(OpCodes.rotqbybi i)
        {
        }

        private void Execute(OpCodes.rotqbi i)
        {
        }

        private void Execute(OpCodes.rotqbii i)
        {
        }

        private void Execute(OpCodes.rothm i)
        {
        }

        private void Execute(OpCodes.rothmi i)
        {
        }

        private void Execute(OpCodes.rotm i)
        {
        }

        private void Execute(OpCodes.rotmi i)
        {
        }

        private void Execute(OpCodes.rotqmby i)
        {
        }

        private void Execute(OpCodes.rotqmbyi i)
        {
        }

        private void Execute(OpCodes.rotqmbybi i)
        {
        }

        private void Execute(OpCodes.rotqmbi i)
        {
        }

        private void Execute(OpCodes.rotqmbii i)
        {
        }

        private void Execute(OpCodes.rotmah i)
        {
        }

        private void Execute(OpCodes.rotmahi i)
        {
        }

        private void Execute(OpCodes.rotma i)
        {
        }

        private void Execute(OpCodes.rotmai i)
        {
        }
        */

        /*private void Execute(OpCodes.rotqbyi i) 
        {
            uint count = i.I7 & 0x0f;
            if (count == 0)
            {
                m_registers[i.RT].Value = new RegisterValue(m_registers[i.RA].Value.high, m_registers[i.RA].Value.low);
            }
            else
            {
                RegisterValue tmp = new RegisterValue(0);

                uint byteShift = count / 8;
                uint bitShift = count % 8;

                int leftShiftMask = (1 << (int)(bitShift + 1)) - 1;
                int rightShiftMask = ~leftShiftMask;

                for (int j = 0; j < 15; j++)
                    tmp.Value[j] = (byte)(((m_registers[i.RA].Value.Value[(j + byteShift) % 16] & leftShiftMask) << (int)(8 - bitShift)) | ((m_registers[i.RA].Value.Value[(j + byteShift + 1) % 16] & rightShiftMask) >> (int)bitShift));

                m_registers[i.RT].Value = tmp;
            }
        }*/

        #endregion

        #region Compare, Branch, and Halt Instructions
        /*
        private void Execute(OpCodes.heq i)
        {
        }

        private void Execute(OpCodes.heqi i)
        {
        }

        private void Execute(OpCodes.hgt i)
        {
        }

        private void Execute(OpCodes.hgti i)
        {
        }

        private void Execute(OpCodes.hlgt i)
        {
        }

        private void Execute(OpCodes.hlgti i)
        {
        }
        */

        private void Execute(OpCodes.ceqb i)
        {
            m_registers[i.RT].Value = ALUByte(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => a == b ? 0xffu : 0x00u);
        }

        private void Execute(OpCodes.ceqbi i)
        {
            byte x = (byte)(i.I10 & 0xff);
            m_registers[i.RT].Value = ALUByte(m_registers[i.RA].Value, null, null, (a, b, c, carry) => a == x ? 0xffu : 0x00u);
        }

        private void Execute(OpCodes.ceqh i)
        {
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => a == b ? 0xffffu : 0x0000u);
        }

        private void Execute(OpCodes.ceqhi i)
        {
            ushort x = (ushort)(RepLeftBit(i, 0) & 0xffff);
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => a == x ? 0xffffu : 0x0000u);
        }

        private void Execute(OpCodes.ceq i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => a == b ? 0xffffffff : 0x00000000);
        }

        private void Execute(OpCodes.ceqi i)
        {
            uint x = RepLeftBit(i, 0);
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => a == x ? 0xffffffff : 0x00000000);
        }

        private void Execute(OpCodes.cgtb i)
        {
            m_registers[i.RT].Value = ALUByte(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => (int)(a << 8) > (int)(b << 8) ? 0xffu : 0x00u);
        }

        private void Execute(OpCodes.cgtbi i)
        {
            byte x = (byte)(i.I10 & 0xff);
            m_registers[i.RT].Value = ALUByte(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (int)(a << 8) > (int)(x << 8) ? 0xffu : 0x00u);
        }

        private void Execute(OpCodes.cgth i)
        {
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => (short)a > (short)b ? 0xffffu : 0x0000u);
        }

        private void Execute(OpCodes.cgthi i)
        {
            ushort x = (ushort)(RepLeftBit(i, 0) & 0xffff);
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (short)a > (short)x ? 0xffffu : 0x0000u);
        }

        private void Execute(OpCodes.cgt i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => (int)a > (int)b ? 0xffffffff : 0x00000000);
        }

        private void Execute(OpCodes.cgti i)
        {
            uint x = RepLeftBit(i, 0);
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => (int)a > (int)x ? 0xffffffff : 0x00000000);
        }

        private void Execute(OpCodes.clgtb i)
        {
            m_registers[i.RT].Value = ALUByte(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => a > b ? 0xffu : 0x00u);
        }

        private void Execute(OpCodes.clgtbi i)
        {
            byte x = (byte)(i.I10 & 0xff);
            m_registers[i.RT].Value = ALUByte(m_registers[i.RA].Value, null, null, (a, b, c, carry) => a > x ? 0xffu : 0x00u);
        }

        private void Execute(OpCodes.clgth i)
        {
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => a > b ? 0xffffu : 0x0000u);
        }

        private void Execute(OpCodes.clgthi i)
        {
            ushort x = (ushort)(RepLeftBit(i, 0) & 0xffff);
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => a > x ? 0xffffu : 0x0000u);
        }

        private void Execute(OpCodes.clgt i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, null, (a, b, c, carry) => a > b ? 0xffffffff : 0x00000000);
        }

        private void Execute(OpCodes.clgti i)
        {
            uint x = RepLeftBit(i, 0);
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, null, null, (a, b, c, carry) => a > x ? 0xffffffff : 0x00000000);
        }

        private void Execute(OpCodes.br i)
        {
            this.PC = ((this.PC - 4) + RepLeftBit(i, 2)) & m_spe.LSLR;
        }

        private void Execute(OpCodes.bra i)
        {
            this.PC = RepLeftBit(i, 2) & m_spe.LSLR;
        }

        private void Execute(OpCodes.brsl i)
        {
            m_registers[i.RT].Value = new RegisterValue(0);
            m_registers[i.RT].Word = this.PC & m_spe.LSLR;
            this.PC = (uint)((this.PC - 4) + (int)RepLeftBit(i, 2)) & m_spe.LSLR;
        }

        private void Execute(OpCodes.brasl i)
        {
            m_registers[i.RT].Value = new RegisterValue(0);
            m_registers[i.RT].Word = this.PC & m_spe.LSLR;
            this.PC = RepLeftBit(i, 2) & m_spe.LSLR;
        }

        private void Execute(OpCodes.bi i)
        {
            //TODO: Deal with interrupts
            this.PC = m_registers[i.RA].Word & m_spe.LSLR & 0xfffffffc;
        }

        private void Execute(OpCodes.iret i)
        {
            //TODO: Deal with interrupts
            this.PC = m_SRR0.Word;

            throw new Exception("Not reviewed");
        }

        private void Execute(OpCodes.bisled i)
        {
            //TODO: Deal with interrupts
            uint t = m_registers[i.RA].Word & m_spe.LSLR & 0xfffffffc;
            uint u = m_spe.LSLR & this.PC;

            m_registers[i.RT].Value = new RegisterValue(0);
            m_registers[i.RT].Word = u;

            /*
            if (external event)
                this.PC = t;
            else
                this.PC = U;
            */

            throw new Exception("Not reviewed");
        }

        private void Execute(OpCodes.bisl i)
        {
            //TODO: Deal with interrupts
            uint t = m_registers[i.RA].Word & m_spe.LSLR & 0xfffffffc;
            uint u = m_spe.LSLR & this.PC;

            m_registers[i.RT].Value = new RegisterValue(0);
            m_registers[i.RT].Word = u;
            this.PC = t;
        }

        private void Execute(OpCodes.brnz i)
        {
            if (m_registers[i.RT].Word != 0)
                this.PC = ((this.PC - 4) + RepLeftBit(i, 2)) & m_spe.LSLR & 0xFFFFFFFC;
            else
                this.PC = this.PC & m_spe.LSLR;
        }

        private void Execute(OpCodes.brz i)
        {
            if (m_registers[i.RT].Word == 0)
                this.PC = ((this.PC - 4) + RepLeftBit(i, 2)) & m_spe.LSLR & 0xFFFFFFFC;
            else
                this.PC = this.PC & m_spe.LSLR;
        }

        private void Execute(OpCodes.brhnz i)
        {
            if (m_registers[i.RT].Halfword != 0)
                this.PC = ((this.PC - 4) + (RepLeftBit(i, 2) & 0xffff)) & m_spe.LSLR & 0xFFFFFFFC;
            else
                this.PC = this.PC & m_spe.LSLR;
        }

        private void Execute(OpCodes.brhz i)
        {
            if (m_registers[i.RT].Halfword == 0)
                this.PC = ((this.PC - 4) + (RepLeftBit(i, 2) & 0xffff)) & m_spe.LSLR & 0xFFFFFFFC;
            else
                this.PC = this.PC & m_spe.LSLR;
        }

        private void Execute(OpCodes.biz i)
        {
            //TODO: Deal with interrupts
            uint t = m_registers[i.RA].Word & m_spe.LSLR & 0xfffffffc;
            uint u = m_spe.LSLR & this.PC;

            if (m_registers[i.RT].Word == 0)
                this.PC = t & m_spe.LSLR & 0xfffffffc;
            else
                this.PC = u;

            throw new Exception("Not reviewed");
        }

        private void Execute(OpCodes.binz i)
        {
            //TODO: Deal with interrupts
            uint t = m_registers[i.RA].Word & m_spe.LSLR & 0xfffffffc;
            uint u = m_spe.LSLR & this.PC;

            if (m_registers[i.RT].Word != 0)
                this.PC = t & m_spe.LSLR & 0xfffffffc;
            else
                this.PC = u;

            throw new Exception("Not reviewed");
        }

        private void Execute(OpCodes.bihz i)
        {
            //TODO: Deal with interrupts
            uint t = m_registers[i.RA].Word & m_spe.LSLR & 0xfffffffc;
            uint u = m_spe.LSLR & this.PC;

            if (m_registers[i.RT].Halfword == 0)
                this.PC = t & m_spe.LSLR & 0xfffffffc;
            else
                this.PC = u;

            throw new Exception("Not reviewed");
        }

        private void Execute(OpCodes.bihnz i)
        {
            //TODO: Deal with interrupts
            uint t = m_registers[i.RA].Word & m_spe.LSLR & 0xfffffffc;
            uint u = m_spe.LSLR & this.PC;

            if (m_registers[i.RT].Halfword != 0)
                this.PC = t & m_spe.LSLR & 0xfffffffc;
            else
                this.PC = u;

            throw new Exception("Not reviewed");
        }

        #endregion

        #region Hint-for-Branch Instructions
        /// <summary>
        /// The address of the branch target is given by the contents of the preferred slot of register RA. The RO field gives the signed word offset from the hbr 
        /// instruction to the branch instruction.
        /// If the P feature bit is set, hbr does not hint a branch. Instead, it hints that this is the proper implementation-specific moment to perform inline
        /// prefetching. Inline prefetching is the instruction fetch function necessary to run linearly sequential program text. To obtain optimal performance, 
        /// some implementations of the SPU may require help scheduling these inline prefetches of local storage when the program is also doing loads and stores. 
        /// See the implementation-specific SPU documentation for information about when this might be beneficial. When the P feature bit is set, the instruction 
        /// ignores the value of RA. The relative offset (RO) field, formed by concatenating ROH (high) and ROL (low), must be set to zero.
        /// </summary>
        /// <param name="i"></param>
        private void Execute(OpCodes.hbr i) { }

        /// <summary>
        /// The address of the branch target is specified by an address in the I16 field. The value has 2 bits of zero appended on the right before it is used.
        /// The RO field, formed by concatenating ROH (high) and ROL (low), gives the signed word offset from the hbra instruction to the branch instruction.
        /// </summary>
        /// <param name="i"></param>
        private void Execute(OpCodes.hbra i) { }

        /// <summary>
        /// The address of the branch target is specified by a word offset given in the I16 field. The signed I16 field is added to the address of the hbrr instruction to determine the absolute address of the branch target.
        /// The RO field, formed by concatenating ROH (high) and ROL (low), gives the signed word offset from the hbrr instruction to the branch instruction.
        /// </summary>
        /// <param name="i"></param>
        private void Execute(OpCodes.hbrr i) { }
        
        #endregion

        #region Floating-Point Instructions
        /*
        private void Execute(OpCodes.fa i)
        {
        }

        private void Execute(OpCodes.dfa i)
        {
        }

        private void Execute(OpCodes.fs i)
        {
        }

        private void Execute(OpCodes.dfs i)
        {
        }

        private void Execute(OpCodes.fm i)
        {
        }

        private void Execute(OpCodes.dfm i)
        {
        }

        private void Execute(OpCodes.fma i)
        {
        }

        private void Execute(OpCodes.dfma i)
        {
        }

        private void Execute(OpCodes.fnms i)
        {
        }

        private void Execute(OpCodes.dfnms i)
        {
        }

        private void Execute(OpCodes.fms i)
        {
        }

        private void Execute(OpCodes.dfms i)
        {
        }

        private void Execute(OpCodes.dfnma i)
        {
        }

        private void Execute(OpCodes.frest i)
        {
        }

        private void Execute(OpCodes.frsqest i)
        {
        }

        private void Execute(OpCodes.fi i)
        {
        }

        private void Execute(OpCodes.csflt i)
        {
        }

        private void Execute(OpCodes.cflts i)
        {
        }

        private void Execute(OpCodes.cuflt i)
        {
        }

        private void Execute(OpCodes.cfltu i)
        {
        }

        private void Execute(OpCodes.frds i)
        {
        }

        private void Execute(OpCodes.fesd i)
        {
        }

        private void Execute(OpCodes.dfceq i)
        {
        }

        private void Execute(OpCodes.dfcmeq i)
        {
        }

        private void Execute(OpCodes.dfcgt i)
        {
        }

        private void Execute(OpCodes.dfcmgt i)
        {
        }

        private void Execute(OpCodes.dftsv i)
        {
        }

        private void Execute(OpCodes.fceq i)
        {
        }

        private void Execute(OpCodes.fcmeq i)
        {
        }

        private void Execute(OpCodes.fcgt i)
        {
        }

        private void Execute(OpCodes.fcmgt i)
        {
        }

        private void Execute(OpCodes.fscrwr i)
        {
        }

        private void Execute(OpCodes.fscrrd i)
        {
        }
        */

        #endregion

        #region Control Instructions

        /// <summary>
        /// Execution of the program in the SPU stops, and the external environment is signaled. No further instructions are executed.
        /// </summary>
        /// <param name="i"></param>
        private void Execute(OpCodes.stop i)
        {
            this.PC = (PC - 4) & m_spe.LSLR;
            this.m_doRun = false;
        }

        /// <summary>
        /// Execution of the program in the SPU stops.
        /// </summary>
        /// <param name="i"></param>
        private void Execute(OpCodes.stopd i)
        {
            this.PC = (PC - 4) & m_spe.LSLR;
            this.m_doRun = false;
        }

        /// <summary>
        /// This instruction has no effect on the execution of the program. It exists to provide implementation-defined control of instruction issuance.
        /// </summary>
        /// <param name="i"></param>
        private void Execute(OpCodes.lnop i) { }

        /// <summary>
        ///  This instruction has no effect on the execution of the program. It exists to provide implementation-defined control of instruction issuance. RT 
        ///  is a false target. Implementations can schedule instructions as though this instruction produces a value into RT. Programs can avoid unnecessary 
        ///  delay by programming RT so as not to appear to source data for nearby subsequent instructions. False targets are not written.
        /// </summary>
        /// <param name="i"></param>
        private void Execute(OpCodes.nop i) { }

        /// <summary>
        /// This instruction has no effect on the execution of the program other than to cause the processor to wait until all pending store instructions have 
        /// completed before fetching the next sequential instruction. This instruction must be used following a store instruction that modifies the instruction 
        /// stream.
        /// The C feature bit causes channel synchronization to occur before instruction synchronization occurs. Channel synchronization allows an SPU state 
        /// modified through channel instructions to affect execution.
        /// </summary>
        /// <param name="i"></param>
        private void Execute(OpCodes.sync i) { }

        /// <summary>
        /// This instruction forces all earlier load, store, and channel instructions to complete before proceeding. No subsequent load, store, or channel 
        /// instructions can start until the previous instructions complete. The dsync instruction allows SPU software to ensure that the local storage data 
        /// would be consistent if it were observed by another entity. This instruction does not affect any prefetching of instructions that the processor
        /// might have done. Synchronization is discussed in more detail in Section 13 Synchronization and Ordering on page 253.
        /// </summary>
        /// <param name="i"></param>
        private void Execute(OpCodes.dsync i) { }

        /*
        private void Execute(OpCodes.mfspr i)
        {
        }

        private void Execute(OpCodes.mtspr i)
        {
        }
        */

        #endregion

        #region Channel Instructions

        /*
        private void Execute(OpCodes.rdch i)
        {
        }

        private void Execute(OpCodes.rchcnt i)
        {
        }

        private void Execute(OpCodes.wrch i)
        {
        }
        */

        #endregion
    }
}
