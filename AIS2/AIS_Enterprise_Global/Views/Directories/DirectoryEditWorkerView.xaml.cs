﻿using AIS_Enterprise_Global.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AIS_Enterprise_Global.Views.Directories
{
    /// <summary>
    /// Логика взаимодействия для WorkerView.xaml
    /// </summary>
    public partial class DirectoryEditWorkerView : Window
    {
        public DirectoryEditWorkerView()
        {
            InitializeComponent();

            Closing += DirectoryEditWorkerView_Closing;
        }

        void DirectoryEditWorkerView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var directoryEditWorkerViewModel = (DirectoryEditWorkerViewModel)this.DataContext;

            if (!directoryEditWorkerViewModel.IsChangeWorker && MessageBox.Show("Если вы закроете форму, то информация о работнике, которую вы изменили, не будет сохранена. " + 
                "Вы действительно хотите закрыть форму?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
