using System.Windows;
using AIS_Enterprise_AV.WareHouse;

namespace AIS_Enterprise_AV
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

			var scheme = new Scheme();
			scheme.ShowDialog();


			//string pathUpdater = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).FullName,
			//	"Updater/AIS_Enterprise_Updater.exe");

			//if (File.Exists(pathUpdater))
			//{
			//	Process.Start(pathUpdater);
			//}

			//if (DataContext.TryConnection())
			//{
			//	HelperMethods.ShowView(new MainViewModel(), new MainView());
			//}
			//else
			//{
			//	HelperMethods.ShowView(new InitializingDBViewModel(), new InitializingDBView());
			//}
		}
	}
}
