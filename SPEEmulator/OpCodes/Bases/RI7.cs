using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator.OpCodes.Bases
{
    /// <summary>
    /// RI10 based layout is for 7 bit litterals with with a single source and target register, 
    /// using an 11 bit opcode
    /// </summary>
    class RI7 : R
    {
        /// <summary>
        /// Constructs an RI7 base
        /// </summary>
        /// <param name="bitpattern">The OpCode bitpattern</param>
        protected RI7(string bitpattern)
            : base(bitpattern)
        {
        }

        /// <summary>
        /// Gets or sets the literal
        /// </summary>
        public uint I7
        {
            get { return (m_value >> (REGISTER_SIZE * 2)) & 0x7f; }
            set
            {
                System.Diagnostics.Trace.Assert((value & ~0x7f) == 0);
                m_value &= ~((uint)0x7f << (REGISTER_SIZE * 2));
                m_value |= (value & 0x7f) << (REGISTER_SIZE * 2);
            }
        }
    }
}
