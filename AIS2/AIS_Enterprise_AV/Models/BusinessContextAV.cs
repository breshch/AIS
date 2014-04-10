using AIS_Enterprise_AV.Models.Directories;
using AIS_Enterprise_AV.Models.Infos;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Temps;
using AIS_Enterprise_Global.Models;
using AIS_Enterprise_Global.Models.Currents;
using AIS_Enterprise_Global.Models.Directories;
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

        #endregion


        #region InitializeDefaultDataBase

        public void InitializeDefaultDataBase()
        {
            if (_dc.Database.Exists())
            {
                _dc.Database.Delete();

                _dc.Dispose();
                _dc = new DataContextAV();
            }

            _dc.Database.Create();
            InputDateToDataBase(2014);

            var company = new DirectoryCompany { Name = "АВ" };

            _dc.DirectoryCompanies.Add(company);
            _dc.SaveChanges();

            var rC = new DirectoryRC { Name = "КО-5" };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "МО-5" };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "ПАМ-16" };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "МО-2" };
            _dc.DirectoryRCs.Add(rC);
            rC = new DirectoryRC { Name = "ПАМ-1" };

            _dc.DirectoryRCs.Add(rC);
            _dc.SaveChanges();

            var typeOfPost = new DirectoryTypeOfPost { Name = "Склад" };

            _dc.DirectoryTypeOfPosts.Add(typeOfPost);
            _dc.SaveChanges();

            var post = new DirectoryPost
            {
                Name = "Грузчик",
                DirectoryTypeOfPost = typeOfPost,
                DirectoryCompany = company,
                Date = new DateTime(2014, 01, 01),
                UserWorkerSalary = 25000,
                UserWorkerHalfSalary = 10000
            };

            _dc.DirectoryPosts.Add(post);

            post = new DirectoryPost
            {
                Name = "Карщик",
                DirectoryTypeOfPost = typeOfPost,
                DirectoryCompany = company,
                Date = new DateTime(2014, 01, 01),
                UserWorkerSalary = 27000,
                UserWorkerHalfSalary = 10000
            };

            _dc.DirectoryPosts.Add(post);
            _dc.SaveChanges();


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
                            ChangeDate = DateTime.Now.AddDays(-40)
                        }
                    })
            };

            var holidays = new List<DateTime>();
            for (DateTime date = slave.StartDate; date <= DateTime.Now; date = date.AddMonths(1))
            {
                holidays.AddRange(GetWeekendsInMonth(date.Year, date.Month).ToList());
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
                SalaryAV = 2000,
                SalaryFenox = 1000,
                Panalty = 500,
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
                SalaryAV = 2000,
                SalaryFenox = 1000,
                Panalty = 500,
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
                            DirectoryPost = _dc.DirectoryPosts.First(),
                            ChangeDate = DateTime.Now.AddDays(-12),
                            FireDate = DateTime.Now.AddDays(-8)
                        },
                        new CurrentPost
                        {
                            DirectoryPost = _dc.DirectoryPosts.First(p => p.Name == "Карщик"),
                            ChangeDate = DateTime.Now.AddDays(-7),
                            FireDate = DateTime.Now.AddDays(-5)
                        },
                        new CurrentPost
                        {
                            DirectoryPost = _dc.DirectoryPosts.First(),
                            ChangeDate = DateTime.Now.AddDays(-4)
                        }
                    })
            };

            holidays = new List<DateTime>();
            for (DateTime date = slave.StartDate; date <= DateTime.Now; date = date.AddMonths(1))
            {
                holidays.AddRange(GetWeekendsInMonth(date.Year, date.Month).ToList());
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
                SalaryAV = 1000,
                SalaryFenox = 2000,
                Panalty = 1500,
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
                SalaryAV = 1000,
                SalaryFenox = 2000,
                Panalty = 1500,
                Inventory = 1500,
                BirthDays = 1500,
                Bonus = 2000
            };

            slave.InfoMonthes.Add(infoMonth);

            _dc.DirectoryWorkers.Add(slave);
            _dc.SaveChanges();
        }

        #endregion


        #region DirectoryRC

        public IQueryable<DirectoryRC> GetDirectoryRCs()
        {
            return _dc.DirectoryRCs;
        }

        public DirectoryRC AddDirectoryRC(string directoryRCName)
        {
            var directoryRC = new DirectoryRC
            {
                Name = directoryRCName,
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
                    overTime.DirectoryRCs.Add(_dc.DirectoryRCs.First(r => r.Name == directoryRC.Name));
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

                foreach (var directoryRC in directoryRCs)
                {
                    overTime.DirectoryRCs.Add(_dc.DirectoryRCs.First(r => r.Name == directoryRC.Name));
                }

                _dc.SaveChanges();
            }
        }

        public InfoOverTime GetInfoOverTime(DateTime date)
        {
            return _dc.InfoOverTimes.FirstOrDefault(o => DbFunctions.DiffDays(o.StartDate, date) == 0);
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
