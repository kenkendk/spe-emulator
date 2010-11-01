namespace SPEEmulatorTestApp
{
    partial class Memory
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxHex = new System.Windows.Forms.CheckBox();
            this.checkBoxASCII = new System.Windows.Forms.CheckBox();
            this.checkBoxInst = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(12, 52);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(580, 700);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Adress";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(250, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Hex (4x word)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(488, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "ASCII";
            // 
            // checkBoxHex
            // 
            this.checkBoxHex.AutoSize = true;
            this.checkBoxHex.Checked = true;
            this.checkBoxHex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHex.Location = new System.Drawing.Point(12, 12);
            this.checkBoxHex.Name = "checkBoxHex";
            this.checkBoxHex.Size = new System.Drawing.Size(78, 17);
            this.checkBoxHex.TabIndex = 7;
            this.checkBoxHex.Text = "Show HEX";
            this.checkBoxHex.UseVisualStyleBackColor = true;
            this.checkBoxHex.CheckedChanged += new System.EventHandler(this.checkBoxInst_CheckedChanged);
            // 
            // checkBoxASCII
            // 
            this.checkBoxASCII.AutoSize = true;
            this.checkBoxASCII.Checked = true;
            this.checkBoxASCII.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxASCII.Location = new System.Drawing.Point(96, 12);
            this.checkBoxASCII.Name = "checkBoxASCII";
            this.checkBoxASCII.Size = new System.Drawing.Size(83, 17);
            this.checkBoxASCII.TabIndex = 8;
            this.checkBoxASCII.Text = "Show ASCII";
            this.checkBoxASCII.UseVisualStyleBackColor = true;
            this.checkBoxASCII.CheckedChanged += new System.EventHandler(this.checkBoxInst_CheckedChanged);
            // 
            // checkBoxInst
            // 
            this.checkBoxInst.AutoSize = true;
            this.checkBoxInst.Location = new System.Drawing.Point(185, 12);
            this.checkBoxInst.Name = "checkBoxInst";
            this.checkBoxInst.Size = new System.Drawing.Size(110, 17);
            this.checkBoxInst.TabIndex = 9;
            this.checkBoxInst.Text = "Show Instructions";
            this.checkBoxInst.UseVisualStyleBackColor = true;
            this.checkBoxInst.CheckedChanged += new System.EventHandler(this.checkBoxInst_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1135, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Instruction";
            this.label2.Visible = false;
            // 
            // Memory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 764);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBoxInst);
            this.Controls.Add(this.checkBoxASCII);
            this.Controls.Add(this.checkBoxHex);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "Memory";
            this.Text = "Memory";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxHex;
        private System.Windows.Forms.CheckBox checkBoxASCII;
        private System.Windows.Forms.CheckBox checkBoxInst;
        private System.Windows.Forms.Label label2;
    }
}