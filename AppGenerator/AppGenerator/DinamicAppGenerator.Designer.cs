namespace AppGenerator
{
    partial class DinamicAppGenerator
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
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGrammer = new System.Windows.Forms.TextBox();
            this.txtSourceCodeDest = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxGenerateDbScript = new System.Windows.Forms.CheckBox();
            this.btnBrowseGrammer = new System.Windows.Forms.Button();
            this.btnBrowsePath = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.Enabled = false;
            this.btnGenerate.Location = new System.Drawing.Point(268, 230);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(100, 28);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose Your grammer:";
            // 
            // txtGrammer
            // 
            this.txtGrammer.Location = new System.Drawing.Point(15, 29);
            this.txtGrammer.Name = "txtGrammer";
            this.txtGrammer.Size = new System.Drawing.Size(614, 22);
            this.txtGrammer.TabIndex = 2;
            // 
            // txtSourceCodeDest
            // 
            this.txtSourceCodeDest.Location = new System.Drawing.Point(15, 116);
            this.txtSourceCodeDest.Name = "txtSourceCodeDest";
            this.txtSourceCodeDest.Size = new System.Drawing.Size(614, 22);
            this.txtSourceCodeDest.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(284, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Choose generated source code destination:";
            // 
            // cbxGenerateDbScript
            // 
            this.cbxGenerateDbScript.AutoSize = true;
            this.cbxGenerateDbScript.Location = new System.Drawing.Point(15, 191);
            this.cbxGenerateDbScript.Name = "cbxGenerateDbScript";
            this.cbxGenerateDbScript.Size = new System.Drawing.Size(195, 21);
            this.cbxGenerateDbScript.TabIndex = 5;
            this.cbxGenerateDbScript.Text = "Generate Database Script";
            this.cbxGenerateDbScript.UseVisualStyleBackColor = true;
            // 
            // btnBrowseGrammer
            // 
            this.btnBrowseGrammer.Location = new System.Drawing.Point(239, 57);
            this.btnBrowseGrammer.Name = "btnBrowseGrammer";
            this.btnBrowseGrammer.Size = new System.Drawing.Size(162, 24);
            this.btnBrowseGrammer.TabIndex = 6;
            this.btnBrowseGrammer.Text = "Browse Grammer";
            this.btnBrowseGrammer.UseVisualStyleBackColor = true;
            this.btnBrowseGrammer.Click += new System.EventHandler(this.BtnBrowseGrammer_Click);
            // 
            // btnBrowsePath
            // 
            this.btnBrowsePath.Enabled = false;
            this.btnBrowsePath.Location = new System.Drawing.Point(268, 144);
            this.btnBrowsePath.Name = "btnBrowsePath";
            this.btnBrowsePath.Size = new System.Drawing.Size(100, 29);
            this.btnBrowsePath.TabIndex = 7;
            this.btnBrowsePath.Text = "Browse";
            this.btnBrowsePath.UseVisualStyleBackColor = true;
            this.btnBrowsePath.Click += new System.EventHandler(this.BtnBrowsePath_Click);
            // 
            // DinamicAppGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 271);
            this.Controls.Add(this.btnBrowsePath);
            this.Controls.Add(this.btnBrowseGrammer);
            this.Controls.Add(this.cbxGenerateDbScript);
            this.Controls.Add(this.txtSourceCodeDest);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtGrammer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGenerate);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "DinamicAppGenerator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DinamicAppGenerator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGrammer;
        private System.Windows.Forms.TextBox txtSourceCodeDest;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbxGenerateDbScript;
        private System.Windows.Forms.Button btnBrowseGrammer;
        private System.Windows.Forms.Button btnBrowsePath;
    }
}