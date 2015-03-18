using System.ServiceProcess;

namespace AIS_Enterprise_CardService
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new CardService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
