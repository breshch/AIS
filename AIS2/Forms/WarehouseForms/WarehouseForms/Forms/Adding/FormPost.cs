using ModelDB;
using ModelDB.Directories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarehouseForms.Forms.Adding
{
    public partial class FormPost : Form
    {
        private Context _db = new Context();

        public FormPost()
        {
            InitializeComponent();
        }

        private void FormPost_Load(object sender, EventArgs e)
        {
            comboBoxNameOfCompany.Items.AddRange(_db.DirectoryCompanies.Select(c => c.Name).ToArray());
            comboBoxTypeOfPost.Items.AddRange(_db.DirectoryTypeOfPosts.Select(p => p.Name).ToArray());
            dateTimePickerDate.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddDays(1).AddSeconds(-1);

            AddRows();

        }

        private void AddRows()
        {
            dataGridViewPosts.Rows.Clear();

            foreach (var post in _db.DirectoryPosts.ToList())
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridViewPosts);

                row.Cells[0].Value = post.Id;
                row.Cells[1].Value = post.Name;
                row.Cells[2].Value = post.DirectoryTypeOfPost.Name;
                row.Cells[3].Value = post.DirectoryCompany.Name;
                row.Cells[4].Value = post.Date.ToShortDateString();
                row.Cells[5].Value = post.UserWorkerSalary;
                row.Cells[6].Value = post.UserHalfWorkerSalary;

                dataGridViewPosts.Rows.Add(row);
            }
        }

        private void ClearForm()
        {
            textBoxNameOfPost.ResetText();
            textBoxHalfWorkerSalary.ResetText();
            textBoxWorkerSalary.ResetText();

            dateTimePickerDate.Value = DateTime.Now;

            comboBoxTypeOfPost.SelectedIndex = -1;
            comboBoxNameOfCompany.SelectedIndex = -1;
        }

        private void FormFill()
        {
            AddRows();

            ClearForm();
        }

        private void FormPost_FormClosing(object sender, FormClosingEventArgs e)
        {
            _db.Dispose();
        }

        private bool IsValidateAdd()
        {
            if (!string.IsNullOrWhiteSpace(textBoxNameOfPost.Text))
            {
                if (comboBoxTypeOfPost.SelectedIndex != -1)
                {
                    if (comboBoxNameOfCompany.SelectedIndex != -1)
                    {
                        double salary;
                        if (double.TryParse(textBoxWorkerSalary.Text.Replace(".", ","), out salary) && salary > 0)
                        {
                            if (double.TryParse(textBoxHalfWorkerSalary.Text.Replace(".", ","), out salary) && salary >= 0)
                            {
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("Неверно заполнено поле \"совместительство\".");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Неверно заполнено поле \"оклад\".");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите компанию");
                    }
                }
                else
                {
                    MessageBox.Show("Выберите вид должности");
                }
            }
            else
            {
                MessageBox.Show("Неверно заполнено название должности");
            }
            return false;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (IsValidateAdd())
            {
                string nameOfCompany = comboBoxNameOfCompany.SelectedItem.ToString();
                string typeOfPost = comboBoxTypeOfPost.SelectedItem.ToString();
                var post = _db.DirectoryPosts.FirstOrDefault(p => EntityFunctions.DiffDays(p.Date, dateTimePickerDate.Value) == 0 && p.Name == textBoxNameOfPost.Text &&
                    p.DirectoryCompany.Name == nameOfCompany);

                if (post == null)
                {
                    post = new DirectoryPost
                    {
                        Name = textBoxNameOfPost.Text,
                        DirectoryCompany = _db.DirectoryCompanies.First(c => c.Name == nameOfCompany),
                        DirectoryTypeOfPost = _db.DirectoryTypeOfPosts.First(p => p.Name == typeOfPost),
                        Date = dateTimePickerDate.Value,
                        UserWorkerSalary = double.Parse(textBoxWorkerSalary.Text.Replace(".", ",")),
                        UserHalfWorkerSalary = double.Parse(textBoxHalfWorkerSalary.Text.Replace(".", ","))
                    };
                    _db.DirectoryPosts.Add(post);
                }
                else
                {
                    post.Name = textBoxNameOfPost.Text;
                    post.DirectoryCompany = _db.DirectoryCompanies.First(c => c.Name == nameOfCompany);
                    post.DirectoryTypeOfPost = _db.DirectoryTypeOfPosts.First(p => p.Name == typeOfPost);
                    post.Date = dateTimePickerDate.Value;
                    post.UserWorkerSalary = double.Parse(textBoxWorkerSalary.Text.Replace(".", ","));
                    post.UserHalfWorkerSalary = double.Parse(textBoxHalfWorkerSalary.Text.Replace(".", ","));
                }
                
                _db.SaveChanges();

                FormFill();
            }
        }

        private void dataGridViewPosts_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int id = int.Parse(dataGridViewPosts.SelectedRows[0].Cells[0].Value.ToString());
            var post = _db.DirectoryPosts.Find(id);

            textBoxNameOfPost.Text = post.Name;
            comboBoxTypeOfPost.SelectedItem = post.DirectoryTypeOfPost.Name;
            comboBoxNameOfCompany.SelectedItem = post.DirectoryCompany.Name;
            dateTimePickerDate.Value = post.Date;
            textBoxWorkerSalary.Text = post.UserWorkerSalary.ToString();
            textBoxHalfWorkerSalary.Text = post.UserHalfWorkerSalary.ToString();
        }
    }
}
