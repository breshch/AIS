using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using GasStatusService;

namespace AIS_Enterprise_Gas
{
	public class Program
	{
		static void Main(string[] args)
		{
			if (args.Any())
			{
				switch (args[0])
				{
					case "-c":
						var gas = new GasProcessing();
						gas.GetBalance();
						Console.ReadKey();
						break;
					case "-i":
						string pathApp = Path.Combine(Environment.CurrentDirectory, "AIS_Enterprise_Gas.exe");
						Process.Start("cmd", @"/C C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe " +
							pathApp);
						break;
					case "-u":
						Process.Start("cmd", "/C sc delete GasStatusService");
						break;
					default:
						throw new Exception();
				}
			}
			else
			{
				ServiceBase[] ServicesToRun;
				ServicesToRun = new ServiceBase[]
				{
					new Service1()
				};
				ServiceBase.Run(ServicesToRun);
			}
		}
	}
}
