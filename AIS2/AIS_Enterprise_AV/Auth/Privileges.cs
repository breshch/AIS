﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.Auth
{
	public static class Privileges
	{
		private static UserPrivileges[] _userPrivileges;
		private static int _userId;

		public static int UserId
		{
			get
			{
				return _userId;
			}
		}


		public static void LoadUserPrivileges(int userId)
		{
			_userId = userId;

			using (var bc = new BusinessContext())
			{
				_userPrivileges = bc.GetPrivileges(userId);
			}
		}

		public static bool HasAccess(UserPrivileges privilege)
		{
			return _userPrivileges.Contains(privilege);
		}

		public static UserPrivileges[] GetUserPrivileges()
		{
			return _userPrivileges;
		}
	}
}
