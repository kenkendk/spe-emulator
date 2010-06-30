using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator.OpCodes.Bases
{
    /// <summary>
    /// RI8 based layout is for 8 bit litterals with with a single source and target register, 
    /// using a 10 bit opcode.
    /// This instruction class is not named in the documentation.
    /// </summary>
    class RI8 : R
    {
        /// <summary>
        /// Constructs an RI8 base
        /// </summary>
        /// <param name="bitpattern">The OpCode bitpattern</param>
        public RI8(string bitpattern)
            : base(bitpattern, 10)
        {
        }

        /// <summary>
        /// Gets or sets the literal
        /// </summary>
        public uint I8
        {
            get { return (m_value >> (REGISTER_SIZE * 2)) & 0xff; }
            set
            {
                System.Diagnostics.Trace.Assert((value & ~0xff) == 0);
                m_value &= ~((uint)0xff << (REGISTER_SIZE * 2));
                m_value |= (value & 0xff) << (REGISTER_SIZE * 2);
            }
        }

        public override string ToString()
        {
            return base.ToString() + ", I8=" + I8.ToString() + " (" + ((int)(((I8 & 0x80) != 0 ? 0xffffff00 : 0x00000000) | I8)).ToString() + ")";
        }
    }
}
