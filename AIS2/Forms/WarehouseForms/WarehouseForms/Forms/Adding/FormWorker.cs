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
using System.Data.Entity;

namespace WarehouseForms.Forms.Adding
{
    public partial class FormWorker : Form
    {
        private QueryTemplates _qt;

        public FormWorker()
        {
            InitializeComponent();

            Context db = new Context();
            _qt = new QueryTemplates(db);
        }
        private void CloseContext()
        {
            _qt.Close();
        }

        private void FormWorker_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseContext();
        }

        private bool IsValidateAdd()
        {
            if (!string.IsNullOrWhiteSpace(textBoxLastName.Text))
            {
                if (!string.IsNullOrWhiteSpace(textBoxFirstName.Text))
                {
                    if (!string.IsNullOrWhiteSpace(textBoxAddress.Text))
                    {
                        if (!string.IsNullOrWhiteSpace(textBoxCellPhone.Text))
                        {
                            if (dataGridViewCompanyAndPost.RowCount > 0)
                            {
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("Невыбраны компания и должнось.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Неверно заполнено поле \"Мобильный телефон\"");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверно заполнено поле \"Адрес\"");
                    }
                }
                else
                {
                    MessageBox.Show("Неверно заполнено поле \"Имя\"");
                }
            }
            else
            {
                MessageBox.Show("Неверно заполнено поле \"Фамилия\"");
            }
            return false;
        }

        private void AddWorker()
        {
            if (IsValidateAdd())
            {
                DirectoryWorker worker = new DirectoryWorker();
                worker.LastName = textBoxLastName.Text;
                worker.FirstName = textBoxFirstName.Text;
                
                if (!string.IsNullOrWhiteSpace(textBoxMidName.Text))
                {
                    worker.MidName = textBoxMidName.Text;
                }

                worker.Gender = radioButtonMale.Checked ? Gender.Male : Gender.Female;

                worker.BirthDay = dateTimePickerBirthDay.Value;
                worker.Address = textBoxAddress.Text;
                worker.CellPhone = textBoxCellPhone.Text;
                
                if (!string.IsNullOrWhiteSpace(textBoxHomePhone.Text))
                {
                   worker.HomePhone = textBoxHomePhone.Text; 
                }

                worker.StartDate = dateTimePickerDateOfStart.Value;


                for (int i = 0; i < dataGridViewCompanyAndPost.RowCount; i++)
                {
                    var currentPost = new CurrentPost
                    {
                        ChangeDate = DateTime.Parse(dataGridViewCompanyAndPost[5, i].Value.ToString()),
                        DirectoryPostId = int.Parse(dataGridViewCompanyAndPost[0, i].Value.ToString())
                    };

                    worker.Posts.Add(currentPost);
                }

                _qt.AddDirectoryWorker(worker);
                _qt.Save();
                ClearForm();
            }
        }

        private void ClearForm()
        {
            textBoxLastName.ResetText();
            textBoxFirstName.ResetText();
            textBoxMidName.ResetText();
            radioButtonMale.Checked = true;
            dateTimePickerBirthDay.Value = DateTime.Now;
            textBoxAddress.ResetText();
            textBoxCellPhone.ResetText();
            textBoxHomePhone.ResetText();
            dateTimePickerDateOfStart.Value = DateTime.Now;
            dataGridViewCompanyAndPost.Rows.Clear();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddWorker();
        }

        private void FormWorker_Load(object sender, EventArgs e)
        {
        
        }

        private void buttonAddCompanyAndPost_Click(object sender, EventArgs e)
        {
            using (var form = new FormAddComanyAnadPostToWorker())
            {
                form.ShowDialog();
                var currentTmpPost = form.GetPost();
                var directoryPost = _qt.GetDirectoryPost(currentTmpPost.Date, currentTmpPost.PostName, currentTmpPost.CompanyName);

                AddCurrentPostToDataGrid(directoryPost, currentTmpPost.Date);
            }
        }

        private void AddCurrentPostToDataGrid(DirectoryPost post, DateTime postChangeDate)
        {
            var row = new DataGridViewRow();
            row.CreateCells(dataGridViewCompanyAndPost);
           
            row.Cells[0].Value = post.Id;
            row.Cells[1].Value = post.DirectoryCompany.Name;
            row.Cells[2].Value = post.Name;
            row.Cells[3].Value = post.UserWorkerSalary.ToString();
            row.Cells[4].Value = post.UserHalfWorkerSalary.ToString();
            row.Cells[5].Value = postChangeDate.ToShortDateString();
            
            dataGridViewCompanyAndPost.Rows.Add(row);
        }

        private void buttonRemoveCompanyAndPost_Click(object sender, EventArgs e)
        {
            
            RemoveCurrentPostFromDataGrid();
        }

        private void RemoveCurrentPostFromDataGrid()
        {
            if (dataGridViewCompanyAndPost.SelectedRows.Count >0)
            {
                int index = dataGridViewCompanyAndPost.SelectedRows[0].Index;
                dataGridViewCompanyAndPost.Rows.RemoveAt(index);
            }
        }
    }
}
