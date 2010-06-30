using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator.OpCodes.Bases
{
    /// <summary>
    /// RI10 based layout is for 10 bit litterals with with a single source and target register, 
    /// using an 8 bit opcode
    /// </summary>
    class RI10 : R
    {
        /// <summary>
        /// Constructs the an RR base
        /// </summary>
        /// <param name="bitpattern">The OpCode bitpattern</param>
        protected RI10(string bitpattern)
            : base(bitpattern, 8)
        {
        }

        /// <summary>
        /// Gets or sets the literal
        /// </summary>
        public uint I10
        {
            get { return (m_value >> (REGISTER_SIZE * 2)) & 0x3ff; }
            set
            {
                System.Diagnostics.Trace.Assert((value & ~0x3ff) == 0);
                m_value &= ~((uint)0x3ff << (REGISTER_SIZE * 2));
                m_value |= (value & 0x3ff) << (REGISTER_SIZE * 2);
            }
        }

        public override string ToString()
        {
            return base.ToString() + ", I10: " + I10.ToString() + "(" + ((short)(((I10 & 0x200) != 0 ? 0xfffffc00 : 0x00000000) | I10)).ToString() + ")";
        }
    }
}
