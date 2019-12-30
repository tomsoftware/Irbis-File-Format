namespace Irbis_Format
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.labFrameCount = new System.Windows.Forms.Label();
            this.labFrameCountNo = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.picResult = new System.Windows.Forms.PictureBox();
            this.lblMax = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTextInfo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFrameIndex = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picResult)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(625, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(70, 45);
            this.button1.TabIndex = 0;
            this.button1.Text = "load";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(12, 25);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(409, 20);
            this.txtFileName.TabIndex = 1;
            this.txtFileName.Text = "C:\\Irbis-Format\\start.irb";
            // 
            // labFrameCount
            // 
            this.labFrameCount.AutoSize = true;
            this.labFrameCount.Location = new System.Drawing.Point(506, 66);
            this.labFrameCount.Name = "labFrameCount";
            this.labFrameCount.Size = new System.Drawing.Size(69, 13);
            this.labFrameCount.TabIndex = 2;
            this.labFrameCount.Text = "Frame count:";
            // 
            // labFrameCountNo
            // 
            this.labFrameCountNo.AutoSize = true;
            this.labFrameCountNo.Location = new System.Drawing.Point(524, 79);
            this.labFrameCountNo.Name = "labFrameCountNo";
            this.labFrameCountNo.Size = new System.Drawing.Size(10, 13);
            this.labFrameCountNo.TabIndex = 3;
            this.labFrameCountNo.Text = "-";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(625, 62);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 45);
            this.button2.TabIndex = 4;
            this.button2.Text = "next frame";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // picResult
            // 
            this.picResult.Location = new System.Drawing.Point(127, 53);
            this.picResult.Name = "picResult";
            this.picResult.Size = new System.Drawing.Size(364, 271);
            this.picResult.TabIndex = 5;
            this.picResult.TabStop = false;
            // 
            // lblMax
            // 
            this.lblMax.AutoSize = true;
            this.lblMax.Location = new System.Drawing.Point(524, 158);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(10, 13);
            this.lblMax.TabIndex = 7;
            this.lblMax.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(506, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "max-value:";
            // 
            // lblMin
            // 
            this.lblMin.AutoSize = true;
            this.lblMin.Location = new System.Drawing.Point(524, 197);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(10, 13);
            this.lblMin.TabIndex = 9;
            this.lblMin.Text = "-";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(506, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "min-value:";
            // 
            // lblTextInfo
            // 
            this.lblTextInfo.AutoSize = true;
            this.lblTextInfo.Location = new System.Drawing.Point(12, 66);
            this.lblTextInfo.Name = "lblTextInfo";
            this.lblTextInfo.Size = new System.Drawing.Size(10, 13);
            this.lblTextInfo.TabIndex = 10;
            this.lblTextInfo.Text = "-";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Text:";
            // 
            // lblFrameIndex
            // 
            this.lblFrameIndex.AutoSize = true;
            this.lblFrameIndex.Location = new System.Drawing.Point(524, 119);
            this.lblFrameIndex.Name = "lblFrameIndex";
            this.lblFrameIndex.Size = new System.Drawing.Size(10, 13);
            this.lblFrameIndex.TabIndex = 13;
            this.lblFrameIndex.Text = "-";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(506, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Frame-index:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Source";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 414);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblFrameIndex);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTextInfo);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblMax);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.picResult);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.labFrameCountNo);
            this.Controls.Add(this.labFrameCount);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "IRBIS File Format";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label labFrameCount;
        private System.Windows.Forms.Label labFrameCountNo;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox picResult;
        private System.Windows.Forms.Label lblMax;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTextInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFrameIndex;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
    }
}

