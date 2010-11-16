using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SPEEmulatorTestApp
{
    public partial class Registers : Form
    {
        private SPEEmulator.SPEProcessor m_spe = null;
        private bool m_loaded = false;

        private TreeNode m_R0 = new TreeNode("Return Address / Link Register (R0)");
        private TreeNode m_R1 = new TreeNode("Stack pointer information (R1)");
        private TreeNode m_R2 = new TreeNode("Environment pointer (R2)");
        private TreeNode m_R3 = new TreeNode("Function’s argument list and its return value (R3-R74)");
        private TreeNode m_R75 = new TreeNode("Scratch Registers (R75-R79)");
        private TreeNode m_R80 = new TreeNode("Local variable registers (R80-R127)");

        public Registers(SPEEmulator.SPEProcessor spe)
        {
            InitializeComponent();
            LoadSPE(spe);
        }

        private void LoadSPE(SPEEmulator.SPEProcessor spe)
        {
            m_spe = spe;

            if (m_loaded || m_spe == null)
                return;

            treeView1.ShowNodeToolTips = true;
            treeView1.Nodes.Clear();

            m_R0.Nodes.Add("Register 0");
            m_R1.Nodes.Add("Register 1");
            m_R2.Nodes.Add("Register 2");

            for(int i = 3; i < 75; i++) 
                m_R3.Nodes.Add("Register " + i);

            for (int i = 75; i < 80; i++)
                m_R75.Nodes.Add("Register " + i);

            for (int i = 80; i < 128; i++)
                m_R80.Nodes.Add("Register " + i);

            treeView1.Nodes.AddRange(new TreeNode[] { m_R0, m_R1, m_R2, m_R3, m_R75, m_R80 });

            FirstLoad();

            m_loaded = true;
        }

        public void FirstLoad()
        {

            int count = 0;

            foreach (SPEEmulator.Register register in m_spe.SPU.Register)
            {
                if (count == 0)
                    m_R0.Nodes[0].Nodes.Add(register.Value.ToString());
                else if (count == 1)
                    m_R1.Nodes[0].Nodes.Add(register.Value.ToString());
                else if (count == 2)
                    m_R2.Nodes[0].Nodes.Add(register.Value.ToString());
                else if (count > 79)
                    m_R80.Nodes[count - 80].Nodes.Add(register.Value.ToString());
                else if (count > 74)
                    m_R75.Nodes[count - 75].Nodes.Add(register.Value.ToString());
                else if (count > 2)
                    m_R3.Nodes[count - 3].Nodes.Add(register.Value.ToString());

                count++;
            }
        }

        private void Changed(TreeNode node, string text)
        {
            if (node.Text != text)
            {
                node.ToolTipText = "New:\t" + text + "\nOld:\t" + node.Text;
                node.Text = text;
                node.BackColor = Color.Red;
                
                treeView1.SelectedNode = node;
                treeView1.SelectedNode = null;

                node.Expand();

                TreeNode parent = node.Parent;

                while (parent != null)
                {
                    parent.Expand();
                    parent = parent.Parent;
                }
                
                node.Parent.EnsureVisible();
                node.EnsureVisible();
            }
            else
            {
                node.BackColor = Color.White;
            }
        }

        public void Reload()
        {
            if (m_spe == null)
                return;

            try
            {
                treeView1.BeginUpdate();
                int count = 0;
                //treeView1.SelectedNode = m_R0;

                foreach (SPEEmulator.Register register in m_spe.SPU.Register)
                {
                    if (count == 0)
                        Changed(m_R0.Nodes[0].Nodes[0], register.Value.ToString());
                    else if (count == 1)
                        Changed(m_R1.Nodes[0].Nodes[0], register.Value.ToString());
                    else if (count == 2)
                        Changed(m_R2.Nodes[0].Nodes[0], register.Value.ToString());
                    else if (count > 79)
                        Changed(m_R80.Nodes[count - 80].Nodes[0], register.Value.ToString());
                    else if (count > 74)
                        Changed(m_R75.Nodes[count - 75].Nodes[0], register.Value.ToString());
                    else if (count > 2)
                        Changed(m_R3.Nodes[count - 3].Nodes[0], register.Value.ToString());

                    count++;
                }
            }
            finally
            {
                treeView1.EndUpdate();
            }
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            var test = e.Node.Parent.Text.ToString().Substring(9);

            int index = -1;
            int.TryParse(test, out index);

            ulong high = 0;
            ulong low = 0;

            string newText = e.Label;

            if (!newText.StartsWith("0x"))
                newText = "0x" + newText;


            if (index == -1 || string.IsNullOrEmpty(e.Label) || e.Node.Nodes.Count > 0 || newText.Length != 34)
            {
                e.CancelEdit = true;
                return;
            }

            try
            {
                high = Convert.ToUInt64(newText.Substring(2).Substring(0, 16), 16);
                low = Convert.ToUInt64(newText.Substring(2).Substring(16), 16);
            }
            catch (Exception)
            {
                e.CancelEdit = true;
                return;
            }

            m_spe.SPU.Register[index].Value = new SPEEmulator.RegisterValue(high, low);
        }
    }
}
