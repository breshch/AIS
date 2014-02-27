using AIS_Enterprise.Models.Directories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise.Models
{
    public class BusinessContext : IDisposable
    {
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

        public DirectoryPost AddDirectoryPost(string name, DirectoryTypeOfPost typeOfPost, DirectoryCompany company, DateTime date, string userWorkerSalary, string userWorkerHalfSalary)
        {
            var directoryPost = new DirectoryPost
            {
                Name =  name,
                DirectoryTypeOfPost = typeOfPost,
                DirectoryCompany  = company,
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



        #region IDisposable

        public void Dispose()
        {
            _dc.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
