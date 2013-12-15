namespace ALW.TTU._2400P1
{
    partial class MainForm
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
            this.btnEuclid = new System.Windows.Forms.Button();
            this.btnErastothanes = new System.Windows.Forms.Button();
            this.grpAlgorithm = new System.Windows.Forms.GroupBox();
            this.grpAlgorithm.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEuclid
            // 
            this.btnEuclid.AutoSize = true;
            this.btnEuclid.Location = new System.Drawing.Point(79, 35);
            this.btnEuclid.Name = "btnEuclid";
            this.btnEuclid.Size = new System.Drawing.Size(115, 30);
            this.btnEuclid.TabIndex = 0;
            this.btnEuclid.Text = "Euclid";
            this.btnEuclid.UseVisualStyleBackColor = true;
            this.btnEuclid.Click += new System.EventHandler(this.btnEuclid_Click);
            // 
            // btnErastothanes
            // 
            this.btnErastothanes.AutoSize = true;
            this.btnErastothanes.Location = new System.Drawing.Point(79, 71);
            this.btnErastothanes.Name = "btnErastothanes";
            this.btnErastothanes.Size = new System.Drawing.Size(115, 30);
            this.btnErastothanes.TabIndex = 1;
            this.btnErastothanes.Text = "Erastothanes";
            this.btnErastothanes.UseVisualStyleBackColor = true;
            this.btnErastothanes.Click += new System.EventHandler(this.btnErastothanes_Click);
            // 
            // grpAlgorithm
            // 
            this.grpAlgorithm.Controls.Add(this.btnErastothanes);
            this.grpAlgorithm.Controls.Add(this.btnEuclid);
            this.grpAlgorithm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpAlgorithm.Location = new System.Drawing.Point(9, 13);
            this.grpAlgorithm.Name = "grpAlgorithm";
            this.grpAlgorithm.Size = new System.Drawing.Size(272, 136);
            this.grpAlgorithm.TabIndex = 2;
            this.grpAlgorithm.TabStop = false;
            this.grpAlgorithm.Text = "Choose a GCD Algorithm";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 162);
            this.Controls.Add(this.grpAlgorithm);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CSC2400 Program 1";
            this.grpAlgorithm.ResumeLayout(false);
            this.grpAlgorithm.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEuclid;
        private System.Windows.Forms.Button btnErastothanes;
        private System.Windows.Forms.GroupBox grpAlgorithm;
    }
}