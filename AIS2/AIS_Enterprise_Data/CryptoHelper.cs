using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data
{
	public static class CryptoHelper
	{
		public static string GetHash(string str)
		{
			var data = Encoding.ASCII.GetBytes(str);

			var sha1 = new SHA1CryptoServiceProvider();
			var sha1Data = sha1.ComputeHash(data);

			return Encoding.ASCII.GetString(sha1Data);
		}
	}
}
