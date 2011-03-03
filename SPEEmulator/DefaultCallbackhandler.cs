using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator
{
    internal class DefaultCallbackhandler
    {
        private SPEProcessor m_spe;

        private enum C99Function : uint
        {
            CLEARERR = 0x01,
            FCLOSE,
            FEOF,
            FERROR,
            FFLUSH,
            FGETC,
            FGETPOS,
            FGETS,
            FILENO,
            FOPEN,
            FPUTC,
            FPUTS,
            FREAD,
            FREOPEN,
            FSEEK,
            FSETPOS,
            FTELL,
            FWRITE,
            GETC,
            GETCHAR,
            GETS,
            PERROR,
            PUTC,
            PUTCHAR,
            PUTS,
            REMOVE,
            RENAME,
            REWIND,
            SETBUF,
            SETVBUF,
            SYSTEM,
            TMPFILE,
            TMPNAM,
            UNGETC,
            VFPRINTF,
            VFSCANF,
            VPRINTF,
            VSCANF,
            VSNPRINTF,
            VSPRINTF,
            VSSCANF,
            LAST_OPCODE,
        }

        public DefaultCallbackhandler(SPEProcessor owner)
        {
            m_spe = owner;
        }

        public bool C99Handler(byte[] ls, uint ls_offset)
        {
            EndianBitConverter cv = new EndianBitConverter(ls);
            uint func = cv.ReadUInt(ls_offset);
            C99Function pfunc = (C99Function)((func >> 24) & 0xff);
            uint ls_args = func & 0xffffff;

            switch (pfunc)
            {
                case C99Function.VPRINTF:
                    {
                        string format = m_spe.ReadLSString(cv.ReadUInt(LS_ARG_ADDR(ls_args, 0)));
                        m_spe.RaisePrintfIssued(printf(m_spe, ls_args, format));
                        cv.WriteUInt(LS_ARG_ADDR(ls_args, 0), 0u);
                        return true;
                    }
                default:
                    m_spe.RaiseMissingMethodError(string.Format("The method {0} is not implemented", pfunc));
                    break;
            }

            return false;
        }

        private static uint LS_ARG_ADDR(uint ls_args, int index)
        {
            return ((uint)index * 16u) + ls_args;
        }

        private static string printf(SPEProcessor spe, uint ls_args, string format)
        {
            List<object> data = new List<object>();
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"\%(\d*\$)?([\'\#\-\+ ]*)(\d*)(?:\.(\d+))?([hl])?(?<token>[dioxXucsfeEgGpn%])"); ;
            //"%[parameter][flags][width][.precision][length]type"

            uint offset = spe.ReadLSWord(LS_ARG_ADDR(ls_args, 1));
            uint caller_stack = spe.ReadLSWord(LS_ARG_ADDR(ls_args, 1) + 16);

            foreach (System.Text.RegularExpressions.Match m in r.Matches(format))
            {
                switch (m.Groups["token"].Value[0])
                {
                    case 'c':
                    case 'd':
                    case 'i':
                        data.Add((int)spe.ReadLSWord(offset));
                        break;
                    case 'e':
                    case 'E':
                    case 'f':
                    case 'F':
                    case 'g':
                    case 'G':
                        data.Add(spe.ReadLSDouble(offset));
                        break;
                    case 'h':
                        data.Add((int)spe.ReadLSWord(offset));
                        break;
                    case 'p':
                    case 'o':
                    case 'u':
                    case 'x':
                    case 'X':
                        data.Add((int)spe.ReadLSWord(offset));
                        break;
                    case 's':
                        data.Add(spe.ReadLSString(offset));
                        break;
                    default:
                        throw new Exception(string.Format("Unable to understand printf token: {0}", m.Value));
                }

                offset += 16;
                if (spe.ReadLSWord(offset) == caller_stack)
                    offset += 32;
            }

            return AT.MIN.Tools.sprintf(format, data.ToArray());
        }

        public bool DefaultPosixHandler(byte[] ls, uint ls_offset)
        {
            EndianBitConverter cv = new EndianBitConverter(ls);
            uint func = cv.ReadUInt(ls_offset);
            m_spe.RaiseMissingMethodError(string.Format("The posix method {0} is not implemented", func));
            return true;
        }

        public bool DefaultLibeaHandler(byte[] ls, uint ls_offset)
        {
            EndianBitConverter cv = new EndianBitConverter(ls);
            uint func = cv.ReadUInt(ls_offset);
            m_spe.RaiseMissingMethodError(string.Format("The libea method {0} is not implemented", func));
            return true;
        }
    }
}
