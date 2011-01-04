namespace SPEEmulatorTestApp
{
    partial class EditMemForm
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
            this.components = new System.ComponentModel.Container();
            this.lsWord = new System.Windows.Forms.TextBox();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.updateBtn = new System.Windows.Forms.Button();
            this.lsAddress = new System.Windows.Forms.NumericUpDown();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.lsAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // lsWord
            // 
            this.lsWord.Location = new System.Drawing.Point(44, 59);
            this.lsWord.Name = "lsWord";
            this.lsWord.Size = new System.Drawing.Size(100, 20);
            this.lsWord.TabIndex = 2;
            this.lsWord.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(137, 98);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(75, 23);
            this.CloseBtn.TabIndex = 3;
            this.CloseBtn.Text = "Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // updateBtn
            // 
            this.updateBtn.Location = new System.Drawing.Point(44, 98);
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.Size = new System.Drawing.Size(75, 23);
            this.updateBtn.TabIndex = 4;
            this.updateBtn.Text = "Update";
            this.updateBtn.UseVisualStyleBackColor = true;
            this.updateBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // lsAddress
            // 
            this.lsAddress.Hexadecimal = true;
            this.lsAddress.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.lsAddress.Location = new System.Drawing.Point(44, 28);
            this.lsAddress.Name = "lsAddress";
            this.lsAddress.Size = new System.Drawing.Size(120, 20);
            this.lsAddress.TabIndex = 1;
            this.lsAddress.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // EditMemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 145);
            this.Controls.Add(this.lsAddress);
            this.Controls.Add(this.updateBtn);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.lsWord);
            this.Name = "EditMemForm";
            this.Text = "Edit memory";
            ((System.ComponentModel.ISupportInitialize)(this.lsAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox lsWord;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.Button updateBtn;
        private System.Windows.Forms.NumericUpDown lsAddress;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}