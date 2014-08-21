namespace AIS_Enterprise_Global.Migrations
{
    using AIS_Enterprise_Data;
    using AIS_Enterprise_Data.Directories;
    using AIS_Enterprise_Data.Helpers;
    using AIS_Enterprise_Data.Infos;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DataContext _dc)
        {
            //int id = _dc.DirectoryUserStatusPrivileges.First(p => p.Name == "MenuVisibility_Reports_ReportSalary").Id;
            //_dc.CurrentUserStatusPrivileges.RemoveRange(_dc.CurrentUserStatusPrivileges.Where(p => p.DirecoryUserStatusPrivilegeId == id));
            //_dc.DirectoryUserStatusPrivileges.Remove(_dc.DirectoryUserStatusPrivileges.First(p => p.Name == "MenuVisibility_Reports_ReportSalary"));


            //_dc.DirectoryUserStatusPrivileges.Add(new DirectoryUserStatusPrivilege { Name = "MenuVisibility_Reports_ReportCash" });
            //_dc.DirectoryUserStatusPrivileges.Add(new DirectoryUserStatusPrivilege { Name = "MenuVisibility_Reports_ReportSalaryMinsk" });
            

            //_dc.SaveChanges();
        }
    }
}
