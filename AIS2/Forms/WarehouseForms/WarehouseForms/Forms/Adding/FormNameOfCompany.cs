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
    public partial class FormNameOfCompany : Form
    {
        private Context _db = new Context();

        public FormNameOfCompany()
        {
            InitializeComponent();
        }

        private void FormNameOfCompany_Load(object sender, EventArgs e)
        {
            comboBoxTypeOfCompanies.Items.AddRange(_db.DirectoryTypeOfCompanies.Select(t => t.Name).ToArray());
            FormFill();
        }

        private void FormFill()
        {
            ClearForm();
            listBoxNameOfCompanies.Items.AddRange(_db.DirectoryCompanies.Select(t => t.Name).ToArray());
            buttonRemove.Enabled = listBoxNameOfCompanies.Items.Count > 0 ? true : false;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (IsValidateAdd())
            {
                string typeOfCompany = comboBoxTypeOfCompanies.SelectedItem.ToString();

                var directoryTypeOfCompany = _db.DirectoryTypeOfCompanies.First(t => t.Name == typeOfCompany);
                _db.DirectoryCompanies.Add(new DirectoryCompany { Name = textBoxNameOfCompany.Text, DirectoryTypeOfCompany = directoryTypeOfCompany });
                _db.SaveChanges();

                ClearForm();
                FormFill();
            }
        }

        private bool IsValidateAdd()
        {
            if (!string.IsNullOrWhiteSpace(textBoxNameOfCompany.Text))
            {
                if (comboBoxTypeOfCompanies.SelectedIndex != -1)
                {
                    if (!_db.DirectoryCompanies.Select(t => t.Name).Contains(textBoxNameOfCompany.Text))
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Компания с таким названием уже существует." + Environment.NewLine + "Введите другое название компании.");
                    }
                }
                else
                {
                    MessageBox.Show("Не выбран тип компании.");
                }
            }
            else
            {
                MessageBox.Show("Заполните название компании.");
            }

            return false;
        }

        private void ClearForm()
        {
            listBoxNameOfCompanies.Items.Clear();
            textBoxNameOfCompany.Clear();
            comboBoxTypeOfCompanies.SelectedIndex = -1;
        }

        private void textBoxNameOfCompany_TextChanged(object sender, EventArgs e)
        {

        }

        private bool IsValidateRemove(DirectoryCompany directoryCompany)
        {
            if (!_db.InfoCompanies.Select(c => c.DirectoryCompanyId).Contains(directoryCompany.Id))
            {
                return true;
            }
            else
            {
                MessageBox.Show(" В этой компании уже есть сотрудники ");
            }
            return false;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listBoxNameOfCompanies.SelectedIndex != -1)
            {
                string nameOfCompanies = listBoxNameOfCompanies.SelectedItem.ToString();
                DirectoryCompany directoryCompany = _db.DirectoryCompanies.First(t => t.Name == nameOfCompanies);

                if (IsValidateRemove(directoryCompany))
                {
                    _db.DirectoryCompanies.Remove(directoryCompany);
                    _db.SaveChanges();

                    FormFill();
                }
            }
        }

        private void FormNameOfCompany_FormClosing(object sender, FormClosingEventArgs e)
        {
            _db.Dispose();
        }
    }
}
