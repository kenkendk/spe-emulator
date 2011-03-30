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
        private static readonly byte[] SPU_NOTE = new byte[] { 
            0, 0, 0, 0x8, 
            0, 0, 0, 0xc,
            0, 0, 0, 0x1,
            0x53, 0x50, 0x55, 0x4e, 0x41, 0x4d, 0x45, 0x00, //"SPUNAME\0"
            0x74, 0x65, 0x73, 0x74, 0x63, 0x6f, 0x6d, 0x70, 0x69, 0x6c, 0x65, 0x00 //"testcompile\0"
        };

        private static string SPU_NOTE_NAME = ".note.spu_name";
        private static string INIT_NAME = ".init";
        private static string SECTION_HEADER_STR_TABLENAME = ".shstrtab";

        private static readonly uint PROGRAMHEADER_SIZE = (uint)Marshal.SizeOf(typeof(ProgramHeader));
        private static readonly uint SECTIONHEADER_SIZE = (uint)Marshal.SizeOf(typeof(SectionHeader));

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

            public st_type Type { get { return (st_type)(info & 0x0f); } }
            public st_bind Bind { get { return (st_bind)(info >> 4); } }
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

        public enum st_type : byte
        {
            notype = 0,
            @object = 1,
            func = 2,
            section = 3,
            file = 4,
            loproc = 13,
            medproc = 14,
            hiproc = 15
        }

        public enum st_bind : byte
        {
            local = 0,
            global = 1,
            weak = 2,
            loproc = 13,
            medproc = 14,
            hiproc = 15
        }

        private static byte[] WriteStruct<T>(T item)
        {
            IntPtr data = IntPtr.Zero;
            try
            {
                int size = Marshal.SizeOf(typeof(T));
                byte[] result = new byte[size];
                data = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)));
                Marshal.StructureToPtr(item, data, false);
                Marshal.Copy(data, result, 0, size);
                return result;
            }
            finally
            {
                if (data != IntPtr.Zero)
                    Marshal.FreeHGlobal(data);
            }

        }

        private static T ReadStruct<T>(Stream s)
        {
            GCHandle handle = new GCHandle();
            try
            {
                handle = GCHandle.Alloc(ReadBlock(s, Marshal.SizeOf(typeof(T))), GCHandleType.Pinned);
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }
        }

        private static byte[] ReadBlock(Stream s, int size)
        {
            byte[] tmp = new byte[size];
            int read = 0;
            int a;
            while (read < size)
                if ((a = s.Read(tmp, read, size - read)) == 0)
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
        private SymbolTableEntry[] m_symbols;
        private int m_stringtableindex = -1;
        private int m_symboltableindex = -1;

        /// <summary>
        /// Writes a bare-metal ELF-SPE header for a code fragment of the given size
        /// </summary>
        /// <param name="codesize">The size of the following code block</param>
        /// <param name="entryOffset">The byte offset at which the start function resides</param>
        /// <param name="stream">The stream to write the header into</param>
        public static void EmitELFHeader(uint codesize, uint entryOffset, System.IO.Stream stream)
        {
            ELF32Header header = new ELF32Header();
            header.magic1 = MAGIC_HEADER[0];
            header.magic2 = MAGIC_HEADER[1];
            header.magic3 = MAGIC_HEADER[2];
            header.magic4 = MAGIC_HEADER[3];

            header.@class = 1;
            header.mversion = 1;

            header.data_enc = 2;
            header.type = 2;
            header.dummy = 0;
            header.ehsize = (ushort)Marshal.SizeOf(typeof(ELF32Header)); //52;
            header.entry = entryOffset;
            header.padding = 0;
            header.phnum = 2;
            header.phentsize = (ushort)(PROGRAMHEADER_SIZE);
            header.phoff = header.ehsize;
            header.shnum = 4;
            header.shentsize = (ushort)(SECTIONHEADER_SIZE);
            header.shoff = header.phoff + (uint)(header.phentsize * header.phnum);
            header.shstrndx = 3;

            header.machine = 23;
            header.version = 1;
            header.flags = 0;

            ProgramHeader[] progheaders = new ProgramHeader[header.phnum];
            SectionHeader[] sectionheaders = new SectionHeader[header.shnum];

            MemoryStream spu_note = new MemoryStream();
            MemoryStream sh_strings = new MemoryStream();

            uint first_area = header.shoff + (uint)(header.shentsize * header.shnum);

            while (first_area % 16 != 0)
            {
                first_area++;
                spu_note.WriteByte(0);
            }

            progheaders[0].align = 128;
            progheaders[0].filesz = codesize;
            progheaders[0].memsz = codesize;
            progheaders[0].offset = first_area;
            progheaders[0].paddr = 0;
            progheaders[0].vaddr = 0;
            progheaders[0].type = 1;
            progheaders[0].flags = 0x7;

            progheaders[1].align = 16;
            progheaders[1].filesz = (uint)SPU_NOTE.Length;
            progheaders[1].memsz = 0;
            progheaders[1].offset = first_area;
            progheaders[1].paddr = 0;
            progheaders[1].vaddr = 0;
            progheaders[1].type = 4;
            progheaders[1].flags = 0x4;
            
            spu_note.Write(SPU_NOTE, 0, SPU_NOTE.Length);
            first_area += (uint)SPU_NOTE.Length;

            //Null entry
            sectionheaders[0].name = 0;
            sectionheaders[0].type = 0;
            sectionheaders[0].addr = 0;
            sectionheaders[0].offset = 0;
            sectionheaders[0].size = 0;
            sectionheaders[0].entsize = 0;
            sectionheaders[0].flags = 0;
            sectionheaders[0].link = 0;
            sectionheaders[0].info = 0;
            sectionheaders[0].addralign = 0;

            sh_strings.WriteByte(0);

            //.init
            sectionheaders[1].name = (uint)sh_strings.Length;
            sectionheaders[1].type = 1;
            sectionheaders[1].addr = 0;
            sectionheaders[1].offset = progheaders[0].offset;
            sectionheaders[1].size = progheaders[0].filesz;
            sectionheaders[1].entsize = 0;
            sectionheaders[1].flags = 6;
            sectionheaders[1].link = 0;
            sectionheaders[1].info = 0;
            sectionheaders[1].addralign = 4;

            byte[] tmp = System.Text.Encoding.ASCII.GetBytes(INIT_NAME);
            sh_strings.Write(tmp, 0, tmp.Length);
            sh_strings.WriteByte(0);

            //.note
            sectionheaders[2].name = (uint)sh_strings.Length;
            sectionheaders[2].type = 7;
            sectionheaders[2].addr = 0;
            sectionheaders[2].offset = progheaders[1].offset;
            sectionheaders[2].size = progheaders[1].filesz;
            sectionheaders[2].entsize = 0;
            sectionheaders[2].flags = 0;
            sectionheaders[2].link = 0;
            sectionheaders[2].info = 0;
            sectionheaders[2].addralign = 16;

            tmp = System.Text.Encoding.ASCII.GetBytes(SPU_NOTE_NAME);
            sh_strings.Write(tmp, 0, tmp.Length);
            sh_strings.WriteByte(0);


            //.shstrtab
            sectionheaders[3].name = (uint)sh_strings.Length;

            tmp = System.Text.Encoding.ASCII.GetBytes(SECTION_HEADER_STR_TABLENAME);
            sh_strings.Write(tmp, 0, tmp.Length);
            sh_strings.WriteByte(0);

            sectionheaders[3].type = 3;
            sectionheaders[3].addr = 0;
            sectionheaders[3].offset = first_area;
            sectionheaders[3].size = (uint)sh_strings.Length;
            sectionheaders[3].entsize = 0;
            sectionheaders[3].flags = 0;
            sectionheaders[3].link = 0;
            sectionheaders[3].info = 0;
            sectionheaders[3].addralign = 1;

            first_area += sectionheaders[3].size;

            while (first_area % 16 != 0)
            {
                sh_strings.WriteByte(0);
                first_area++;
            }

            progheaders[0].offset = sectionheaders[1].offset = first_area;

            if (BitConverter.IsLittleEndian)
            {
                header = FixEndian(header);
                
            }

            tmp = WriteStruct(header);
            stream.Write(tmp, 0, tmp.Length);

            foreach (ProgramHeader ph in progheaders)
            {
                if (BitConverter.IsLittleEndian)
                    tmp = WriteStruct(FixEndian(ph));
                else
                    tmp = WriteStruct(ph);
                stream.Write(tmp, 0, tmp.Length);
            }

            foreach (SectionHeader sh in sectionheaders)
            {
                if (BitConverter.IsLittleEndian)
                    tmp = WriteStruct(FixEndian(sh));
                else
                    tmp = WriteStruct(sh);
                stream.Write(tmp, 0, tmp.Length);
            }

            spu_note.Position = 0;
            spu_note.CopyTo(stream);

            sh_strings.Position = 0;
            sh_strings.CopyTo(stream);
        }

        private static byte[] CreateAsciizString(string value)
        {
            byte[] tmp = new byte[System.Text.Encoding.ASCII.GetByteCount(value) + 1];
            System.Text.Encoding.ASCII.GetBytes(value, 0, value.Length, tmp, 0);
            return tmp;
        }

        public ELFReader(Stream s)
        {
            m_header = ReadStruct<ELF32Header>(s);

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
                s.Seek(m_header.phoff + (i * m_header.phentsize), SeekOrigin.Begin);
                m_progheaders[i] = ReadStruct<ProgramHeader>(s);
                if (convertEndian)
                    m_progheaders[i] = FixEndian(m_progheaders[i]);
            }

            m_sectionheaders = new SectionHeader[m_header.shnum];
            for (int i = 0; i < m_sectionheaders.Length; i++)
            {
                s.Seek(m_header.shoff + (i * m_header.shentsize), SeekOrigin.Begin);
                m_sectionheaders[i] = ReadStruct<SectionHeader>(s);
                if (convertEndian)
                    m_sectionheaders[i] = FixEndian(m_sectionheaders[i]);
            }

            m_programsections = new byte[m_progheaders.Length][];
            for (int i = 0; i < m_programsections.Length; i++)
            {
                s.Seek(m_progheaders[i].offset, SeekOrigin.Begin);
                m_programsections[i] = ReadBlock(s, (int)m_progheaders[i].filesz);
            }

            m_sectionsections = new byte[m_sectionheaders.Length][];
            for (int i = 0; i < m_sectionheaders.Length; i++)
            {
                s.Seek(m_sectionheaders[i].offset, SeekOrigin.Begin);
                m_sectionsections[i] = ReadBlock(s, (int)m_sectionheaders[i].size);

                if (m_sectionheaders[i].type == 0x3)
                    m_stringtableindex = i;
                else if (m_sectionheaders[i].type == 0x2)
                    m_symboltableindex = i;
            }

            if (m_symboltableindex >= 0)
            {
                int size = Marshal.SizeOf(typeof(SymbolTableEntry));
                int count = (int)m_sectionheaders[m_symboltableindex].size / size;
                m_symbols = new SymbolTableEntry[count];
                using (MemoryStream ms = new MemoryStream(m_sectionsections[m_symboltableindex]))
                    for (int i = 0; i < count; i++)
                    {
                        m_symbols[i] = ReadStruct<SymbolTableEntry>(ms);
                        if (convertEndian)
                            m_symbols[i] = FixEndian(m_symbols[i]);
                    }
            }
        }

        private string FindString(uint index)
        {
            if (m_stringtableindex < 0)
                return "";
            StringBuilder sb = new StringBuilder();
            while (index < m_sectionsections[m_stringtableindex].Length && m_sectionsections[m_stringtableindex][index] != 0)
                sb.Append((char)m_sectionsections[m_stringtableindex][index++]);
            
            return sb.ToString();
        }

        public void Disassemble(TextWriter sw)
        {
            try
            {
                OpCodes.OpCodeParser parser = new OpCodes.OpCodeParser();

                List<SymbolTableEntry> functions = m_symbols == null ? new List<SymbolTableEntry>() : m_symbols.Where(x => x.Type == st_type.func && x.Bind == st_bind.global).ToList();
                List<SymbolTableEntry> labels = m_symbols == null ? new List<SymbolTableEntry>() : m_symbols.Where(x => x.Type == st_type.notype && x.Bind == st_bind.local).ToList();

                for(int i = 0; i < m_progheaders.Length; i++)
                    if (m_progheaders[i].type == 1 && (m_progheaders[i].flags & 0x1) == 1) //Load and execute
                    {
                        sw.WriteLine("# Program section {0} at virtual offset 0x{1:x8}, file offset 0x{2:x8}", i, m_progheaders[i].vaddr, m_progheaders[i].offset);
                        sw.WriteLine();

                        for (int j = 0; j < m_progheaders[i].memsz; j += 4)
                        {
                            try
                            {
                                foreach (var v in functions.FindAll(x => (x.value + x.size) == (uint)j && x.size > 0))
                                    sw.WriteLine("#END ." + FindString(v.name));

                                foreach (var v in functions.FindAll(x => x.value == (uint)j))
                                    sw.WriteLine("." + FindString(v.name));

                                foreach (var v in labels.FindAll(x => x.value == (uint)j))
                                    sw.WriteLine(FindString(v.name) + ":");

                                OpCodes.Bases.Instruction op = parser.FindCode(m_programsections[i], (uint)j);
                                sw.WriteLine("0x{0:x4}: {1}", j, op.ToString());
                            }
                            catch
                            {
                                sw.WriteLine("0x{0:x4}: Unrecognized instruction {1:x2}{2:x2}{3:x2}{4:x2}", j, m_programsections[i][j], m_programsections[i][j + 1], m_programsections[i][j + 2], m_programsections[i][j + 3]);
                            }
                        }
                    }
            }
            finally
            {
                if (sw != null)
                    sw.Flush();
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
                    if (m_progheaders[i].vaddr < m_header.entry && m_progheaders[i].vaddr + m_progheaders[i].memsz > m_header.entry)
                        spe.SPU.CodeSize = (uint)m_progheaders[i].vaddr + m_progheaders[i].memsz; //TODO: If execution is not at boundary 0x0, this counter is wrong
                }
        }

        private static T FixEndian<T>(T item) where T : struct
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
