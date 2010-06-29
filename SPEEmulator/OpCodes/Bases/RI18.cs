using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator.OpCodes.Bases
{
    /// <summary>
    /// RI16 based layout is for 18 bit litterals with with a target register, 
    /// using an 7 bit opcode
    /// </summary>
    class RI18 : R0
    {
        /// <summary>
        /// Constructs an RI18 base
        /// </summary>
        /// <param name="bitpattern">The OpCode bitpattern</param>
        protected RI18(string bitpattern)
            : base(bitpattern, 7)
        {
        }

        /// <summary>
        /// Gets or sets the literal
        /// </summary>
        public uint I18
        {
            get { return (m_value >> REGISTER_SIZE) & 0x3ffff; }
            set
            {
                System.Diagnostics.Trace.Assert((value & ~0x3ffff) == 0);
                m_value &= ~((uint)0x3ffff << REGISTER_SIZE);
                m_value |= (value & 0x3ffff) << REGISTER_SIZE;
            }
        }

        public override string ToString()
        {
            return base.ToString() + ", I18: " + I18.ToString() + "(" + ((int)I18).ToString() + ")";
        }
    }
}
