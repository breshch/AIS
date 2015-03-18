using System.ComponentModel;
using System.Windows;
using AIS_Enterprise_Global.ViewModels;

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

        void DirectoryEditWorkerView_Closing(object sender, CancelEventArgs e)
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
