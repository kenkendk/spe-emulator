using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator.OpCodes.Bases
{
    /// <summary>
    /// R based layout is for instructions with one source registers and one target register,
    /// using 11 bits for the opcode, seven bits are unused.
    /// This instruction class is not named in the documentation.
    /// </summary>
    class R : R0
    {
        /// <summary>
        /// Constructs an R base
        /// </summary>
        /// <param name="bitpattern">The OpCode bitpattern</param>
        protected R(string bitpattern)
            : base(bitpattern)
        {
        }

        /// <summary>
        /// Constructs an R base
        /// </summary>
        /// <param name="bitpattern">The OpCode bitpattern</param>
        protected R(string bitpattern, int opcodesize)
            : base(bitpattern, opcodesize)
        {
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


        public override string ToString()
        {
            return base.ToString() + ", RA: " + RA.ToString() + " (" + ((int)RA).ToString() + ")";
        }
    }
}
