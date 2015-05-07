using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Gas
{
	public class Program
	{
		static void Main(string[] args)
		{
			var gas = new GasProcessing();
			var task = Task.Factory.StartNew(gas.GetBalance);

			task.Wait();
		}
	}
}
