using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using AIS_Enterprise_AV.Properties;
using AIS_Enterprise_AV.ViewModels;

namespace AIS_Enterprise_AV.Views
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Default.Language);

            InitializeComponent();

            var mainViewModel = new MainViewModel();
            this.DataContext = mainViewModel;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //PasswordBoxPass.Password = null;
        }
    }
}
