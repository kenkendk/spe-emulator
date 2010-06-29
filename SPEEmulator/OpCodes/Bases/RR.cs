using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator.OpCodes.Bases
{
    /// <summary>
    /// RR based layout is for instructions with two source registers and one target register,
    /// using 11 bits for the opcode
    /// </summary>
    class RR : R
    {
        /// <summary>
        /// Constructs an RR base
        /// </summary>
        /// <param name="bitpattern">The OpCode bitpattern</param>
        protected RR(string bitpattern)
            : base(bitpattern)
        {
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

        public override string ToString()
        {
            return base.ToString() + ", RB: " + RB.ToString() + "(" + ((int)RB).ToString() + ")";
        }
    }
}
