using System.Collections.Generic;
using System.Data.SqlClient;
using AIS_Enterprise_Data.Properties;

namespace AIS_Enterprise_Data
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
            string nameButler = Settings.Default.NameButler;
            string passwordButler = Settings.Default.PasswordButler;

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

        public static IEnumerable<string> GetDataBases(string serverName)
        {
            var conn = new SqlConnection(string.Format("Data Source={0}; Initial Catalog={1}; User ID={2}; Password={3};", serverName, "master", "huy", "huy"));

            conn.Open();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand(
                 @"SELECT [name] 
                   FROM sys.databases
                   WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');
                   ", conn);

            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                yield return myReader.GetString(0);
            }


            //            string queryCreateUser = string.Format(
            //                   @"SELECT [name] 
            //                     FROM sys.databases
            //                     WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');
            //                    ");

            //            var dbRowSqlQuery = bc.DataContext.Database.SqlQuery(typeof(string), queryCreateUser);
            //            foreach (var item in dbRowSqlQuery)
            //            {
            //                yield return item.ToString();
            //            }
        }
    }
}
