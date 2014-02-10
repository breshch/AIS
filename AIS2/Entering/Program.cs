//using ModelDbAv;
//using ModelDbCars;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Entering
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Database.SetInitializer<AvContext>(new DropCreateDatabaseIfModelChanges<AvContext>());
            //Database.SetInitializer<CarsContext>(new DropCreateDatabaseIfModelChanges<CarsContext>());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormEnter());
        }
    }
}
