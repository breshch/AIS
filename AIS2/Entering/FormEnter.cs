using ModelDB;
//using AvForms;
//using CarsForms;
//using Global;
//using ModelDbAv;
//using ModelDbCars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarehouseForms.Forms;


namespace Entering
{
    public partial class FormEnter : Form
    {
        public FormEnter()
        {
            InitializeComponent();
        }

        private void FormEnter_Load(object sender, EventArgs e)
        {
            using (var db = new Context())
            {
                db.Database.Initialize(true);
            }

            //for (int i = 66; i < 69; i++)
            //{
            //    Helper.DeleteWorker(i);
            //}

            //Helper.DeleteOfficeWorker(3);

            //using (var db = new CarsContext())
            //{
            //    db.Database.Delete();
            //    db.Database.Create();
            //}

            //using (var db = new AvContext())
            //{
            //    Helper.CreateNewDb(db);
            //}


            comboBoxProject.Items.Add("Табель");
            comboBoxProject.Items.Add("Машины");
            comboBoxProject.SelectedIndex = 0;


            //using (var db = new CarsContext())
            //{
            //    HelperCars.CreateNewDb(db);
            //}

            //using (var db = new AvContext())
            //{
            //    //Helper.CreateTriggers(db);
            //    SqlDependency.Start(db.Database.Connection.ConnectionString);
            //    comboBoxFIO.Items.AddRange(db.LogIns.Select(l => l.FIO).ToArray());
            //    //comboBoxFIO.SelectedIndex = 0;
            //    //textBoxPassword.Text = "123";
            //}
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            FormDayTabel formDayTabel = new FormDayTabel();
            formDayTabel.Show();
            //Entering();
        }

        private void FormEnter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Entering();
            }
        }

        private void Entering()
        {
            //if (comboBoxFIO.SelectedIndex != -1)
            //{
            //    using (var db = new AvContext())
            //    {
            //        string project = comboBoxProject.SelectedItem.ToString();
            //        string fio = comboBoxFIO.SelectedItem.ToString();
            //        LogIn login = db.LogIns.First(l => l.FIO == fio);

            //        if (login.UserStatus != UserStatus.Admin)
            //        {
            //            if (textBoxPassword.Text == login.Password)
            //            {
            //                if (project == "Табель")
            //                {
            //                    Globals.UserStatus = login.UserStatus;

            //                    if (Globals.UserStatus == UserStatus.Bookkeeper)
            //                    {
            //                        FormMonthTabel formMonthTabel = new FormMonthTabel();
            //                        formMonthTabel.Show();
            //                    }
            //                    else if (Globals.UserStatus == UserStatus.User)
            //                    {
            //                        FormDayTabel formDayTabel = new FormDayTabel();
            //                        formDayTabel.Show();
            //                    }
            //                }
            //                else if (project == "Машины")
            //                {
            //                     FormCarRequest formCarRequest = new FormCarRequest();
            //                     formCarRequest.Show();
            //                }

            //                this.Visible = false;
            //            }
            //            else
            //            {
            //                MessageBox.Show("Пароль введён неверно");
            //            }
            //        }
            //        else
            //        {
            //            SHA256 sha = SHA256.Create();
            //            byte[] bytes = Encoding.UTF8.GetBytes(textBoxPassword.Text);
            //            byte[] hashbytes = sha.ComputeHash(bytes);
            //            string hashPass = Encoding.UTF8.GetString(hashbytes);

            //            if (hashPass == login.Password)
            //            {
            //                if (project == "Табель")
            //                {
            //                    Globals.UserStatus = login.UserStatus;

            //                    FormDayTabel formDayTabel = new FormDayTabel();
            //                    formDayTabel.Show();
            //                }
            //                else if (project == "Машины")
            //                {
            //                     FormCarRequest formCarRequest = new FormCarRequest();
            //                     formCarRequest.Show();
            //                }

            //                this.Visible = false;
            //            }
            //            else
            //            {
            //                MessageBox.Show("Пароль введён неверно");
            //            }
            //        }
            //    }
            //}
        }
    }
}
