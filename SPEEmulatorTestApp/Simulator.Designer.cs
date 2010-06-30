namespace SPEEmulatorTestApp
{
    partial class Simulator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ELFFilename = new System.Windows.Forms.TextBox();
            this.BrowseELFFile = new System.Windows.Forms.Button();
            this.LoadELF = new System.Windows.Forms.RadioButton();
            this.LoadCIL = new System.Windows.Forms.RadioButton();
            this.BrowseCILFile = new System.Windows.Forms.Button();
            this.CILFilename = new System.Windows.Forms.TextBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.OutputText = new System.Windows.Forms.TextBox();
            this.ReadNormalMboxButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.NormalMboxStatus = new System.Windows.Forms.Label();
            this.AutoReadNormalMbox = new System.Windows.Forms.CheckBox();
            this.AutoReadIntrMbox = new System.Windows.Forms.CheckBox();
            this.IntrMboxStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ReadIntrMboxButton = new System.Windows.Forms.Button();
            this.OutMboxStatus = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.WriteOutMboxButton = new System.Windows.Forms.Button();
            this.OutMboxValue = new System.Windows.Forms.NumericUpDown();
            this.SimulationControls = new System.Windows.Forms.Panel();
            this.CILBrowsePanel = new System.Windows.Forms.Panel();
            this.ELFBrowsePanel = new System.Windows.Forms.Panel();
            this.FilenamePanel = new System.Windows.Forms.Panel();
            this.SimulationButtonPanel = new System.Windows.Forms.Panel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.DisassembleElf = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.OutMboxValue)).BeginInit();
            this.SimulationControls.SuspendLayout();
            this.CILBrowsePanel.SuspendLayout();
            this.ELFBrowsePanel.SuspendLayout();
            this.FilenamePanel.SuspendLayout();
            this.SimulationButtonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ELFFilename
            // 
            this.ELFFilename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ELFFilename.Location = new System.Drawing.Point(0, 0);
            this.ELFFilename.Name = "ELFFilename";
            this.ELFFilename.Size = new System.Drawing.Size(208, 20);
            this.ELFFilename.TabIndex = 1;
            this.ELFFilename.Text = "..\\..\\..\\test-apps\\fac-asm";
            // 
            // BrowseELFFile
            // 
            this.BrowseELFFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.BrowseELFFile.Location = new System.Drawing.Point(285, 0);
            this.BrowseELFFile.Name = "BrowseELFFile";
            this.BrowseELFFile.Size = new System.Drawing.Size(24, 20);
            this.BrowseELFFile.TabIndex = 2;
            this.BrowseELFFile.Text = "...";
            this.BrowseELFFile.UseVisualStyleBackColor = true;
            this.BrowseELFFile.Click += new System.EventHandler(this.BrowseELFFile_Click);
            // 
            // LoadELF
            // 
            this.LoadELF.AutoSize = true;
            this.LoadELF.Checked = true;
            this.LoadELF.Location = new System.Drawing.Point(16, 0);
            this.LoadELF.Name = "LoadELF";
            this.LoadELF.Size = new System.Drawing.Size(71, 17);
            this.LoadELF.TabIndex = 3;
            this.LoadELF.TabStop = true;
            this.LoadELF.Text = "Load ELF";
            this.LoadELF.UseVisualStyleBackColor = true;
            this.LoadELF.CheckedChanged += new System.EventHandler(this.LoadELF_CheckedChanged);
            // 
            // LoadCIL
            // 
            this.LoadCIL.AutoSize = true;
            this.LoadCIL.Location = new System.Drawing.Point(16, 24);
            this.LoadCIL.Name = "LoadCIL";
            this.LoadCIL.Size = new System.Drawing.Size(68, 17);
            this.LoadCIL.TabIndex = 6;
            this.LoadCIL.Text = "Load CIL";
            this.LoadCIL.UseVisualStyleBackColor = true;
            this.LoadCIL.CheckedChanged += new System.EventHandler(this.LoadCIL_CheckedChanged);
            // 
            // BrowseCILFile
            // 
            this.BrowseCILFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.BrowseCILFile.Location = new System.Drawing.Point(285, 0);
            this.BrowseCILFile.Name = "BrowseCILFile";
            this.BrowseCILFile.Size = new System.Drawing.Size(24, 20);
            this.BrowseCILFile.TabIndex = 5;
            this.BrowseCILFile.Text = "...";
            this.BrowseCILFile.UseVisualStyleBackColor = true;
            this.BrowseCILFile.Click += new System.EventHandler(this.BrowseCILFile_Click);
            // 
            // CILFilename
            // 
            this.CILFilename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CILFilename.Location = new System.Drawing.Point(0, 0);
            this.CILFilename.Name = "CILFilename";
            this.CILFilename.Size = new System.Drawing.Size(285, 20);
            this.CILFilename.TabIndex = 4;
            // 
            // StartButton
            // 
            this.StartButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.StartButton.Location = new System.Drawing.Point(126, 8);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 7;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.PauseButton.Location = new System.Drawing.Point(214, 8);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(75, 23);
            this.PauseButton.TabIndex = 8;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Output";
            // 
            // OutputText
            // 
            this.OutputText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputText.Location = new System.Drawing.Point(8, 16);
            this.OutputText.Multiline = true;
            this.OutputText.Name = "OutputText";
            this.OutputText.ReadOnly = true;
            this.OutputText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.OutputText.Size = new System.Drawing.Size(405, 223);
            this.OutputText.TabIndex = 13;
            // 
            // ReadNormalMboxButton
            // 
            this.ReadNormalMboxButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ReadNormalMboxButton.Location = new System.Drawing.Point(208, 249);
            this.ReadNormalMboxButton.Name = "ReadNormalMboxButton";
            this.ReadNormalMboxButton.Size = new System.Drawing.Size(75, 23);
            this.ReadNormalMboxButton.TabIndex = 14;
            this.ReadNormalMboxButton.Text = "Read mbox";
            this.ReadNormalMboxButton.UseVisualStyleBackColor = true;
            this.ReadNormalMboxButton.Click += new System.EventHandler(this.ReadNormalMboxButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 249);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Normal mbox";
            // 
            // NormalMboxStatus
            // 
            this.NormalMboxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NormalMboxStatus.AutoSize = true;
            this.NormalMboxStatus.Location = new System.Drawing.Point(88, 249);
            this.NormalMboxStatus.Name = "NormalMboxStatus";
            this.NormalMboxStatus.Size = new System.Drawing.Size(63, 13);
            this.NormalMboxStatus.TabIndex = 16;
            this.NormalMboxStatus.Text = "0 messages";
            // 
            // AutoReadNormalMbox
            // 
            this.AutoReadNormalMbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AutoReadNormalMbox.AutoSize = true;
            this.AutoReadNormalMbox.Location = new System.Drawing.Point(288, 249);
            this.AutoReadNormalMbox.Name = "AutoReadNormalMbox";
            this.AutoReadNormalMbox.Size = new System.Drawing.Size(97, 17);
            this.AutoReadNormalMbox.TabIndex = 17;
            this.AutoReadNormalMbox.Text = "Autoread mbox";
            this.AutoReadNormalMbox.UseVisualStyleBackColor = true;
            this.AutoReadNormalMbox.CheckedChanged += new System.EventHandler(this.AutoReadNormalMbox_CheckedChanged);
            // 
            // AutoReadIntrMbox
            // 
            this.AutoReadIntrMbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AutoReadIntrMbox.AutoSize = true;
            this.AutoReadIntrMbox.Location = new System.Drawing.Point(288, 273);
            this.AutoReadIntrMbox.Name = "AutoReadIntrMbox";
            this.AutoReadIntrMbox.Size = new System.Drawing.Size(97, 17);
            this.AutoReadIntrMbox.TabIndex = 21;
            this.AutoReadIntrMbox.Text = "Autoread mbox";
            this.AutoReadIntrMbox.UseVisualStyleBackColor = true;
            this.AutoReadIntrMbox.CheckedChanged += new System.EventHandler(this.AutoReadIntrMbox_CheckedChanged);
            // 
            // IntrMboxStatus
            // 
            this.IntrMboxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.IntrMboxStatus.AutoSize = true;
            this.IntrMboxStatus.Location = new System.Drawing.Point(88, 273);
            this.IntrMboxStatus.Name = "IntrMboxStatus";
            this.IntrMboxStatus.Size = new System.Drawing.Size(63, 13);
            this.IntrMboxStatus.TabIndex = 20;
            this.IntrMboxStatus.Text = "0 messages";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 273);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Intr mbox";
            // 
            // ReadIntrMboxButton
            // 
            this.ReadIntrMboxButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ReadIntrMboxButton.Location = new System.Drawing.Point(208, 273);
            this.ReadIntrMboxButton.Name = "ReadIntrMboxButton";
            this.ReadIntrMboxButton.Size = new System.Drawing.Size(75, 23);
            this.ReadIntrMboxButton.TabIndex = 18;
            this.ReadIntrMboxButton.Text = "Read mbox";
            this.ReadIntrMboxButton.UseVisualStyleBackColor = true;
            this.ReadIntrMboxButton.Click += new System.EventHandler(this.ReadIntrMboxButton_Click);
            // 
            // OutMboxStatus
            // 
            this.OutMboxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OutMboxStatus.AutoSize = true;
            this.OutMboxStatus.Location = new System.Drawing.Point(88, 305);
            this.OutMboxStatus.Name = "OutMboxStatus";
            this.OutMboxStatus.Size = new System.Drawing.Size(63, 13);
            this.OutMboxStatus.TabIndex = 24;
            this.OutMboxStatus.Text = "4 messages";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 305);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Out mbox";
            // 
            // WriteOutMboxButton
            // 
            this.WriteOutMboxButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.WriteOutMboxButton.Location = new System.Drawing.Point(208, 305);
            this.WriteOutMboxButton.Name = "WriteOutMboxButton";
            this.WriteOutMboxButton.Size = new System.Drawing.Size(75, 23);
            this.WriteOutMboxButton.TabIndex = 22;
            this.WriteOutMboxButton.Text = "Send msg";
            this.WriteOutMboxButton.UseVisualStyleBackColor = true;
            this.WriteOutMboxButton.Click += new System.EventHandler(this.WriteOutMboxButton_Click);
            // 
            // OutMboxValue
            // 
            this.OutMboxValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OutMboxValue.Location = new System.Drawing.Point(288, 305);
            this.OutMboxValue.Name = "OutMboxValue";
            this.OutMboxValue.Size = new System.Drawing.Size(96, 20);
            this.OutMboxValue.TabIndex = 25;
            // 
            // SimulationControls
            // 
            this.SimulationControls.Controls.Add(this.OutMboxValue);
            this.SimulationControls.Controls.Add(this.OutMboxStatus);
            this.SimulationControls.Controls.Add(this.label7);
            this.SimulationControls.Controls.Add(this.WriteOutMboxButton);
            this.SimulationControls.Controls.Add(this.AutoReadIntrMbox);
            this.SimulationControls.Controls.Add(this.IntrMboxStatus);
            this.SimulationControls.Controls.Add(this.label5);
            this.SimulationControls.Controls.Add(this.ReadIntrMboxButton);
            this.SimulationControls.Controls.Add(this.AutoReadNormalMbox);
            this.SimulationControls.Controls.Add(this.NormalMboxStatus);
            this.SimulationControls.Controls.Add(this.label1);
            this.SimulationControls.Controls.Add(this.ReadNormalMboxButton);
            this.SimulationControls.Controls.Add(this.OutputText);
            this.SimulationControls.Controls.Add(this.label2);
            this.SimulationControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SimulationControls.Location = new System.Drawing.Point(0, 48);
            this.SimulationControls.Name = "SimulationControls";
            this.SimulationControls.Size = new System.Drawing.Size(419, 335);
            this.SimulationControls.TabIndex = 26;
            this.SimulationControls.Visible = false;
            // 
            // CILBrowsePanel
            // 
            this.CILBrowsePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CILBrowsePanel.Controls.Add(this.CILFilename);
            this.CILBrowsePanel.Controls.Add(this.BrowseCILFile);
            this.CILBrowsePanel.Enabled = false;
            this.CILBrowsePanel.Location = new System.Drawing.Point(96, 24);
            this.CILBrowsePanel.Name = "CILBrowsePanel";
            this.CILBrowsePanel.Size = new System.Drawing.Size(309, 20);
            this.CILBrowsePanel.TabIndex = 27;
            // 
            // ELFBrowsePanel
            // 
            this.ELFBrowsePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ELFBrowsePanel.Controls.Add(this.ELFFilename);
            this.ELFBrowsePanel.Controls.Add(this.DisassembleElf);
            this.ELFBrowsePanel.Controls.Add(this.BrowseELFFile);
            this.ELFBrowsePanel.Location = new System.Drawing.Point(96, 0);
            this.ELFBrowsePanel.Name = "ELFBrowsePanel";
            this.ELFBrowsePanel.Size = new System.Drawing.Size(309, 20);
            this.ELFBrowsePanel.TabIndex = 28;
            // 
            // FilenamePanel
            // 
            this.FilenamePanel.Controls.Add(this.ELFBrowsePanel);
            this.FilenamePanel.Controls.Add(this.CILBrowsePanel);
            this.FilenamePanel.Controls.Add(this.LoadCIL);
            this.FilenamePanel.Controls.Add(this.LoadELF);
            this.FilenamePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FilenamePanel.Location = new System.Drawing.Point(0, 0);
            this.FilenamePanel.Name = "FilenamePanel";
            this.FilenamePanel.Size = new System.Drawing.Size(419, 48);
            this.FilenamePanel.TabIndex = 29;
            // 
            // SimulationButtonPanel
            // 
            this.SimulationButtonPanel.Controls.Add(this.PauseButton);
            this.SimulationButtonPanel.Controls.Add(this.StartButton);
            this.SimulationButtonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SimulationButtonPanel.Location = new System.Drawing.Point(0, 383);
            this.SimulationButtonPanel.Name = "SimulationButtonPanel";
            this.SimulationButtonPanel.Size = new System.Drawing.Size(419, 35);
            this.SimulationButtonPanel.TabIndex = 30;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // DisassembleElf
            // 
            this.DisassembleElf.Dock = System.Windows.Forms.DockStyle.Right;
            this.DisassembleElf.Location = new System.Drawing.Point(208, 0);
            this.DisassembleElf.Name = "DisassembleElf";
            this.DisassembleElf.Size = new System.Drawing.Size(77, 20);
            this.DisassembleElf.TabIndex = 3;
            this.DisassembleElf.Text = "Disassemble";
            this.DisassembleElf.UseVisualStyleBackColor = true;
            this.DisassembleElf.Click += new System.EventHandler(this.DisassembleElf_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "s";
            this.saveFileDialog.Filter = "Assembly files (*.s, *.asm)|*.s;*.asm|All files (*.*)|*.*";
            this.saveFileDialog.Title = "Selft file to save disassembly in";
            // 
            // Simulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 418);
            this.Controls.Add(this.SimulationControls);
            this.Controls.Add(this.SimulationButtonPanel);
            this.Controls.Add(this.FilenamePanel);
            this.Name = "Simulator";
            this.Text = "SPE Simulator";
            this.Load += new System.EventHandler(this.Simulator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OutMboxValue)).EndInit();
            this.SimulationControls.ResumeLayout(false);
            this.SimulationControls.PerformLayout();
            this.CILBrowsePanel.ResumeLayout(false);
            this.CILBrowsePanel.PerformLayout();
            this.ELFBrowsePanel.ResumeLayout(false);
            this.ELFBrowsePanel.PerformLayout();
            this.FilenamePanel.ResumeLayout(false);
            this.FilenamePanel.PerformLayout();
            this.SimulationButtonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox ELFFilename;
        private System.Windows.Forms.Button BrowseELFFile;
        private System.Windows.Forms.RadioButton LoadELF;
        private System.Windows.Forms.RadioButton LoadCIL;
        private System.Windows.Forms.Button BrowseCILFile;
        private System.Windows.Forms.TextBox CILFilename;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button PauseButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox OutputText;
        private System.Windows.Forms.Button ReadNormalMboxButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label NormalMboxStatus;
        private System.Windows.Forms.CheckBox AutoReadNormalMbox;
        private System.Windows.Forms.CheckBox AutoReadIntrMbox;
        private System.Windows.Forms.Label IntrMboxStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ReadIntrMboxButton;
        private System.Windows.Forms.Label OutMboxStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button WriteOutMboxButton;
        private System.Windows.Forms.NumericUpDown OutMboxValue;
        private System.Windows.Forms.Panel SimulationControls;
        private System.Windows.Forms.Panel CILBrowsePanel;
        private System.Windows.Forms.Panel ELFBrowsePanel;
        private System.Windows.Forms.Panel FilenamePanel;
        private System.Windows.Forms.Panel SimulationButtonPanel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button DisassembleElf;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

