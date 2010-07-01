using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator
{
    static class C99DefaultHandler
    {
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

        public static bool HandleOp(SPEProcessor spe, uint func)
        {
            
            C99Function pfunc = (C99Function)((func >> 24) & 0xff);
            uint ls_args = func & 0xffffff;

            ls_args += 16; //TODO: Figure out why this is required

            switch (pfunc)
            {
                case C99Function.VPRINTF:
                    {
                        string format = spe.ReadLSString(spe.ReadLSWord(LS_ARG_ADDR(ls_args, 0)));
                        spe.RaisePrintfIssued(printf(spe, ls_args, format));
                        return true;
                    }
                default:
                    spe.RaiseMissingMethodError(string.Format("The method {0} is not implemented", pfunc));
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
            //TODO: This regexp does not support all formats known to printf
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"\%[cdieEfFgGhlpouxXs]");

            uint offset = spe.ReadLSWord(LS_ARG_ADDR(ls_args, 1));
            uint caller_stack = spe.ReadLSWord(LS_ARG_ADDR(ls_args, 1) + 16);

            foreach (System.Text.RegularExpressions.Match m in r.Matches(format))
            {
                switch (m.Value[1])
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
                    case 'l':
                        //TODO: fix this
                        throw new Exception("TODO: support l (L) in printf");
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
    }
}
