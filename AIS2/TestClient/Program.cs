using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.AVBusinessLayerReference;

namespace TestClient
{
	class Program
	{
		private static IAVBusinessLayer businessLayer;

		static void Main(string[] args)
		{
			businessLayer = new AVBusinessLayerClient();


			Console.ReadLine();
		}
	}
}
