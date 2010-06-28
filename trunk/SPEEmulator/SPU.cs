using System;
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

        private uint m_pc;

        private Dictionary<Type, System.Reflection.MethodInfo> m_executionFunctions;

        public SPU(SPEProcessor spe, uint registercount = 128)
        {
            m_spe = spe;
            m_registers = new Register[registercount];
            for (int i = 0; i < m_registers.Length; i++)
                m_registers[i] = new Register();
            m_parser = new OpCodes.OpCodeParser();

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
            while (true)
            {
                uint opcode =
                    (uint)(m_spe.LS[PC] << (8 * 3)) |
                    ((uint)m_spe.LS[PC + 1] << (8 * 2)) |
                    ((uint)m_spe.LS[PC + 2] << (8 * 1)) |
                    ((uint)m_spe.LS[PC + 3] << (8 * 0))
                    ;

                SPEEmulator.OpCodes.Bases.Instruction op = m_parser.FindCode(opcode);
                Execute(op);
                PC += 4;
            }
        }

        /// <summary>
        /// Gets or sets the Program Counter (PC)
        /// </summary>
        public uint PC
        {
            get { return m_pc; }
            set { m_pc = value; }
        }

        private void Execute(OpCodes.Bases.Instruction i)
        {
            System.Reflection.MethodInfo mi;
            if (!m_executionFunctions.TryGetValue(i.GetType(), out mi) || mi == null)
                throw new Exception(string.Format("Unable to execute instruction of type {0}", i.Mnemonic));
            mi.Invoke(this, new object[] { i });
        }

        private void CopyToLS(Register r, long lsOffset)
        {
            Array.Copy(r.Value.Value, 0, m_spe.LS, lsOffset, 16); 
        }

        private void CopyFromLS(Register r, long lsOffset)
        {
            Array.Copy(m_spe.LS, (lsOffset & m_spe.LSLR), r.Value.Value, 0, 16);
        }

        private void Execute(OpCodes.lqd i)
        {
            //TODO: Signextend
            CopyFromLS(m_registers[i.RT], (((i.I10 << 4) + m_registers[i.RA].Word & (~0xf))));
        }

        private void Execute(OpCodes.lqx i)
        {
            CopyFromLS(m_registers[i.RT], m_registers[i.RB].Word + m_registers[i.RA].Word & (~0xf));
        }

        private void Execute(OpCodes.lqa i)
        {
            uint tmp = (i.I16 & 0x8000) != 0 ? 0xffff0000 : 0x00000000;
            CopyFromLS(m_registers[i.RT], (((i.I16 | tmp) << 2) & (~0xf)));
        }

        private void Execute(OpCodes.lqr i)
        {
            uint tmp = (i.I16 & 0x8000) != 0 ? 0xffff0000 : 0x00000000;
            CopyFromLS(m_registers[i.RT], (((i.I16 | tmp) << 2) + PC) & (~0xf));
        }

        private void Execute(OpCodes.stqd i)
        {
            CopyToLS(m_registers[i.RT], ((i.I10 << 4) + m_registers[i.RA].Word) & ~0xf);
        }

        private void Execute(OpCodes.stqx i)
        {
            CopyToLS(m_registers[i.RT], (m_registers[i.RA].Word + m_registers[i.RB].Word) & ~0xf);
        }

        private void Execute(OpCodes.stqa i)
        {
            uint tmp = (i.I16 & 0x8000) != 0 ? 0xffff0000 : 0x00000000;
            CopyToLS(m_registers[i.RT], ((i.I16 | tmp) << 2) & ~0xf);
        }

        private void Execute(OpCodes.stqr i)
        {
            uint tmp = (i.I16 & 0x8000) != 0 ? 0xffff0000 : 0x00000000;
            CopyToLS(m_registers[i.RT], (((i.I16 | tmp) << 2) + PC) & (~0xf));
        }

        private void Execute(OpCodes.cbd i)
        {
            uint index = (i.I7 + m_registers[i.RA].Word) & 0xf;
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
            uint index = (i.I7 + m_registers[i.RA].Word) & 0xf;
            m_registers[i.RT].Value.high = 0x1011121314151617u;
            m_registers[i.RT].Value.low = 0x18191A1B1C1D1E1Fu;
            m_registers[i.RT].Value.Value[index] = 0x02;
            m_registers[i.RT].Value.Value[index + 1] = 0x03;
        }

        private void Execute(OpCodes.chx i)
        {
            uint index = (m_registers[i.RB].Word + m_registers[i.RA].Word) & 0xf;
            m_registers[i.RT].Value.high = 0x1011121314151617u;
            m_registers[i.RT].Value.low = 0x18191A1B1C1D1E1Fu;
            m_registers[i.RT].Value.Value[index] = 0x02;
            m_registers[i.RT].Value.Value[index + 1] = 0x03;
        }

        private void Execute(OpCodes.cwd i)
        {
            uint index = (i.I7 + m_registers[i.RA].Word) & 0xf;
            m_registers[i.RT].Value.high = 0x1011121314151617u;
            m_registers[i.RT].Value.low = 0x18191A1B1C1D1E1Fu;
            m_registers[i.RT].Value.Value[index] = 0x00;
            m_registers[i.RT].Value.Value[index + 1] = 0x01;
            m_registers[i.RT].Value.Value[index + 2] = 0x02;
            m_registers[i.RT].Value.Value[index + 3] = 0x03;
        }

        private void Execute(OpCodes.cwx i)
        {
            uint index = (m_registers[i.RB].Word + m_registers[i.RA].Word) & 0xf;
            m_registers[i.RT].Value.high = 0x1011121314151617u;
            m_registers[i.RT].Value.low = 0x18191A1B1C1D1E1Fu;
            m_registers[i.RT].Value.Value[index] = 0x00;
            m_registers[i.RT].Value.Value[index + 1] = 0x01;
            m_registers[i.RT].Value.Value[index + 2] = 0x02;
            m_registers[i.RT].Value.Value[index + 3] = 0x03;
        }

        private void Execute(OpCodes.cdd i)
        {
            uint index = (i.I7 + m_registers[i.RA].Word) & 0xf;
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
            uint index = (m_registers[i.RB].Word + m_registers[i.RA].Word) & 0xf;
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

        private void Execute(OpCodes.ilh i)
        {
            for (int j = 0; j < 8; j++)
            {
                m_registers[i.RT].Value.Value[(j * 2) + 1] = (byte)(i.I16 & 0xff);
                m_registers[i.RT].Value.Value[j * 2] = (byte)((i.I16 >> 8) & 0xff);
            }
        }

        private void Execute(OpCodes.ilhu i)
        {
            uint v = (i.I16 << 2);

            for (int j = 0; j < 16; j += 4)
            {
                m_registers[i.RT].Value.Value[j + 3] = (byte)(v & 0xff);
                m_registers[i.RT].Value.Value[j + 2] = (byte)((v >> 8) & 0xff);
                m_registers[i.RT].Value.Value[j + 1] = (byte)((v >> 16) & 0xff);
                m_registers[i.RT].Value.Value[j] = (byte)((v >> 24) & 0xff);
            }
        }

        private void Execute(OpCodes.il i)
        {
            m_registers[i.RT].Value = new RegisterValue(((i.I16 & 0x8000) != 0 ? 0xffff0000 : 0x00000000) | i.I16);
        }

        private void Execute(OpCodes.ila i)
        {
            m_registers[i.RT].Value = new RegisterValue(i.I18);
        }

        private void Execute(OpCodes.iohl i)
        {
            m_registers[i.RT].Value |= new RegisterValue(i.I16);
        }

        private void Execute(OpCodes.fsmbi i)
        {
            for (int j = 0; j < 15; j++)
                m_registers[i.RT].Value.Value[j] = (byte)(((i.I16 >> j) & 0x1) == 0 ? 0x00 : 0xff);
        }

        private void Execute(OpCodes.ah i)
        {
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => (a + b) & 0xffff);
        }

        private void Execute(OpCodes.ahi i)
        {
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, new RegisterValue(((i.I10 & 0x200) != 0 ? 0xfffffc00 : 0x00000000) | i.I10), m_registers[i.RT].Value, (a, b, c, carry) => (a + b) & 0xffff);
        }

        private void Execute(OpCodes.a i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => (a + b) & 0xffffffff);
        }

        private void Execute(OpCodes.ai i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, new RegisterValue(((i.I10 & 0x200) != 0 ? 0xfffffc00 : 0x00000000) | i.I10), m_registers[i.RT].Value, (a, b, c, carry) => (a + b) & 0xffffffff);
        }

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

            for (int j = 15; j > 0; j--)
            {
                carry = exec(
                    a.Value[j]
                    ,
                    b.Value[j]
                    ,
                    c.Value[j]
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
        private RegisterValue ALUHalfWord(RegisterValue a, RegisterValue b, RegisterValue c, Func<uint, uint, uint, uint, uint> exec)
        {
            RegisterValue x = new RegisterValue();
            ulong carry = 0;

            for (int j = 14; j > 0; j -= 2)
            {
                carry = exec(
                    (((uint)a.Value[j + 1] << 8) | (uint)a.Value[j])
                    ,
                    (((uint)b.Value[j + 1] << 8) | (uint)b.Value[j])
                    ,
                    (((uint)c.Value[j + 1] << 8) | (uint)c.Value[j])
                    ,
                    (uint)(carry >> 16)
                    );

                x.Value[j] = (byte)(carry & 0xff);
                x.Value[j + 1] = (byte)((carry >> 8) & 0xff);

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
            for (int j = 11; j > 0; j -= 4)
            {
                carry =
                    exec(
                        (((uint)a.Value[j + 3] << 24) | ((uint)a.Value[j + 2] << 16) | ((uint)a.Value[j + 1] << 8) | (uint)a.Value[j])
                        ,
                        (((uint)b.Value[j + 3] << 24) | ((uint)b.Value[j + 2] << 16) | ((uint)b.Value[j + 1] << 8) | (uint)b.Value[j])
                        ,
                        (((uint)c.Value[j + 3] << 24) | ((uint)c.Value[j + 2] << 16) | ((uint)c.Value[j + 1] << 8) | (uint)c.Value[j])
                        ,
                        (uint)(carry >> 32)
                    );

                x.Value[j + 3] = (byte)((carry >> 24) & 0xff);
                x.Value[j + 2] = (byte)((carry >> 16) & 0xff);
                x.Value[j + 1] = (byte)((carry >> 8) & 0xff);
                x.Value[j] = (byte)(carry & 0xff);
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
        private RegisterValue ALUDoubleWord(RegisterValue a, RegisterValue b, RegisterValue c, Func<ulong, ulong, ulong, ulong, ulong> exec)
        {
            RegisterValue x = new RegisterValue();
            ulong carry = 0;
            for (int j = 7; j > 0; j -= 8)
            {
                carry =
                    exec(
                        (((ulong)a.Value[j + 7] << 56) | ((ulong)a.Value[j + 6] << 48) | ((ulong)a.Value[j + 5] << 40) | ((ulong)a.Value[j + 4] << 32) | ((ulong)a.Value[j + 3] << 24) | ((ulong)a.Value[j + 2] << 16) | ((ulong)a.Value[j + 1] << 8) | (ulong)a.Value[j])
                        ,
                        (((ulong)b.Value[j + 7] << 56) | ((ulong)b.Value[j + 6] << 48) | ((ulong)b.Value[j + 5] << 40) | ((ulong)b.Value[j + 4] << 32) | ((ulong)b.Value[j + 3] << 24) | ((ulong)b.Value[j + 2] << 16) | ((ulong)b.Value[j + 1] << 8) | (ulong)b.Value[j])
                        ,
                        (((ulong)c.Value[j + 7] << 56) | ((ulong)c.Value[j + 6] << 48) | ((ulong)c.Value[j + 5] << 40) | ((ulong)c.Value[j + 4] << 32) | ((ulong)c.Value[j + 3] << 24) | ((ulong)c.Value[j + 2] << 16) | ((ulong)c.Value[j + 1] << 8) | (ulong)c.Value[j])
                        ,
                        (ulong)(carry >> 32)
                    );

                x.Value[j + 7] = (byte)((carry >> 56) & 0xff);
                x.Value[j + 6] = (byte)((carry >> 48) & 0xff);
                x.Value[j + 5] = (byte)((carry >> 40) & 0xff);
                x.Value[j + 4] = (byte)((carry >> 32) & 0xff);
                x.Value[j + 3] = (byte)((carry >> 24) & 0xff);
                x.Value[j + 2] = (byte)((carry >> 16) & 0xff);
                x.Value[j + 1] = (byte)((carry >> 8) & 0xff);
                x.Value[j] = (byte)(carry & 0xff);
            }

            return x;
        }

        private void Execute(OpCodes.sfh i)
        {
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => (a + ~b + 1) & 0xffff);
        }

        private void Execute(OpCodes.sfhi i)
        {
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, new RegisterValue(((i.I10 & 0x200) != 0 ? 0xfffffc00 : 0x00000000) | i.I10), m_registers[i.RT].Value, (a, b, c, carry) => (a + ~b + 1) & 0xffff);
        }

        private void Execute(OpCodes.sf i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => (a + ~b + 1) & 0xffffffff);
        }

        private void Execute(OpCodes.sfi i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, new RegisterValue(((i.I10 & 0x200) != 0 ? 0xfffffc00 : 0x00000000) | i.I10), m_registers[i.RT].Value, (a, b, c, carry) => (a + ~b + 1) & 0xffffffff);
        }

        private void Execute(OpCodes.addx i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => (a + b + (c & 0x1)) & 0xffffffff);
        }

        private void Execute(OpCodes.cg i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => a + b > 0xffffffffu ? 1u : 0u);
        }

        private void Execute(OpCodes.cgx i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => a + b + (c & 0x1) > 0xffffffffu ? 1u : 0u);
        }

        private void Execute(OpCodes.sfx i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => (a + ~b + 1 + ~(c & 0x1)) & 0xffffffff);
        }

        private void Execute(OpCodes.bg i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => a > b ? 0u : 1u);
        }

        private void Execute(OpCodes.bgx i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => ((a + ~b + 1) + ~(c & 0x1)) > 0xffffffff ? 0u : 1u);
        }

        private void Execute(OpCodes.mpy i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => { 
                var tmp = ((a & 0xffff) * (b & 0xffff));
                return ((a & 0x8000) != (b & 0x8000)) ? (~tmp + 1) : tmp;
            });
        }

        private void Execute(OpCodes.mpyu i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => ((a & 0xffff) * (b & 0xffff)));
        }

        private void Execute(OpCodes.mpyi i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, new RegisterValue(((i.I10 & 0x200) != 0 ? 0xfffffc00 : 0x00000000) | i.I10), m_registers[i.RT].Value, (a, b, c, carry) =>
            {
                var tmp = ((a & 0xffff) * (b & 0xffff));
                return ((a & 0x8000) != (b & 0x8000)) ? (~tmp + 1) : tmp;
            });
        }

        private void Execute(OpCodes.mpyui i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, new RegisterValue(((i.I10 & 0x200) != 0 ? 0xfffffc00 : 0x00000000) | i.I10), m_registers[i.RT].Value, (a, b, c, carry) => ((a & 0xffff) * (b & 0xffff)));
        }

        private void Execute(OpCodes.mpya i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RC].Value, (a, b, c, carry) => {
                var tmp = ((a & 0xffff) * (b & 0xffff));
                tmp = ((a & 0x8000) != (b & 0x8000)) ? (~tmp + 1) : tmp;
                return tmp + c;
            });
        }

        private void Execute(OpCodes.mpyh i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => {
                var tmp = a >> 16;
                if ((tmp & 0x8000) != 0)
                    tmp |= 0xffff0000;

                var tmp2 = (tmp * b) << 16;
                tmp2 = ((tmp & 0x8000) != (b & 0x8000)) ? (~tmp2 + 1) : tmp2;
                return tmp2;
            });

        }

        private void Execute(OpCodes.mpys i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => {
                var tmp = ((a & 0xffff) * (b & 0xffff)) >> 16;
                tmp = ((a & 0x8000) != (b & 0x8000)) ? (~tmp + 1) : tmp;

                if (tmp > 0xffff)
                    return tmp | 0xffff0000;
                else
                    return tmp;
            });
        }

        private void Execute(OpCodes.mpyhh i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => {
                var tmp = ((a >> 16) * (b >> 16));
                tmp = ((a & 0x8000) != (b & 0x8000)) ? (~tmp + 1) : tmp;
                return tmp;
            });
        }

        private void Execute(OpCodes.mpyhha i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) =>
            {
                var tmp = ((a >> 16) * (b >> 16));
                tmp = ((a & 0x8000) != (b & 0x8000)) ? (~tmp + 1) : tmp;
                tmp += c;
                return tmp;
            });
        }

        private void Execute(OpCodes.mpyhhu i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => ((a >> 16) * (b >> 16)));
        }

        private void Execute(OpCodes.mpyhhau i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => ((a >> 16) * (b >> 16)) + c);
        }

        private void Execute(OpCodes.clz i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RT].Value, m_registers[i.RT].Value, (a, b, c, carry) => {
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
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RT].Value, m_registers[i.RT].Value, (a, b, c, carry) =>
            {
                uint x = 0x80000000;
                uint res = 0;
                for (int j = 0; j < 32; j++)
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
            int slot = 0;

            m_registers[i.RT].Value = ALUByte(m_registers[i.RA].Value, m_registers[i.RT].Value, m_registers[i.RT].Value, (a, b, c, carry) => {
                uint res = ((pref >> slot) & 0x1) == 0 ? 0x00u : 0xffu;
                slot++;
                return res;
            });
        }

        private void Execute(OpCodes.fsmh i)
        {
            uint pref = m_registers[i.RA].Word & 0xff;
            int slot = 0;

            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, m_registers[i.RT].Value, m_registers[i.RT].Value, (a, b, c, carry) =>
            {
                uint res = ((pref >> slot) & 0x1) == 0 ? 0x0000u : 0xffffu;
                slot++;
                return res;
            });
        }

        private void Execute(OpCodes.fsm i)
        {
            uint pref = m_registers[i.RA].Word & 0xf;
            int slot = 0;

            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RT].Value, m_registers[i.RT].Value, (a, b, c, carry) =>
            {
                uint res = ((pref >> slot) & 0x1) == 0 ? 0x00000000u : 0xffffffffu;
                slot++;
                return res;
            });
        }

        private void Execute(OpCodes.gbb i)
        {
            uint res = 0;
            int slot = 0;

            ALUByte(m_registers[i.RA].Value, m_registers[i.RT].Value, m_registers[i.RT].Value, (a, b, c, carry) =>
            {
                if ((a & 0x1) != 0)
                    res |= (1u << slot);
                slot++;
                return 0;
            });

            m_registers[i.RT].Value = new RegisterValue(res);
        }


        private void Execute(OpCodes.gbh i)
        {
            uint res = 0;
            int slot = 0;

            ALUHalfWord(m_registers[i.RA].Value, m_registers[i.RT].Value, m_registers[i.RT].Value, (a, b, c, carry) =>
            {
                if ((a & 0x1) != 0)
                    res |= (1u << slot);
                slot++;
                return 0;
            });

            m_registers[i.RT].Value = new RegisterValue(res);
        }

        private void Execute(OpCodes.gb i)
        {
            uint res = 0;
            int slot = 0;

            ALUWord(m_registers[i.RA].Value, m_registers[i.RT].Value, m_registers[i.RT].Value, (a, b, c, carry) =>
            {
                if ((a & 0x1) != 0)
                    res |= (1u << slot);
                slot++;
                return 0;
            });

            m_registers[i.RT].Value = new RegisterValue(res);
        }

        private void Execute(OpCodes.avgb i)
        {
            m_registers[i.RT].Value = ALUByte(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => ((uint)a + (uint)b + 1u) >> 1);
        }

        private void Execute(OpCodes.absdb i)
        {
            m_registers[i.RT].Value = ALUByte(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => (uint)((b > a) ? b - a : a - b));
        }

        private void Execute(OpCodes.sumb i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) =>
            {
                uint tmpa = (a & 0xff) + ((a >> 8) & 0xff) + ((a >> 16) & 0xff) + ((a >> 24) & 0xff);
                uint tmpb = (b & 0xff) + ((b >> 8) & 0xff) + ((b >> 16) & 0xff) + ((b >> 24) & 0xff);

                return (tmpa << 16) | tmpb;
            });
        }

        private void Execute(OpCodes.xsbh i)
        {
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, m_registers[i.RT].Value, m_registers[i.RT].Value, (a, b, c, carry) => (a & 0x80) != 0 ? 0xff00 : 0x0000 | (a & 0xff));
        }

        private void Execute(OpCodes.xshw i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RT].Value, m_registers[i.RT].Value, (a, b, c, carry) => (a & 0x8000) != 0 ? 0xffff0000 : 0x00000000 | (a & 0xffff));
        }

        private void Execute(OpCodes.xswd i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RT].Value, m_registers[i.RT].Value, (a, b, c, carry) => (a & 0x80000000) != 0 ? 0xffffffff00000000 : 0x0000000000000000 | (a & 0xffffffffu));
        }
    
        private void Execute(OpCodes.and i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => a & b);
        }

        private void Execute(OpCodes.andc i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => a & ~b);
        }

        private void Execute(OpCodes.andbi i)
        {
            m_registers[i.RT].Value = ALUByte(m_registers[i.RA].Value, new RegisterValue(i.I10 & 0xff), m_registers[i.RT].Value, (a, b, c, carry) => (uint)(a & b));
        }

        private void Execute(OpCodes.andhi i)
        {
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, new RegisterValue((i.I10 & 0x200) != 0 ? 0xfc00 : 0x0000 | i.I10), m_registers[i.RT].Value, (a, b, c, carry) => (uint)(a & b));
        }

        private void Execute(OpCodes.andi i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, new RegisterValue((i.I10 & 0x200) != 0 ? 0xfffffc00 : 0x00000000 | i.I10), m_registers[i.RT].Value, (a, b, c, carry) => (uint)(a & b));
        }

        private void Execute(OpCodes.or i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => a | b);
        }

        private void Execute(OpCodes.orc i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => a | ~b);
        }

        private void Execute(OpCodes.orbi i)
        {
            m_registers[i.RT].Value = ALUByte(m_registers[i.RA].Value, new RegisterValue(i.I10 & 0xff), m_registers[i.RT].Value, (a, b, c, carry) => (uint)(a | b));
        }

        private void Execute(OpCodes.orhi i)
        {
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, new RegisterValue((i.I10 & 0x200) != 0 ? 0xfc00 : 0x0000 | i.I10), m_registers[i.RT].Value, (a, b, c, carry) => (uint)(a | b));
        }

        private void Execute(OpCodes.ori i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, new RegisterValue((i.I10 & 0x200) != 0 ? 0xfffffc00 : 0x00000000 | i.I10), m_registers[i.RT].Value, (a, b, c, carry) => (uint)(a | b));
        }

        private void Execute(OpCodes.orx i)
        {
            uint res = 0;
            int slot = 0;

            ALUWord(m_registers[i.RA].Value, m_registers[i.RT].Value, m_registers[i.RT].Value, (a, b, c, carry) =>
            {
                res |= ((a & 0xff) | ((a >> 8) & 0xff) | ((a >> 16) & 0xff) | ((a >> 24) & 0xff)) << slot;
                slot += 8;
                return 0;
            });

            m_registers[i.RT].Value = new RegisterValue(res);
        }

        private void Execute(OpCodes.xor i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => a ^ b);
        }

        private void Execute(OpCodes.xorbi i)
        {
            m_registers[i.RT].Value = ALUByte(m_registers[i.RA].Value, new RegisterValue(i.I10 & 0xff), m_registers[i.RT].Value, (a, b, c, carry) => (uint)(a ^ b));
        }

        private void Execute(OpCodes.xorhi i)
        {
            m_registers[i.RT].Value = ALUHalfWord(m_registers[i.RA].Value, new RegisterValue((i.I10 & 0x200) != 0 ? 0xfc00 : 0x0000 | i.I10), m_registers[i.RT].Value, (a, b, c, carry) => (uint)(a ^ b));
        }

        private void Execute(OpCodes.xori i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, new RegisterValue((i.I10 & 0x200) != 0 ? 0xfffffc00 : 0x00000000 | i.I10), m_registers[i.RT].Value, (a, b, c, carry) => (uint)(a ^ b));
        }

        private void Execute(OpCodes.nand i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => ~(a & b));
        }

        private void Execute(OpCodes.nor i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => ~(a | b));
        }

        private void Execute(OpCodes.eqv i)
        {
            m_registers[i.RT].Value = ALUWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => (a ^ (~b)));
        }

        private void Execute(OpCodes.selb i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RC].Value, (a, b, c, carry) => ((c & b) | ((~c) & a)));
        }

        /*private void Execute(OpCodes.shufb i)
        {
            m_registers[i.RT].Value = ALUDoubleWord(m_registers[i.RA].Value, m_registers[i.RB].Value, m_registers[i.RT].Value, (a, b, c, carry) => );
        }*/
    }
}
