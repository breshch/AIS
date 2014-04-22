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

        public virtual void RefreshContext()
        {
            _dc.Dispose();
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

        public DirectoryPost GetDirectoryPost(string postName)
        {
            return _dc.DirectoryPosts.FirstOrDefault(p => p.Name == postName);
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

        public bool ExistsDirectoryPost(string name)
        {
            return _dc.DirectoryPosts.Any(p => p.Name == name);
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
            return _dc.DirectoryWorkers;
        }

        public IQueryable<DirectoryWorker> GetDirectoryWorkers(int year, int month)
        {
            var firstDateInMonth = new DateTime(year, month, 1);
            var lastDateInMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            
            return _dc.DirectoryWorkers.Where(w => DbFunctions.DiffDays(w.StartDate, lastDateInMonth) >= 0 && 
                (w.FireDate == null || (w.FireDate != null && DbFunctions.DiffDays(w.FireDate.Value, firstDateInMonth) <= 0)));
        }

        public DirectoryWorker GetDirectoryWorker(int workerId)
        {
            return _dc.DirectoryWorkers.Find(workerId);
        }

        public DirectoryWorker GetDirectoryWorker(string lastName, string firstName)
        {
            return _dc.DirectoryWorkers.FirstOrDefault(w => w.LastName == lastName && w.FirstName == firstName);
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

        public IEnumerable<InfoDate> GetInfoDatePanalties(int workerId, int year, int month)
        {
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

        public IEnumerable<InfoDate> GetInfoDates(int workerId, int year, int month)
        {
            var worker = GetDirectoryWorker(workerId);
            return worker.InfoDates.Where(d => d.Date.Year == year && d.Date.Month == month);
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

        #endregion


        #region InfoPanalty

        public InfoPanalty GetInfoPanalty(int workerId, DateTime date)
        {
            var worker = GetDirectoryWorker(workerId);
            return worker.InfoDates.First(d => d.Date.Date == date.Date).InfoPanalty;
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
                    p.DirectoryCompany.Name == currentCompanyAndPost.DirectoryPost.DirectoryCompany.Name)
            };
            worker.CurrentCompaniesAndPosts.Add(currentPost);

            _dc.SaveChanges();
        }

        public IEnumerable<CurrentPost> GetCurrentPosts(int workerId, int year, int month, int lastDayInMonth)
        {
            var lastDateInMonth = new DateTime(year, month, lastDayInMonth);
            var firstDateInMonth = new DateTime(year, month, 1);

            var worker = GetDirectoryWorker(workerId);

            return worker.CurrentCompaniesAndPosts.Where(p => p.ChangeDate.Date <= lastDateInMonth.Date && p.FireDate == null ||
                p.FireDate != null && p.FireDate.Value >= firstDateInMonth.Date && p.ChangeDate.Date <= lastDateInMonth.Date);
        }

        public CurrentPost GetCurrentPost(int workerId, DateTime date)
        {
            var worker = GetDirectoryWorker(workerId);

            return worker.CurrentCompaniesAndPosts.First(p => p.ChangeDate.Date <= date.Date && p.FireDate == null ||
                p.FireDate != null && p.FireDate.Value >= date.Date && p.ChangeDate.Date <= date.Date);
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


                            if (!dc.DirectoryWeekends.Select(h => h.Date).Contains(date))
                            {
                                var directoryHoliday = new DirectoryWeekend { Date = date };
                                dc.DirectoryWeekends.Add(directoryHoliday);
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
                int holiDays = dc.DirectoryWeekends.Where(h => h.Date.Year == year && h.Date.Month == month).Count();
                return DateTime.DaysInMonth(year, month) - holiDays;
            }
        }

        public IQueryable<DateTime> GetWeekendsInMonth(int year, int month)
        {
            return _dc.DirectoryWeekends.Where(h => h.Date.Year == year && h.Date.Month == month).Select(h => h.Date);
        }

        public bool IsWeekend(DateTime date)
        {
            return _dc.DirectoryWeekends.Any(w => DbFunctions.DiffDays(w.Date, date) == 0);
        }

        #endregion
    }
}
