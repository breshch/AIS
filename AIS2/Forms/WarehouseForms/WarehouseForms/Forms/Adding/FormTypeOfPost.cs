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
    public partial class FormTypeOfPost : Form
    {
        private Context _db = new Context();

        public FormTypeOfPost()
        {
            InitializeComponent();
        }

        private void FormTypeOfPost_Load(object sender, EventArgs e)
        {
            listBoxTypeOfPosts.Items.Clear();
            FormFill();
        }

        private void FormFill()
        {
            ClearForm();
            listBoxTypeOfPosts.Items.AddRange(_db.DirectoryTypeOfPosts.Select(t => t.Name).ToArray());

            buttonRemove.Enabled = listBoxTypeOfPosts.Items.Count > 0 ? true : false;
        }

        private void FormTypeOfPost_FormClosing(object sender, FormClosingEventArgs e)
        {
            _db.Dispose();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (IsValidateAdd())
            {
                _db.DirectoryTypeOfPosts.Add(new DirectoryTypeOfPost { Name = textBoxTypeOfPost.Text });
                _db.SaveChanges();

                ClearForm();
                FormFill();
            }
        }

        private bool IsValidateAdd()
        {
            if (!string.IsNullOrWhiteSpace(textBoxTypeOfPost.Text))
            {
                if (!_db.DirectoryTypeOfPosts.Select(t => t.Name).Contains(textBoxTypeOfPost.Text))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Такой тип должности уже существует." + Environment.NewLine + "Введите другой тип должности.");
                }
            }
            else
            {
                MessageBox.Show("Заполните тип должности.");
            }

            return false;
        }

        private bool IsValidateRemove(DirectoryTypeOfPost directoryTypeOfPost)
        {
            if (!_db.DirectoryPosts.Select(c => c.DirectoryTypeOfPostId).Contains(directoryTypeOfPost.Id))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Невозможно удалить данный тип должности," + Environment.NewLine + "так как существует должность, принадлежащая типу "
                    + directoryTypeOfPost.Name.ToString());
            }
            return false;
        }

        private void ClearForm()
        {
            listBoxTypeOfPosts.Items.Clear();
            textBoxTypeOfPost.Clear();
        }

        private void textBoxTypeOfPost_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listBoxTypeOfPosts.SelectedIndex != -1)
            {
                string typeOfPosts = listBoxTypeOfPosts.SelectedItem.ToString();
                var directoryTypeOfPost = _db.DirectoryTypeOfPosts.First(t => t.Name == typeOfPosts);

                if (IsValidateRemove(directoryTypeOfPost))
                {
                    _db.DirectoryTypeOfPosts.Remove(directoryTypeOfPost);
                    _db.SaveChanges();

                    FormFill();
                }
            }
        }
    }
}
