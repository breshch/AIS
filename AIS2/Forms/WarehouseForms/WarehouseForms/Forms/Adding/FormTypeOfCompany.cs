using ModelDB;
using ModelDB.Directories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarehouseForms.Forms.Adding
{
    public partial class FormTypeOfCompany : Form
    {
        private QueryTemplates _qt;

        public FormTypeOfCompany()
        {
            InitializeComponent();
        }

        private void FormTypeOfCompany_Load(object sender, EventArgs e)
        {
            var db = new Context();
            _qt = new QueryTemplates(db);

            listBoxTypeOfCompanies.Items.Clear();
            FormFill();
        }

        private void FormFill()
        {
            ClearForm();
            listBoxTypeOfCompanies.Items.AddRange(_qt.GetDirectoryTypeOfCompanyNames().ToArray());

            buttonRemove.Enabled = listBoxTypeOfCompanies.Items.Count > 0 ? true : false;
        }

        private void FormTypeOfCompany_FormClosing(object sender, FormClosingEventArgs e)
        {
            _qt.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (IsValidateAdd())
            {
                _qt.AddDirectoryTypeOfCompany(textBoxTypeOfCompany.Text);
                _qt.Save();

                ClearForm();
                FormFill();
            }
        }

        private bool IsValidateAdd()
        {
            if (!string.IsNullOrWhiteSpace(textBoxTypeOfCompany.Text))
            {
                if (!_qt.GetDirectoryTypeOfCompanyNames().Contains(textBoxTypeOfCompany.Text))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Такой тип компании уже существует." + Environment.NewLine + "Введите другой тип компании.");
                }
            }
            else
            {
                MessageBox.Show("Заполните тип компании.");
            }

            return false;
        }

        private bool IsValidateRemove(DirectoryTypeOfCompany directoryTypeOfCompany)
        {
            if (!_qt.GetDirectoryTypeOfCompanyIds().Contains(directoryTypeOfCompany.Id))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Невозможно удалить данный тип компании," + Environment.NewLine + "так как существует компания, принадлежащая типу "
                    + directoryTypeOfCompany.Name.ToString());
            }
            return false;
        }

        private void ClearForm()
        {
            listBoxTypeOfCompanies.Items.Clear();
            textBoxTypeOfCompany.Clear();
        }

        private void textBoxTypeOfCompany_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listBoxTypeOfCompanies.SelectedIndex != -1)
            {
                string typeOfCompany = listBoxTypeOfCompanies.SelectedItem.ToString();
                var directoryTypeOfCompany = _qt.GetDirectoryTypeOfCompany(typeOfCompany);

                if (IsValidateRemove(directoryTypeOfCompany))
                {
                    _qt.RemoveDirectoryTypeOfCompany(directoryTypeOfCompany);
                    _qt.Save();

                    FormFill();
                }
            }
        }
    }
}
