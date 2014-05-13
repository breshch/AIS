using AIS_Enterprise_AV.Models.Currents;
using AIS_Enterprise_AV.Models.Directories;
using AIS_Enterprise_AV.Models.Infos;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Temps;
using AIS_Enterprise_Global.Models;
using AIS_Enterprise_Global.Models.Currents;
using AIS_Enterprise_Global.Models.Directories;
using AIS_Enterprise_Global.Models.Helpers;
using AIS_Enterprise_Global.Models.Infos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Models
{
    public class BusinessContextAV : BusinessContext
    {
        #region Base

        private DataContextAV _dc;

        public BusinessContextAV() : base()
        {
            _dc = new DataContextAV();
        }

        public DataContextAV DataContextAV
        {
            get
            {
                return _dc;
            }
        }

        public void InitializeDatabase()
        {
            _dc.Database.Initialize(false);
        }

        public override void RefreshContext()
        {
            base.RefreshContext();

            _dc.Dispose();
            _dc = new DataContextAV();
        }

        public void CreateDatabase()
        {
            if (_dc.Database.Exists())
            {
                _dc.Database.Delete();
                _dc.Dispose();
                _dc = new DataContextAV();
            }

            _dc.Database.Create();
        }

        public override void Dispose()
        {
            base.Dispose();

            _dc.Dispose();
            GC.SuppressFinalize(this);
        }

        public override void SaveChanges()
        {
            base.SaveChanges();

            _dc.SaveChanges();
        }

        #endregion


        #region InitializeDefaultDataBase

        private enum PostName 
        {
            ЗавСкладом,
            Грузчик,
            Карщик,
            Кладовщик,
            ЗамЗавСкладом,
            Оператор,
            Оклейщик,
            КарщикКладовщик,
            Логист,
            БригадирОклейщик
        }

        public void InitializeDefaultDataBaseWithoutWorkers()
        {
            if (_dc.Database.Exists())
            {
                _dc.Database.Delete();

                _dc.Dispose();
                _dc = new DataContextAV();
            }

            _dc.Database.Create();
            InputDateToDataBase(2014);

            var parameterBirthday = new Parameter { Name = "Birthday" , Value = "500" };
            _dc.Parameters.Add(parameterBirthday);

            var parameterLastDate = new Parameter { Name = "LastDate", Value = DateTime.Now.ToString() };
            _dc.Parameters.Add(parameterLastDate);
            
            _dc.SaveChanges();


            var companyAV = new DirectoryCompany { Name = "АВ" };
            _dc.DirectoryCompanies.Add(companyAV);

            var companyFenox = new DirectoryCompany { Name = "Фенокс" };
            _dc.DirectoryCompanies.Add(companyFenox);

            _dc.SaveChanges();

            var rC = new DirectoryRC { Name = "МО-5", DirectoryCompany = companyFenox, Percentes = 50 };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "КО-5", DirectoryCompany = companyAV,Percentes = 20 };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "ПАМ-16",DirectoryCompany = companyAV, Percentes = 20 };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "МО-2", DirectoryCompany = companyFenox, Percentes = 5 };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "ПАМ-1", DirectoryCompany = companyFenox, Percentes = 5 };

            _dc.DirectoryRCs.Add(rC);
            _dc.SaveChanges();

            var typeOfPost = new DirectoryTypeOfPost { Name = "Склад" };

            _dc.DirectoryTypeOfPosts.Add(typeOfPost);

            typeOfPost = new DirectoryTypeOfPost { Name = "Офис" };

            _dc.DirectoryTypeOfPosts.Add(typeOfPost);
            _dc.SaveChanges();

            foreach (var postName in Enum.GetNames(typeof(PostName)))
            {
                var post = new DirectoryPost
                {
                    Name = postName,
                    DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Склад"),
                    DirectoryCompany = companyAV,
                    Date = new DateTime(2011, 01, 01),
                };

                var postNameEnum = (PostName)Enum.Parse(typeof(PostName), postName);

                switch (postNameEnum)
                {
                    case PostName.ЗавСкладом:
                        post.UserWorkerSalary = 30000;
                        post.AdminWorkerSalary = 42000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                    case PostName.Грузчик:
                        post.UserWorkerSalary = 22000;
                        post.AdminWorkerSalary = 22000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                    case PostName.Карщик:
                        post.UserWorkerSalary = 25000;
                        post.AdminWorkerSalary = 25000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                    case PostName.Кладовщик:
                        post.UserWorkerSalary = 25000;
                        post.AdminWorkerSalary = 25000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                    case PostName.ЗамЗавСкладом:
                        post.UserWorkerSalary = 30000;
                        post.AdminWorkerSalary = 30000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                    case PostName.Оператор:
                        post.UserWorkerSalary = 25000;
                        post.AdminWorkerSalary = 25000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                    case PostName.Оклейщик:
                        post.UserWorkerSalary = 18000;
                        post.AdminWorkerSalary = 18000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                    case PostName.КарщикКладовщик:
                        post.UserWorkerSalary = 27000;
                        post.AdminWorkerSalary = 27000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                    case PostName.Логист:
                        post.UserWorkerSalary = 25000;
                        post.AdminWorkerSalary = 25000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                    case PostName.БригадирОклейщик:
                        post.UserWorkerSalary = 18000;
                        post.AdminWorkerSalary = 18000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                }

                _dc.DirectoryPosts.Add(post);
                _dc.SaveChanges();
            }

            foreach (var postName in Enum.GetNames(typeof(PostName)))
            {
                var post = new DirectoryPost
                {
                    Name = postName,
                    DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Склад"),
                    DirectoryCompany = companyFenox,
                    Date = new DateTime(2011, 01, 01),
                };

                var postNameEnum = (PostName)Enum.Parse(typeof(PostName), postName);

                switch (postNameEnum)
                {
                    case PostName.ЗавСкладом:
                        post.UserWorkerSalary = 30000;
                        post.AdminWorkerSalary = 42000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                    case PostName.Грузчик:
                        post.UserWorkerSalary = 22000;
                        post.AdminWorkerSalary = 22000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                    case PostName.Карщик:
                        post.UserWorkerSalary = 25000;
                        post.AdminWorkerSalary = 25000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                    case PostName.Кладовщик:
                        post.UserWorkerSalary = 25000;
                        post.AdminWorkerSalary = 25000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                    case PostName.ЗамЗавСкладом:
                        post.UserWorkerSalary = 30000;
                        post.AdminWorkerSalary = 30000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                    case PostName.Оператор:
                        post.UserWorkerSalary = 25000;
                        post.AdminWorkerSalary = 25000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                    case PostName.Оклейщик:
                        post.UserWorkerSalary = 18000;
                        post.AdminWorkerSalary = 18000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                    case PostName.КарщикКладовщик:
                        post.UserWorkerSalary = 27000;
                        post.AdminWorkerSalary = 27000;
                        post.UserWorkerHalfSalary = 10000;
                        break;
                }

                _dc.DirectoryPosts.Add(post);
                _dc.SaveChanges();
            }
        }

        public void InitializeDefaultDataBaseWithWorkers()
        {
            InitializeDefaultDataBaseWithoutWorkers();

            var slave = new DirectoryWorker
            {
                LastName = "Пупкин",
                FirstName = "Василий",
                MidName = "Васильевич",
                BirthDay = new DateTime(1979, 12, 31),
                Address = "Москва",
                CellPhone = "+7985325642",
                HomePhone = "+7495231568",
                StartDate = DateTime.Now.AddDays(-45),
                FireDate = null,
                Gender = Gender.Male,
                CurrentCompaniesAndPosts = new List<CurrentPost>(new[] 
                    { 
                        new CurrentPost
                        {
                            DirectoryPost = _dc.DirectoryPosts.First(),
                            ChangeDate = DateTime.Now.AddDays(-45)
                        }
                    })
            };

            var holidays = new List<DateTime>();
            for (DateTime date = slave.StartDate; date <= DateTime.Now; date = date.AddMonths(1))
            {
                holidays.AddRange(GetHolidays(date.Year, date.Month).ToList());
            }

            for (DateTime date = slave.StartDate; date <= DateTime.Now; date = date.AddDays(1))
            {
                var infoDate = new InfoDate();
                infoDate.Date = date;

                if (!holidays.Any(h => h.Date.Date == date.Date))
                {
                    infoDate.DescriptionDay = (DescriptionDay)HelperMethods.GetRandomNumber(0, 6);

                    if (infoDate.DescriptionDay == DescriptionDay.Был)
                    {
                        infoDate.CountHours = HelperMethods.GetRandomNumber(1, 9);
                    }
                }
                else
                {
                    infoDate.DescriptionDay = DescriptionDay.Был;
                }

                slave.InfoDates.Add(infoDate);
            }

            var infoMonth = new InfoMonth
            {
                Date = DateTime.Now,
                PrepaymentCash = 1500,
                PrepaymentBankTransaction = 1000,
                VocationPayment = 500,
                CardAV = 2000,
                CardFenox = 1000,
                Panalty = 0,
                Inventory = 1000,
                BirthDays = 500,
                Bonus = 5000
            };

            slave.InfoMonthes.Add(infoMonth);

            infoMonth = new InfoMonth
            {
                Date = DateTime.Now.AddMonths(-1),
                PrepaymentCash = 1500,
                PrepaymentBankTransaction = 1000,
                VocationPayment = 500,
                CardAV = 2000,
                CardFenox = 1000,
                Panalty = 0,
                Inventory = 1000,
                BirthDays = 500,
                Bonus = 5000
            };

            slave.InfoMonthes.Add(infoMonth);

            _dc.DirectoryWorkers.Add(slave);

            slave = new DirectoryWorker
            {
                LastName = "Пупкина",
                FirstName = "Василиса",
                MidName = "Васильевна",
                BirthDay = new DateTime(1917, 10, 15),
                Address = "Питер",
                CellPhone = "+7985333642",
                HomePhone = "+7495231568",
                StartDate = DateTime.Now.AddDays(-40),
                FireDate = null,
                Gender = Gender.Female,
                CurrentCompaniesAndPosts = new List<CurrentPost>(new[] 
                    { 
                        new CurrentPost
                        {
                            DirectoryPost = _dc.DirectoryPosts.First(p => p.Name == "Кладовщик" && p.DirectoryCompany.Name == "АВ"),
                            ChangeDate = DateTime.Now.AddDays(-40),
                            IsTwoCompanies = true
                        }
                    })
            };

            holidays = new List<DateTime>();
            for (DateTime date = slave.StartDate; date <= DateTime.Now; date = date.AddMonths(1))
            {
                holidays.AddRange(GetHolidays(date.Year, date.Month).ToList());
            }

            for (DateTime date = slave.StartDate; date <= DateTime.Now; date = date.AddDays(1))
            {
                var infoDate = new InfoDate();
                infoDate.Date = date;

                if (!holidays.Any(h => h.Date.Date == date.Date))
                {
                    infoDate.DescriptionDay = (DescriptionDay)HelperMethods.GetRandomNumber(0, 6);

                    if (infoDate.DescriptionDay == DescriptionDay.Был)
                    {
                        infoDate.CountHours = HelperMethods.GetRandomNumber(1, 9);
                    }
                }
                else
                {
                    infoDate.DescriptionDay = DescriptionDay.Был;
                }

                slave.InfoDates.Add(infoDate);
            }

            infoMonth = new InfoMonth
            {
                Date = DateTime.Now.AddMonths(-1),
                PrepaymentCash = 1000,
                PrepaymentBankTransaction = 500,
                VocationPayment = 1500,
                CardAV = 1000,
                CardFenox = 2000,
                Panalty = 0,
                Inventory = 1500,
                BirthDays = 1500,
                Bonus = 2000
            };

            slave.InfoMonthes.Add(infoMonth);

            infoMonth = new InfoMonth
            {
                Date = DateTime.Now,
                PrepaymentCash = 1000,
                PrepaymentBankTransaction = 500,
                VocationPayment = 1500,
                CardAV = 1000,
                CardFenox = 2000,
                Panalty = 0,
                Inventory = 1500,
                BirthDays = 1500,
                Bonus = 2000
            };

            slave.InfoMonthes.Add(infoMonth);

            _dc.DirectoryWorkers.Add(slave);

            var slaveOffice = new DirectoryWorker
            {
                LastName = "Залупкина",
                FirstName = "Анна",
                MidName = "Васильевна",
                BirthDay = new DateTime(1979, 12, 31),
                Address = "Москва",
                CellPhone = "+7985325642",
                HomePhone = "+7495231568",
                StartDate = DateTime.Now.AddDays(-45),
                FireDate = null,
                Gender = Gender.Female,
                CurrentCompaniesAndPosts = new List<CurrentPost>(new[] 
                    { 
                        new CurrentPost
                        {
                            DirectoryPost = _dc.DirectoryPosts.First(p => p.Name == "ГлавБух" && p.DirectoryCompany.Name == "АВ"),
                            ChangeDate = DateTime.Now.AddDays(-45),
                            IsTwoCompanies = true
                        }
                    })
            };

            holidays = new List<DateTime>();
            for (DateTime date = slaveOffice.StartDate; date <= DateTime.Now; date = date.AddMonths(1))
            {
                holidays.AddRange(GetHolidays(date.Year, date.Month).ToList());
            }

            for (DateTime date = slaveOffice.StartDate; date <= DateTime.Now; date = date.AddDays(1))
            {
                var infoDate = new InfoDate();
                infoDate.Date = date;
                
                if (!holidays.Any(h => h.Date.Date == date.Date))
                {
                    infoDate.CountHours = 8;
                }

                slaveOffice.InfoDates.Add(infoDate);
            }

            infoMonth = new InfoMonth
            {
                Date = DateTime.Now,
                PrepaymentCash = 0,
                PrepaymentBankTransaction = 0,
                VocationPayment = 500,
                CardAV = 2000,
                CardFenox = 0,
                Panalty = 0,
                Inventory = 0,
                BirthDays = 0,
                Bonus = 0
            };

            slaveOffice.InfoMonthes.Add(infoMonth);

            infoMonth = new InfoMonth
            {
                Date = DateTime.Now.AddMonths(-1),
                PrepaymentCash = 0,
                PrepaymentBankTransaction = 0,
                VocationPayment = 1000,
                CardAV = 3000,
                CardFenox = 0,
                Panalty = 0,
                Inventory = 0,
                BirthDays = 0,
                Bonus = 0
            };

            slaveOffice.InfoMonthes.Add(infoMonth);

            _dc.DirectoryWorkers.Add(slaveOffice);

            slaveOffice = new DirectoryWorker
            {
                LastName = "Хуялкина",
                FirstName = "Залупа",
                MidName = "Жоповна",
                BirthDay = new DateTime(1979, 12, 31),
                Address = "Трипездянск",
                CellPhone = "+7985325642",
                HomePhone = "+7495231568",
                StartDate = DateTime.Now.AddDays(-45),
                FireDate = null,
                Gender = Gender.Female,
                CurrentCompaniesAndPosts = new List<CurrentPost>(new[] 
                    { 
                        new CurrentPost
                        {
                            DirectoryPost = _dc.DirectoryPosts.First(p => p.Name == "ГлавБух" && p.DirectoryCompany.Name == "АВ"),
                            ChangeDate = DateTime.Now.AddDays(-45)
                        }
                    })
            };

            holidays = new List<DateTime>();
            for (DateTime date = slaveOffice.StartDate; date <= DateTime.Now; date = date.AddMonths(1))
            {
                holidays.AddRange(GetHolidays(date.Year, date.Month).ToList());
            }

            for (DateTime date = slaveOffice.StartDate; date <= DateTime.Now; date = date.AddDays(1))
            {
                var infoDate = new InfoDate();
                infoDate.Date = date;

                if (!holidays.Any(h => h.Date.Date == date.Date))
                {
                    infoDate.CountHours = 8;
                }

                slaveOffice.InfoDates.Add(infoDate);
            }

            infoMonth = new InfoMonth
            {
                Date = DateTime.Now,
                PrepaymentCash = 0,
                PrepaymentBankTransaction = 0,
                VocationPayment = 1500,
                CardAV = 0,
                CardFenox = 2000,
                Panalty = 0,
                Inventory = 0,
                BirthDays = 0,
                Bonus = 0
            };

            slaveOffice.InfoMonthes.Add(infoMonth);

            infoMonth = new InfoMonth
            {
                Date = DateTime.Now.AddMonths(-1),
                PrepaymentCash = 0,
                PrepaymentBankTransaction = 0,
                VocationPayment = 1000,
                CardAV = 0,
                CardFenox = 5000,
                Panalty = 0,
                Inventory = 0,
                BirthDays = 0,
                Bonus = 0
            };

            slaveOffice.InfoMonthes.Add(infoMonth);

            _dc.DirectoryWorkers.Add(slaveOffice);

            _dc.SaveChanges();
        }

        public void InitializeDefaultDataBaseWithOfficeWorkers()
        {
            if (!_dc.Database.Exists())
            {
                _dc.Database.Create();
                InputDateToDataBase(2014);
            }

            var officePost = new DirectoryPost
            {
                Name = "ГлавБух_Чернецкая",
                DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Офис"),
                DirectoryCompany = _dc.DirectoryCompanies.First(c => c.Name == "АВ"),
                Date = new DateTime(2011, 01, 01),
                UserWorkerSalary = 45000,
                AdminWorkerSalary = 45000,
                UserWorkerHalfSalary = 45000
            };

            _dc.DirectoryPosts.Add(officePost);

            officePost = new DirectoryPost
            {
                Name = "Бухгалтер_Казёнова",
                DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Офис"),
                DirectoryCompany = _dc.DirectoryCompanies.First(c => c.Name == "АВ"),
                Date = new DateTime(2011, 01, 01),
                UserWorkerSalary = 40000,
                AdminWorkerSalary = 40000,
                UserWorkerHalfSalary = 0
            };

            _dc.DirectoryPosts.Add(officePost);

            officePost = new DirectoryPost
            {
                Name = "Бухгалтер_Дикрет_Губашева",
                DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Офис"),
                DirectoryCompany = _dc.DirectoryCompanies.First(c => c.Name == "АВ"),
                Date = new DateTime(2011, 01, 01),
                UserWorkerSalary = 0,
                AdminWorkerSalary = 0,
                UserWorkerHalfSalary = 0
            };

            _dc.DirectoryPosts.Add(officePost);

            officePost = new DirectoryPost
            {
                Name = "Бухгалтер_Гаганова",
                DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Офис"),
                DirectoryCompany = _dc.DirectoryCompanies.First(c => c.Name == "Фенокс"),
                Date = new DateTime(2011, 01, 01),
                UserWorkerSalary = 30000,
                AdminWorkerSalary = 30000,
                UserWorkerHalfSalary = 0
            };

            _dc.DirectoryPosts.Add(officePost);

            officePost = new DirectoryPost
            {
                Name = "Бухгалтер_Крицкая",
                DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Офис"),
                DirectoryCompany = _dc.DirectoryCompanies.First(c => c.Name == "АВ"),
                Date = new DateTime(2011, 01, 01),
                UserWorkerSalary = 30000,
                AdminWorkerSalary = 30000,
                UserWorkerHalfSalary = 0
            };

            _dc.DirectoryPosts.Add(officePost);

            officePost = new DirectoryPost
            {
                Name = "Бухгалтер_Рыжова",
                DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Офис"),
                DirectoryCompany = _dc.DirectoryCompanies.First(c => c.Name == "АВ"),
                Date = new DateTime(2011, 01, 01),
                UserWorkerSalary = 12000,
                AdminWorkerSalary = 12000,
                UserWorkerHalfSalary = 13000
            };

            _dc.DirectoryPosts.Add(officePost);

            officePost = new DirectoryPost
            {
                Name = "Директор",
                DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Офис"),
                DirectoryCompany = _dc.DirectoryCompanies.First(c => c.Name == "АВ"),
                Date = new DateTime(2011, 01, 01),
                UserWorkerSalary = 35000,
                AdminWorkerSalary = 35000,
                UserWorkerHalfSalary = 30000
            };

            _dc.DirectoryPosts.Add(officePost);
            
            _dc.SaveChanges();

            AddOfficeWorker("Шагай", "Александр", "Грузчик", "Фенокс", false, true);

            AddOfficeWorker("Чернецкая", "Наталья", "ГлавБух_Чернецкая", "АВ", true);
            AddOfficeWorker("Казёнова", "Ольга", "Бухгалтер_Казёнова", "АВ", false);
            AddOfficeWorker("Губашева", "Людмила", "Бухгалтер_Дикрет_Губашева", "АВ", false);
            AddOfficeWorker("Гаганова", "Любовь", "Бухгалтер_Гаганова", "Фенокс", false);
            AddOfficeWorker("Крицкая", "Анна", "Бухгалтер_Крицкая", "АВ", false);
            AddOfficeWorker("Рыжова", "Елена", "Бухгалтер_Рыжова", "АВ", true);
            AddOfficeWorker("Брещенко", "Алексей", "Директор", "АВ", true);
        }

        private void AddOfficeWorker(string lastName, string firstName, string postName, string companyName, bool isTwoCompanies, bool isAngel = false)
        {
            var workerOffice = new DirectoryWorker
            {
                LastName = lastName,
                FirstName = firstName,
                MidName = "",
                BirthDay = new DateTime(1975, 01, 09),
                Address = "",
                CellPhone = "",
                HomePhone = "",
                StartDate = new DateTime(2014, 01, 01),
                FireDate = null,
                Gender = Gender.Female,
                IsAngel = isAngel,
                CurrentCompaniesAndPosts = new List<CurrentPost>(new[] 
                    { 
                        new CurrentPost
                        {
                            DirectoryPost = _dc.DirectoryPosts.First(p => p.Name == postName && p.DirectoryCompany.Name == companyName),
                            ChangeDate = new DateTime(2014, 01, 01),
                            IsTwoCompanies = isTwoCompanies
                        }
                    })
            };

            var holidays = new List<DateTime>();
            for (DateTime date = workerOffice.StartDate; date <= DateTime.Now; date = date.AddMonths(1))
            {
                holidays.AddRange(GetHolidays(date.Year, date.Month).ToList());
            }

            for (DateTime date = workerOffice.StartDate; date <= DateTime.Now; date = date.AddDays(1))
            {
                var infoDate = new InfoDate();
                infoDate.Date = date;

                if (!holidays.Any(h => h.Date.Date == date.Date))
                {
                    infoDate.CountHours = 8;
                }

                workerOffice.InfoDates.Add(infoDate);
            }

            for (DateTime date = workerOffice.StartDate; date <= DateTime.Now; date = date.AddMonths(1))
            {
                var infoMonth = new InfoMonth
                {
                    Date = new DateTime(date.Year, date.Month, 1),
                    PrepaymentCash = 0,
                    PrepaymentBankTransaction = 0,
                    VocationPayment = 0,
                    CardAV = 0,
                    CardFenox = 0,
                    Panalty = 0,
                    Inventory = 0,
                    BirthDays = 0,
                    Bonus = 0
                };
                workerOffice.InfoMonthes.Add(infoMonth);
            }
            _dc.SaveChanges();

            _dc.DirectoryWorkers.Add(workerOffice);

            _dc.SaveChanges();
        }

        #endregion


        #region DirectoryRC

        public IQueryable<DirectoryRC> GetDirectoryRCs()
        {
            return _dc.DirectoryRCs;
        }

        public DirectoryRC AddDirectoryRC(string directoryRCName, DirectoryCompany directoryCompany, int percentes)
        {
            var directoryRC = new DirectoryRC
            {
                Name = directoryRCName,
                DirectoryCompany = directoryCompany,
                Percentes = percentes
            };

            _dc.DirectoryRCs.Add(directoryRC);

            _dc.SaveChanges();

            return directoryRC;
        }

        public void RemoveDirectoryRC(int directoryRCId)
        {
            var directoryRC = _dc.DirectoryRCs.Find(directoryRCId);
            _dc.DirectoryRCs.Remove(directoryRC);

            _dc.SaveChanges();
        }

        #endregion


        #region InfoOverTime

        public void AddInfoOverTime(DateTime startDate, DateTime endDate, ICollection<DirectoryRC> directoryRCs, string description)
        {
            var overTime = _dc.InfoOverTimes.FirstOrDefault(o => DbFunctions.DiffDays(o.StartDate, startDate) == 0);
            
            if (overTime == null)
            {
                overTime = new InfoOverTime
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    Description = description
                };

                foreach (var directoryRC in directoryRCs)
                {
                    var currentRC = new CurrentRC
                    {
                        DirectoryRC = directoryRC
                    };

                    overTime.CurrentRCs.Add(currentRC);
                }
                

                _dc.InfoOverTimes.Add(overTime);

                _dc.SaveChanges();
            }
        }

        public void EditInfoOverTime(DateTime startDate, DateTime endDate, List<DirectoryRC> directoryRCs, string description)
        {
            var overTime = _dc.InfoOverTimes.FirstOrDefault(o => DbFunctions.DiffDays(o.StartDate, startDate) == 0);

            if (overTime != null)
            {
                overTime.StartDate = startDate;
                overTime.EndDate = endDate;
                overTime.Description = description;

                overTime.CurrentRCs.Clear();

                foreach (var directoryRC in directoryRCs)
                {
                    var currentRC = new CurrentRC
                    {
                        DirectoryRC = directoryRC
                    };

                    overTime.CurrentRCs.Add(currentRC);
                }

                _dc.SaveChanges();
            }
        }

        public InfoOverTime GetInfoOverTime(DateTime date)
        {
            return _dc.InfoOverTimes.FirstOrDefault(o => DbFunctions.DiffDays(o.StartDate, date) == 0);
        }

        public IQueryable<InfoOverTime> GetInfoOverTimes(int year, int month)
        {
            return _dc.InfoOverTimes.Where(o => o.StartDate.Year == year && o.StartDate.Month == month);
        }

        public IQueryable<DateTime> GetInfoOverTimeDates(int year, int month)
        {
            return _dc.InfoOverTimes.Where(o => o.StartDate.Year == year && o.StartDate.Month == month).Select(o => o.StartDate);
        }

        public bool IsInfoOverTimeDate(DateTime date)
        {
            return _dc.InfoOverTimes.Any(o => DbFunctions.DiffDays(o.StartDate, date) == 0);
        }

        public void RemoveInfoOverTime(DateTime date)
        {
            var infoOverTime = _dc.InfoOverTimes.FirstOrDefault(o => DbFunctions.DiffDays(o.StartDate, date) == 0);
            if (infoOverTime != null)
            {
                _dc.InfoOverTimes.Remove(infoOverTime);
                _dc.SaveChanges();
            }
        }
        #endregion

        
    }
}
