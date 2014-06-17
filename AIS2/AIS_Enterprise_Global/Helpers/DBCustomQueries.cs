using AIS_Enterprise_Global.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Helpers
{
    public static class DBCustomQueries
    {
        public static void AddUser(DataContext dc, string name, string password)
        {
            string dataBaseName = dc.Database.Connection.Database;

            string queryCreateUser = string.Format(
                   @"CREATE LOGIN {0} 
                   WITH PASSWORD = '{1}';
                   USE {2};
                   CREATE USER {0} FOR LOGIN {0};
                   ", name, password, dataBaseName);

            dc.Database.ExecuteSqlCommand(queryCreateUser);

            string queryAddRole = string.Format("ALTER SERVER ROLE [sysadmin] ADD MEMBER [{0}];", name);
            dc.Database.ExecuteSqlCommand(queryAddRole);
        }

        public static void EditUser(DataContext dc, string prevName, string newName, string password)
        {
            string dataBaseName = dc.Database.Connection.Database;

             string queryEditUser = string.Format(
                   @"ALTER LOGIN {0} WITH NAME = {1};
                     ALTER LOGIN {1} WITH PASSWORD = '{2}';
                     USE {3};
                     ALTER USER {0} WITH NAME = {1};
                   ", prevName, newName, password, dataBaseName);

            dc.Database.ExecuteSqlCommand(queryEditUser);
        }

        public static void AddUserButler(DataContext dc)
        {
            string nameButler = Properties.Settings.Default.NameButler;
            string passwordButler = Properties.Settings.Default.PasswordButler;

            string dataBaseName = dc.Database.Connection.Database;

            string queryCreateUser = string.Format(
                    @"IF NOT EXISTS 
                    (SELECT name  
                    FROM master.sys.server_principals
                    WHERE name = '{0}')
                    BEGIN
                        CREATE LOGIN {0}
                        WITH PASSWORD = '{1}'
                    END;
                    USE {2};
                    CREATE USER {0} FOR LOGIN {0};
                    ", nameButler, passwordButler, dataBaseName);

            dc.Database.ExecuteSqlCommand(queryCreateUser);
        }

        public static IEnumerable<string> GetDataBases(BusinessContext bc)
        {
            string queryCreateUser = string.Format(
                   @"SELECT [name]
                     FROM master.dbo.sysdatabases
                     WHERE dbid > 4;
                    ");

            var dbRowSqlQuery = bc.DataContext.Database.SqlQuery(typeof(string), queryCreateUser);
            foreach (var item in dbRowSqlQuery)
            {
                yield return item.ToString();
            }
        }
    }
}
