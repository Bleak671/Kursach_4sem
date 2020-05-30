using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursach_
{
    public partial class FormEnterance : Form
    {
        public FormEnterance()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormMenu frm = new FormMenu(textBoxName.Text);
            frm.Show();
        }
    }
}
