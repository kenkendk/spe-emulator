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
    public partial class EditMemForm : Form
    {
        private SPEEmulator.SPEProcessor m_spe = null;

        public EditMemForm()
        {
            InitializeComponent();
        }

        public void LoadSPE(SPEEmulator.SPEProcessor spe)
        {
            if (spe == null || spe.LS == null)
                return;

            m_spe = spe;

            lsAddress.Maximum = m_spe.LS.Count() - 4;

            numericUpDown1_ValueChanged(lsAddress, new EventArgs());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (m_spe == null)
            {
                MessageBox.Show("The SPE has not been loaded", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int value = (int)lsAddress.Value;
            
            if (value < 0 || value > m_spe.LS.Count() || value % 4 != 0)
                value = value - (value % 4);

            m_spe.WriteLSWord((uint)value, Convert.ToUInt32(lsWord.Text, 16));
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (m_spe == null)
            {
                MessageBox.Show("The SPE has not been loaded", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (lsAddress.Value < 0 || lsAddress.Value > m_spe.LS.Count() || lsAddress.Value % 4 != 0)
                lsAddress.Value = lsAddress.Value - (lsAddress.Value % 4);

            string text = Convert.ToString(m_spe.ReadLSWord((uint)lsAddress.Value), 16);

            while (text.Length < 8)
                text = "0" + text;

            lsWord.Text = text;
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                uint value = Convert.ToUInt32(lsWord.Text, 16);

                if (value > 0xffffffff)
                {
                    errorProvider1.SetError(lsWord, "Value is to large");
                    e.Cancel = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(lsWord, ex.Message);
                e.Cancel = true;
                return;
            }

            errorProvider1.Clear();
        }
    }
}
