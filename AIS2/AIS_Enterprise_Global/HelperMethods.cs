using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Directories;

namespace AIS_Enterprise_Global.Helpers
{
    public static class HelperMethods
    {
        private const string _serversPath = "Settings\\Servers.txt";

        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static int GetRandomNumber(int min, int max)
        {
            lock (syncLock)
            { 
                return getrandom.Next(min, max);
            }
        }

        public static bool IsFisrtTimeMoreSecondTime(DateTime firstTime, DateTime secondTime)
        {
            return firstTime.Hour > secondTime.Hour || (firstTime.Hour == secondTime.Hour && firstTime.Minute > secondTime.Minute || 
                (firstTime.Minute == secondTime.Minute && firstTime.Second > secondTime.Second));
        }

        public static double Round(double value)
        {
            double rest = value % 10;
            if (rest > 0 && rest < 1)
            {
                return Math.Floor(value);
            }
            else if (rest > 9)
            {
                return Math.Ceiling(value);
            }
            else
            {
                return Math.Round(value);
            }
        }

        public static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            return null;
        }

        public static DateTime GetLastDateInMonth(int year, int month)
        {
            return DateTime.Now.Year == year && DateTime.Now.Month == month ? DateTime.Now :
                new DateTime(year, month, DateTime.DaysInMonth(year, month));
        }

        public static void ShowView(ViewModelBase viewModel, Window window)
        {
            window.DataContext = viewModel;
            window.ShowDialog();
        }

        public static bool IsPrivilege(BusinessContext bc, UserPrivileges userPrivilege)
        {
            return DirectoryUser.Privileges.Contains(userPrivilege.ToString());
        }

        public static bool IsPrivilege(List<string> privileges, UserPrivileges userPrivilege)
        {
            return privileges.Contains(userPrivilege.ToString());
        }

        public static List<string> GetServers()
        {
            if (File.Exists(_serversPath))
            {
                return File.ReadAllLines(_serversPath).ToList();
            }
            else
            {
                return new List<string>();
            }
        }

        public static void AddServer(string serverName)
        {
            if (!File.Exists(_serversPath))
            {
                Directory.CreateDirectory("Settings");
                var fileStream = File.Create(_serversPath);
                fileStream.Close();
            }

            using (var sr = new StreamReader(_serversPath))
            {

            }

            var servers = File.ReadAllLines(_serversPath).ToList();
            
            if (!servers.Contains(serverName))
            {
                File.AppendAllLines(_serversPath, new List<string> { serverName });
            }
        }

        public static void CloseWindow(object parameter)
        {
            var window = (Window)parameter;


            if (window != null)
            {
                window.Close();
            }
        }

        public static bool IsNewVersion(BusinessContext bc, ref Version lastVersion)
        {
            var ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://logistikon.ru/domains/logistikon.ru/AIS_Enterprise_AV/Application%20Files/");

            ftpRequest.Credentials = new NetworkCredential("breshch", "Mp7200aA");
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
            var response = (FtpWebResponse)ftpRequest.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var directories = new List<string>();

                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    directories.Add(line);
                    line = streamReader.ReadLine();
                }

                var ftpVersion = Version.Parse(directories.OrderByDescending(d => d).First().Substring(18).Replace("_", "."));

                bool isNewVersion = ftpVersion > lastVersion;
                if (ftpVersion > lastVersion)
                {
                    lastVersion = ftpVersion;
                }

                return isNewVersion;
            }
        }
    }
}
