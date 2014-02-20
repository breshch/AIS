using ModelDB.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ModelDB
{
    public class QueryTemplates
    {
        private Context _db;

        public QueryTemplates(Context db)
        {
            _db = db;
        }

        #region Get

        #region DirectoryTypeOfCompany
        public IQueryable<string> GetDirectoryTypeOfCompanyNames()
        {
            return _db.DirectoryTypeOfCompanies.Select(t => t.Name);
        }
        
        public IQueryable<int> GetDirectoryTypeOfCompanyIds()
        {
            return _db.DirectoryCompanies.Select(c => c.DirectoryTypeOfCompanyId);
        }
        
        public DirectoryTypeOfCompany GetDirectoryTypeOfCompany(string typeOfCompany)
        {
            return _db.DirectoryTypeOfCompanies.First(t => t.Name == typeOfCompany);
        }
        #endregion DirectoryTypeOfCompany

        #region DirectoryCompanies
        public IQueryable<DirectoryCompany> GetDirectoryCompanies()
        {
            return _db.DirectoryCompanies;
        }

        public IQueryable<string> GetDirectoryCompanyNames()
        {
            return _db.DirectoryCompanies.Select(t => t.Name);
        }

        public DirectoryCompany GetDirectoryCompany(int id)
        {
            return _db.DirectoryCompanies.Find(id);
        }

        public DirectoryCompany GetDirectoryCompany(string name)
        {
            return _db.DirectoryCompanies.First(c => c.Name == name);
        }
        #endregion DirectoryCompanies

        #region DirectoryTypeOfPost
        public IQueryable<string> GetDirectoryTypeOfPostNames()
        {
            return _db.DirectoryTypeOfPosts.Select(t => t.Name);
        }

        public DirectoryTypeOfPost GetDirectoryTypeOfPost(string typeOfPost)
        {
            return _db.DirectoryTypeOfPosts.First(t => t.Name == typeOfPost);
        }

        public IQueryable<int> GetDirectoryPostIds()
        {
            return _db.DirectoryPosts.Select(c => c.DirectoryTypeOfPostId);
        }
        #endregion DirectoryTypeOfPost

        #region DirectoryPost
        public IQueryable<DirectoryPost> GetDirectoryPosts()
        {
            return _db.DirectoryPosts;
        }

        public DirectoryPost GetDirectoryPost(int id)
        {
            return _db.DirectoryPosts.Find(id);
        }

        public DirectoryPost GetDirectoryPost(Func<DirectoryPost, bool> func)
        {
            return _db.DirectoryPosts.FirstOrDefault(func);
        }
        #endregion DirectoryPost

        #region CurrentCompany
        public IQueryable<int> GetCurrentCompanyIds()
        {
            return _db.CurrentCompanies.Select(c => c.DirectoryCompanyId);
        }
        #endregion CurrentCompany

        #region CurrentPost
        public IQueryable<int> GetCurrentPostIds()
        {
            return _db.CurrentPosts.Select(p => p.DirectoryPostId);
        }
        #endregion CurrentPost

        #endregion Get


        #region Add

        public void AddDirectoryTypeOfCompany(string name)
        {
            _db.DirectoryTypeOfCompanies.Add(new DirectoryTypeOfCompany { Name = name });
        }

        public void AddDirectoryCompany(string name, DirectoryTypeOfCompany directoryTypeOfCompany)
        {
            _db.DirectoryCompanies.Add(new DirectoryCompany { Name = name, DirectoryTypeOfCompany = directoryTypeOfCompany });
        }

        public void AddDirectoryTypeOfPost(string name)
        {
            _db.DirectoryTypeOfPosts.Add(new DirectoryTypeOfPost { Name = name });
        }

        public void AddDirectoryPost(DirectoryPost post)
        {
            _db.DirectoryPosts.Add(post);
        }

        #endregion Add


        #region Remove

        public void RemoveDirectoryTypeOfCompany(DirectoryTypeOfCompany directoryTypeOfCompany)
        {
            _db.DirectoryTypeOfCompanies.Remove(directoryTypeOfCompany);
        }

        public void RemoveDirectoryCompany(DirectoryCompany directoryCompany)
        {
            _db.DirectoryCompanies.Remove(directoryCompany);
        }

        public void RemoveDirectoryTypeOfPost(DirectoryTypeOfPost directoryTypeOfPost)
        {
            _db.DirectoryTypeOfPosts.Remove(directoryTypeOfPost);
        }

        public void RemoveDirectoryPost(DirectoryPost directoryPost)
        {
            _db.DirectoryPosts.Remove(directoryPost);
        }

        #endregion Remove


        #region Others
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Close()
        {
            _db.Dispose();
        }
        #endregion Others
    }
}
