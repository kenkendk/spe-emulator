using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator.OpCodes.Bases
{
    /// <summary>
    /// RInt based layout is for instructions with one source registers and one target register,
    /// using 11 bits for the opcode, some bits are used to toggle interrupts.
    /// This instruction class is not named in the documentation.
    /// </summary>
    class RInt : R
    {
        /// <summary>
        /// Constructs an RInt base
        /// </summary>
        /// <param name="bitpattern">The OpCode bitpattern</param>
        public RInt(string bitpattern)
            : base(bitpattern)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating if interrupts are enabled at target
        /// </summary>
        public bool E
        {
            get { return (m_value & 0x40000) != 0; }
            set
            {
                if (value)
                    m_value |= 0x40000;
                else
                    m_value &= ~0x40000u;
            }

        }

        /// <summary>
        /// Gets or sets a value indicating if interrupts are disabled at target
        /// </summary>
        public bool D
        {
            get { return (m_value & 0x80000) != 0; }
            set
            {
                if (value)
                    m_value |= 0x80000;
                else
                    m_value &= ~0x80000u;
            }
        }

        public override string ToString()
        {
            return Mnemonic.ToString() + " " + RA.ToString() + " " + RT.ToString();
        }

    }
}
