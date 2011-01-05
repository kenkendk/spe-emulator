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
        private Registers m_formRegister = null;
        private List<string> m_outputlines = new List<string>();

        public Simulator(string[] args)
        {
            InitializeComponent();
            if (args != null && args.Length == 1)
                ELFFilename.Text = args[0];
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

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (m_spe == null || m_spe.State == SPEEmulator.SPEState.NotStarted || m_spe.State == SPEEmulator.SPEState.Terminated)
                Start(false);
            else
                m_spe.Stop();
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
            if (this.InvokeRequired)
                this.Invoke(new SPEEmulator.StatusEventDelegate(SPEInMboxRead), spe);
            else
            {
                int s = spe.InMboxSize;
                OutMboxStatus.Text = string.Format("{0} {1}", s, s != 1 ? "messages" : "message");
            }
        }

        private void SPEIntrMboxWritten(SPEEmulator.SPEProcessor spe)
        {
            if (this.InvokeRequired)
                this.Invoke(new SPEEmulator.StatusEventDelegate(SPEIntrMboxWritten), spe);
            else
            {
                int s = spe.MboxIntrSize;
                IntrMboxStatus.Text = string.Format("{0} {1}", s, s != 1 ? "messages" : "message");
            }
        }

        private void SPEMboxWritten(SPEEmulator.SPEProcessor spe)
        {
            if (this.InvokeRequired)
                this.Invoke(new SPEEmulator.StatusEventDelegate(SPEMboxWritten), spe);
            else
            {
                int s = spe.MboxSize;
                NormalMboxStatus.Text = string.Format("{0} {1}", s, s != 1 ? "messages" : "message");
            }
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
                    label3.Text = "PC: 0x0000";
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
                    //m_spe = null;
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
                m_outputlines.Add(text);
                while (m_outputlines.Count > 100)
                    m_outputlines.RemoveAt(0);
                
                OutputText.Text = string.Join("", m_outputlines.ToArray());
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
                                r.Disassemble(new System.IO.StreamWriter(fso));
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

        private void Start(bool singleStepping)
        {
            if (m_spe == null || m_spe.State == SPEEmulator.SPEState.NotStarted || m_spe.State == SPEEmulator.SPEState.Terminated)
            {
                m_outputlines = new List<string>();
                OutputText.Text = "";

                m_spe = new SPEEmulator.SPEProcessor();
                if (m_formRegister != null)
                    m_formRegister.Close();
                m_formRegister = new Registers(m_spe);
                m_formRegister.Show();
                m_formRegister.Top = this.Top;
                m_formRegister.Left = this.Right;

                m_spe.SPEStarted += new SPEEmulator.StatusEventDelegate(SPEStatusChanged);
                m_spe.SPEStopped += new SPEEmulator.StatusEventDelegate(SPEStatusChanged);
                m_spe.SPEPaused += new SPEEmulator.StatusEventDelegate(SPEStatusChanged);
                m_spe.SPEResumed += new SPEEmulator.StatusEventDelegate(SPEStatusChanged);
                m_spe.InstructionExecuted += new SPEEmulator.InformationEventDelegate(m_spe_InstructionExecuted);

                m_spe.InstructionExecuting += new SPEEmulator.InformationEventDelegate(SPE_InstructionExecuting);
                m_spe.MissingMethodError += new SPEEmulator.InformationEventDelegate(SPE_MissingMethodError);
                m_spe.InvalidOpCodeError += new SPEEmulator.InformationEventDelegate(SPE_InvalidOpCodeError);
                m_spe.PrintfIssued += new SPEEmulator.InformationEventDelegate(SPE_PrintfIssued);
                m_spe.Warning += new SPEEmulator.WarningEventDelegate(SPE_Warning);
                m_spe.Exit += new SPEEmulator.ExitEventDelegate(SPE_Exit);

                //m_spe.SPU.Breakpoints = new uint[] { 0x2e4 };

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

                m_spe.Start(singleStepping);
            }
        }

        void m_spe_InstructionExecuted(SPEEmulator.SPEProcessor sender, string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SPEEmulator.InformationEventDelegate(m_spe_InstructionExecuted), sender, message);
            }
            else
            {
                if (m_formRegister != null || !m_formRegister.IsDisposed)
                    m_formRegister.Reload();
            }
        }


        private void StepButton_Click(object sender, EventArgs e)
        {
            if (m_spe == null || m_spe.State == SPEEmulator.SPEState.NotStarted || m_spe.State == SPEEmulator.SPEState.Terminated)
                Start(true);
            else
                m_spe.Step();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Memory mem = new Memory();
            mem.LoadSPE(m_spe);
            mem.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (m_formRegister == null || m_formRegister.IsDisposed)
            {
                m_formRegister = new Registers(m_spe);
                m_formRegister.Show();
                m_formRegister.Top = this.Top;
                m_formRegister.Left = this.Right;
            }
        }

        public void StartAndPause()
        {
            Start(true);
        }

        public SPEEmulator.SPEProcessor SPE { get { return m_spe; } }
    }
}
