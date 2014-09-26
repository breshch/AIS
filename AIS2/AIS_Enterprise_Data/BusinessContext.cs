using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Helpers;
using AIS_Enterprise_Data.Infos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UnidecodeSharpFork;
using AIS_Enterprise_Data.Temps;

namespace AIS_Enterprise_Data
{
    public class BusinessContext : IDisposable
    {
        #region Base

        private const string TYPE_OF_POST_OFFICE = "Офис";

        private DataContext _dc;

        public BusinessContext()
        {
            _dc = new DataContext();
        }

        public virtual void RefreshContext()
        {
            if (_dc != null)
            {
                _dc.Dispose();
            }

            _dc = new DataContext();
        }

        public virtual void Dispose()
        {
            _dc.Dispose();
            GC.SuppressFinalize(this);
        }

        public virtual void SaveChanges()
        {
            _dc.SaveChanges();
        }

        public DataContext DataContext
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

        public void CreateDatabase()
        {
            if (_dc.Database.Exists())
            {
                _dc.Database.Delete();

                RefreshContext();
            }

            _dc.Database.Create();
        }

        public void RemoveDB()
        {
            if (_dc.Database.Exists())
            {
                Database.SetInitializer(new DropCreateDatabaseAlways<DataContext>());

                //_dc.Database.Delete();

                RefreshContext();
            }
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

        public void InitializeEmptyDB()
        {
            for (int year = 2011; year <= 2014; year++)
            {
                InputDateToDataBase(year);
            }

            var parameterLastDate = new Parameter { Name = "LastDate", Value = DateTime.Now.ToString() };
            _dc.Parameters.Add(parameterLastDate);

            var parameterDefaultCostsDate = new Parameter { Name = "DefaultCostsDate", Value = DateTime.MinValue.ToString() };
            _dc.Parameters.Add(parameterDefaultCostsDate);

            _dc.SaveChanges();


            var userStatus = new DirectoryUserStatus { Name = "Администратор" };

            foreach (var privilege in Enum.GetNames(typeof(UserPrivileges)))
            {
                var directoryUserStatusPrivilege = new DirectoryUserStatusPrivilege { Name = privilege };
                _dc.DirectoryUserStatusPrivileges.Add(directoryUserStatusPrivilege);

                var currentUserStatusPrivilege = new CurrentUserStatusPrivilege { DirectoryUserStatusPrivilege = directoryUserStatusPrivilege };
                _dc.CurrentUserStatusPrivileges.Add(currentUserStatusPrivilege);

                userStatus.Privileges.Add(currentUserStatusPrivilege);
            }

            _dc.DirectoryUserStatuses.Add(userStatus);
            _dc.SaveChanges();
        }

        public void InitializeDefaultDataBaseWithoutWorkers()
        {
            InitializeEmptyDB();

            var parameterBirthday = new Parameter { Name = "Birthday", Value = "500" };
            _dc.Parameters.Add(parameterBirthday);

            var currencyValue = new CurrencyValue { Name = "TotalCard" };
            _dc.CurrencyValues.Add(currencyValue);

            currencyValue = new CurrencyValue { Name = "TotalCash" };
            _dc.CurrencyValues.Add(currencyValue);

            currencyValue = new CurrencyValue { Name = "TotalSafe" };
            _dc.CurrencyValues.Add(currencyValue);

            currencyValue = new CurrencyValue { Name = "TotalLoan" };
            _dc.CurrencyValues.Add(currencyValue);

            currencyValue = new CurrencyValue { Name = "TotalPrivateLoan" };
            _dc.CurrencyValues.Add(currencyValue);

            var companyAV = new DirectoryCompany { Name = "АВ" };
            _dc.DirectoryCompanies.Add(companyAV);

            var companyFenox = new DirectoryCompany { Name = "Фенокс" };
            _dc.DirectoryCompanies.Add(companyFenox);

            _dc.SaveChanges();

            var rC = new DirectoryRC { Name = "МО-5", DescriptionName = "Фенокс", ReportName = "ОТ.AM5 / M05", Percentes = 50 };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "КО-5", DescriptionName = "АВ-Автотехник", ReportName = "ОТ.КО5/К05", Percentes = 20 };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "ПАМ-16", DescriptionName = "Кедр", ReportName = "П.АМ16/М01.016", Percentes = 20 };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "МО-2", DescriptionName = "Фенокс иномарки", ReportName = "ОТ.AM2/M02", Percentes = 5 };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "ПАМ-1", DescriptionName = "Фенокс Минск (с.п.)", ReportName = "П.АМ1/М01.1", Percentes = 5 };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "КО-2", DescriptionName = "Масло Антонар", ReportName = "ОТ.КО2/К02", Percentes = 0 };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "КО-1", DescriptionName = "Антонар", ReportName = "ОТ.КО1/К01", Percentes = 0 };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "МД-2", DescriptionName = "Медицина", ReportName = "ОТ.МД2", Percentes = 0 };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "ПАМ-24", DescriptionName = "Запчасти", ReportName = "П.АМ24", Percentes = 0 };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "26А", DescriptionName = "26А", ReportName = "26А", Percentes = 0 };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "ВСЕ", DescriptionName = "ВСЕ", ReportName = "ВСЕ", Percentes = 0 };
            _dc.DirectoryRCs.Add(rC);

            _dc.SaveChanges();

            var costItem = new DirectoryCostItem { Name = "Аренда (3001)" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "З/п (701)" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "Закупка товара (1090)" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "Инвестиции" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "Канцтовары (1207)" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "МБП (1209)" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "Представительские (9025)" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "Приход" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "Прочие (9025)" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "Реклама (5061)" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "Связь (502)" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "Сертификация (5052)" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "Таможня (6811)" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "Таможня (6813)" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "Топливо (103)" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "Транспорт (5031)" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "Упаковка (1023)" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "УСО (509)" };
            _dc.DirectoryCostItems.Add(costItem);
            costItem = new DirectoryCostItem { Name = "26А" };
            _dc.DirectoryCostItems.Add(costItem);

            _dc.SaveChanges();


            var note = new DirectoryNote { Description = "Склад" };
            _dc.DirectoryNotes.Add(note);
            note = new DirectoryNote { Description = "Пежо" };
            _dc.DirectoryNotes.Add(note);
            note = new DirectoryNote { Description = "Зарплата" };
            _dc.DirectoryNotes.Add(note);
            note = new DirectoryNote { Description = "Переработка" };
            _dc.DirectoryNotes.Add(note);
            note = new DirectoryNote { Description = "Дождь" };
            _dc.DirectoryNotes.Add(note);
            note = new DirectoryNote { Description = "АПЦ" };
            _dc.DirectoryNotes.Add(note);
            note = new DirectoryNote { Description = "РосАвтоПром" };
            _dc.DirectoryNotes.Add(note);
            note = new DirectoryNote { Description = "Паллеты" };
            _dc.DirectoryNotes.Add(note);

            _dc.SaveChanges();


            var transportCompany = new DirectoryTransportCompany { Name = "Кузин", IsCash = false };
            _dc.DirectoryTransportCompanies.Add(transportCompany);
            transportCompany = new DirectoryTransportCompany { Name = "Логистикон", IsCash = false };
            _dc.DirectoryTransportCompanies.Add(transportCompany);
            transportCompany = new DirectoryTransportCompany { Name = "Павловский Посад", IsCash = true };
            _dc.DirectoryTransportCompanies.Add(transportCompany);

            _dc.SaveChanges();


            var keepingName = new DirectoryKeepingName { Name = "Сейф" };
            _dc.DirectoryKeepingNames.Add(keepingName);
            keepingName = new DirectoryKeepingName { Name = "Карточка" };
            _dc.DirectoryKeepingNames.Add(keepingName);
            keepingName = new DirectoryKeepingName { Name = "Чернецкая" };
            _dc.DirectoryKeepingNames.Add(keepingName);
            keepingName = new DirectoryKeepingName { Name = "Наличные" };
            _dc.DirectoryKeepingNames.Add(keepingName);
            keepingName = new DirectoryKeepingName { Name = "Резерв" };
            _dc.DirectoryKeepingNames.Add(keepingName);
            keepingName = new DirectoryKeepingName { Name = "Аванс" };
            _dc.DirectoryKeepingNames.Add(keepingName);
            keepingName = new DirectoryKeepingName { Name = "Бобров" };
            _dc.DirectoryKeepingNames.Add(keepingName);
            keepingName = new DirectoryKeepingName { Name = "Пежо" };
            _dc.DirectoryKeepingNames.Add(keepingName);

            _dc.SaveChanges();


            var keepingDescription = new DirectoryKeepingDescription { Name = "ФАР" };
            _dc.DirectoryKeepingDescriptions.Add(keepingDescription);
            keepingDescription = new DirectoryKeepingDescription { Name = "Зарплата" };
            _dc.DirectoryKeepingDescriptions.Add(keepingDescription);
            keepingDescription = new DirectoryKeepingDescription { Name = "За Боброва" };
            _dc.DirectoryKeepingDescriptions.Add(keepingDescription);
            keepingDescription = new DirectoryKeepingDescription { Name = "За USA" };
            _dc.DirectoryKeepingDescriptions.Add(keepingDescription);

            _dc.SaveChanges();


            var typeOfPost = new DirectoryTypeOfPost { Name = "Склад" };

            _dc.DirectoryTypeOfPosts.Add(typeOfPost);

            typeOfPost = new DirectoryTypeOfPost { Name = "Офис" };

            _dc.DirectoryTypeOfPosts.Add(typeOfPost);
            _dc.SaveChanges();

            foreach (var company in _dc.DirectoryCompanies.ToList())
            {
                foreach (var postName in Enum.GetNames(typeof(PostName)))
                {
                    var post = new DirectoryPost
                    {
                        Name = postName,
                        DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Склад"),
                        DirectoryCompany = company
                    };

                    var postNameEnum = (PostName)Enum.Parse(typeof(PostName), postName);

                    var postSalary = new DirectoryPostSalary
                    {
                        Date = new DateTime(2011, 01, 01)
                    };

                    switch (postNameEnum)
                    {
                        case PostName.ЗавСкладом:
                            postSalary.UserWorkerSalary = 30000;
                            postSalary.AdminWorkerSalary = 42000;
                            postSalary.UserWorkerHalfSalary = 10000;
                            break;
                        case PostName.Грузчик:
                            postSalary.UserWorkerSalary = 22000;
                            postSalary.AdminWorkerSalary = 22000;
                            postSalary.UserWorkerHalfSalary = 10000;
                            break;
                        case PostName.Карщик:
                            postSalary.UserWorkerSalary = 25000;
                            postSalary.AdminWorkerSalary = 25000;
                            postSalary.UserWorkerHalfSalary = 10000;
                            break;
                        case PostName.Кладовщик:
                            postSalary.UserWorkerSalary = 25000;
                            postSalary.AdminWorkerSalary = 25000;
                            postSalary.UserWorkerHalfSalary = 10000;
                            break;
                        case PostName.ЗамЗавСкладом:
                            postSalary.UserWorkerSalary = 30000;
                            postSalary.AdminWorkerSalary = 30000;
                            postSalary.UserWorkerHalfSalary = 10000;
                            break;
                        case PostName.Оператор:
                            postSalary.UserWorkerSalary = 25000;
                            postSalary.AdminWorkerSalary = 25000;
                            postSalary.UserWorkerHalfSalary = 10000;
                            break;
                        case PostName.Оклейщик:
                            postSalary.UserWorkerSalary = 17000;
                            postSalary.AdminWorkerSalary = 17000;
                            postSalary.UserWorkerHalfSalary = 10000;
                            break;
                        case PostName.КарщикКладовщик:
                            postSalary.UserWorkerSalary = 27000;
                            postSalary.AdminWorkerSalary = 27000;
                            postSalary.UserWorkerHalfSalary = 10000;
                            break;
                        case PostName.Логист:
                            postSalary.UserWorkerSalary = 25000;
                            postSalary.AdminWorkerSalary = 25000;
                            postSalary.UserWorkerHalfSalary = 10000;
                            break;
                        case PostName.БригадирОклейщик:
                            postSalary.UserWorkerSalary = 18000;
                            postSalary.AdminWorkerSalary = 18000;
                            postSalary.UserWorkerHalfSalary = 10000;
                            break;
                    }

                    post.DirectoryPostSalaries.Add(postSalary);

                    _dc.DirectoryPosts.Add(post);
                    _dc.SaveChanges();
                }
            }
        }

        public void InitializeDefaultDataBaseWithWorkers()
        {
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
                    infoDate.DescriptionDay = (DescriptionDay)GetRandomNumber(0, 6);

                    if (infoDate.DescriptionDay == DescriptionDay.Был)
                    {
                        infoDate.CountHours = GetRandomNumber(1, 9);
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
                    infoDate.DescriptionDay = (DescriptionDay)GetRandomNumber(0, 6);

                    if (infoDate.DescriptionDay == DescriptionDay.Был)
                    {
                        infoDate.CountHours = GetRandomNumber(1, 9);
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

            _dc.SaveChanges();
        }

        public void InitializeDefaultDataBaseWithOfficeWorkers()
        {
            var officePost = new DirectoryPost
            {
                Name = "ГлавБух_Чернецкая",
                DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Офис"),
                DirectoryCompany = _dc.DirectoryCompanies.First(c => c.Name == "АВ"),
                DirectoryPostSalaries = new List<DirectoryPostSalary>
                {
                    new DirectoryPostSalary
                    {
                        Date = new DateTime(2011, 01, 01),
                        UserWorkerSalary = 45000,
                        AdminWorkerSalary = 45000,
                        UserWorkerHalfSalary = 45000
                    }
                }
            };

            _dc.DirectoryPosts.Add(officePost);

            officePost = new DirectoryPost
            {
                Name = "Бухгалтер_Казёнова",
                DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Офис"),
                DirectoryCompany = _dc.DirectoryCompanies.First(c => c.Name == "АВ"),
                DirectoryPostSalaries = new List<DirectoryPostSalary>
                {
                    new DirectoryPostSalary
                    {
                        Date = new DateTime(2011, 01, 01),
                        UserWorkerSalary = 40000,
                        AdminWorkerSalary = 40000,
                        UserWorkerHalfSalary = 0
                    }
                }
            };

            _dc.DirectoryPosts.Add(officePost);

            officePost = new DirectoryPost
            {
                Name = "Бухгалтер_Дикрет_Губашева",
                DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Офис"),
                DirectoryCompany = _dc.DirectoryCompanies.First(c => c.Name == "АВ"),
                DirectoryPostSalaries = new List<DirectoryPostSalary>
                {
                    new DirectoryPostSalary
                    {
                        Date = new DateTime(2011, 01, 01),
                        UserWorkerSalary = 0,
                        AdminWorkerSalary = 0,
                        UserWorkerHalfSalary = 0
                    }
                }
            };

            _dc.DirectoryPosts.Add(officePost);

            officePost = new DirectoryPost
            {
                Name = "Бухгалтер_Гаганова",
                DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Офис"),
                DirectoryCompany = _dc.DirectoryCompanies.First(c => c.Name == "Фенокс"),
                DirectoryPostSalaries = new List<DirectoryPostSalary>
                {
                    new DirectoryPostSalary
                    {
                        Date = new DateTime(2011, 01, 01),
                        UserWorkerSalary = 30000,
                        AdminWorkerSalary = 30000,
                        UserWorkerHalfSalary = 0
                    }
                }
            };

            _dc.DirectoryPosts.Add(officePost);

            officePost = new DirectoryPost
            {
                Name = "Бухгалтер_Крицкая",
                DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Офис"),
                DirectoryCompany = _dc.DirectoryCompanies.First(c => c.Name == "АВ"),
                DirectoryPostSalaries = new List<DirectoryPostSalary>
                {
                    new DirectoryPostSalary
                    {
                        Date = new DateTime(2011, 01, 01),
                        UserWorkerSalary = 30000,
                        AdminWorkerSalary = 30000,
                        UserWorkerHalfSalary = 0
                    }
                }
            };

            _dc.DirectoryPosts.Add(officePost);

            officePost = new DirectoryPost
            {
                Name = "Бухгалтер_Рыжова",
                DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Офис"),
                DirectoryCompany = _dc.DirectoryCompanies.First(c => c.Name == "АВ"),
                DirectoryPostSalaries = new List<DirectoryPostSalary>
                {
                    new DirectoryPostSalary
                    {
                        Date = new DateTime(2011, 01, 01),
                        UserWorkerSalary = 12000,
                        AdminWorkerSalary = 12000,
                        UserWorkerHalfSalary = 13000
                    }
                }
            };

            _dc.DirectoryPosts.Add(officePost);

            officePost = new DirectoryPost
            {
                Name = "Директор",
                DirectoryTypeOfPost = _dc.DirectoryTypeOfPosts.First(t => t.Name == "Офис"),
                DirectoryCompany = _dc.DirectoryCompanies.First(c => c.Name == "АВ"),
                DirectoryPostSalaries = new List<DirectoryPostSalary>
                {
                    new DirectoryPostSalary
                    {
                        Date = new DateTime(2011, 01, 01),
                        UserWorkerSalary = 35000,
                        AdminWorkerSalary = 35000,
                        UserWorkerHalfSalary = 30000
                    }
                }
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

        private void AddOfficeWorker(string lastName, string firstName, string postName, string companyName, bool isTwoCompanies, bool isDeadSpirit = false)
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
                IsDeadSpirit = isDeadSpirit,
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


        #region DirectoryCompany

        public IEnumerable<string> GetDirectoryCompanies(int workerId, int year, int month, int lastDayInMonth)
        {
            var currentPosts = GetCurrentPosts(workerId, year, month, lastDayInMonth);
            var directoryPosts = currentPosts.Select(p => p.DirectoryPost);

            return directoryPosts.Select(p => p.DirectoryCompany.Name).Distinct();
        }

        public IQueryable<DirectoryCompany> GetDirectoryCompanies()
        {
            return _dc.DirectoryCompanies;
        }

        public DirectoryCompany AddDirectoryCompany(string directoryCompanyName)
        {
            var directoryCompany = new DirectoryCompany
            {
                Name = directoryCompanyName,
            };

            _dc.DirectoryCompanies.Add(directoryCompany);

            _dc.SaveChanges();

            return directoryCompany;
        }

        public void RemoveDirectoryCompany(int directoryCompanyId)
        {
            var directoryCompany = _dc.DirectoryCompanies.Find(directoryCompanyId);
            _dc.DirectoryCompanies.Remove(directoryCompany);

            _dc.SaveChanges();
        }

        #endregion


        #region DirectoryTypeOfPost

        public IQueryable<DirectoryTypeOfPost> GetDirectoryTypeOfPosts()
        {
            return _dc.DirectoryTypeOfPosts;
        }

        public DirectoryTypeOfPost AddDirectoryTypeOfPost(string directoryTypeOfPostName)
        {
            var directoryTypeOfPost = new DirectoryTypeOfPost
            {
                Name = directoryTypeOfPostName
            };

            _dc.DirectoryTypeOfPosts.Add(directoryTypeOfPost);

            _dc.SaveChanges();

            return directoryTypeOfPost;
        }

        public void RemoveDirectoryTypeOfPost(int directoryTypeOfPostId)
        {
            var directoryTypeOfPost = _dc.DirectoryTypeOfPosts.Find(directoryTypeOfPostId);
            _dc.DirectoryTypeOfPosts.Remove(directoryTypeOfPost);

            _dc.SaveChanges();
        }

        public DirectoryTypeOfPost GetDirectoryTypeOfPost(int workerId, DateTime date)
        {
            return GetCurrentPost(workerId, date).DirectoryPost.DirectoryTypeOfPost;
        }

        #endregion


        #region DirectoryPost

        public IQueryable<DirectoryPost> GetDirectoryPosts()
        {
            return _dc.DirectoryPosts;
        }

        public IQueryable<DirectoryPost> GetDirectoryPosts(DirectoryCompany company)
        {
            return _dc.DirectoryPosts.Where(p => p.DirectoryCompanyId == company.Id);
        }

        public DirectoryPost GetDirectoryPost(string postName)
        {
            return _dc.DirectoryPosts.FirstOrDefault(p => p.Name == postName);
        }

        public DirectoryPost AddDirectoryPost(string name, DirectoryTypeOfPost typeOfPost, DirectoryCompany company, List<DirectoryPostSalary> postSalaries)
        {
            var directoryPost = new DirectoryPost
            {
                Name = name,
                DirectoryTypeOfPost = typeOfPost,
                DirectoryCompany = company,
                DirectoryPostSalaries = postSalaries
            };

            _dc.DirectoryPosts.Add(directoryPost);
            _dc.SaveChanges();

            return directoryPost;
        }

        public DirectoryPost EditDirectoryPost(int postId, string name, DirectoryTypeOfPost typeOfPost, DirectoryCompany company, List<DirectoryPostSalary> postSalaries)
        {
            var directoryPost = _dc.DirectoryPosts.Find(postId);
            directoryPost.Name = name;
            directoryPost.DirectoryTypeOfPost = typeOfPost;
            directoryPost.DirectoryCompany = company;
            _dc.DirectoryPostSalaries.RemoveRange(directoryPost.DirectoryPostSalaries);
            _dc.SaveChanges();
            directoryPost.DirectoryPostSalaries = postSalaries;
            _dc.SaveChanges();

            return directoryPost;
        }

        public void RemoveDirectoryPost(DirectoryPost post)
        {
            Log(LoggingOptions.Fatal, "Удаление должности", post.Name, post.DirectoryTypeOfPost.Name, post.DirectoryCompany.Name);

            _dc.DirectoryPosts.Remove(post);
            _dc.SaveChanges();
        }

        public bool ExistsDirectoryPost(string name)
        {
            return _dc.DirectoryPosts.Any(p => p.Name == name);
        }

        #endregion


        #region DirectoryWorker


        public IQueryable<DirectoryWorker> GetDeadSpiritDirectoryWorkers(DateTime date)
        {
            return _dc.DirectoryWorkers.Where(w => w.IsDeadSpirit && (DbFunctions.DiffDays(w.StartDate, date) >= 0 &&
                (w.FireDate == null || (w.FireDate != null && DbFunctions.DiffDays(w.FireDate.Value, date) <= 0))));
        }


        public DirectoryWorker AddDirectoryWorker(string lastName, string firstName, string midName, Gender gender,
            DateTime birthDay, string address, string homePhone, string cellPhone, DateTime startDate, BitmapImage photo,
            DateTime? fireDate, ICollection<CurrentCompanyAndPost> currentCompaniesAndPosts, bool isDeadSpirit)
        {
            byte[] dataPhoto = null;
            if (photo != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(photo));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    dataPhoto = ms.ToArray();
                }
            }

            var worker = new DirectoryWorker
            {
                LastName = lastName,
                FirstName = firstName,
                MidName = midName,
                Gender = gender,
                BirthDay = birthDay,
                Address = address,
                HomePhone = homePhone,
                CellPhone = cellPhone,
                StartDate = startDate,
                DirectoryPhoto = new DirectoryPhoto { Photo = dataPhoto },
                FireDate = fireDate,
                CurrentCompaniesAndPosts = new List<CurrentPost>(currentCompaniesAndPosts.Select(c => new CurrentPost { ChangeDate = c.PostChangeDate, FireDate = c.PostFireDate, DirectoryPostId = c.DirectoryPost.Id, IsTwoCompanies = c.IsTwoCompanies })),
                IsDeadSpirit = isDeadSpirit
            };

            _dc.DirectoryWorkers.Add(worker);
            _dc.SaveChanges();

            for (var date = startDate; date <= DateTime.Now; date = date.AddDays(1))
            {
                var infoDate = new InfoDate
                {
                    Date = date,
                    DescriptionDay = DescriptionDay.Был,
                    CountHours = null
                };

                if (!IsWeekend(date))
                {
                    infoDate.CountHours = 8;
                }

                worker.InfoDates.Add(infoDate);
            }

            _dc.SaveChanges();

            double birthday = GetParameterValue<double>("Birthday");
            for (var date = startDate; date <= DateTime.Now; date = date.AddMonths(1))
            {
                var infoMonth = new InfoMonth
                {
                    Date = new DateTime(date.Year, date.Month, 1),
                    BirthDays = !worker.IsDeadSpirit ? birthday : 0
                };

                worker.InfoMonthes.Add(infoMonth);
            }

            _dc.SaveChanges();

            return worker;
        }

        public DirectoryWorker AddDirectoryWorker(string lastName, string firstName, string midName, Gender gender,
            DateTime birthDay, string address, string homePhone, string cellPhone, DateTime startDate,
            DateTime? fireDate, CurrentPost currentPost)
        {
            var worker = new DirectoryWorker
            {
                LastName = lastName,
                FirstName = firstName,
                MidName = midName,
                Gender = gender,
                BirthDay = birthDay,
                Address = address,
                HomePhone = homePhone,
                CellPhone = cellPhone,
                StartDate = startDate,
                FireDate = fireDate,
                CurrentCompaniesAndPosts = new List<CurrentPost> { currentPost }
            };

            _dc.DirectoryWorkers.Add(worker);
            _dc.SaveChanges();

            return worker;
        }

        public IQueryable<DirectoryWorker> GetDirectoryWorkers()
        {
            return _dc.DirectoryWorkers.Include(w => w.CurrentCompaniesAndPosts.Select(c => c.DirectoryPost.DirectoryTypeOfPost));
        }

        public IQueryable<DirectoryWorker> GetDirectoryWorkers(int year, int month)
        {
            var firstDateInMonth = new DateTime(year, month, 1);
            var lastDateInMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            return _dc.DirectoryWorkers.Where(w => DbFunctions.DiffDays(w.StartDate, lastDateInMonth) >= 0 &&
                (w.FireDate == null || (w.FireDate != null && DbFunctions.DiffDays(w.FireDate.Value, firstDateInMonth) <= 0)));
        }

        public IQueryable<DirectoryWorker> GetDirectoryWorkersMonthTimeSheet(int year, int month)
        {
            var firstDateInMonth = new DateTime(year, month, 1);
            var lastDateInMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            return _dc.DirectoryWorkers.Where(w => DbFunctions.DiffDays(w.StartDate, lastDateInMonth) >= 0 &&
                (w.FireDate == null || (w.FireDate != null && DbFunctions.DiffDays(w.FireDate.Value, firstDateInMonth) <= 0))).
                Include(w => w.CurrentCompaniesAndPosts.Select(c => c.DirectoryPost.DirectoryTypeOfPost));
        }

        public IEnumerable<DirectoryWorker> GetDirectoryWorkers(int year, int month, bool isOffice)
        {
            var firstDateInMonth = new DateTime(year, month, 1);
            var lastDateInMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var workers = GetDirectoryWorkers(year, month).ToList();

            foreach (var worker in workers)
            {
                var post = GetCurrentPost(worker.Id, lastDateInMonth);

                if (isOffice)
                {
                    if (post.DirectoryPost.DirectoryTypeOfPost.Name == TYPE_OF_POST_OFFICE && worker.StartDate.Date <= lastDateInMonth.Date &&
                        (worker.FireDate == null || (worker.FireDate != null && worker.FireDate.Value.Date >= firstDateInMonth.Date)))
                    {
                        yield return worker;
                    }
                }
                else
                {
                    if (post.DirectoryPost.DirectoryTypeOfPost.Name != TYPE_OF_POST_OFFICE && worker.StartDate.Date <= lastDateInMonth.Date &&
                        (worker.FireDate == null || (worker.FireDate != null && worker.FireDate.Value.Date >= firstDateInMonth.Date)))
                    {
                        yield return worker;
                    }
                }
            }
        }

        public IQueryable<DirectoryWorker> GetDirectoryWorkers(DateTime fromDate, DateTime toDate)
        {
            return _dc.DirectoryWorkers.Where(w => DbFunctions.DiffDays(w.StartDate, toDate) >= 0 && (w.FireDate == null || w.FireDate != null && DbFunctions.DiffDays(w.FireDate.Value, fromDate) <= 0));
        }

        public IEnumerable<DirectoryWorker> GetDirectoryWorkersWithInfoDatesAndPanalties(int year, int month, bool isOffice)
        {
            var firstDateInMonth = new DateTime(year, month, 1);
            var lastDateInMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var workers = GetDirectoryWorkers(year, month).Include(w => w.InfoDates).Include("InfoDates.InfoPanalty").ToList();

            foreach (var worker in workers)
            {
                var post = GetCurrentPost(worker.Id, lastDateInMonth);

                if (isOffice)
                {
                    if (post.DirectoryPost.DirectoryTypeOfPost.Name == TYPE_OF_POST_OFFICE && worker.StartDate.Date <= lastDateInMonth.Date &&
                        (worker.FireDate == null || (worker.FireDate != null && worker.FireDate.Value.Date >= firstDateInMonth.Date)))
                    {
                        yield return worker;
                    }
                }
                else
                {
                    if (post.DirectoryPost.DirectoryTypeOfPost.Name != TYPE_OF_POST_OFFICE && worker.StartDate.Date <= lastDateInMonth.Date &&
                        (worker.FireDate == null || (worker.FireDate != null && worker.FireDate.Value.Date >= firstDateInMonth.Date)))
                    {
                        yield return worker;
                    }
                }
            }
        }

        public DirectoryWorker GetDirectoryWorker(int workerId)
        {
            return _dc.DirectoryWorkers.Find(workerId);
        }

        public DirectoryWorker GetDirectoryWorkerWithPosts(int workerId)
        {
            return _dc.DirectoryWorkers.Include(w => w.CurrentCompaniesAndPosts.Select(p => p.DirectoryPost.DirectoryPostSalaries)).First(w => w.Id == workerId);
        }

        public DirectoryWorker GetDirectoryWorker(string lastName, string firstName)
        {
            return _dc.DirectoryWorkers.FirstOrDefault(w => w.LastName == lastName && w.FirstName == firstName);
        }

        public DirectoryWorker EditDirectoryWorker(int id, string lastName, string firstName, string midName, Gender gender, DateTime birthDay, string address, string homePhone,
            string cellPhone, DateTime startDate, BitmapImage photo, DateTime? fireDate, ICollection<CurrentCompanyAndPost> currentCompaniesAndPosts, bool isDeadSpirit)
        {
            var directoryWorker = GetDirectoryWorker(id);

            byte[] dataPhoto = null;
            if (photo != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(photo));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    dataPhoto = ms.ToArray();
                }
            }

            directoryWorker.LastName = lastName;
            directoryWorker.FirstName = firstName;
            directoryWorker.MidName = midName;
            directoryWorker.Gender = gender;
            directoryWorker.BirthDay = birthDay;
            directoryWorker.Address = address;
            directoryWorker.HomePhone = homePhone;
            directoryWorker.CellPhone = cellPhone;
            directoryWorker.StartDate = startDate;

            if (directoryWorker.DirectoryPhoto == null)
            {
                directoryWorker.DirectoryPhoto = new DirectoryPhoto();
            }

            directoryWorker.DirectoryPhoto.Photo = dataPhoto;
            directoryWorker.FireDate = fireDate;

            _dc.CurrentPosts.RemoveRange(directoryWorker.CurrentCompaniesAndPosts);

            directoryWorker.CurrentCompaniesAndPosts = new List<CurrentPost>(currentCompaniesAndPosts.Select(c => new CurrentPost { ChangeDate = c.PostChangeDate, FireDate = c.PostFireDate, DirectoryPostId = c.DirectoryPost.Id, IsTwoCompanies = c.IsTwoCompanies }));
            directoryWorker.IsDeadSpirit = isDeadSpirit;

            _dc.SaveChanges();

            return directoryWorker;
        }

        #endregion


        #region InfoDates

        public IEnumerable<InfoDate> GetInfoDatePanalties(int workerId, int year, int month)
        {
            var worker = GetDirectoryWorker(workerId);
            return worker.InfoDates.Where(d => d.Date.Year == year && d.Date.Month == month && d.InfoPanalty != null);
        }

        public IEnumerable<InfoDate> GetInfoDatePanaltiesWithoutCash(int workerId, int year, int month)
        {
            RefreshContext();
            var worker = GetDirectoryWorker(workerId);
            return worker.InfoDates.Where(d => d.Date.Year == year && d.Date.Month == month && d.InfoPanalty != null);
        }

        public void EditInfoDateHour(int workerId, DateTime date, string hour)
        {
            var worker = GetDirectoryWorker(workerId);
            var infoDate = worker.InfoDates.First(d => d.Date.Date == date.Date);

            if (hour != "В")
            {
                if (Enum.IsDefined(typeof(DescriptionDay), hour))
                {
                    infoDate.CountHours = null;
                    infoDate.DescriptionDay = (DescriptionDay)Enum.Parse(typeof(DescriptionDay), hour);
                }
                else
                {
                    infoDate.CountHours = double.Parse(hour);
                    infoDate.DescriptionDay = DescriptionDay.Был;
                }
            }
            else
            {
                infoDate.CountHours = null;
                infoDate.DescriptionDay = DescriptionDay.Был;
            }

            _dc.SaveChanges();
        }

        public IQueryable<InfoDate> GetInfoDates(DateTime date)
        {
            return _dc.InfoDates.Where(d => DbFunctions.DiffDays(d.Date, date) == 0);
        }

        public IQueryable<InfoDate> GetInfoDates(int workerId, int year, int month)
        {
            var worker = GetDirectoryWorker(workerId);
            return worker.InfoDates.AsQueryable().Include(d => d.InfoPanalty).Where(d => d.Date.Year == year && d.Date.Month == month);
        }

        public IQueryable<InfoDate> GetInfoDates(int year, int month)
        {
            return _dc.InfoDates.Where(d => d.Date.Year == year && d.Date.Month == month).Include(d => d.InfoPanalty);
        }

        public double? IsOverTime(InfoDate infoDate, List<DateTime> weekEnds)
        {
            if (weekEnds.Any(w => w.Date == infoDate.Date.Date))
            {
                return infoDate.CountHours != null ? infoDate.CountHours : null;
            }
            else
            {
                if (infoDate.CountHours != null)
                {
                    return infoDate.CountHours > 8 ? infoDate.CountHours - 8 : null;
                }
                else
                {
                    return null;
                }
            }
        }

        public void EditDeadSpiritHours(int deadSpiritWorkerId, DateTime date, double hoursSpiritWorker)
        {
            var deadSpiritWorker = GetDirectoryWorker(deadSpiritWorkerId);

            var infoDateDeadSpirit = deadSpiritWorker.InfoDates.First(d => d.Date.Date == date.Date);
            infoDateDeadSpirit.CountHours = hoursSpiritWorker;
            _dc.SaveChanges();
        }

        public DateTime GetLastWorkDay(int workerId)
        {
            var worker = GetDirectoryWorker(workerId);
            var infoDate = worker.InfoDates.OrderByDescending(d => d.Date).FirstOrDefault(d => d.CountHours != null);
            return infoDate != null ? infoDate.Date : DateTime.Now;
        }

        #endregion


        #region InfoMonth

        public void EditInfoMonthPayment(int workerId, DateTime date, string propertyName, double propertyValue)
        {
            var worker = GetDirectoryWorker(workerId);
            var infoMonth = worker.InfoMonthes.First(m => m.Date.Year == date.Year && m.Date.Month == date.Month);

            infoMonth.GetType().GetProperty(propertyName).SetValue(infoMonth, propertyValue);
            _dc.SaveChanges();
        }

        public IQueryable<int> GetYears()
        {
            return _dc.InfoMonthes.Select(m => m.Date.Year).Distinct();
        }

        public IQueryable<int> GetMonthes(int year)
        {
            return _dc.InfoMonthes.Where(m => m.Date.Year == year).Select(m => m.Date.Month).Distinct();
        }

        public InfoMonth GetInfoMonth(int workerId, int year, int month)
        {
            var worker = GetDirectoryWorker(workerId);
            return worker.InfoMonthes.First(m => m.Date.Year == year && m.Date.Month == month);
        }

        public IQueryable<InfoMonth> GetInfoMonthes(int year, int month)
        {
            return _dc.InfoMonthes.Where(m => m.Date.Year == year && m.Date.Month == month);
        }

        #endregion


        #region InfoPanalty

        public InfoPanalty GetInfoPanalty(int workerId, DateTime date)
        {
            var worker = GetDirectoryWorker(workerId);
            return worker.InfoDates.AsQueryable().First(d => d.Date.Date == date.Date).InfoPanalty;
        }

        public bool IsInfoPanalty(int workerId, DateTime date)
        {
            var worker = GetDirectoryWorker(workerId);
            return worker.InfoDates.First(d => d.Date.Date == date.Date).InfoPanalty != null;
        }

        public InfoPanalty AddInfoPanalty(int workerId, DateTime date, double summ, string description)
        {
            var worker = GetDirectoryWorker(workerId);
            var infoPanalty = new InfoPanalty
            {
                Summ = summ,
                Description = description
            };

            worker.InfoDates.First(d => d.Date.Date == date.Date).InfoPanalty = infoPanalty;

            _dc.SaveChanges();
            return infoPanalty;
        }


        public InfoPanalty EditInfoPanalty(int workerId, DateTime date, double summ, string description)
        {
            var worker = GetDirectoryWorker(workerId);

            var infoPanalty = worker.InfoDates.First(d => d.Date.Date == date.Date).InfoPanalty;
            infoPanalty.Summ = summ;
            infoPanalty.Description = description;

            _dc.SaveChanges();
            return infoPanalty;
        }

        public void RemoveInfoPanalty(int workerId, DateTime date)
        {
            var worker = GetDirectoryWorker(workerId);

            var infoPanalty = worker.InfoDates.First(d => d.Date.Date == date.Date).InfoPanalty;
            _dc.InfoPanalties.Remove(infoPanalty);
            _dc.SaveChanges();
        }

        #endregion


        #region CurrentPost

        public void AddCurrentPost(int workerId, CurrentCompanyAndPost currentCompanyAndPost)
        {
            var worker = _dc.DirectoryWorkers.Find(workerId);

            if (worker.CurrentCompaniesAndPosts.Any())
            {
                var lastCurrentPost = worker.CurrentCompaniesAndPosts.OrderByDescending(c => c.ChangeDate).First();
                lastCurrentPost.FireDate = currentCompanyAndPost.PostChangeDate.AddDays(-1);
            }

            var currentPost = new CurrentPost
            {
                ChangeDate = currentCompanyAndPost.PostChangeDate,
                DirectoryPost = _dc.DirectoryPosts.First(p => p.Name == currentCompanyAndPost.DirectoryPost.Name &&
                    p.DirectoryCompany.Name == currentCompanyAndPost.DirectoryPost.DirectoryCompany.Name),
                IsTwoCompanies = currentCompanyAndPost.IsTwoCompanies
            };
            worker.CurrentCompaniesAndPosts.Add(currentPost);

            _dc.SaveChanges();
        }

        public IEnumerable<CurrentPost> GetCurrentPosts(DateTime lastDateInMonth)
        {
            var firstDateInMonth = new DateTime(lastDateInMonth.Year, lastDateInMonth.Month, 1);

            return _dc.CurrentPosts.Where(p => DbFunctions.DiffDays(p.ChangeDate, lastDateInMonth) > 0 && p.FireDate == null ||
                p.FireDate != null && DbFunctions.DiffDays(p.FireDate.Value, firstDateInMonth) < 0 && DbFunctions.DiffDays(p.ChangeDate, lastDateInMonth) > 0);
        }

        public IEnumerable<CurrentPost> GetCurrentPosts(int workerId, int year, int month, int lastDayInMonth)
        {
            var lastDateInMonth = new DateTime(year, month, lastDayInMonth);
            var firstDateInMonth = new DateTime(year, month, 1);

            var worker = GetDirectoryWorker(workerId);

            return worker.CurrentCompaniesAndPosts.Where(p => p.ChangeDate.Date <= lastDateInMonth.Date && p.FireDate == null ||
                p.FireDate != null && p.FireDate.Value.Date >= firstDateInMonth.Date && p.ChangeDate.Date <= lastDateInMonth.Date);
        }

        public CurrentPost GetCurrentPost(int workerId, DateTime date)
        {
            var worker = GetDirectoryWorker(workerId);

            return worker.CurrentCompaniesAndPosts.First(p => p.ChangeDate.Date <= date.Date && p.FireDate == null ||
                p.FireDate != null && p.FireDate.Value.Date >= date.Date && p.ChangeDate.Date <= date.Date);
        }

        #endregion


        #region Calendar

        public void InputDateToDataBase(int year)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Calendar", year.ToString() + ".txt");

            using (var sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (line[11] == 'в')
                    {
                        var date = DateTime.Parse(line.Substring(0, 10));


                        if (!_dc.DirectoryHolidays.Select(h => h.Date).Contains(date))
                        {
                            var directoryHoliday = new DirectoryHoliday { Date = date };
                            _dc.DirectoryHolidays.Add(directoryHoliday);
                        }
                    }
                }
            }
            _dc.SaveChanges();
        }

        public int GetCountWorkDaysInMonth(int year, int month)
        {

            int holiDays = _dc.DirectoryHolidays.Where(h => h.Date.Year == year && h.Date.Month == month).Count();
            return DateTime.DaysInMonth(year, month) - holiDays;
        }

        public IQueryable<DateTime> GetHolidays(int year, int month)
        {
            return _dc.DirectoryHolidays.Where(h => h.Date.Year == year && h.Date.Month == month).Select(h => h.Date);
        }

        public IQueryable<DateTime> GetHolidays(DateTime fromDate, DateTime toDate)
        {
            return _dc.DirectoryHolidays.Where(h => DbFunctions.DiffDays(h.Date, fromDate) <= 0 && DbFunctions.DiffDays(h.Date, toDate) >= 0).Select(h => h.Date);
        }

        public bool IsWeekend(DateTime date)
        {
            return _dc.DirectoryHolidays.Any(w => DbFunctions.DiffDays(w.Date, date) == 0);
        }

        #endregion


        #region Parameter

        //public void AddParameter(string name, string value)
        //{
        //    var parameter = new Parameter
        //    {
        //        Name = name,
        //        Value = value
        //    };

        //    _dc.Parameters.Add(parameter);
        //    _dc.SaveChanges();
        //}

        public void EditParameter(string name, string value)
        {
            _dc.Parameters.First(p => p.Name == name).Value = value;
            _dc.SaveChanges();
        }

        public T GetParameterValue<T>(string name)
        {
            var parameter = _dc.Parameters.First(p => p.Name == name);
            _dc.Entry<Parameter>(parameter).Reload();

            string value = parameter.Value;

            return (T)Convert.ChangeType(value, typeof(T));
        }

        #endregion


        #region Initialize

        public void InitializeAbsentDates()
        {
            var lastDate = GetParameterValue<DateTime>("LastDate");

            double birthday = GetParameterValue<double>("Birthday");

            if (DateTime.Now.Date > lastDate.Date)
            {
                var workers = GetDirectoryWorkers(lastDate, DateTime.Now).ToList();
                var holidays = GetHolidays(lastDate.AddDays(-14), DateTime.Now).ToList();

                for (var date = lastDate.AddDays(1); date.Date <= DateTime.Now.Date; date = date.AddDays(1))
                {
                    var firstDateInMonth = new DateTime(date.Year, date.Month, 1);
                    foreach (var worker in workers)
                    {
                        if (!worker.InfoMonthes.Any(m => m.Date.Year == date.Year && m.Date.Month == date.Month))
                        {
                            var infoMonth = new InfoMonth();
                            infoMonth.Date = firstDateInMonth;
                            if (worker.CurrentCompaniesAndPosts.Last().DirectoryPost.DirectoryTypeOfPost.Name != "Офис")
                            {
                                infoMonth.BirthDays = !worker.IsDeadSpirit ? birthday : 0;
                            };
                            worker.InfoMonthes.Add(infoMonth);

                            _dc.SaveChanges();
                        }

                        if (worker.StartDate.Date <= date.Date && (worker.FireDate == null || worker.FireDate != null && worker.FireDate.Value.Date >= date.Date))
                        {
                            if (!worker.InfoDates.Any(d => d.Date.Date == date.Date))
                            {
                                int countPrevDays = 0;
                                InfoDate prevInfoDate = null;
                                do
                                {
                                    countPrevDays++;
                                    prevInfoDate = worker.InfoDates.FirstOrDefault(d => d.Date.Date == date.AddDays(-countPrevDays).Date);
                                } while (prevInfoDate == null || prevInfoDate != null && holidays.Any(h => h.Date == prevInfoDate.Date.Date));

                                var prevDescriptionDay = DescriptionDay.Был;
                                if (prevInfoDate != null)
                                {
                                    prevDescriptionDay = prevInfoDate.DescriptionDay;
                                }

                                var infoDate = new InfoDate
                                {
                                    Date = date,
                                    DescriptionDay = DescriptionDay.Был
                                };

                                if (!holidays.Any(h => h.Date == date.Date))
                                {
                                    if (prevDescriptionDay == DescriptionDay.Был)
                                    {
                                        infoDate.CountHours = 8;
                                    }

                                    infoDate.DescriptionDay = prevDescriptionDay;
                                }

                                worker.InfoDates.Add(infoDate);

                                var salaryDate = new DateTime(date.Year, date.Month, 5);
                                if (lastDate.Date < salaryDate.Date && date.Date >= salaryDate.Date)
                                {
                                    InitializeWorkerLoanPayments(worker, worker.InfoMonthes.First(m => m.Date.Year == date.Year && m.Date.Month == date.Month), salaryDate);
                                }
                            }
                        }
                    }

                    _dc.SaveChanges();

                    EditParameter("LastDate", date.ToString());
                    lastDate = date;
                }
            }
        }

        private void InitializeWorkerLoanPayments(DirectoryWorker worker, InfoMonth infoMonth, DateTime salaryDate)
        {
            var infoLoans = _dc.InfoLoans.Where(s => s.DirectoryWorkerId == worker.Id && (s.DateLoanPayment == null ||
                (s.DateLoanPayment != null && DbFunctions.DiffDays(s.DateLoanPayment, salaryDate) <= 0))).ToList();

            if (infoLoans.Any())
            {
                foreach (var loan in infoLoans)
                {
                    var payments = _dc.InfoPayments.Where(p => p.InfoLoanId == loan.Id).ToList();
                    if (payments.Any())
                    {
                        var payment = payments.FirstOrDefault(p => p.Date.Date == salaryDate.Date);
                        if (payment != null)
                        {
                            infoMonth.PrepaymentCash = payment.Summ;
                            EditCurrencyValueSummChange("TotalLoan", loan.Currency, payment.Summ);

                            AddInfoSafe(payment.Date, true, payment.Summ, loan.Currency, CashType.Наличка, "Возврат долга: " + worker.FullName);

                            break;
                        }
                    }
                }
            }
        }

        #endregion


        #region DirectoryUserStatus
        public IQueryable<DirectoryUserStatus> GetDirectoryUserStatuses()
        {
            return _dc.DirectoryUserStatuses;
        }

        public DirectoryUserStatus AddDirectoryUserStatus(string name, List<CurrentUserStatusPrivilege> privileges)
        {
            var directoryUserStatus = new DirectoryUserStatus { Name = name, Privileges = privileges };
            _dc.DirectoryUserStatuses.Add(directoryUserStatus);

            _dc.SaveChanges();

            return directoryUserStatus;
        }

        public void EditDirectoryUserStatus(int userStatusId, string userStatusName, List<CurrentUserStatusPrivilege> privileges)
        {
            var userStatus = _dc.DirectoryUserStatuses.Find(userStatusId);
            userStatus.Name = userStatusName;
            _dc.CurrentUserStatusPrivileges.RemoveRange(userStatus.Privileges);
            userStatus.Privileges = privileges;

            _dc.SaveChanges();
        }

        public void RemoveDirectoryUserStatus(int id)
        {
            var directoryUserStatus = _dc.DirectoryUserStatuses.Include(s => s.Privileges).First(s => s.Id == id);
            _dc.DirectoryUserStatuses.Remove(directoryUserStatus);

            _dc.SaveChanges();
        }

        #endregion


        #region DirectoryUser

        public IQueryable<DirectoryUser> GetDirectoryUsers()
        {
            return _dc.DirectoryUsers;
        }

        public DirectoryUser GetDirectoryUser(int userId)
        {
            return _dc.DirectoryUsers.Find(userId);
        }

        public DirectoryUser AddDirectoryUser(string userName, string password, DirectoryUserStatus userStatus)
        {
            string transcriptionName = userName.Unidecode().Replace(" ", "").Replace("'", "");

            var user = new DirectoryUser
            {
                UserName = userName,
                TranscriptionName = transcriptionName,
                CurrentUserStatus = new CurrentUserStatus { DirectoryUserStatus = userStatus }
            };

            _dc.DirectoryUsers.Add(user);
            _dc.SaveChanges();

            DBCustomQueries.AddUser(_dc, transcriptionName, password);

            _dc.Database.Connection.ConnectionString = "";

            return user;
        }

        public DirectoryUser AddDirectoryUserAdmin(string userName, string password)
        {
            var userStatus = _dc.DirectoryUserStatuses.First(s => s.Name == "Администратор");

            string transcriptionName = userName.Unidecode().Replace(" ", "").Replace("'", "");

            var user = new DirectoryUser
            {
                UserName = userName,
                TranscriptionName = transcriptionName,
                CurrentUserStatus = new CurrentUserStatus { DirectoryUserStatus = userStatus }
            };

            _dc.DirectoryUsers.Add(user);
            _dc.SaveChanges();

            DBCustomQueries.AddUser(_dc, transcriptionName, password);

            return user;
        }

        public void AddUserButler()
        {
            DBCustomQueries.AddUserButler(_dc);
        }

        public void EditDirectoryUser(int userId, string userName, string password, DirectoryUserStatus userStatus)
        {
            string transcriptionName = userName.Unidecode().Replace(" ", "").Replace("'", "");


            var user = _dc.DirectoryUsers.Find(userId);

            string prevName = user.TranscriptionName;

            user.UserName = userName;
            user.TranscriptionName = transcriptionName;

            var prevCurrentUserStatus = user.CurrentUserStatus;

            user.CurrentUserStatus = new CurrentUserStatus { DirectoryUserStatus = userStatus };

            _dc.SaveChanges();

            _dc.CurrentUserStatuses.Remove(prevCurrentUserStatus);
            _dc.SaveChanges();

            DBCustomQueries.EditUser(_dc, prevName, userName, password);
        }


        public void RemoveDirectoryUser(DirectoryUser user)
        {
            _dc.DirectoryUsers.Remove(user);

            _dc.SaveChanges();
        }


        #endregion


        #region DirectoryUserStatusPrivilege

        public DirectoryUserStatusPrivilege GetDirectoryUserStatusPrivilege(string privilegeName)
        {
            return _dc.DirectoryUserStatusPrivileges.First(p => p.Name == privilegeName);
        }

        public List<string> GetPrivileges(int userId)
        {
            int currentUserStatusId = _dc.DirectoryUsers.Find(userId).CurrentUserStatusId;
            int directoryUserStatusId = _dc.CurrentUserStatuses.Find(currentUserStatusId).DirectoryUserStatusId;
            var directoryUserStatusPrivilegeIds = _dc.CurrentUserStatusPrivileges.Where(p => p.DirectoryUserStatusId == directoryUserStatusId).Select(p => p.DirectoryUserStatusPrivilegeId);
            var privileges = _dc.DirectoryUserStatusPrivileges.Where(p => directoryUserStatusPrivilegeIds.Contains(p.Id)).Select(p => p.Name).ToList();
            return privileges;
        }

        #endregion


        #region DirectoryRC

        public IQueryable<DirectoryRC> GetDirectoryRCs()
        {
            return _dc.DirectoryRCs;
        }

        public IQueryable<DirectoryRC> GetDirectoryRCsByPercentage()
        {
            return _dc.DirectoryRCs.Where(r => r.Percentes > 0);
        }

        public DirectoryRC GetDirectoryRC(string name)
        {
            return GetDirectoryRCs().First(r => r.Name == name);
        }

        public DirectoryRC AddDirectoryRC(string directoryRCName, string descriptionName, int percentes)
        {
            var directoryRC = new DirectoryRC
            {
                Name = directoryRCName,
                DescriptionName = descriptionName,
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

        public IQueryable<DirectoryRC> GetDirectoryRCsMonthIncoming(int year, int month)
        {
            return GetInfoCosts(year, month).Where(c => c.DirectoryCostItem.Name == "Приход").Select(c => c.DirectoryRC).Distinct();
        }

        public IQueryable<DirectoryRC> GetDirectoryRCsMonthExpense(int year, int month)
        {
            return GetInfoCosts(year, month).Where(c => !c.IsIncoming && c.Currency == Currency.RUR).Select(c => c.DirectoryRC).Distinct();
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

        public void EditInfoOverTime(DateTime date, double hoursOverTime)
        {
            var overTime = GetInfoOverTime(date);
            if (overTime != null)
            {
                overTime.EndDate = overTime.StartDate.AddHours(hoursOverTime);
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


        #region InfoCost

        public InfoCost EditInfoCost(DateTime date, DirectoryCostItem costItem, DirectoryRC rc, DirectoryNote note, bool isIncomming, double summ, Currency currency, double weight)
        {
            var infoCosts = GetInfoCosts(date).ToList();
            var infoCost = infoCosts.FirstOrDefault(c => c.DirectoryCostItem.Id == costItem.Id && c.DirectoryRCId == rc.Id && c.CurrentNotes.First().DirectoryNoteId == note.Id);
            if (infoCost == null)
            {
                infoCost = new InfoCost
                {
                    GroupId = Guid.NewGuid(),
                    Date = date,
                    DirectoryCostItemId = costItem.Id,
                    DirectoryRC = rc,
                    CurrentNotes = new List<CurrentNote> { new CurrentNote { DirectoryNote = note } },
                    Summ = summ,
                    Currency = currency,
                    IsIncoming = isIncomming,
                    Weight = weight,
                };

                _dc.InfoCosts.Add(infoCost);

                AddInfoSafe(infoCost.Date, infoCost.IsIncoming, infoCost.Summ, currency, CashType.Наличка, rc.Name + " " + infoCost.ConcatNotes);
            }
            else
            {
                double prevSumm = infoCost.Summ;

                infoCost.Summ = summ;
                infoCost.Currency = currency;
                infoCost.IsIncoming = isIncomming;
                infoCost.Weight = weight;

                if (infoCost.Summ - prevSumm != 0)
                {
                    AddInfoSafe(infoCost.Date, infoCost.IsIncoming, infoCost.Summ - prevSumm, currency, CashType.Наличка, infoCost.DirectoryRC.Name + " " + infoCost.ConcatNotes);
                }
            }

            _dc.SaveChanges();
            return infoCost;
        }
        public IQueryable<InfoCost> GetInfoCosts(DateTime date)
        {
            return _dc.InfoCosts.Where(c => DbFunctions.DiffDays(date, c.Date) == 0);
        }

        public IQueryable<InfoCost> GetInfoCosts(int year, int month)
        {
            return _dc.InfoCosts.Include(c => c.DirectoryCostItem).Include(c => c.DirectoryRC).
                Include(c => c.DirectoryTransportCompany).Include(c => c.CurrentNotes.Select(n => n.DirectoryNote)).
                Where(c => c.Date.Year == year && c.Date.Month == month).OrderBy(c => c.Date);
        }

        public IQueryable<InfoCost> GetInfoCostsRCIncoming(int year, int month, string rcName)
        {
            return _dc.InfoCosts.Where(c => c.Date.Year == year && c.Date.Month == month && c.DirectoryRC.Name == rcName && c.DirectoryCostItem.Name == "Приход");
        }

        public IQueryable<InfoCost> GetInfoCosts26Expense(int year, int month)
        {
            return _dc.InfoCosts.Where(c => c.Date.Year == year && c.Date.Month == month && c.DirectoryRC.Name == "26А" && !c.IsIncoming);
        }

        public IEnumerable<InfoCost> GetInfoCostsRCAndAll(int year, int month, string rcName)
        {
            var infoCosts = GetInfoCosts(year, month).ToList();
            var infoCostsRC = infoCosts.Where(c => c.DirectoryRC.Name == rcName && c.Currency == Currency.RUR).ToList();

            if (_dc.DirectoryRCs.First(r => r.Name == rcName).Percentes > 0)
            {
                infoCostsRC.AddRange(infoCosts.Where(c => c.DirectoryRC.Name == "ВСЕ" && c.Currency == Currency.RUR).ToList());
            }

            return infoCostsRC;
        }

        public IQueryable<InfoCost> GetInfoCostsTransportAndNoAllAndExpenseOnly(int year, int month)
        {
            return GetInfoCosts(year, month).Include(c => c.DirectoryCostItem).Include(c => c.DirectoryRC).
                Include(c => c.DirectoryTransportCompany).Include(c => c.CurrentNotes.Select(n => n.DirectoryNote)).
                Where(c => !c.IsIncoming && c.DirectoryCostItem.Name == "Транспорт (5031)" && c.DirectoryRC.Name != "ВСЕ");
        }

        public IQueryable<InfoCost> GetInfoCostsTransportAndNoAllAndExpenseOnly(DateTime date)
        {
            return GetInfoCosts(date).Where(c => !c.IsIncoming && c.DirectoryCostItem.Name == "Транспорт (5031)" && c.DirectoryRC.Name != "ВСЕ");
        }

        public void AddInfoCosts(DateTime date, DirectoryCostItem directoryCostItem, bool isIncoming, DirectoryTransportCompany transportCompany, double summ, Currency currency, List<Transport> transports)
        {
            var groupId = Guid.NewGuid();
            if (directoryCostItem.Name == "Транспорт (5031)" && (transports[0].DirectoryRC.Name != "26А" || !isIncoming))
            {
                double commonWeight = transports.Sum(t => t.Weight);

                foreach (var rc in transports.Select(t => t.DirectoryRC.Name).Distinct())
                {
                    var currentNotes = new List<CurrentNote>();
                    double weightRC = 0;

                    foreach (var transport in transports.Where(t => t.DirectoryRC.Name == rc))
                    {
                        currentNotes.Add(new CurrentNote { DirectoryNote = _dc.DirectoryNotes.Find(transport.DirectoryNote.Id) });
                        weightRC += transport.Weight;
                    }

                    var infoCost = new InfoCost
                    {
                        GroupId = groupId,
                        Date = date,
                        DirectoryCostItem = _dc.DirectoryCostItems.Find(directoryCostItem.Id),
                        DirectoryRC = _dc.DirectoryRCs.First(r => r.Name == rc),
                        IsIncoming = isIncoming,
                        DirectoryTransportCompany = transportCompany != null ? _dc.DirectoryTransportCompanies.Find(transportCompany.Id) : null,
                        Summ = weightRC != 0 ? Math.Round(summ / commonWeight * weightRC, 0) : summ,
                        Currency = currency,
                        CurrentNotes = currentNotes,
                        Weight = weightRC
                    };

                    _dc.InfoCosts.Add(infoCost);

                    AddInfoSafe(infoCost.Date, infoCost.IsIncoming, infoCost.Summ, currency, CashType.Наличка, infoCost.DirectoryRC.Name + " " + infoCost.ConcatNotes);
                }
            }
            else
            {
                var infoCost = new InfoCost
                {
                    GroupId = groupId,
                    Date = date,
                    DirectoryCostItem = _dc.DirectoryCostItems.Find(directoryCostItem.Id),
                    DirectoryRC = _dc.DirectoryRCs.Find(transports.First().DirectoryRC.Id),
                    IsIncoming = isIncoming,
                    DirectoryTransportCompany = transportCompany != null ? _dc.DirectoryTransportCompanies.Find(transportCompany.Id) : null,
                    Summ = summ,
                    Currency = currency,
                    CurrentNotes = new List<CurrentNote> { new CurrentNote { DirectoryNote = _dc.DirectoryNotes.Find(transports.First().DirectoryNote.Id) } },
                    Weight = 0
                };

                _dc.InfoCosts.Add(infoCost);
                AddInfoSafe(infoCost.Date, infoCost.IsIncoming, infoCost.Summ, currency, CashType.Наличка, infoCost.DirectoryRC.Name + " " + infoCost.ConcatNotes);
            }

            _dc.SaveChanges();
        }

        public void RemoveInfoCost(InfoCost infoCost)
        {
            var infoCosts = GetInfoCosts(infoCost.Date).Where(c => c.GroupId == infoCost.GroupId).ToList();

            foreach (var cost in infoCosts)
            {
                AddInfoSafe(infoCost.Date, !infoCost.IsIncoming, infoCost.Summ, infoCost.Currency, CashType.Наличка, infoCost.DirectoryRC.Name + " " + infoCost.ConcatNotes);
            }

            _dc.InfoCosts.RemoveRange(infoCosts);
            _dc.SaveChanges();
        }

        public IEnumerable<int> GetInfoCostYears()
        {
            return _dc.InfoCosts.Select(c => c.Date.Year).Distinct().OrderBy(y => y).ToList();
        }

        public IEnumerable<int> GetInfoCostMonthes(int year)
        {
            return _dc.InfoCosts.Where(c => c.Date.Year == year).Select(c => c.Date.Month).Distinct().OrderBy(m => m).ToList();
        }

        public double GetInfoCost26Summ(int year, int month)
        {
            var infoCosts = GetInfoCosts(year, month).Where(c => c.DirectoryRC.Name == "26А" && c.IsIncoming);
            return infoCosts.Any() ? infoCosts.Sum(c => c.Summ) : 0;
        }

        public string[] GetInfoCostsIncomingTotalSummsCurrency(int year, int month, string rcName, bool? isIncoming = null, string costItem = null)
        {
            string[] totalSumms = new string[Enum.GetNames(typeof(Currency)).Count()];

            for (int i = 0; i < totalSumms.Count(); i++)
            {
                var currency = (Currency)Enum.Parse(typeof(Currency), Enum.GetName(typeof(Currency), i));
                var infoCosts = _dc.InfoCosts.Where(c => c.Date.Year == year && c.Date.Month == month && c.Currency == currency && c.DirectoryRC.Name == rcName);

                if (isIncoming != null)
                {
                    infoCosts = infoCosts.Where(c => c.IsIncoming == isIncoming.Value);
                }

                if (costItem != null)
                {
                    infoCosts = infoCosts.Where(c => c.DirectoryCostItem.Name == costItem);
                }

                if (infoCosts.Any())
                {
                    double totalSummCurrency = infoCosts.Sum(c => c.Summ);
                    totalSumms[i] = totalSummCurrency != 0 ? Converting.DoubleToCurrency(totalSummCurrency, currency) : null;
                }
            }

            return totalSumms;
        }

        #endregion


        #region InfoLoan

        public InfoLoan AddInfoLoan(DateTime date, string loanTakerName, DirectoryWorker directoryWorker, double summ, Currency currency, int countPayments, string description)
        {
            DirectoryLoanTaker loanTaker = null;

            if (loanTakerName != null)
            {
                loanTaker = _dc.DirectoryLoanTakers.FirstOrDefault(l => l.Name == loanTakerName);

                if (loanTaker == null)
                {
                    loanTaker = AddDirectoryLoanTaker(loanTakerName);
                };

                AddInfoSafe(date, false, summ, currency, CashType.Наличка, "Выдача долга: " + loanTaker.Name);
            }
            else
            {
                AddInfoSafe(date, false, summ, currency, CashType.Наличка, "Выдача долга: " + directoryWorker.FullName);
            }


            var infoLoan = new InfoLoan
            {
                DateLoan = date,
                DirectoryLoanTaker = loanTaker,
                DirectoryWorker = directoryWorker,
                Summ = summ,
                Currency = currency,
                CountPayments = countPayments,
                Description = description,
            };

            _dc.InfoLoans.Add(infoLoan);
            _dc.SaveChanges();

            EditCurrencyValueSummChange("TotalLoan", currency, summ);

            var dateLoanPayment = new DateTime(date.Year, date.Month, 5);

            if (loanTakerName == null)
            {
                double onePaySumm = Math.Round(summ / countPayments, 0);

                for (int i = 0; i < countPayments; i++)
                {
                    dateLoanPayment = dateLoanPayment.AddMonths(1);

                    var infoPayment = new InfoPayment
                    {
                        Date = dateLoanPayment,
                        Summ = onePaySumm,
                        InfoLoanId = infoLoan.Id
                    };

                    _dc.InfoPayments.Add(infoPayment);
                }

                EditInfoMonthPayment(directoryWorker.Id, date, "PrepaymentCash", onePaySumm);

                infoLoan.DateLoanPayment = dateLoanPayment;
            }

            _dc.SaveChanges();

            return infoLoan;
        }

        public InfoLoan EditInfoLoan(int id, DateTime date, string loanTakerName, DirectoryWorker directoryWorker, double summ, Currency currency, int countPayments, string description)
        {
            DirectoryLoanTaker loanTaker = null;

            if (loanTakerName != null)
            {
                loanTaker = _dc.DirectoryLoanTakers.FirstOrDefault(l => l.Name == loanTakerName);

                if (loanTaker == null)
                {
                    loanTaker = AddDirectoryLoanTaker(loanTakerName);
                };
            }

            var infoLoan = _dc.InfoLoans.Find(id);

            EditCurrencyValueSummChange("TotalLoan", currency, summ - infoLoan.Summ);

            infoLoan.DateLoan = date;
            infoLoan.DirectoryLoanTaker = loanTaker;
            infoLoan.DirectoryWorker = directoryWorker;
            infoLoan.Summ = summ;
            infoLoan.Currency = currency;
            infoLoan.CountPayments = countPayments;
            infoLoan.Description = description;

            _dc.SaveChanges();

            return infoLoan;
        }

        public void RemoveInfoLoan(InfoLoan selectedInfoLoan)
        {
            _dc.InfoLoans.Remove(selectedInfoLoan);
            _dc.SaveChanges();
        }

        public IQueryable<InfoLoan> GetInfoLoans(DateTime from, DateTime to)
        {
            return _dc.InfoLoans.Where(s => DbFunctions.DiffDays(from, s.DateLoan) >= 0 && DbFunctions.DiffDays(to, s.DateLoan) <= 0 &&
                (s.DateLoanPayment == null || s.DateLoanPayment != null && DbFunctions.DiffDays(DateTime.Now, s.DateLoanPayment) >= 0)).
                OrderByDescending(s => s.DateLoan);
        }

        public double GetLoans()
        {
            return _dc.InfoLoans.Where(s => s.DateLoanPayment == null).ToList().Sum(s => s.RemainingSumm);
        }

        #endregion


        #region InfoPrivateLoan

        public InfoPrivateLoan AddInfoPrivateLoan(DateTime date, string loanTakerName, DirectoryWorker directoryWorker, double summ, Currency currency, int countPayments, string description)
        {
            DirectoryLoanTaker loanTaker = null;

            if (loanTakerName != null)
            {
                loanTaker = _dc.DirectoryLoanTakers.FirstOrDefault(l => l.Name == loanTakerName);

                if (loanTaker == null)
                {
                    loanTaker = AddDirectoryLoanTaker(loanTakerName);
                };
            }

            var infoPrivateLoan = new InfoPrivateLoan
            {
                DateLoan = date,
                DirectoryLoanTaker = loanTaker,
                DirectoryWorker = directoryWorker,
                Summ = summ,
                Currency = currency,
                CountPayments = countPayments,
                Description = description,
            };

            _dc.InfoPrivateLoans.Add(infoPrivateLoan);
            _dc.SaveChanges();

            EditCurrencyValueSummChange("TotalPrivateLoan", currency, summ);

            var datePrivateLoanPayment = new DateTime(date.Year, date.Month, 5);

            if (loanTakerName == null)
            {
                double onePaySumm = Math.Round(summ / countPayments, 0);

                for (int i = 0; i < countPayments; i++)
                {
                    datePrivateLoanPayment = datePrivateLoanPayment.AddMonths(1);

                    var infoPrivatePayment = new InfoPrivatePayment
                    {
                        Date = datePrivateLoanPayment,
                        Summ = onePaySumm,
                        InfoPrivateLoanId = infoPrivateLoan.Id
                    };

                    _dc.InfoPrivatePayments.Add(infoPrivatePayment);
                }

                if (countPayments > 1)
                {
                    infoPrivateLoan.DateLoanPayment = datePrivateLoanPayment;
                }
            }

            _dc.SaveChanges();

            return infoPrivateLoan;
        }

        public InfoPrivateLoan EditInfoPrivateLoan(int id, DateTime date, string loanTakerName, DirectoryWorker directoryWorker, double summ, Currency currency, int countPayments, string description)
        {
            DirectoryLoanTaker loanTaker = null;

            if (loanTakerName != null)
            {
                loanTaker = _dc.DirectoryLoanTakers.FirstOrDefault(l => l.Name == loanTakerName);

                if (loanTaker == null)
                {
                    loanTaker = AddDirectoryLoanTaker(loanTakerName);
                };
            }

            var infoPrivateLoan = _dc.InfoPrivateLoans.Find(id);

            EditCurrencyValueSummChange("TotalPrivateLoan", currency, summ - infoPrivateLoan.Summ);

            infoPrivateLoan.DateLoan = date;
            infoPrivateLoan.DirectoryLoanTaker = loanTaker;
            infoPrivateLoan.DirectoryWorker = directoryWorker;
            infoPrivateLoan.Summ = summ;
            infoPrivateLoan.Currency = currency;
            infoPrivateLoan.CountPayments = countPayments;
            infoPrivateLoan.Description = description;

            _dc.SaveChanges();

            return infoPrivateLoan;
        }

        public void RemoveInfoPrivateLoan(InfoPrivateLoan selectedInfoPrivateLoan)
        {
            _dc.InfoPrivateLoans.Remove(selectedInfoPrivateLoan);
            _dc.SaveChanges();
        }

        public IQueryable<InfoPrivateLoan> GetInfoPrivateLoans(DateTime from, DateTime to)
        {
            return _dc.InfoPrivateLoans.Where(s => DbFunctions.DiffDays(from, s.DateLoan) >= 0 && DbFunctions.DiffDays(to, s.DateLoan) <= 0 &&
                (s.DateLoanPayment == null || s.DateLoanPayment != null && DbFunctions.DiffDays(DateTime.Now, s.DateLoanPayment) >= 0)).
                OrderByDescending(s => s.DateLoan);
        }

        public double GetPrivateLoans()
        {
            return _dc.InfoPrivateLoans.Where(s => s.DateLoanPayment == null).ToList().Sum(s => s.RemainingSumm);
        }

        #endregion


        #region DirectoryCostItem

        public IQueryable<DirectoryCostItem> GetDirectoryCostItems()
        {
            return _dc.DirectoryCostItems;
        }

        public DirectoryCostItem GetDirectoryCostItem(string costItemName)
        {
            return GetDirectoryCostItems().First(c => c.Name == costItemName);
        }

        #endregion


        #region DirectoryNote

        public IQueryable<DirectoryNote> GetDirectoryNotes()
        {
            return _dc.DirectoryNotes;
        }

        public DirectoryNote GetDirectoryNote(string description)
        {
            return _dc.DirectoryNotes.First(n => n.Description == description);
        }

        public bool IsDirectoryNote(string note)
        {
            return _dc.DirectoryNotes.Select(n => n.Description).Contains(note);
        }

        public DirectoryNote AddDirectoryNote(string note)
        {
            var directoryNote = _dc.DirectoryNotes.FirstOrDefault(n => n.Description == note);

            if (directoryNote != null)
            {
                return directoryNote;
            }

            directoryNote = new DirectoryNote { Description = note };

            _dc.DirectoryNotes.Add(directoryNote);

            _dc.SaveChanges();

            return directoryNote;

        }
        #endregion


        #region DefaultCosts

        public IQueryable<DefaultCost> GetDefaultCosts()
        {
            return _dc.DefaultCosts;
        }

        public DefaultCost AddDefaultCost(DirectoryCostItem costItem, DirectoryRC rc, DirectoryNote note, double summ, int day)
        {
            var defaultCost = new DefaultCost
            {
                DirectoryCostItem = costItem,
                DirectoryRC = rc,
                DirectoryNote = note,
                SummOfPayment = summ,
                DayOfPayment = day
            };
            _dc.DefaultCosts.Add(defaultCost);
            _dc.SaveChanges();

            var infoCost = new InfoCost
            {
                Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day),
                DirectoryCostItemId = defaultCost.DirectoryCostItemId,
                DirectoryRCId = defaultCost.DirectoryRCId,
                CurrentNotes = new List<CurrentNote> { new CurrentNote { DirectoryNoteId = defaultCost.DirectoryNoteId } },
                Summ = defaultCost.SummOfPayment,
                IsIncoming = false,
                Weight = 0,
            };

            _dc.InfoCosts.Add(infoCost);
            _dc.SaveChanges();

            return defaultCost;
        }

        public void EditDefaultCost(int id, DirectoryCostItem costItem, DirectoryRC rc, DirectoryNote note, double summ, int day)
        {
            var defaultCost = _dc.DefaultCosts.Find(id);

            defaultCost.DirectoryCostItem = costItem;
            defaultCost.DirectoryRC = rc;
            defaultCost.DirectoryNote = note;
            defaultCost.SummOfPayment = summ;
            defaultCost.DayOfPayment = day;

            _dc.SaveChanges();
        }

        public void RemoveDefaultCost(DefaultCost defaultCost)
        {
            _dc.DefaultCosts.Remove(defaultCost);
            _dc.SaveChanges();
        }

        public void InitializeDefaultCosts()
        {
            var defaultCostsDate = GetParameterValue<DateTime>("DefaultCostsDate");
            var currentDate = DateTime.Now;

            if (defaultCostsDate.Year < currentDate.Year || (defaultCostsDate.Year == currentDate.Year && defaultCostsDate.Month < currentDate.Month))
            {
                var defaultCosts = GetDefaultCosts().ToList();

                foreach (var defaultCost in defaultCosts)
                {
                    var infoCostDate = new DateTime(currentDate.Year, currentDate.Month, defaultCost.DayOfPayment);
                    var infoCosts = GetInfoCosts(infoCostDate).ToList();
                    if (!infoCosts.Any(c => c.DirectoryCostItem.Id == defaultCost.DirectoryCostItemId && c.DirectoryRCId == defaultCost.DirectoryRCId &&
                        c.CurrentNotes.First().DirectoryNoteId == defaultCost.DirectoryNoteId && c.Summ == defaultCost.SummOfPayment))
                    {
                        var infoCost = new InfoCost
                        {
                            Date = infoCostDate,
                            DirectoryCostItemId = defaultCost.DirectoryCostItemId,
                            DirectoryRCId = defaultCost.DirectoryRCId,
                            CurrentNotes = new List<CurrentNote> { new CurrentNote { DirectoryNoteId = defaultCost.DirectoryNoteId } },
                            Summ = defaultCost.SummOfPayment,
                            IsIncoming = false,
                            Weight = 0,
                        };

                        _dc.InfoCosts.Add(infoCost);
                    }
                }

                _dc.SaveChanges();
                EditParameter("DefaultCostsDate", DateTime.Now.ToString());
            }
        }

        #endregion


        #region DirectoryTransportCompanies

        public IQueryable<DirectoryTransportCompany> GetDirectoryTransportCompanies()
        {
            return _dc.DirectoryTransportCompanies;
        }


        #endregion


        #region CurrentRC

        public IEnumerable<CurrentRC> GetCurrentRCs(IEnumerable<int> ids)
        {
            return _dc.CurrentRCs.Include(r => r.DirectoryRC).Where(r => ids.Contains(r.InfoOverTimeId));
        }

        #endregion


        #region Log

        public void Log(LoggingOptions loggingOptions, string message, params string[] parameters)
        {
            string description = message;

            if (parameters.Any())
            {
                description += ": ";
            }

            for (int i = 0; i < parameters.Count(); i++)
            {
                if (i != parameters.Count() - 1)
                {
                    description += parameters[i] + "; ";
                }
                else
                {
                    description += parameters[i];
                }
            }

            var log = new Log
            {
                Date = DateTime.Now,
                Level = loggingOptions.ToString(),
                Logger = DirectoryUser.CurrentUserName,
                Description = description
            };

            _dc.Logs.Add(log);
            _dc.SaveChanges();
        }

        public IQueryable<Log> GetLogs(DateTime date)
        {
            return _dc.Logs.Where(l => DbFunctions.DiffDays(l.Date, date) == 0);
        }


        #endregion


        #region DirectoryLoanTaker

        public IQueryable<DirectoryLoanTaker> GetDirectoryLoanTakers()
        {
            return _dc.DirectoryLoanTakers;
        }

        public DirectoryLoanTaker AddDirectoryLoanTaker(string name)
        {
            var loanTaker = new DirectoryLoanTaker
            {
                Name = name
            };

            _dc.DirectoryLoanTakers.Add(loanTaker);
            _dc.SaveChanges();

            return loanTaker;
        }

        #endregion


        #region InfoPayments

        public IQueryable<InfoPayment> GetInfoPayments(int infoLoanId)
        {
            return _dc.InfoPayments.Where(p => p.InfoLoanId == infoLoanId).OrderBy(p => p.Date);
        }

        public InfoPayment AddInfoPayment(int infoLoanId, DateTime date, double summ)
        {
            var infoPayment = new InfoPayment
            {
                Date = date,
                Summ = summ,
                InfoLoanId = infoLoanId
            };

            _dc.InfoPayments.Add(infoPayment);
            _dc.SaveChanges();

            var infoLoan = _dc.InfoLoans.Find(infoLoanId);
            EditCurrencyValueSummChange("TotalLoan", infoLoan.Currency, -summ);
            AddInfoSafe(date, true, summ, infoLoan.Currency, CashType.Наличка, "Возврат долга: " +
                (infoLoan.DirectoryLoanTakerId == null ? infoLoan.DirectoryWorker.FullName : infoLoan.DirectoryLoanTaker.Name));

            return infoPayment;
        }

        public void RemoveInfoPayment(InfoPayment selectedInfoPayment)
        {
            var currency = _dc.InfoLoans.Find(selectedInfoPayment.InfoLoanId).Currency;
            EditCurrencyValueSummChange("TotalLoan", currency, selectedInfoPayment.Summ);

            _dc.InfoPayments.Remove(selectedInfoPayment);
            _dc.SaveChanges();
        }

        #endregion


        #region Random
        private const string _serversPath = "Settings\\Servers.txt";

        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static int GetRandomNumber(int min, int max)
        {
            lock (syncLock)
            {
                return getrandom.Next(min, max);
            }
        }
        #endregion


        #region InfoSafe

        public InfoSafe AddInfoSafe(DateTime date, bool isIncoming, double summCash, Currency currency, CashType cashType, string description)
        {
            var infoSafe = new InfoSafe
            {
                Date = date,
                IsIncoming = isIncoming,
                Summ = summCash,
                Currency = currency,
                CashType = cashType,
                Description = description
            };

            _dc.InfoSafes.Add(infoSafe);
            _dc.SaveChanges();

            if (cashType == CashType.Наличка)
            {
                CalcTotalSumm("TotalCash", isIncoming, summCash, currency);
            }

            return infoSafe;
        }

        public InfoSafe AddInfoSafeHand(DateTime date, bool isIncoming, double summCash, Currency currency, CashType cashType, string description)
        {
            var infoSafe = new InfoSafe
            {
                Date = date,
                IsIncoming = isIncoming,
                Summ = summCash,
                Currency = currency,
                CashType = cashType,
                Description = description
            };

            _dc.InfoSafes.Add(infoSafe);
            _dc.SaveChanges();

            if (cashType == CashType.Наличка)
            {
                CalcTotalSumm("TotalCash", isIncoming, summCash, currency);
                CalcTotalSumm("TotalSafe", !isIncoming, summCash, currency);
            }

            return infoSafe;
        }

        private void CalcTotalSumm(string totalSummName, bool isIncoming, double summCash, Currency currency)
        {
            double summ = GetCurrencyValueSumm(totalSummName, currency);

            summ += isIncoming ? summCash : -summCash;

            EditCurrencyValueSumm(totalSummName, currency, summ);
        }

        public InfoSafe AddInfoSafeCard(DateTime date, double availableSumm, Currency currency, string description)
        {
            double prevAvailableSumm = GetCurrencyValue("TotalCard").RUR;

            double summ = Math.Round(availableSumm - prevAvailableSumm, 2);
            bool isIncoming = summ >= 0 ? true : false;

            if (!_dc.InfoSafes.Any(s => DbFunctions.DiffSeconds(s.Date, date) == 0 && s.CashType == CashType.Карточка && s.Description == description))
            {
                EditCurrencyValueSumm("TotalCard", Currency.RUR, availableSumm);

                return AddInfoSafe(date, isIncoming, Math.Abs(summ), currency, CashType.Карточка, description);
            }

            return null;
        }

        public IQueryable<InfoSafe> GetInfoSafes()
        {
            return _dc.InfoSafes.OrderByDescending(s => s.Date);
        }

        public IQueryable<InfoSafe> GetInfoSafes(CashType cashType, DateTime from, DateTime to)
        {
            return _dc.InfoSafes.Where(s => s.CashType == cashType && DbFunctions.DiffDays(from, s.Date) >= 0 && DbFunctions.DiffDays(to, s.Date) <= 0).OrderByDescending(s => s.Date);
        }

        public void RemoveInfoSafe(InfoSafe infoSafe)
        {
            if (infoSafe.CashType == CashType.Наличка)
            {
                CalcTotalSumm("TotalCash", !infoSafe.IsIncoming, infoSafe.Summ, infoSafe.Currency);
                CalcTotalSumm("TotalSafe", infoSafe.IsIncoming, infoSafe.Summ, infoSafe.Currency);
            }

            _dc.InfoSafes.Remove(infoSafe);
            _dc.SaveChanges();
        }

        #endregion


        #region InfoPrivatePayments

        public IQueryable<InfoPrivatePayment> GetInfoPrivatePayments(int infoSafeId)
        {
            return _dc.InfoPrivatePayments.Where(p => p.InfoPrivateLoanId == infoSafeId).OrderBy(p => p.Date);
        }

        public InfoPrivatePayment AddInfoPrivatePayment(int infoPrivateLoanId, DateTime date, double summ)
        {
            var infoPrivatePayment = new InfoPrivatePayment
            {
                Date = date,
                Summ = summ,
                InfoPrivateLoanId = infoPrivateLoanId
            };

            _dc.InfoPrivatePayments.Add(infoPrivatePayment);
            _dc.SaveChanges();

            var currency = _dc.InfoPrivateLoans.Find(infoPrivateLoanId).Currency;
            EditCurrencyValueSummChange("TotalPrivateLoan", currency, -summ);

            return infoPrivatePayment;
        }

        public void RemoveInfoPrivatePayment(InfoPrivatePayment selectedInfoPrivatePayment)
        {
            var currency = _dc.InfoPrivateLoans.Find(selectedInfoPrivatePayment.InfoPrivateLoanId).Currency;
            EditCurrencyValueSummChange("TotalPrivateLoan", currency, selectedInfoPrivatePayment.Summ);

            _dc.InfoPrivatePayments.Remove(selectedInfoPrivatePayment);
            _dc.SaveChanges();
        }

        #endregion


        #region CurrencyValue

        public CurrencyValue GetCurrencyValue(string name)
        {
            var currencyValue = _dc.CurrencyValues.First(c => c.Name == name);
            _dc.Entry<CurrencyValue>(currencyValue).Reload();

            return currencyValue;
        }

        public double GetCurrencyValueSumm(string name, Currency currency)
        {
            var currencyValue = GetCurrencyValue(name);

            double summ = 0;
            switch (currency)
            {
                case Currency.RUR:
                    summ = currencyValue.RUR;
                    break;
                case Currency.USD:
                    summ = currencyValue.USD;
                    break;
                case Currency.EUR:
                    summ = currencyValue.EUR;
                    break;
                case Currency.BYR:
                    summ = currencyValue.BYR;
                    break;
                default:
                    break;
            }

            return summ;
        }

        public void EditCurrencyValueSumm(string name, Currency currency, double summ)
        {
            var currencyValue = GetCurrencyValue(name);

            switch (currency)
            {
                case Currency.RUR:
                    currencyValue.RUR = summ;
                    break;
                case Currency.USD:
                    currencyValue.USD = summ;
                    break;
                case Currency.EUR:
                    currencyValue.EUR = summ;
                    break;
                case Currency.BYR:
                    currencyValue.BYR = summ;
                    break;
                default:
                    break;
            }

            _dc.SaveChanges();
        }

        public void EditCurrencyValueSummChange(string name, Currency currency, double summ)
        {
            var currencyValue = GetCurrencyValue(name);

            switch (currency)
            {
                case Currency.RUR:
                    currencyValue.RUR += summ;
                    break;
                case Currency.USD:
                    currencyValue.USD += summ;
                    break;
                case Currency.EUR:
                    currencyValue.EUR += summ;
                    break;
                case Currency.BYR:
                    currencyValue.BYR += summ;
                    break;
                default:
                    break;
            }

            _dc.SaveChanges();
        }

        #endregion


        #region DirectoryPostSalary

        public IQueryable<DirectoryPostSalary> GetDirectoryPostSalaries(int postId)
        {
            return _dc.DirectoryPostSalaries.Where(s => s.DirectoryPostId == postId);
        }

        public DirectoryPostSalary GetDirectoryPostSalaryByDate(int postId, DateTime date)
        {
            return _dc.DirectoryPostSalaries.Where(s => s.DirectoryPostId == postId).OrderByDescending(s => s.Date).First(s => DbFunctions.DiffDays(date, s.Date) <= 0);
        }

        //date = 20/1/14

        //10/1/14 15000
        //1/1/13  10000
        
        
        #endregion
    }
}
