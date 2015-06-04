using System.Data.Entity.Migrations;
using AIS_Enterprise_Data;

namespace AIS_Enterprise_Global.Migrations
{
	public sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
			//AutomaticMigrationsEnabled = true;
			//AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DataContext _dc)
        {
        }
    }
}
