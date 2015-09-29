using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Messaging;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Gas;

namespace AIS_Enterprise_Gas
{
	public class Program
	{
		private static void Main(string[] args)
		{
			if (args.Any())
			{
				string serviceName = "AIS_Enterprise_Gas";
				switch (args[0])
				{
					case "-c":
						var gas = new GasProcessing();
						gas.GetBalance();
						Console.ReadKey();
						break;
					case "-i":
						if (GetServiceStatus(serviceName) != "notInstalled")
						{
							if (GetServiceStatus(serviceName) == "started")
							{
								StopService(serviceName);
								DeleteService(serviceName);
							}
							else if (GetServiceStatus(serviceName) == "stopped")
							{
								DeleteService(serviceName);
							}
						}
						
							InstallService();
							
								StatrtService(serviceName);
							
						break;
					case "-u":
						DeleteService(serviceName);
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

		private static void DeleteService(string serviceName)
		{
			Process.Start("cmd", "/C sc delete " + serviceName);
			//KillProcess();
		}

		private static void InstallService()
		{
			string pathApp = Path.Combine(Environment.CurrentDirectory, "AIS_Enterprise_Gas.exe");
			Process.Start("cmd", @"/C C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe " + pathApp);
			//KillProcess();
		}

		private static void StatrtService(string serviceName)
		{
			var sc = new ServiceController(serviceName);
			sc.Start();
			sc.WaitForStatus(ServiceControllerStatus.Running);
			//KillProcess();
		}

		private static void StopService(string serviceName)
		{
			var sc = new ServiceController(serviceName);
			sc.Stop();
			sc.WaitForStatus(ServiceControllerStatus.Running);
			//KillProcess();
		}

		private static string GetServiceStatus(string serviceName)
		{

			ServiceController[] services = ServiceController.GetServices();
			string status = null;

			foreach (ServiceController service in services)
			{

				if ((service.ServiceName == serviceName) && (service.Status == ServiceControllerStatus.Running))
				{
					status = "started";
				}
				else if ((service.ServiceName == serviceName) && (service.Status == ServiceControllerStatus.Stopped))
				{
					status = "stopped";
				}
				else
				{
					status = "notInstalled";
				}
			}
			return status;
		}

		private static void KillProcess()
		{
			var proceses = Process.GetProcesses();
			foreach (var proces in proceses)
			{

				if (proces.ToString() == "AIS_Enterprise_Gas.vshost.exe")
				{
					proces.Kill();
				}
			}

		}

	}

}
