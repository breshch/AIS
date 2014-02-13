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
        private Context _db = new Context();

        public FormTypeOfCompany()
        {
            InitializeComponent();
        }

        private void FormTypeOfCompany_Load(object sender, EventArgs e)
        {
            listBoxTypeOfCompanies.Items.Clear();
            FormFill();
        }

        private void FormFill()
        {
            ClearForm();
            listBoxTypeOfCompanies.Items.AddRange(_db.DirectoryTypeOfCompanies.Select(t => t.Name).ToArray());

            buttonRemove.Enabled = listBoxTypeOfCompanies.Items.Count > 0 ? true : false;
        }

        private void FormTypeOfCompany_FormClosing(object sender, FormClosingEventArgs e)
        {
            _db.Dispose();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (IsValidateAdd())
            {
                _db.DirectoryTypeOfCompanies.Add(new DirectoryTypeOfCompany { Name = textBoxTypeOfCompany.Text });
                _db.SaveChanges();

                ClearForm();
                FormFill();
            }
        }

        private bool IsValidateAdd()
        {
            if (!string.IsNullOrWhiteSpace(textBoxTypeOfCompany.Text))
            {
                if (!_db.DirectoryTypeOfCompanies.Select(t => t.Name).Contains(textBoxTypeOfCompany.Text))
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
            if (!(_db.DirectoryCompanies.Select(c => c.DirectoryTypeOfCompanyId).Contains(directoryTypeOfCompany.Id)
                || _db.InfoCompanies.Select(c => c.DirectoryCompany.DirectoryTypeOfCompanyId).Contains(directoryTypeOfCompany.Id)))
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
                string typeOfCompanies = listBoxTypeOfCompanies.SelectedItem.ToString();
                DirectoryTypeOfCompany directoryTypeOfCompany = _db.DirectoryTypeOfCompanies.First(t => t.Name == typeOfCompanies);

                if (IsValidateRemove(directoryTypeOfCompany))
                {
                    _db.DirectoryTypeOfCompanies.Remove(directoryTypeOfCompany);
                    _db.SaveChanges();

                    FormFill();
                }
            }
        }
    }
}
