using System;
using Shared.Enums;

namespace AVClient
{
	public static class Privileges
	{
		static Privileges()
		{
			UserPrivileges = Enum.GetNames(typeof (UserPrivileges));
		}

		public static string[] UserPrivileges { get; set; }
	}
}
