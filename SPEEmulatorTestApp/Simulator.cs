using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SPEEmulatorTestApp
{
    public partial class Simulator : Form
    {
        private SPEEmulator.SPEProcessor m_spe = null;

        public Simulator()
        {
            InitializeComponent();
        }

        private void LoadELF_CheckedChanged(object sender, EventArgs e)
        {
            ELFBrowsePanel.Enabled = LoadELF.Checked;
        }

        private void LoadCIL_CheckedChanged(object sender, EventArgs e)
        {
            CILBrowsePanel.Enabled = LoadCIL.Checked;
        }

        private void BrowseELFFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                ELFFilename.Text = openFileDialog.FileName;
        }

        private void BrowseCILFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                CILFilename.Text = openFileDialog.FileName;
        }

        private void AutoReadNormalMbox_CheckedChanged(object sender, EventArgs e)
        {
            ReadNormalMboxButton.Enabled = !AutoReadNormalMbox.Checked;
        }

        private void AutoReadIntrMbox_CheckedChanged(object sender, EventArgs e)
        {
            ReadIntrMboxButton.Enabled = !AutoReadIntrMbox.Checked;
        }

        private void Simulator_Load(object sender, EventArgs e)
        {
            this.Height = 150;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (m_spe == null || m_spe.State == SPEEmulator.SPEState.NotStarted || m_spe.State == SPEEmulator.SPEState.Terminated)
            {
                m_spe = new SPEEmulator.SPEProcessor();

                m_spe.SPEStarted += new SPEEmulator.StatusEventDelegate(SPEStatusChanged);
                m_spe.SPEStopped += new SPEEmulator.StatusEventDelegate(SPEStatusChanged);
                m_spe.SPEPaused += new SPEEmulator.StatusEventDelegate(SPEStatusChanged);
                m_spe.SPEResumed += new SPEEmulator.StatusEventDelegate(SPEStatusChanged);

                m_spe.MboxWritten += new SPEEmulator.StatusEventDelegate(SPEMboxWritten);
                m_spe.IntrMboxWritten += new SPEEmulator.StatusEventDelegate(SPEIntrMboxWritten);
                m_spe.InMboxRead += new SPEEmulator.StatusEventDelegate(SPEInMboxRead);

                try
                {
                    using (System.IO.FileStream fs = System.IO.File.OpenRead(ELFFilename.Text))
                    {
                        SPEEmulator.ELFReader r = new SPEEmulator.ELFReader(fs);
                        r.SetupExecutionEnv(m_spe);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, string.Format("Unable to load file: {0}", ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                m_spe.Start();
            }
            else
            {
                m_spe.Stop();
            }
        }

        private void SPEInMboxRead(SPEEmulator.SPEProcessor spe)
        {
            int s = spe.InMboxSize;
            OutMboxStatus.Text = string.Format("{0} {1}", s, s != 1 ? "messages" : "message");
        }

        private void SPEIntrMboxWritten(SPEEmulator.SPEProcessor spe)
        {
            int s = spe.MboxSize;
            IntrMboxStatus.Text = string.Format("{0} {1}", s, s != 1 ? "messages" : "message");
        }

        private void SPEMboxWritten(SPEEmulator.SPEProcessor spe)
        {
            int s = spe.MboxIntrSize;
            NormalMboxStatus.Text = string.Format("{0} {1}", s, s != 1 ? "messages" : "message");
        }

        private void SPEStatusChanged(SPEEmulator.SPEProcessor spe)
        {
            if (spe.State == SPEEmulator.SPEState.Running)
            {
                this.Height = Math.Max(this.Height, 335 + SimulationButtonPanel.Height);
                SimulationControls.Visible = SimulationControls.Enabled = true;
                FilenamePanel.Enabled = false;
                StartButton.Text = "Stop";
                PauseButton.Text = "Pause";
            }
            else if (spe.State == SPEEmulator.SPEState.Terminated)
            {
                SimulationControls.Enabled = false;
                FilenamePanel.Enabled = true;
                StartButton.Text = "Start";
                m_spe = null;
            }
            else if (spe.State == SPEEmulator.SPEState.Paused)
            {
                PauseButton.Text = "Resume";
            }
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (m_spe != null)
            {
                if (m_spe.State == SPEEmulator.SPEState.Running)
                    m_spe.Pause();
                else if (m_spe.State == SPEEmulator.SPEState.Paused)
                    m_spe.Resume();
            }
        }

        private void WriteOutMboxButton_Click(object sender, EventArgs e)
        {
            if (m_spe != null)
            {
                if (!m_spe.WriteMbox((uint)OutMboxValue.Value, false))
                    MessageBox.Show("Failed to write as mailbox was full");

                SPEInMboxRead(m_spe);
            }
        }

        private void ReadIntrMboxButton_Click(object sender, EventArgs e)
        {
            if (m_spe != null)
            {
                uint? x = m_spe.ReadIntrMbox(false);
                if (x == null)
                    MessageBox.Show("Failed to read as interrupt mailbox was empty");
                else
                    WriteOutputText(string.Format("Consumed interrupt mailbox value {0}", x.Value));

                SPEIntrMboxWritten(m_spe);
            }

        }

        private void WriteOutputText(string text)
        {
            OutputText.Text += text;
            OutputText.SelectionStart = OutputText.Text.Length;
            OutputText.SelectionLength = 0;
        }

        private void ReadNormalMboxButton_Click(object sender, EventArgs e)
        {
            if (m_spe != null)
            {
                uint? x = m_spe.ReadMbox(false);
                if (x == null)
                    MessageBox.Show("Failed to read as normal mailbox was empty");
                else
                    WriteOutputText(string.Format("Consumed normal mailbox value {0}", x.Value));

                SPEMboxWritten(m_spe);
            }
        }
    }
}
