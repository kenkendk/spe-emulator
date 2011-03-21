using System;
namespace SPEEmulator
{
    public interface IEndianBitConverter
    {
        void WriteBytes(uint offset, byte[] data);
        void ReadBytes(uint offset, byte[] data);
        void WriteBytes(byte[] data);
        void ReadBytes(byte[] data);

        uint Position { get; set; }
        byte ReadByte();
        byte ReadByte(uint offset);
        double ReadDouble();
        double ReadDouble(uint offset);
        float ReadFloat();
        float ReadFloat(uint offset);
        uint ReadUInt();
        uint ReadUInt(uint offset);
        ulong ReadULong();
        ulong ReadULong(uint offset);
        ushort ReadUShort();
        ushort ReadUShort(uint offset);
        void WriteByte(byte value);
        void WriteByte(uint offset, byte value);
        void WriteDouble(double value);
        void WriteDouble(uint offset, double value);
        void WriteFloat(float value);
        void WriteFloat(uint offset, float value);
        void WriteUInt(uint offset, uint value);
        void WriteUInt(uint value);
        void WriteULong(uint offset, ulong value);
        void WriteULong(ulong value);
        void WriteUShort(ushort value);
        void WriteUShort(uint offset, ushort value);
    }
}
