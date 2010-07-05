using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator
{
    /// <summary>
    /// Represents a single SPU register, 128 bits wide
    /// </summary>
    class Register
    {
        private RegisterValue m_value = new RegisterValue();

        /// <summary>
        /// Gets or sets the current register value
        /// </summary>
        public RegisterValue Value 
        {
            get { return m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// Gets or sets the register byte value from the preferred slot
        /// </summary>
        public byte Byte
        {
            get { return (byte)((m_value.high >> 32) & 0xff); }
            set 
            {
                m_value.high &= ~(0xfful << 32);
                m_value.high |= ((ulong)value << 32);
            }
        }

        /// <summary>
        /// Gets or sets the register halfword value from the preferred slot
        /// </summary>
        public ushort Halfword
        {
            get { return (ushort)((m_value.high >> 32) & 0xffff); }
            set
            {
                m_value.high &= ~(0xfffful << 32);
                m_value.high |= ((ulong)value << 32);
            }
        }

        /// <summary>
        /// Gets or sets the register word value from the preferred slot
        /// </summary>
        public uint Word
        {
            get { return (uint)((m_value.high >> 32) & 0xffffffff); }
            set
            {
                m_value.high &= ~(0xffffffff << 32);
                m_value.high |= ((ulong)value << 32);
            }
        }

        /// <summary>
        /// Gets or sets the register doubleword value from the preferred slot
        /// </summary>
        public ulong Doubleword
        {
            get { return m_value.high; }
            set { m_value.high = value; }
        }
    }

    /*
    static class Extension
    {
        public static int GetBit(this byte b, int offset)
        {
            if (offset < 0 || offset > 7)
                throw new ArgumentOutOfRangeException("offset");

            return (b >> (7 - offset)) & 0x1;
        }

        public static byte SetBit(this byte b, int offset, int value)
        {
            if (offset < 0 || offset > 7)
                throw new ArgumentOutOfRangeException("offset");
            if (value != 0 && value != 1)
                throw new ArgumentOutOfRangeException("value");

            return (b ^= (byte)((value % 2) << (7 - offset)));
        }
    }
    */
    
    /// <summary>
    /// Represents an SPU register value,
    /// emulated by using 16 byte values
    /// </summary>
    class RegisterValue
    {
        private byte[] m_value = new byte[16];

        public byte[] Value { get { return m_value; } }

        /*
        public int getBit(int bytes, int offset)
        {
            return m_value[((bytes * 8) - offset) / 8].GetBit(((bytes * 8) - offset) % 8);
        }

        public void setBit(int bytes, int offset, int value)
        {
            m_value[((bytes * 8) - offset) / 8].SetBit(((bytes * 8) - offset) % 8, value);
        }
        */

        public bool Negative
        {
            get { return (m_value[0] & 0x80) != 0; }
            set
            {
                if (value)
                    m_value[0] |= 0x80;
                else
                    m_value[0] &= 0x7f;
            }
        }

        public ulong low
        {
            get 
            { 
                return 
                    (ulong)m_value[15] | 
                    ((ulong)m_value[14] << 8) | 
                    ((ulong)m_value[13] << (8*2)) | 
                    ((ulong)m_value[12] << (8*3)) |
                    ((ulong)m_value[11] << (8*4)) |
                    ((ulong)m_value[10] << (8*5)) | 
                    ((ulong)m_value[9] << (8*6)) | 
                    ((ulong)m_value[8] << (8*7)); 
            }
            set
            {
                m_value[15] = (byte)(value & 0xff);
                m_value[14] = (byte)((value >> 8) & 0xff);
                m_value[13] = (byte)((value >> (8 * 2)) & 0xff);
                m_value[12] = (byte)((value >> (8 * 3)) & 0xff);
                m_value[11] = (byte)((value >> (8 * 4)) & 0xff);
                m_value[10] = (byte)((value >> (8 * 5)) & 0xff);
                m_value[9] = (byte)((value >> (8 * 6)) & 0xff);
                m_value[8] = (byte)((value >> (8 * 7)) & 0xff);
            }
        }

        public ulong high
        {
            get
            {
                return
                    (ulong)m_value[7] |
                    ((ulong)m_value[6] << 8) |
                    ((ulong)m_value[5] << (8 * 2)) |
                    ((ulong)m_value[4] << (8 * 3)) |
                    ((ulong)m_value[3] << (8 * 4)) |
                    ((ulong)m_value[2] << (8 * 5)) |
                    ((ulong)m_value[1] << (8 * 6)) |
                    ((ulong)m_value[0] << (8 * 7));
            }
            set
            {
                m_value[7] = (byte)(value & 0xff);
                m_value[6] = (byte)((value >> 8) & 0xff);
                m_value[5] = (byte)((value >> (8 * 2)) & 0xff);
                m_value[4] = (byte)((value >> (8 * 3)) & 0xff);
                m_value[3] = (byte)((value >> (8 * 4)) & 0xff);
                m_value[2] = (byte)((value >> (8 * 5)) & 0xff);
                m_value[1] = (byte)((value >> (8 * 6)) & 0xff);
                m_value[0] = (byte)((value >> (8 * 7)) & 0xff);
            }
        }

        public RegisterValue()
            : this(0)
        {
        }

        public RegisterValue(RegisterValue r)
            : this(r.high, r.low)
        {
        }

        public RegisterValue(ulong value)
        {
            this.high = 0;
            this.low = value;
        }

        public RegisterValue(ulong h, ulong l)
        {
            low = l;
            high = h;
        }

        public override string ToString()
        {
            return string.Format("0x{0:x16}{1:x16}", high, low);
        }

        public override bool Equals(object obj)
        {
            if (obj is RegisterValue)
                return ((RegisterValue)obj).high == this.high && ((RegisterValue)obj).low == this.low;
            else
                return false;
        }

        public static RegisterValue operator +(RegisterValue a, RegisterValue b)
        {
            int carry = 0;
            RegisterValue rx = new RegisterValue();
            for(int i = rx.m_value.Length - 1; i > 0; i--)
            {
                carry = a.m_value[i] + b.m_value[i] + carry;
                rx.m_value[i] = (byte)(carry & 0xff);
                carry = carry >> 8;
            }

            return rx;
        }

        public static RegisterValue operator -(RegisterValue a, RegisterValue b)
        {
            uint borrow = 0;
            RegisterValue rx = new RegisterValue();
            for (int i = rx.m_value.Length - 1; i > 0; i--)
            {
                uint totalSub;
                totalSub = b.m_value[i] + borrow;

                if (a.m_value[i] < totalSub)
                {
                    rx.m_value[i] = (byte)((a.m_value[i] + 0x100) - totalSub);
                    borrow = 1;
                }
                else
                {
                    rx.m_value[i] = (byte)(a.m_value[i] - totalSub);
                    borrow = 0;
                }
            }

            return rx;
        }

        public static RegisterValue operator *(RegisterValue a, RegisterValue b)
        {
            RegisterValue rx = new RegisterValue();
            for (int i = a.m_value.Length - 1; i > 0; i--)
            {
                uint carry = 0;
                for (int j = b.m_value.Length - 1; j > 0; j--)
                {
                    if (i + j < rx.m_value.Length)
                    {
                        carry += ((uint)a.m_value[i] * b.m_value[j]) + rx.m_value[i + j];

                        rx.m_value[i + j] = (byte)(carry & 0xFF);
                        carry = carry >> 8;
                    }
                }

                if (i + b.m_value.Length < rx.m_value.Length) 
                    rx.m_value[i + b.m_value.Length] = (byte)carry;
            }

            return rx;
        }

        public static RegisterValue operator /(RegisterValue a, RegisterValue b)
        {
            throw new MissingMethodException();
        }

        public static RegisterValue operator %(RegisterValue a, RegisterValue b)
        {
            throw new MissingMethodException();
        }

        public static RegisterValue operator &(RegisterValue a, RegisterValue b)
        {
            RegisterValue rx = new RegisterValue();
            for (int i = 0; i < rx.m_value.Length; i++)
                rx.m_value[i] = (byte)(a.m_value[i] & b.m_value[i]);

            return rx;
        }

        public static RegisterValue operator |(RegisterValue a, RegisterValue b)
        {
            RegisterValue rx = new RegisterValue();
            for (int i = 0; i < rx.m_value.Length; i++)
                rx.m_value[i] = (byte)(a.m_value[i] | b.m_value[i]);

            return rx;
        }

        public static RegisterValue operator >>(RegisterValue value, int count)
        {
            int partials = count % 8;
            int fulls = count - partials;

            RegisterValue rx = new RegisterValue();
            for (int i = 0; i < rx.m_value.Length; i++)
                if (i + fulls < rx.m_value.Length)
                    rx.m_value[i + fulls] = value.m_value[i];

            if (partials != 0)
                for (int i = rx.m_value.Length - 1; i > 0; i--)
                {
                    rx.m_value[i] = (byte)(rx.m_value[i] >> partials);
                    if (i != rx.m_value.Length)
                        rx.m_value[i] |= (byte)(rx.m_value[i - 1] << (8 - partials));
                }

            return rx;
        }

        public static RegisterValue operator <<(RegisterValue value, int count)
        {
            int partials = count % 8;
            int fulls = count - partials;

            RegisterValue rx = new RegisterValue();
            for (int i = 0; i < rx.m_value.Length; i++)
                if (i - fulls > 0)
                    rx.m_value[i - fulls] = value.m_value[i];

            if (partials != 0)
                for (int i = 0; i < rx.m_value.Length; i++)
                {
                    rx.m_value[i] = (byte)(rx.m_value[i] << partials);
                    if (i != 0)
                        rx.m_value[i] |= (byte)(rx.m_value[i + 1] << (8 - partials));
                }

            return rx;
        }

        public static RegisterValue operator +(RegisterValue value)
        {
            throw new MissingMethodException();
        }

        public static RegisterValue operator -(RegisterValue value)
        {
            throw new MissingMethodException();
        }

        public static RegisterValue operator !(RegisterValue value)
        {
            return new RegisterValue(0ul, value.high != 0 || value.low != 0 ? 1ul : 0ul);
        }

        public static RegisterValue operator ~(RegisterValue value)
        {
            RegisterValue rx = new RegisterValue();
            for (int i = 0; i < rx.m_value.Length; i++)
                rx.m_value[i] = (byte)(~value.m_value[i]);

            return rx;
        }

        public static RegisterValue operator ++(RegisterValue value)
        {
            int carry = 1;
            RegisterValue rx = new RegisterValue();
            for (int i = rx.m_value.Length - 1; i > 0; i--)
            {
                carry = value.m_value[i] + carry;
                rx.m_value[i] = (byte)(carry & 0xff);
                carry = carry >> 8;
                if (carry == 0)
                    break;
            }

            return rx;
        }

        public static RegisterValue operator --(RegisterValue value)
        {
            return value - new RegisterValue(1);
        }

        public static bool operator true(RegisterValue value)
        {
            throw new MissingMethodException();
        }

        public static bool operator false(RegisterValue value)
        {
            throw new MissingMethodException();
        }
    }
}
