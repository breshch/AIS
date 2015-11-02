using System.Windows;

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
			installer.InstallApplication();
			
		}
	}

	
}
