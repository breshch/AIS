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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AIS_Enterprise_Installer
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void ButtonInstall_OnClick(object sender, RoutedEventArgs e)
		{
			var installer = new Installer();

			//if (installer.SQLServerCheck())
			//{
			//	installer.InstallSQLServerSilent();
			//}
			
			installer.InstallApplication();
			
		}
	}

	
}
