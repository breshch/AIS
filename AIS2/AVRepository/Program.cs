using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AVRepository
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
