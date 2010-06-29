using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator.OpCodes.Bases
{
    /// <summary>
    /// R0 based layout is for instructions with oone target register,
    /// using 11 bits for the opcode, fourteen bits are unused.
    /// This instruction class is not named in the documentation.
    /// </summary>
    class R0 : Instruction
    {
        /// <summary>
        /// Constructs an R base
        /// </summary>
        /// <param name="bitpattern">The OpCode bitpattern</param>
        protected R0(string bitpattern)
            : this(bitpattern, 11)
        {
        }

        /// <summary>
        /// Constructs an R base
        /// </summary>
        /// <param name="bitpattern">The OpCode bitpattern</param>
        /// <param name="opcodesize">The size of the opcode in bits</param>
        protected R0(string bitpattern, int opcodesize)
            : base(bitpattern)
        {
            System.Diagnostics.Trace.Assert(bitpattern.Replace(" ", "").Length == opcodesize);
        }

        /// <summary>
        /// Gets or sets register target
        /// </summary>
        public uint RT
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
            return base.ToString() + ", RT: " + RT.ToString() + " (" + ((int)RT).ToString() + ")";
        }
    }
}
