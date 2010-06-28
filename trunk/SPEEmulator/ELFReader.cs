using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace SPEEmulator
{
    public class ELFReader
    {
        private static readonly byte[] MAGIC_HEADER = new byte[] { 0x7f, (byte)'E', (byte)'L', (byte)'F' };

        private Stream m_stream;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct ELF32Header
        {
            public byte magic1;
            public byte magic2;
            public byte magic3;
            public byte magic4;
            public byte @class;
            public byte data_enc;
            public byte mversion;
            
            public byte padding;
            
            //8 bytes more
            public ulong dummy;

            public ushort type;
            public ushort machine;
            public uint version;
            public uint entry; //Adr
            public uint phoff; //Off
            public uint shoff; //Off
            public uint flags;
            public ushort ehsize;
            public ushort phentsize;
            public ushort phnum;
            public ushort shentsize;
            public ushort shnum;
            public ushort shstrndx;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct SectionHeader
        {
            public uint name;
            public uint type;
            public uint flags;
            public uint addr; //Adr
            public uint offset; //Off
            public uint size;
            public uint link;
            public uint info;
            public uint addralign;
            public uint entsize;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct SymbolTableEntry
        {
            public uint name;
            public uint value; //adr
            public uint size;
            public byte info;
            public byte other;
            public ushort shndx;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct ProgramHeader
        {
            public uint type;
            public uint offset; //off
            public uint vaddr; //adr
            public uint paddr; //adr
            public uint filesz;
            public uint memsz;
            public uint flags;
            public uint align;
        }

        private T ReadStruct<T>()
        {
            GCHandle handle = new GCHandle();
            try
            {
                handle = GCHandle.Alloc(ReadBlock(Marshal.SizeOf(typeof(T))), GCHandleType.Pinned);
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }
        }

        private byte[] ReadBlock(int size)
        {
            byte[] tmp = new byte[size];
            int read = 0;
            int a;
            while (read < size)
                if ((a = m_stream.Read(tmp, read, size - read)) == 0)
                    throw new InvalidDataException("Premature EOF");
                else
                    read += a;

            return tmp;
        }

        private ELF32Header m_header;
        private ProgramHeader[] m_progheaders;
        private SectionHeader[] m_sectionheaders;
        
        private byte[][] m_programsections;
        private byte[][] m_sectionsections;

        public ELFReader(Stream s)
        {
            m_stream = s;
            m_header = ReadStruct<ELF32Header>();

            if (m_header.magic1 != MAGIC_HEADER[0] || m_header.magic2 != MAGIC_HEADER[1] || m_header.magic3 != MAGIC_HEADER[2] || m_header.magic4 != MAGIC_HEADER[3])
                throw new InvalidDataException("Invalid ELF magic header");

            if (m_header.@class != 1)
                throw new Exception("ELF class must be ELF32");
            if (m_header.mversion != 1)
                throw new Exception("ELF ident version must be 1");

            bool convertEndian = (BitConverter.IsLittleEndian ? 1 : 2) != m_header.data_enc;
            if (convertEndian)
                m_header = FixEndian(m_header);

            if (m_header.type != 2)
                throw new Exception("ELF must be an executable");
            if (m_header.machine != 23)
                throw new Exception("ELF must be for SPU arch");
            if (m_header.version != 1)
                throw new Exception("ELF version must be 1");
            if (m_header.flags != 0)
                throw new Exception("ELF flags must be 0");

            m_progheaders = new ProgramHeader[m_header.phnum];
            for (int i = 0; i < m_progheaders.Length; i++)
            {
                m_stream.Seek(m_header.phoff + (i * m_header.phentsize), SeekOrigin.Begin);
                m_progheaders[i] = ReadStruct<ProgramHeader>();
                if (convertEndian)
                    m_progheaders[i] = FixEndian(m_progheaders[i]);
            }

            m_sectionheaders = new SectionHeader[m_header.shnum];
            for (int i = 0; i < m_sectionheaders.Length; i++)
            {
                m_stream.Seek(m_header.shoff + (i * m_header.shentsize), SeekOrigin.Begin);
                m_sectionheaders[i] = ReadStruct<SectionHeader>();
                if (convertEndian)
                    m_sectionheaders[i] = FixEndian(m_sectionheaders[i]);
            }

            m_programsections = new byte[m_progheaders.Length][];
            for (int i = 0; i < m_programsections.Length; i++)
            {
                m_stream.Seek(m_progheaders[i].offset, SeekOrigin.Begin);
                m_programsections[i] = ReadBlock((int)m_progheaders[i].filesz);
            }

            m_sectionsections = new byte[m_sectionheaders.Length][];
            for (int i = 0; i < m_sectionheaders.Length; i++)
            {
                m_stream.Seek(m_sectionheaders[i].offset, SeekOrigin.Begin);
                m_sectionsections[i] = ReadBlock((int)m_sectionheaders[i].size);
            }
           
        }

        public void SetupExecutionEnv(SPEProcessor spe)
        {
            Array.Clear(spe.LS, 0, spe.LS.Length);

            spe.SPU.PC = m_header.entry; 
            for (int i = 0; i < m_progheaders.Length; i++)
                if (m_progheaders[i].type == 1)
                {
                    Array.Copy(m_programsections[i], 0, spe.LS, m_progheaders[i].vaddr, m_programsections[i].Length);
                }
        }

        private T FixEndian<T>(T item) where T : struct
        {
            //Box it
            object hbox = item;

            foreach (System.Reflection.FieldInfo fi in typeof(T).GetFields())
                if (fi.FieldType == typeof(ushort))
                {
                    byte[] tmp = BitConverter.GetBytes((ushort)fi.GetValue(hbox));
                    tmp = tmp.Reverse().ToArray();
                    fi.SetValue(hbox, BitConverter.ToUInt16(tmp, 0));
                }
                else if (fi.FieldType == typeof(uint))
                {
                    byte[] tmp = BitConverter.GetBytes((uint)fi.GetValue(hbox));
                    tmp = tmp.Reverse().ToArray();
                    fi.SetValue(hbox, BitConverter.ToUInt32(tmp, 0));
                }
                else if (fi.FieldType == typeof(ulong))
                {
                    byte[] tmp = BitConverter.GetBytes((ulong)fi.GetValue(hbox));
                    tmp = tmp.Reverse().ToArray();
                    fi.SetValue(hbox, BitConverter.ToUInt64(tmp, 0));
                }

            //Unbox
            return (T)hbox;
        }
    }
}
