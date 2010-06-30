using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator.OpCodes.Bases
{
    /// <summary>
    /// RI16 based layout is for 16 bit litterals with with a target register, 
    /// using an 9 bit opcode
    /// </summary>
    class RI16 : R0
    {
        /// <summary>
        /// Constructs an RI16 base
        /// </summary>
        /// <param name="bitpattern">The OpCode bitpattern</param>
        protected RI16(string bitpattern)
            : base(bitpattern, 9)
        {
        }

        /// <summary>
        /// Gets or sets the literal
        /// </summary>
        public uint I16
        {
            get { return (m_value >> REGISTER_SIZE) & 0xffff; }
            set
            {
                System.Diagnostics.Trace.Assert((value & ~0xffff) == 0);
                m_value &= ~((uint)0xffff << REGISTER_SIZE);
                m_value |= (value & 0xffff) << REGISTER_SIZE;
            }
        }

        public override string ToString()
        {
            return base.ToString() + ", I16=" + I16.ToString() + " (" + ((short)I16).ToString() + ")";
        }
    }
}
