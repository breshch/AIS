﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using FTP;

namespace Updater
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FTPConnector _ftpConnector;
        private DateTime _dateBackup;
        private const string DefaultFTPFolder = @"ftp://95.31.130.52/";

        public MainWindow()
        {
            InitializeComponent();

            _ftpConnector = new FTPConnector("breshch", "Mp7200aA", DefaultFTPFolder);

            //if (IsNewVersion())
            {
                Backup();
                //_ftpConnector.DownloadDirectory(@"AIS_Enterprise_AV", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AIS_Enterprise_AV"));
            }

            //InitializeTimer();
        }



        private bool IsNewVersion()
        {
            var currentVersion = Version.Parse(File.ReadAllText("Version.txt"));
            var ftpVersion = Version.Parse(_ftpConnector.GetFile("Version.txt"));

            return ftpVersion > currentVersion;
        }

        private void Backup()
        {
            _dateBackup = DateTime.Now;

            ZipFile.CreateFromDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AIS_Enterprise_AV", "Images"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "AIS_Enterprise_AV", "Backup_" + _dateBackup.Ticks + ".zip"));
        }
        

        private void InitializeTimer()
        {
            var timer = new Timer();
            timer.Interval = 1000 * 60 * 10;
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            
        }
    }
}
