using ModelDB;
using ModelDB.Currents;
using ModelDB.Directories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarehouseForms.Forms.Adding
{
    public partial class FormAddComanyAnadPostToWorker : Form
    {
        public class CurrentTempPost
        {
            public DateTime Date { get; set; }
            public string CompanyName { get; set; }
            public string  PostName { get; set; }
        }

        private QueryTemplates _qt;
        private CurrentTempPost _post;

        public FormAddComanyAnadPostToWorker()
        {
            InitializeComponent();

            Context db = new Context();
            _qt = new QueryTemplates(db);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FillCompanyAndPostToWorker();
        }

        private bool IsValidateCompanyAndPost()
        {
            {
                if (comboBoxCompanies.SelectedIndex != -1)
                {
                    if (comboBoxPosts.SelectedIndex != -1)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Невыбрана должность");
                    }
                }
                else
                {
                    MessageBox.Show("Невыбрана компания");
                }
            }
            return false;
        }

        public CurrentTempPost GetPost()
        {
            return _post;
        }

        private void FillCompanyAndPostToWorker()
        {
            if (IsValidateCompanyAndPost())
            {
                _post = new CurrentTempPost 
                { 
                    CompanyName = comboBoxCompanies.SelectedItem.ToString(),
                    PostName = comboBoxPosts.SelectedItem.ToString(),
                    Date = dateTimePickerChangeDate.Value
                };
                this.Close();
            }
        }

        private void FormAddComanyAnadPostToWorker_Load(object sender, EventArgs e)
        {
            comboBoxCompanies.Items.AddRange(_qt.GetDirectoryCompanyNames().ToArray());
        }

        private void comboBoxCompanyies_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nameOfCompany = comboBoxCompanies.SelectedItem.ToString();
           
            comboBoxPosts.Items.Clear();
            comboBoxPosts.Items.AddRange(_qt.GetDirectoryPostNames(nameOfCompany).ToArray());
        }

        private void FormAddComanyAnadPostToWorker_FormClosing(object sender, FormClosingEventArgs e)
        {
            _qt.Close();
        }
    }
}
