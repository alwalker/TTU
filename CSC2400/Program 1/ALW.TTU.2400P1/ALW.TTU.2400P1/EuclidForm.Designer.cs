namespace ALW.TTU._2400P1
{
    partial class EuclidForm
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
            this.lblM = new System.Windows.Forms.Label();
            this.txtN = new System.Windows.Forms.TextBox();
            this.btnStep = new System.Windows.Forms.Button();
            this.lstSteps = new System.Windows.Forms.ListBox();
            this.lblN = new System.Windows.Forms.Label();
            this.txtM = new System.Windows.Forms.TextBox();
            this.lblState = new System.Windows.Forms.Label();
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.grpInput.SuspendLayout();
            this.SuspendLayout();
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
            // btnStep
            // 
            this.btnStep.AutoSize = true;
            this.btnStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStep.Location = new System.Drawing.Point(16, 138);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(89, 31);
            this.btnStep.TabIndex = 0;
            this.btnStep.Text = "Next Step";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // lstSteps
            // 
            this.lstSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSteps.FormattingEnabled = true;
            this.lstSteps.ItemHeight = 20;
            this.lstSteps.Location = new System.Drawing.Point(14, 175);
            this.lstSteps.Name = "lstSteps";
            this.lstSteps.Size = new System.Drawing.Size(585, 224);
            this.lstSteps.TabIndex = 3;
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
            // txtM
            // 
            this.txtM.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtM.Location = new System.Drawing.Point(66, 21);
            this.txtM.Name = "txtM";
            this.txtM.Size = new System.Drawing.Size(100, 26);
            this.txtM.TabIndex = 0;
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblState.Location = new System.Drawing.Point(111, 143);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(109, 20);
            this.lblState.TabIndex = 6;
            this.lblState.Text = "Current State:";
            // 
            // grpInput
            // 
            this.grpInput.Controls.Add(this.txtM);
            this.grpInput.Controls.Add(this.lblM);
            this.grpInput.Controls.Add(this.txtN);
            this.grpInput.Controls.Add(this.lblN);
            this.grpInput.Location = new System.Drawing.Point(205, 12);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(200, 100);
            this.grpInput.TabIndex = 7;
            this.grpInput.TabStop = false;
            // 
            // EuclidForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 410);
            this.Controls.Add(this.grpInput);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.lstSteps);
            this.Controls.Add(this.btnStep);
            this.Name = "EuclidForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Euclid Algorithm";
            this.Load += new System.EventHandler(this.EuclidForm_Load);
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblM;
        private System.Windows.Forms.TextBox txtN;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.ListBox lstSteps;
        private System.Windows.Forms.Label lblN;
        private System.Windows.Forms.TextBox txtM;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.GroupBox grpInput;
    }
}