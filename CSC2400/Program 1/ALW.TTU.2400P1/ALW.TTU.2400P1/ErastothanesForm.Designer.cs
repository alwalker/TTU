namespace ALW.TTU._2400P1
{
    partial class ErastothanesForm
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
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.txtM = new System.Windows.Forms.TextBox();
            this.lblM = new System.Windows.Forms.Label();
            this.txtN = new System.Windows.Forms.TextBox();
            this.lblN = new System.Windows.Forms.Label();
            this.btnStep = new System.Windows.Forms.Button();
            this.grpProgress = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lsbN = new System.Windows.Forms.ListBox();
            this.lsbM = new System.Windows.Forms.ListBox();
            this.lblStep = new System.Windows.Forms.Label();
            this.grpInput.SuspendLayout();
            this.grpProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpInput
            // 
            this.grpInput.Controls.Add(this.txtM);
            this.grpInput.Controls.Add(this.lblM);
            this.grpInput.Controls.Add(this.txtN);
            this.grpInput.Controls.Add(this.lblN);
            this.grpInput.Location = new System.Drawing.Point(42, 12);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(200, 100);
            this.grpInput.TabIndex = 8;
            this.grpInput.TabStop = false;
            // 
            // txtM
            // 
            this.txtM.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtM.Location = new System.Drawing.Point(66, 21);
            this.txtM.Name = "txtM";
            this.txtM.Size = new System.Drawing.Size(100, 26);
            this.txtM.TabIndex = 0;
            // 
            // lblM
            // 
            this.lblM.AutoSize = true;
            this.lblM.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblM.Location = new System.Drawing.Point(34, 24);
            this.lblM.Name = "lblM";
            this.lblM.Size = new System.Drawing.Size(26, 20);
            this.lblM.TabIndex = 0;
            this.lblM.Text = "M:";
            // 
            // txtN
            // 
            this.txtN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtN.Location = new System.Drawing.Point(66, 53);
            this.txtN.Name = "txtN";
            this.txtN.Size = new System.Drawing.Size(100, 26);
            this.txtN.TabIndex = 1;
            // 
            // lblN
            // 
            this.lblN.AutoSize = true;
            this.lblN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblN.Location = new System.Drawing.Point(34, 56);
            this.lblN.Name = "lblN";
            this.lblN.Size = new System.Drawing.Size(24, 20);
            this.lblN.TabIndex = 4;
            this.lblN.Text = "N:";
            // 
            // btnStep
            // 
            this.btnStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStep.Location = new System.Drawing.Point(105, 118);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(75, 32);
            this.btnStep.TabIndex = 11;
            this.btnStep.Text = "Start";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // grpProgress
            // 
            this.grpProgress.Controls.Add(this.label1);
            this.grpProgress.Controls.Add(this.label2);
            this.grpProgress.Controls.Add(this.lsbN);
            this.grpProgress.Controls.Add(this.lsbM);
            this.grpProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpProgress.Location = new System.Drawing.Point(12, 257);
            this.grpProgress.Name = "grpProgress";
            this.grpProgress.Size = new System.Drawing.Size(260, 341);
            this.grpProgress.TabIndex = 12;
            this.grpProgress.TabStop = false;
            this.grpProgress.Text = "Steps";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(56, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "M:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(180, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "N:";
            // 
            // lsbN
            // 
            this.lsbN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsbN.FormattingEnabled = true;
            this.lsbN.ItemHeight = 20;
            this.lsbN.Location = new System.Drawing.Point(132, 51);
            this.lsbN.Name = "lsbN";
            this.lsbN.Size = new System.Drawing.Size(120, 284);
            this.lsbN.TabIndex = 14;
            // 
            // lsbM
            // 
            this.lsbM.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsbM.FormattingEnabled = true;
            this.lsbM.ItemHeight = 20;
            this.lsbM.Location = new System.Drawing.Point(9, 51);
            this.lsbM.Name = "lsbM";
            this.lsbM.Size = new System.Drawing.Size(120, 284);
            this.lsbM.TabIndex = 13;
            // 
            // lblStep
            // 
            this.lblStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStep.Location = new System.Drawing.Point(22, 159);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(241, 98);
            this.lblStep.TabIndex = 13;
            this.lblStep.Text = "Current Step:";
            // 
            // ErastothanesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 610);
            this.Controls.Add(this.lblStep);
            this.Controls.Add(this.grpProgress);
            this.Controls.Add(this.btnStep);
            this.Controls.Add(this.grpInput);
            this.Name = "ErastothanesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ErastothanesForm";
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            this.grpProgress.ResumeLayout(false);
            this.grpProgress.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpInput;
        private System.Windows.Forms.TextBox txtM;
        private System.Windows.Forms.Label lblM;
        private System.Windows.Forms.TextBox txtN;
        private System.Windows.Forms.Label lblN;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.GroupBox grpProgress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lsbN;
        private System.Windows.Forms.ListBox lsbM;
        private System.Windows.Forms.Label lblStep;
    }
}