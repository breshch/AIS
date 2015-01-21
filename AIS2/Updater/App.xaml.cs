using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Updater
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex _mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!IsSingleInstance())
                Environment.Exit(0);
        }

        private static bool IsSingleInstance()
        {
            _mutex = new Mutex(false, "AIS_Enterprise_AV_Updater");

            GC.KeepAlive(_mutex);

            try
            {
                return _mutex.WaitOne(0, false);
            }
            catch (AbandonedMutexException)
            {
                _mutex.ReleaseMutex();
                return _mutex.WaitOne(0, false);
            }
        }
    }
}
