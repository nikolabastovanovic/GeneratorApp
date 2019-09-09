namespace AppGenerator
{
    partial class FormGenerator
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
            this.txtPageNames = new System.Windows.Forms.TextBox();
            this.comboBoxPagesCollection = new System.Windows.Forms.ComboBox();
            this.checkBoxCreateSideBar = new System.Windows.Forms.CheckBox();
            this.buttonInsertImage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(146, 354);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.GenerateButton);
            // 
            // txtPageNames
            // 
            this.txtPageNames.Location = new System.Drawing.Point(12, 12);
            this.txtPageNames.Name = "txtPageNames";
            this.txtPageNames.Size = new System.Drawing.Size(200, 20);
            this.txtPageNames.TabIndex = 1;
            this.txtPageNames.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtPageNames_KeyPress);
            // 
            // comboBoxPagesCollection
            // 
            this.comboBoxPagesCollection.FormattingEnabled = true;
            this.comboBoxPagesCollection.Location = new System.Drawing.Point(12, 38);
            this.comboBoxPagesCollection.Name = "comboBoxPagesCollection";
            this.comboBoxPagesCollection.Size = new System.Drawing.Size(200, 21);
            this.comboBoxPagesCollection.TabIndex = 2;
            this.comboBoxPagesCollection.SelectedIndexChanged += new System.EventHandler(this.ComboBoxPagesCollection_SelectedIndexChanged);
            // 
            // checkBoxCreateSideBar
            // 
            this.checkBoxCreateSideBar.AutoSize = true;
            this.checkBoxCreateSideBar.Location = new System.Drawing.Point(257, 14);
            this.checkBoxCreateSideBar.Name = "checkBoxCreateSideBar";
            this.checkBoxCreateSideBar.Size = new System.Drawing.Size(99, 17);
            this.checkBoxCreateSideBar.TabIndex = 3;
            this.checkBoxCreateSideBar.Text = "Create Side bar";
            this.checkBoxCreateSideBar.UseVisualStyleBackColor = true;
            this.checkBoxCreateSideBar.CheckedChanged += new System.EventHandler(this.CheckBoxCreateSideBar_CheckedChanged);
            // 
            // buttonInsertImage
            // 
            this.buttonInsertImage.Location = new System.Drawing.Point(292, 243);
            this.buttonInsertImage.Name = "buttonInsertImage";
            this.buttonInsertImage.Size = new System.Drawing.Size(99, 23);
            this.buttonInsertImage.TabIndex = 4;
            this.buttonInsertImage.Text = "Insert image";
            this.buttonInsertImage.UseVisualStyleBackColor = true;
            this.buttonInsertImage.Visible = false;
            this.buttonInsertImage.Click += new System.EventHandler(this.ButtonInsertImage_Click);
            // 
            // FormGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 389);
            this.Controls.Add(this.buttonInsertImage);
            this.Controls.Add(this.checkBoxCreateSideBar);
            this.Controls.Add(this.comboBoxPagesCollection);
            this.Controls.Add(this.txtPageNames);
            this.Controls.Add(this.button1);
            this.Name = "FormGenerator";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtPageNames;
        private System.Windows.Forms.ComboBox comboBoxPagesCollection;
        private System.Windows.Forms.CheckBox checkBoxCreateSideBar;
        private System.Windows.Forms.Button buttonInsertImage;
    }
}

