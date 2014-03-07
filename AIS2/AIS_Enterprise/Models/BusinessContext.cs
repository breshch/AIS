using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Temps;
using AIS_Enterprise_Global.Models.Currents;
using AIS_Enterprise_Global.Models.Directories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void Dispose()
        {
            _dc.Dispose();
            GC.SuppressFinalize(this);
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

        public DirectoryCompany RemoveDirectoryCompany(DirectoryCompany directoryCompany)
        {
            directoryCompany = _dc.DirectoryCompanies.Remove(directoryCompany);

            _dc.SaveChanges();

            return directoryCompany;
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

        public DirectoryTypeOfPost RemoveDirectoryTypeOfPost(DirectoryTypeOfPost directoryTypeOfPost)
        {
            directoryTypeOfPost = _dc.DirectoryTypeOfPosts.Remove(directoryTypeOfPost);

            _dc.SaveChanges();

            return directoryTypeOfPost;
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

        public DirectoryWorker AddDirectoryWorker(string lastName, string firstName, string midName, Gender gender, DateTime birthDay, string address, string homePhone, string cellPhone, DateTime startDate,
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
                CurrentCompaniesAndPosts = new List<CurrentPost>(currentCompaniesAndPosts.Select(c => new CurrentPost { ChangeDate = c.PostChangeDate, FireDate = null, DirectoryPostId = c.DirectoryPost.Id }))
            };

            _dc.DirectoryWorkers.Add(worker);
            _dc.SaveChanges();

            return worker;
        }

        public IQueryable<DirectoryWorker> GetDirectoryWorkers()
        {
            return _dc.DirectoryWorkers;
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
            directoryWorker.CurrentCompaniesAndPosts = new List<CurrentPost>(currentCompaniesAndPosts.Select(c => new CurrentPost { ChangeDate = c.PostChangeDate, FireDate = null, DirectoryPostId = c.DirectoryPost.Id }));

            _dc.SaveChanges();
           
            return directoryWorker;
        
        }

        #endregion
    }
}
