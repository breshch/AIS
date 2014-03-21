using AIS_Enterprise_Global.Models.Currents;
using AIS_Enterprise_Global.Models.Directories;
using AIS_Enterprise_Global.Models.Infos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("Default")
        {

        }

        public DbSet<DirectoryCompany> DirectoryCompanies { get; set; }
        public DbSet<DirectoryTypeOfPost> DirectoryTypeOfPosts { get; set; }
        public DbSet<DirectoryPost> DirectoryPosts { get; set; }
        public DbSet<DirectoryWorker> DirectoryWorkers { get; set; }

        public DbSet<DirectoryHoliday> DirectoryHolidays { get; set; }

        public DbSet<CurrentPost> CurrentPosts { get; set; }

        public DbSet<InfoDate> InfoDates { get; set; }
        public DbSet<InfoMonth> InfoMonthes { get; set; }
    }
}
