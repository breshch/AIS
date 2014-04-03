using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Temps;
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

namespace AIS_Enterprise_Global.Models
{
    public class BusinessContext : IDisposable
    {
        #region Base

        private DataContext _dc;

        public BusinessContext()
        {
            _dc = new DataContext();
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
                _dc.Dispose();
                _dc = new DataContext();
            }

            _dc.Database.Create();
        }

        public void Dispose()
        {
            _dc.Dispose();
            GC.SuppressFinalize(this);
        }

        public void RefreshContext()
        {
            _dc.Dispose();
            _dc = new DataContext();
        }

        #endregion


        #region DirectoryCompany

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

        public DirectoryPost AddDirectoryPost(string name, DirectoryTypeOfPost typeOfPost, DirectoryCompany company, DateTime date, string userWorkerSalary, string userWorkerHalfSalary)
        {
            var directoryPost = new DirectoryPost
            {
                Name = name,
                DirectoryTypeOfPost = typeOfPost,
                DirectoryCompany = company,
                Date = date,
                UserWorkerSalary = double.Parse(userWorkerSalary),
                UserWorkerHalfSalary = double.Parse(userWorkerHalfSalary)
            };

            _dc.DirectoryPosts.Add(directoryPost);
            _dc.SaveChanges();

            return directoryPost;
        }

        public DirectoryPost RemoveDirectoryPost(DirectoryPost post)
        {
            _dc.DirectoryPosts.Remove(post);
            _dc.SaveChanges();
            return post;
        }

        #endregion


        #region DirectoryWorker

        public DirectoryWorker AddDirectoryWorker(string lastName, string firstName, string midName, Gender gender,
            DateTime birthDay, string address, string homePhone, string cellPhone, DateTime startDate,
            DateTime? fireDate, ICollection<CurrentCompanyAndPost> currentCompaniesAndPosts)
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
                CurrentCompaniesAndPosts = new List<CurrentPost>(currentCompaniesAndPosts.Select(c => new CurrentPost { ChangeDate = c.PostChangeDate, FireDate = c.PostFireDate, DirectoryPostId = c.DirectoryPost.Id }))
            };

            _dc.DirectoryWorkers.Add(worker);
            _dc.SaveChanges();

            return worker;
        }

        public IQueryable<DirectoryWorker> GetDirectoryWorkers()
        {
            return _dc.DirectoryWorkers;
        }

        public IQueryable<DirectoryWorker> GetDirectoryWorkers(int year, int month)
        {
            var lastDateInMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            var firstDateInMonth = new DateTime(year, month, 1);

            return _dc.DirectoryWorkers.Where(w => DbFunctions.DiffDays(w.StartDate, lastDateInMonth) >= 0 &&
                w.FireDate == null || DbFunctions.DiffDays(w.FireDate.Value, firstDateInMonth) <= 0);
        }

        public DirectoryWorker GetDirectoryWorker(int workerId)
        {
            return _dc.DirectoryWorkers.Find(workerId);
        }

        public DirectoryWorker EditDirectoryWorker(int id, string lastName, string firstName, string midName, Gender gender, DateTime birthDay, string address, string homePhone, string cellPhone, DateTime startDate,
           DateTime? fireDate, ICollection<CurrentCompanyAndPost> currentCompaniesAndPosts)
        {
            var directoryWorker = GetDirectoryWorker(id);

            directoryWorker.LastName = lastName;
            directoryWorker.FirstName = firstName;
            directoryWorker.MidName = midName;
            directoryWorker.Gender = gender;
            directoryWorker.BirthDay = birthDay;
            directoryWorker.Address = address;
            directoryWorker.HomePhone = homePhone;
            directoryWorker.CellPhone = cellPhone;
            directoryWorker.StartDate = startDate;
            directoryWorker.FireDate = fireDate;

            _dc.CurrentPosts.RemoveRange(directoryWorker.CurrentCompaniesAndPosts);

            directoryWorker.CurrentCompaniesAndPosts = new List<CurrentPost>(currentCompaniesAndPosts.Select(c => new CurrentPost { ChangeDate = c.PostChangeDate, FireDate = c.PostFireDate, DirectoryPostId = c.DirectoryPost.Id }));

            _dc.SaveChanges();

            return directoryWorker;
        }



        #endregion


        #region InfoDates

        public void EditInfoDateHour(int workerId, DateTime date, string hour)
        {
            var worker = GetDirectoryWorker(workerId);
            var infoDate = worker.InfoDates.First(d => d.Date.Date == date.Date);

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

            _dc.SaveChanges();
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

        #endregion


        #region InfoOverTime

        public void AddInfoOverTime(DateTime date, string description)
        {
            var overTime = _dc.InfoOverTimes.FirstOrDefault(o => DbFunctions.DiffDays(o.Date, date) == 0);
            if (overTime == null)
            {
                overTime = new InfoOverTime
                {
                    Date = date,
                    Description = description
                };
                _dc.InfoOverTimes.Add(overTime);

                _dc.SaveChanges();
            }
        }

        public InfoOverTime GetInfoOverTime(DateTime date)
        {
            return _dc.InfoOverTimes.FirstOrDefault(o => DbFunctions.DiffDays(o.Date, date) == 0);
        }

        public IQueryable<DateTime> GetInfoOverTimeDates(int year, int month)
        {
            return _dc.InfoOverTimes.Where(o => o.Date.Year == year && o.Date.Month == month).Select(o => o.Date);
        }

        public bool IsInfoOverTimeDate(DateTime date)
        {
            return _dc.InfoOverTimes.Select(o => o.Date).Contains(date);
        }

        public void RemoveInfoOverTime(DateTime date)
        {
            var infoOverTime = _dc.InfoOverTimes.FirstOrDefault(o => DbFunctions.DiffDays(o.Date, date) == 0);
            if (infoOverTime != null)
            {
                _dc.InfoOverTimes.Remove(infoOverTime);
                _dc.SaveChanges();
            }
        }

        public void EditInfoOverTime(DateTime date, string description)
        {
            var overTime = _dc.InfoOverTimes.FirstOrDefault(o => DbFunctions.DiffDays(o.Date, date) == 0);
            if (overTime != null)
            {
                overTime.Description = description;
                _dc.SaveChanges();
            }
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
                    p.DirectoryCompany.Name == currentCompanyAndPost.DirectoryPost.DirectoryCompany.Name)
            };
            worker.CurrentCompaniesAndPosts.Add(currentPost);

            _dc.SaveChanges();
        }

        public IEnumerable<CurrentPost> GetCurrentPosts(int workerId, int year, int month)
        {
            var lastDateInMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            var firstDateInMonth = new DateTime(year, month, 1);

            var worker = _dc.DirectoryWorkers.Find(workerId);

            return worker.CurrentCompaniesAndPosts.Where(p => p.ChangeDate.Date <= lastDateInMonth.Date && p.FireDate == null || p.FireDate.Value >= firstDateInMonth);
        }

        #endregion


        #region Calendar

        public void InputDateToDataBase(int year)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Calendar", year.ToString() + ".txt");

            using (var dc = new DataContext())
            {
                using (var sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        if (line[11] == 'в')
                        {
                            var date = DateTime.Parse(line.Substring(0, 10));


                            if (!dc.DirectoryHolidays.Select(h => h.Date).Contains(date))
                            {
                                var directoryHoliday = new DirectoryHoliday { Date = date };
                                dc.DirectoryHolidays.Add(directoryHoliday);
                            }
                        }
                    }
                }
                dc.SaveChanges();
            }
        }

        public int GetCountWorkDaysInMonth(int year, int month)
        {
            using (var dc = new DataContext())
            {
                int holiDays = dc.DirectoryHolidays.Where(h => h.Date.Year == year && h.Date.Month == month).Count();
                return DateTime.DaysInMonth(year, month) - holiDays;
            }
        }

        public IQueryable<DateTime> GetWeekendsInMonth(int year, int month)
        {
            return _dc.DirectoryHolidays.Where(h => h.Date.Year == year && h.Date.Month == month).Select(h => h.Date);
        }

        #endregion


        #region InitializeDefaultDataBase

        public void InitializeDefaultDataBase()
        {
            if (_dc.Database.Exists())
            {
                _dc.Database.Delete();

                _dc.Dispose();
                _dc = new DataContext();
            }

            _dc.Database.Create();
            InputDateToDataBase(2014);

            var company = new DirectoryCompany { Name = "АВ" };

            _dc.DirectoryCompanies.Add(company);
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
    }
}
