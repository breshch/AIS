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
using System.Data.Entity;

namespace WarehouseForms.Forms.Adding
{
    public partial class FormNameOfCompany : Form
    {
        private QueryTemplates _qt;

       

        public FormNameOfCompany()
        {
            InitializeComponent();
        }

        private void FormNameOfCompany_Load(object sender, EventArgs e)
        {
            var db = new Context();
            _qt = new QueryTemplates(db);

            FormFill();
            comboBoxTypeOfCompanies.Items.AddRange(_qt.GetDirectoryTypeOfCompanyNames().ToArray());
        }

        private void FormFill()
        {
            ClearForm();

            AddRows();

            buttonRemove.Enabled = dataGridViewNameOfCompanies.Rows.Count > 0 ? true : false;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (IsValidateAdd())
            {
                string typeOfCompany = comboBoxTypeOfCompanies.SelectedItem.ToString();

                var directoryTypeOfCompany = _qt.GetDirectoryTypeOfCompany(typeOfCompany);
                _qt.AddDirectoryCompany(textBoxNameOfCompany.Text, directoryTypeOfCompany);
                _qt.Save();

                ClearForm();
                FormFill();
            }
        }

        private void AddRows()
        {

            dataGridViewNameOfCompanies.Rows.Clear();
            foreach (var nameOfCompany in _qt.GetDirectoryCompanies().ToList())
            {
                var row = new DataGridViewRow();
                row.CreateCells(dataGridViewNameOfCompanies);
                
                row.Cells[0].Value = nameOfCompany.Id;
                row.Cells[1].Value = nameOfCompany.Name;
                row.Cells[2].Value = nameOfCompany.DirectoryTypeOfCompany.Name;
                
                dataGridViewNameOfCompanies.Rows.Add(row);
            }
        }

        private bool IsValidateAdd()
        {
            if (!string.IsNullOrWhiteSpace(textBoxNameOfCompany.Text))
            {
                if (comboBoxTypeOfCompanies.SelectedIndex != -1)
                {
                    if (!_qt.GetDirectoryCompanyNames().Contains(textBoxNameOfCompany.Text))
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
            
            textBoxNameOfCompany.Clear();
            comboBoxTypeOfCompanies.SelectedIndex = -1;
        }

        private void textBoxNameOfCompany_TextChanged(object sender, EventArgs e)
        {

        }

        private bool IsValidateRemove(DirectoryCompany directoryCompany)
        {
            if (!_qt.GetCurrentCompanyIds().Contains(directoryCompany.Id))
            {
                return true;
            }
            else
            {
                MessageBox.Show("В этой компании уже есть сотрудники.");
            }
            return false;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (dataGridViewNameOfCompanies.SelectedRows.Count > 0)
            {
                int id = int.Parse(dataGridViewNameOfCompanies.SelectedRows[0].Cells[0].Value.ToString());
                var directoryCompany = _qt.GetDirectoryCompany(id);

                if (IsValidateRemove(directoryCompany))
                {
                    _qt.RemoveDirectoryCompany(directoryCompany);

                    _qt.Save();

                    FormFill();
                }
            }
        }

        private void FormNameOfCompany_FormClosing(object sender, FormClosingEventArgs e)
        {
            _qt.Close();
        }
    }
}
