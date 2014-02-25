using AIS_Enterprise.Models.Directories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("Default")
        {

        }

        public DbSet<DirectoryTypeOfCompany> DirectoryTypeOfCompanies { get; set; }
        public DbSet<DirectoryCompany> DirectoryCompanies { get; set; }
        public DbSet<DirectoryTypeOfPost> DirectoryTypeOfPosts { get; set; }
    }
}
