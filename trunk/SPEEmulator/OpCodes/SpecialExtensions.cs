using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator.OpCodes
{
    partial class hbr
    {
        /// <summary>
        /// Gets or sets source register A
        /// </summary>
        public uint RA
        {
            get { return (m_value >> REGISTER_SIZE) & REGISTER_MASK; }
            set
            {
                System.Diagnostics.Trace.Assert((value & REGISTER_MASK) == 0);
                m_value |= (value & REGISTER_MASK) << REGISTER_SIZE;
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
                System.Diagnostics.Trace.Assert((value & REGISTER_MASK) == 0);
                m_value |= value & REGISTER_MASK;
            }
        }

        /// <summary>
        /// Gets or sets high relative offset
        /// </summary>
        public uint ROH
        {
            get { return (m_value >> (REGISTER_SIZE * 2)) & 0x3; }
            set
            {
                System.Diagnostics.Trace.Assert((value & 0x3) == 0);
                m_value |= (value & 0x3) << (REGISTER_SIZE * 2);
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
                System.Diagnostics.Trace.Assert((value & 0x1ff) == 0);
                ROL = value & REGISTER_MASK;
                ROH = (value >> REGISTER_SIZE) & 0x3f;
            }
        }

        /// <summary>
        /// Gets or sets the inline prefetch flag
        /// </summary>
        public bool P
        {
            get { return (m_value & 0x100000) != 0; }
            set
            {
                if (value)
                    m_value |= 0x100000;
                else
                    m_value &= 0x100000u;
            }
        }

        public override string ToString()
        {
            return base.ToString() + string.Format(" ${0}, RO=0x{1:x8}, P={2}", this.RA, this.RO, this.P);
        }
    }

    partial class stop
    {
        /// <summary>
        /// Gets or sets the Stop and Signal Type
        /// </summary>
        public uint StopAndSignalType
        {
            get { return m_value & 0x3fff; }
            set
            {
                System.Diagnostics.Trace.Assert((value & 0x3fff) == 0);
                m_value |= value & 0x3fff;
            }
        }

        public override string ToString()
        {
            return base.ToString() + string.Format("0x{0:x4}", this.StopAndSignalType);
        }
    }

    partial class sync
    {
        /// <summary>
        /// Gets or sets the channel syncronization bit
        /// </summary>
        public bool ChannelSynchronization
        {
            get { return (m_value & 0x200000) != 0; }
            set
            {
                if (value)
                    m_value |= 0x200000;
                else
                    m_value &= ~0x200000u;
            }
        }

        public override string ToString()
        {
            return base.ToString() + string.Format(" ChannelSync={0}", this.ChannelSynchronization);
        }

    }
}
