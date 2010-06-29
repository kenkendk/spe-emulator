using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator.OpCodes.Bases
{
    /// <summary>
    /// RRR based layout is for instructions with three source registers and one target register,
    /// using 4 bits for the opcode
    /// </summary>
    class RRR : Instruction
    {
        /// <summary>
        /// Constructs an RRR base
        /// </summary>
        /// <param name="bitpattern">The OpCode bitpattern</param>
        protected RRR(string bitpattern)
            : base(bitpattern)
        {
            System.Diagnostics.Trace.Assert(bitpattern.Replace(" ", "").Length == 4);
        }

        /// <summary>
        /// Gets or sets register target
        /// </summary>
        public uint RT
        {
            get { return (m_value >> (REGISTER_SIZE * 3)) & REGISTER_MASK; }
            set
            {
                System.Diagnostics.Trace.Assert((value & ~REGISTER_MASK) == 0);
                m_value &= ~((uint)REGISTER_MASK << (REGISTER_SIZE * 3));
                m_value |= (value & REGISTER_MASK) << (REGISTER_SIZE * 3);
            }
        }

        /// <summary>
        /// Gets or sets source register B
        /// </summary>
        public uint RB
        {
            get { return (m_value >> (REGISTER_SIZE * 2)) & REGISTER_MASK; }
            set
            {
                System.Diagnostics.Trace.Assert((value & ~REGISTER_MASK) == 0);
                m_value &= ~((uint)REGISTER_MASK << (REGISTER_SIZE * 2));
                m_value |= (value & REGISTER_MASK) << (REGISTER_SIZE * 2);
            }
        }

        /// <summary>
        /// Gets or sets source register A
        /// </summary>
        public uint RA
        {
            get { return (m_value >> REGISTER_SIZE) & REGISTER_MASK; }
            set
            {
                System.Diagnostics.Trace.Assert((value & ~REGISTER_MASK) == 0);
                m_value &= ~((uint)REGISTER_MASK << REGISTER_SIZE);
                m_value |= (value & REGISTER_MASK) << REGISTER_SIZE;
            }
        }

        /// <summary>
        /// Gets or sets source register c
        /// </summary>
        public uint RC
        {
            get { return m_value & REGISTER_MASK; }
            set
            {
                System.Diagnostics.Trace.Assert((value & ~REGISTER_MASK) == 0);
                m_value &= ~((uint)REGISTER_MASK);
                m_value |= value & REGISTER_MASK;
            }
        }

        public override string ToString()
        {
            return base.ToString() + ", RT: " + RT.ToString() + "(" + ((int)RT).ToString() + ")" + ", RA: " + RA.ToString() + "(" + ((int)RA).ToString() + ")" + ", RB: " + RB.ToString() + "(" + ((int)RB).ToString() + ")" + ", RC: " + RC.ToString() + "(" + ((int)RC).ToString() + ")";
        }
    }
}
