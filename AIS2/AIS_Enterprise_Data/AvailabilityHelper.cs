using System.Net.NetworkInformation;

namespace AIS_Enterprise_Data
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
