namespace AppGenerator
{
    partial class StaticAppGenerator
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGrammer = new System.Windows.Forms.TextBox();
            this.btnBrowseGrammer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(274, 168);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.GenerateButton);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(12, 122);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(512, 22);
            this.txtPath.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Enabled = false;
            this.btnBrowse.Location = new System.Drawing.Point(166, 168);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(100, 28);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Choose your project path:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Choose your grammer:";
            // 
            // txtGrammer
            // 
            this.txtGrammer.Location = new System.Drawing.Point(12, 33);
            this.txtGrammer.Name = "txtGrammer";
            this.txtGrammer.Size = new System.Drawing.Size(512, 22);
            this.txtGrammer.TabIndex = 4;
            // 
            // btnBrowseGrammer
            // 
            this.btnBrowseGrammer.Location = new System.Drawing.Point(190, 62);
            this.btnBrowseGrammer.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseGrammer.Name = "btnBrowseGrammer";
            this.btnBrowseGrammer.Size = new System.Drawing.Size(159, 28);
            this.btnBrowseGrammer.TabIndex = 6;
            this.btnBrowseGrammer.Text = "Browse Grammer";
            this.btnBrowseGrammer.UseVisualStyleBackColor = true;
            this.btnBrowseGrammer.Click += new System.EventHandler(this.BtnBrowseGrammer_Click);
            // 
            // StaticAppGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 205);
            this.Controls.Add(this.btnBrowseGrammer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtGrammer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "StaticAppGenerator";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGenerator_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtGrammer;
        private System.Windows.Forms.Button btnBrowseGrammer;
    }
}

