using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPEEmulator.OpCodes
{
    public class OpCodeParser
    {
        private class Node
        {
            public int Position;
            public Node One;
            public Node Zero;
            public Bases.Instruction Op;

            public Node(Bases.Instruction op, int position)
            {
                this.Position = position;
                this.Op = op;
            }
        }

        private Node m_root;

        public OpCodeParser()
        {
            string @namespace = System.Reflection.MethodInfo.GetCurrentMethod().ReflectedType.Namespace;
            List<Bases.Instruction> ops = new List<Bases.Instruction>();
            foreach (Type t in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
                if (typeof(Bases.Instruction).IsAssignableFrom(t) && t.Namespace == @namespace)
                    ops.Add((Bases.Instruction)Activator.CreateInstance(t));

#if DEBUG
            //Setup tests

            foreach (Bases.Mnemonic m in Enum.GetValues(typeof(Bases.Mnemonic)))
                if (!ops.Exists(c => c.Mnemonic == m))
                    throw new Exception(string.Format("Unable to locate an opcode for the mnemonic {0}", m));

            foreach (Bases.Instruction o in ops)
                if (ops.Exists(c => c.Mnemonic == o.Mnemonic && c != o))
                    throw new Exception(string.Format("Duplicate mnemonic {0}", o.Mnemonic));

            foreach (Bases.Instruction o in ops)
                if (ops.Exists(c => c.Value == o.Value && c != o))
                    throw new Exception(string.Format("Duplicate value 0x{0:x8}: {1} and {2}", o.Value, o.Mnemonic, ops.First(c => c.Value == o.Value && c != o).Mnemonic));
#endif

            m_root = new Node(ops[0], 31);
            ops.RemoveAt(0);

            foreach(Bases.Instruction o in ops)
            {
                Node n = m_root; //FindNode(o.Value);
                bool done = false;

                while (n.Position > 0)
                {
                    bool newIsZero = ((o.Value >> n.Position) & 0x1) == 0;

                    if (n.Op == null)
                    {
                        if (newIsZero)
                        {
                            if (n.Zero == null)
                            {
                                n.Zero = new Node(o, n.Position - 1);
                                done = true;
                                break;
                            }
                            else
                                n = n.Zero;
                        }
                        else
                        {
                            if (n.One == null)
                            {
                                n.One = new Node(o, n.Position - 1);
                                done = true;
                                break;
                            }
                            else
                                n = n.One;
                        }
                    }
                    else
                    {
                        bool currentIsZero = ((n.Op.Value >> n.Position) & 0x1) == 0;

                        //Move current op down the tree one position
                        Node t = new Node(n.Op, n.Position - 1);
                        if (currentIsZero)
                            n.Zero = t;
                        else
                            n.One = t;
                        n.Op = null;

                        if (currentIsZero == newIsZero)
                        {
                            //Occupies same spot, repeat
                            n = t;
                        }
                        else
                        {
                            //Independant, just insert
                            t = new Node(o, n.Position - 1);

                            if (newIsZero)
                                n.Zero = t;
                            else
                                n.One = t;

                            done = true;
                            break;
                        }
                    }
                }

                if (!done)
                    throw new Exception(string.Format("Opcode {0} 0x{1:x8} is located more than once", o.Mnemonic, o.Value));
            }

        }

        private Node FindNode(uint value)
        {
            int pos = 31;
            Node n = m_root;

            while (pos > 0)
            {
                if (n.Op != null)
                    return n;

                if (((value >> pos) & 0x1) == 0)
                    n = n.Zero;
                else
                    n = n.One;

                if (n == null)
                    throw new InvalidOperationException(string.Format("Unknown OP code 0x{0:x8}", value));

                pos--;
            }

            throw new Exception("Unmatched opcode");
        }

        public Bases.Instruction FindCode(uint value)
        {
            Node n = FindNode(value);
            if ((n.Op.Value & n.Op.Mask) != (value & n.Op.Mask))
                throw new InvalidOperationException(string.Format("Unknown OP code 0x{0:x8}", value));

            //This throws and exception if the opcode is wrong
            n.Op.Value = value;

            return n.Op;
        }

        public Bases.Instruction FindCode(byte[] data, uint offset)
        {
            return FindCode((uint)(data[offset] << (8 * 3)) |
                        ((uint)data[offset + 1] << (8 * 2)) |
                        ((uint)data[offset + 2] << (8 * 1)) |
                        ((uint)data[offset + 3] << (8 * 0)));
        }
    }
}
