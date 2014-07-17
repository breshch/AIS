namespace AIS_Enterprise_Global.Migrations
{
    using AIS_Enterprise_Global.Models;
    using AIS_Enterprise_Global.Models.Directories;
    using AIS_Enterprise_Global.Models.Helpers;
    using AIS_Enterprise_Global.Models.Infos;
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
            
        }
    }
}
