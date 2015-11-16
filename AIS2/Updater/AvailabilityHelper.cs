using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
	public static class AvailabilityHelper
	{
		public static bool IsOnline(string ip)
		{
			var ping = new Ping();
			var pingReply = ping.Send(ip);

			return pingReply.Status == IPStatus.Success;
		}
	}
}
