using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppGenerator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            StaticAppGenerator form = new StaticAppGenerator();
            form.Show();
            this.Hide();
        }

        private void BtnDinamicAppGen_Click(object sender, EventArgs e)
        {
            DinamicAppGenerator form = new DinamicAppGenerator();
            form.Show();
            this.Hide();
        }
    }
}
