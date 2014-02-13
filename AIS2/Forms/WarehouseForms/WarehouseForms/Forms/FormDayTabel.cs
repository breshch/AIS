using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarehouseForms.Forms.Adding;

namespace WarehouseForms.Forms
{
    public partial class FormDayTabel : Form
    {
        public FormDayTabel()
        {
            InitializeComponent();
        }

        private void FormDayTabel_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void типКомпанииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormTypeOfCompany())
            {
                form.ShowDialog();
            }
        }

        private void названиеКомпанииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormNameOfCompany())
            {
                form.ShowDialog();
            }
        }

        private void названиеДолжностиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormTypeOfPost())
            {
                form.ShowDialog();
            }
        }

        private void должностьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormPost())
            {
                form.ShowDialog();   
            }
        }
    }
}
