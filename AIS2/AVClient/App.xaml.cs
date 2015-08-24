using System.Windows;
using AVClient.Configuration;
using AVClient.ViewModels;
using AVClient.Views;

namespace AVClient
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			AutoMapperConfiguration.Configurate();

			//var scheme = new Scheme();
			//scheme.ShowDialog();


			//string pathUpdater = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).FullName,
			//	"Updater/AIS_Enterprise_Updater.exe");

			//if (File.Exists(pathUpdater))
			//{
			//	Process.Start(pathUpdater);
			//}



			HelperMethods.ShowView(new MainViewModel(), new MainView());
			//HelperMethods.ShowView(new InitializingDBViewModel(), new InitializingDBView());
		}
	}
}
