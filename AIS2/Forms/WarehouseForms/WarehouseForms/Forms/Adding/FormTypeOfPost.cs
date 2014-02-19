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
        private QueryTemplates _qt;

        public FormTypeOfPost()
        {
            InitializeComponent();
        }

        private void FormTypeOfPost_Load(object sender, EventArgs e)
        {
            var db = new Context();
            _qt = new QueryTemplates(db);

            listBoxTypeOfPosts.Items.Clear();
            FormFill();
        }

        private void FormFill()
        {
            ClearForm();
            listBoxTypeOfPosts.Items.AddRange(_qt.GetDirectoryTypeOfPostNames().ToArray());

            buttonRemove.Enabled = listBoxTypeOfPosts.Items.Count > 0 ? true : false;
        }

        private void FormTypeOfPost_FormClosing(object sender, FormClosingEventArgs e)
        {
            _qt.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (IsValidateAdd())
            {
                _qt.AddDirectoryTypeOfPost(textBoxTypeOfPost.Text);
                _qt.Save();

                ClearForm();
                FormFill();
            }
        }

        private bool IsValidateAdd()
        {
            if (!string.IsNullOrWhiteSpace(textBoxTypeOfPost.Text))
            {
                if (!_qt.GetDirectoryTypeOfPostNames().Contains(textBoxTypeOfPost.Text))
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
            if (!_qt.GetDirectoryPostIds().Contains(directoryTypeOfPost.Id))
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
                string typeOfPost = listBoxTypeOfPosts.SelectedItem.ToString();
                var directoryTypeOfPost = _qt.GetDirectoryTypeOfPost(typeOfPost);

                if (IsValidateRemove(directoryTypeOfPost))
                {
                    _qt.RemoveDirectoryTypeOfPost(directoryTypeOfPost);
                    _qt.Save();

                    FormFill();
                }
            }
        }
    }
}
