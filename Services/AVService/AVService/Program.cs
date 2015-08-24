using System;
using System.ServiceModel;

namespace AVService
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var host = new ServiceHost(typeof (AVBusinessLayer)))
			{
				host.Open();
				Console.WriteLine("Service is open");
				Console.ReadLine();
			}
		}
	}
}
