using ModelDB.Currents;
using ModelDB.Directories;
using ModelDB.Infos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB
{
    public class Context : DbContext
    {
        public DbSet<CurrentCompany> CurrentCompanies { get; set; }
        public DbSet<CurrentPost> CurrentPosts { get; set; }
        
        public DbSet<DirectoryCar> DirectoryCars  { get; set; }
        public DbSet<DirectoryCompany> DirectoryCompanies { get; set; }
        public DbSet<DirectoryHoliday> DirectoryHolidays { get; set; }
        public DbSet<DirectoryPoD> DirectoryPoDs { get; set; }
        public DbSet<DirectoryPost> DirectoryPosts { get; set; }
        public DbSet<DirectoryRC> DirectoryRCs { get; set; }
        public DbSet<DirectoryTypeOfCar> DirectoryTypeOfCars { get; set; }
        public DbSet<DirectoryTypeOfCompany> DirectoryTypeOfCompanies { get; set; }
        public DbSet<DirectoryTypeOfPost> DirectorTypeOfPosts { get; set; }
        public DbSet<DirectoryWorker> DirectoryWorkers { get; set; }

        public DbSet<InfoCar> InfoCars { get; set; }
        public DbSet<InfoCargo> InfoCargoes { get; set; }
        public DbSet<InfoCompany> InfoCompanies { get; set; }
        public DbSet<InfoDate> InfoDates { get; set; }
        public DbSet<InfoDriver> InfoDrivers { get; set; }
        public DbSet<InfoMonth> InfoMonthes { get; set; }
        public DbSet<InfoPanalty> InfoPanalties { get; set; }
        public DbSet<InfoPermitForCar> InfoPermitForCars { get; set; }
        public DbSet<InfoSalary> InfoSalaries { get; set; }
        public DbSet<InfoWorker> InfoWorkers { get; set; }

    }
}
