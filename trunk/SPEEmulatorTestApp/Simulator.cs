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
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (m_spe == null || m_spe.State == SPEEmulator.SPEState.NotStarted || m_spe.State == SPEEmulator.SPEState.Terminated)
            {
                OutputText.Text = "";

                m_spe = new SPEEmulator.SPEProcessor();

                m_spe.SPEStarted += new SPEEmulator.StatusEventDelegate(SPEStatusChanged);
                m_spe.SPEStopped += new SPEEmulator.StatusEventDelegate(SPEStatusChanged);
                m_spe.SPEPaused += new SPEEmulator.StatusEventDelegate(SPEStatusChanged);
                m_spe.SPEResumed += new SPEEmulator.StatusEventDelegate(SPEStatusChanged);

                m_spe.InstructionExecuting += new SPEEmulator.InformationEventDelegate(SPE_InstructionExecuting);
                m_spe.MissingMethodError += new SPEEmulator.InformationEventDelegate(SPE_MissingMethodError);
                m_spe.InvalidOpCodeError += new SPEEmulator.InformationEventDelegate(SPE_InvalidOpCodeError);
                m_spe.PrintfIssued += new SPEEmulator.InformationEventDelegate(SPE_PrintfIssued);
                m_spe.Warning += new SPEEmulator.WarningEventDelegate(SPE_Warning);
                m_spe.Exit += new SPEEmulator.ExitEventDelegate(SPE_Exit);

                //m_spe.SPU.Breakpoints = new uint[] { 0x0174 };

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

        private void SPE_Exit(SPEEmulator.SPEProcessor sender, uint exitcode)
        {
            WriteOutputText(string.Format("SPE has exited with code 0x{0:x4}", exitcode) + Environment.NewLine);
        }

        private void SPE_PrintfIssued(SPEEmulator.SPEProcessor sender, string message)
        {
            WriteOutputText(message + Environment.NewLine);
        }

        private void SPE_Warning(SPEEmulator.SPEProcessor sender, SPEEmulator.SPEWarning type, string message)
        {
            if (type == SPEEmulator.SPEWarning.BreakPointHit)
                WriteOutputText(message + Environment.NewLine);
            else if (type == SPEEmulator.SPEWarning.ExecuteDataArea)
            {
                if (PrintCodeExecution.Checked)
                    WriteOutputText("* Warning: " + message + Environment.NewLine);
            }
            else
                WriteOutputText("* Warning: " + message + Environment.NewLine);
        }

        private void SPE_InstructionExecuting(SPEEmulator.SPEProcessor sender, string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SPEEmulator.InformationEventDelegate(SPE_InstructionExecuting), sender, message);
            }
            else
            {
                if (PrintCodeExecution.Checked)
                    WriteOutputText(message + Environment.NewLine);
                label3.Text = string.Format("PC: 0x{0:x4}", sender.SPU.PC);
            }
        }

        private void SPE_MissingMethodError(SPEEmulator.SPEProcessor sender, string message)
        {
            WriteOutputText(message + Environment.NewLine);
        }

        private void SPE_InvalidOpCodeError(SPEEmulator.SPEProcessor sender, string message)
        {
            WriteOutputText(message + Environment.NewLine);
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
            if (this.InvokeRequired)
                this.Invoke(new SPEEmulator.StatusEventDelegate(SPEStatusChanged), spe);
            else
            {
                if (spe.State == SPEEmulator.SPEState.Running)
                {
                    WriteOutputText("***** SPE is now running *****" + Environment.NewLine);
                    SimulationControls.Enabled = true;
                    SimulationInteractionControls.Enabled = true;
                    FilenamePanel.Enabled = false;
                    StartButton.Text = "Stop";
                    PauseButton.Text = "Pause";
                    PauseButton.Enabled = true;
                }
                else if (spe.State == SPEEmulator.SPEState.Terminated)
                {
                    WriteOutputText("***** SPE is now terminated *****" + Environment.NewLine);
                    SimulationInteractionControls.Enabled = false;
                    FilenamePanel.Enabled = true;
                    StartButton.Text = "Start";
                    PauseButton.Enabled = false;
                    m_spe = null;
                }
                else if (spe.State == SPEEmulator.SPEState.Paused)
                {
                    WriteOutputText("***** SPE is now paused *****" + Environment.NewLine);
                    PauseButton.Text = "Resume";
                }
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

        private object WriteOutputText(string text)
        {
            if (this.InvokeRequired)
                this.Invoke(new Func<string, object>(WriteOutputText), text);
            else
            {
                OutputText.Text += text;
                OutputText.SelectionStart = OutputText.Text.Length;
                OutputText.SelectionLength = 0;
                OutputText.ScrollToCaret();
            }

            return null;
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

        private void DisassembleElf_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    using (System.IO.FileStream fs = System.IO.File.OpenRead(ELFFilename.Text))
                    {
                        SPEEmulator.ELFReader r = new SPEEmulator.ELFReader(fs);

                        try
                        {
                            using (System.IO.FileStream fso = System.IO.File.Create(saveFileDialog.FileName))
                                r.Disassemble(fso);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, string.Format("Unable to save file: {0}", ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, string.Format("Unable to load file: {0}", ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

        }
    }
}
