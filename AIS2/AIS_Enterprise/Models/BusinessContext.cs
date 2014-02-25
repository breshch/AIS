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


        #region DirectoryTypeOfCompany

        public IQueryable<DirectoryTypeOfCompany> GetDirectoryTypeOfCompanies()
        {
            return _dc.DirectoryTypeOfCompanies;
        }

        public DirectoryTypeOfCompany AddDirectoryTypeOfCompany(string directoryTypeOfCompanyName)
        {
            var directoryTypeOfCompany = new DirectoryTypeOfCompany
            {
                Name = directoryTypeOfCompanyName
            };

            _dc.DirectoryTypeOfCompanies.Add(directoryTypeOfCompany);

            _dc.SaveChanges();

            return directoryTypeOfCompany;
        }

        public DirectoryTypeOfCompany RemoveDirectoryTypeOfCompany(DirectoryTypeOfCompany directoryTypeOfCompany)
        {
            directoryTypeOfCompany = _dc.DirectoryTypeOfCompanies.Remove(directoryTypeOfCompany);

            _dc.SaveChanges();

            return directoryTypeOfCompany;
        }

        #endregion


        #region DirectoryCompany

        public IQueryable<DirectoryCompany> GetDirectoryCompanies()
        {
            return _dc.DirectoryCompanies;
        }

        public DirectoryCompany AddDirectoryCompany(string directoryCompanyName, DirectoryTypeOfCompany directoryTypeOfCompany)
        {
            var directoryCompany = new DirectoryCompany
            {
                Name = directoryCompanyName,
                DirectoryTypeOfCompany = directoryTypeOfCompany
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


        #region IDisposable

        public void Dispose()
        {
            _dc.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
