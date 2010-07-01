using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator.OpCodes.Bases
{
    /// <summary>
    /// RBranch based layout is for instructions with one source registers and one target register,
    /// using 11 bits for the opcode, some bits are used for relative offset.
    /// This branch class is not named in the documentation.
    /// </summary>
    class RBranch : Instruction
    {
        /// <summary>
        /// Constructs an RBranch base
        /// </summary>
        /// <param name="bitpattern">The OpCode bitpattern</param>
        public RBranch(string bitpattern)
            : base(bitpattern)
        {
            System.Diagnostics.Trace.Assert(bitpattern.Replace(" ", "").Length == 7);
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

        /// <summary>
        /// Gets or sets low relative offset
        /// </summary>
        public uint ROL
        {
            get { return m_value & REGISTER_MASK; }
            set
            {
                System.Diagnostics.Trace.Assert((value & ~REGISTER_MASK) == 0);
                m_value &= ~((uint)REGISTER_MASK);
                m_value |= value & REGISTER_MASK;
            }
        }

        /// <summary>
        /// Gets or sets high relative offset
        /// </summary>
        public uint ROH
        {
            get { return (m_value >> (REGISTER_SIZE + 16)) & 0x3; }
            set
            {
                System.Diagnostics.Trace.Assert((value & ~0x3) == 0);
                m_value &= ~((uint)0x3 << (REGISTER_SIZE + 16));
                m_value |= (value & 0x3) << (REGISTER_SIZE + 16);
            }
        }

        /// <summary>
        /// Gets or sets the relative offset
        /// </summary>
        public uint RO
        {
            get { return (ROH << REGISTER_SIZE) | ROL; }
            set
            {
                System.Diagnostics.Trace.Assert((value & ~0x1ff) == 0);
                ROL = value & REGISTER_MASK;
                ROH = (value >> REGISTER_SIZE) & 0x3f;
            }
        }

        public override string ToString()
        {
            return Mnemonic.ToString() + " I16=" + I16.ToString() + " (" + ((int)I16).ToString() + "), RO: " + RO.ToString() + "(" + ((int)RO).ToString() + ")";
        }
    }
}
