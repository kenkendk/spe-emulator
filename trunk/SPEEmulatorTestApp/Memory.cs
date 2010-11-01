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
    public partial class Memory : Form
    {
        private SPEEmulator.SPEProcessor m_spe = null;
        private SPEEmulator.OpCodes.OpCodeParser m_parser;

        public Memory()
        {
            InitializeComponent();
            m_parser = new SPEEmulator.OpCodes.OpCodeParser();
        }


        public void LoadSPE(SPEEmulator.SPEProcessor spe)
        {
            if (spe == null || spe.LS == null)
                return;

            m_spe = spe;


            StringBuilder sbOuter = new StringBuilder();
            StringBuilder sbInner = new StringBuilder();
            StringBuilder sbASCII = new StringBuilder();
            StringBuilder sbInst = new StringBuilder();

            richTextBox1.Clear();

            for (int i = 0; i < m_spe.LS.Count(); )
            {
                string hex = Convert.ToString(i, 16);

                while (hex.Length < 5)
                    hex = "0" + hex;

                sbOuter.Append("0x" + hex + "    ");

                sbInner.Clear();
                sbASCII.Clear();
                sbInst.Clear();

                for (int j = i; j < i + 16; j++)
                {
                    string value = Convert.ToString(spe.LS[j], 16);

                    while (value.Length < 2)
                        value = "0" + value;

                    sbInner.Append(value);

                    if (spe.LS[j] > 0x1f)
                        sbASCII.Append((char)spe.LS[j]);
                    else
                        sbASCII.Append(".");

                    sbInst.Append(value);
                }


                i += 16;

                /*
                sbOuter.Append(sbInner);
                sbOuter.Append("    " + sbInner.ToString(0, 16) + " " + sbInner.ToString(16, 16));
                 */

                if (checkBoxHex.Checked)
                    sbOuter.Append("    " + sbInner.ToString(0, 8) + " " + sbInner.ToString(8, 8) + " " + sbInner.ToString(16, 8) + " " + sbInner.ToString(24, 8));

                if (checkBoxASCII.Checked)
                    sbOuter.Append("    " + sbASCII.ToString());

                if (checkBoxInst.Checked)
                {
                    if (sbInst.Length < 17)
                        sbOuter.Append("                                                                            ");

                    for (int j = 0; j < sbInst.Length; j += 8)
                    {

                        try
                        {
                            uint test = Convert.ToUInt32(sbInst.ToString().Substring(j, 8), 16);
                            string inst = m_parser.FindCode(test).ToString();

                            while (inst.Length < 30)
                                inst += " ";


                            sbOuter.Append("    " + inst + "    ");
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                sbOuter.Append(Environment.NewLine);
            }

            richTextBox1.Text = sbOuter.ToString();
        }

        private void checkBoxInst_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxInst.Checked)
                label2.Visible = true;
            else
                label2.Visible = false;

            if (checkBoxASCII.Checked)
                label5.Visible = true;
            else
                label5.Visible = false;

            if (checkBoxHex.Checked)
                label4.Visible = true;
            else
                label4.Visible = false;

            if (((CheckBox)sender) == checkBoxHex)
            {
                if (((CheckBox)sender).Checked)
                {
                    label5.Left += 300;
                    label2.Left += 300;
                }
                else
                {
                    label5.Left -= 300;
                    label2.Left -= 300;
                }
            }

            if (((CheckBox)sender) == checkBoxASCII)
            {
                if (((CheckBox)sender).Checked)
                    label2.Left += 175;
                else
                    label2.Left -= 175;
            }

            LoadSPE(m_spe);
        }
    }
}
