namespace SeleniumAutotest
{
    partial class InputDataForm
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
            this.BuOk = new System.Windows.Forms.Button();
            this.TeValue = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BuOk
            // 
            this.BuOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BuOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BuOk.Location = new System.Drawing.Point(12, 64);
            this.BuOk.Name = "BuOk";
            this.BuOk.Size = new System.Drawing.Size(609, 23);
            this.BuOk.TabIndex = 0;
            this.BuOk.Text = "OK";
            this.BuOk.UseVisualStyleBackColor = true;
            this.BuOk.Click += new System.EventHandler(this.BuOk_Click);
            // 
            // TeValue
            // 
            this.TeValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TeValue.Location = new System.Drawing.Point(12, 34);
            this.TeValue.Name = "TeValue";
            this.TeValue.Size = new System.Drawing.Size(609, 20);
            this.TeValue.TabIndex = 1;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(12, 9);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(35, 13);
            this.Label1.TabIndex = 2;
            this.Label1.Text = "label1";
            // 
            // InputDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 99);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.TeValue);
            this.Controls.Add(this.BuOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "InputDataForm";
            this.Text = "Введите данные";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BuOk;
        private System.Windows.Forms.TextBox TeValue;
        private System.Windows.Forms.Label Label1;
    }
}