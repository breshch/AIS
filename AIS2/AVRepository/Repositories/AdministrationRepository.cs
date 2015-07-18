using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Directories;

namespace AVRepository.Repositories
{
	public class AdministrationRepository : BaseRepository
	{
		public DirectoryUserStatus[] GetDirectoryUserStatuses()
		{
			using (var db = GetContext())
			{
				return db.DirectoryUserStatuses.ToArray();
			}
		}

		public DirectoryUserStatus AddDirectoryUserStatus(string name, List<CurrentUserStatusPrivilege> privileges)
		{
			using (var db = GetContext())
			{
				var directoryUserStatus = new DirectoryUserStatus { Name = name, Privileges = privileges };
				db.DirectoryUserStatuses.Add(directoryUserStatus);

				db.SaveChanges();

				return directoryUserStatus;
			}
		}

		public void EditDirectoryUserStatus(int userStatusId, string userStatusName,
			List<CurrentUserStatusPrivilege> privileges)
		{
			using (var db = GetContext())
			{
				var userStatus = db.DirectoryUserStatuses.Find(userStatusId);
				userStatus.Name = userStatusName;
				db.CurrentUserStatusPrivileges.RemoveRange(userStatus.Privileges);
				userStatus.Privileges = privileges;

				db.SaveChanges();
			}
		}

		public void RemoveDirectoryUserStatus(int id)
		{
			using (var db = GetContext())
			{
				var directoryUserStatus = db.DirectoryUserStatuses.Include(s => s.Privileges).First(s => s.Id == id);
				db.DirectoryUserStatuses.Remove(directoryUserStatus);

				db.SaveChanges();
			}
		}



		public DirectoryUser[] GetDirectoryUsers()
		{
			using (var db = GetContext())
			{
				return db.DirectoryUsers.ToArray();
			}
		}

		public DirectoryUser GetDirectoryUser(int userId)
		{
			using (var db = GetContext())
			{
				return db.DirectoryUsers.Find(userId);
			}
		}

		public DirectoryUser AddDirectoryUser(string userName, string password, DirectoryUserStatus userStatus)
		{
			using (var db = GetContext())
			{
				var user = new DirectoryUser
				{
					UserName = userName,
					CurrentUserStatus = new CurrentUserStatus { DirectoryUserStatus = userStatus }
				};

				db.DirectoryUsers.Add(user);
				db.SaveChanges();

				db.Database.Connection.ConnectionString = "";

				return user;
			}
		}

		public DirectoryUser AddDirectoryUserAdmin(string userName, string password)
		{
			using (var db = GetContext())
			{
				var userStatus = db.DirectoryUserStatuses.First(s => s.Name == "Администратор");

				var user = new DirectoryUser
				{
					UserName = userName,
					CurrentUserStatus = new CurrentUserStatus { DirectoryUserStatus = userStatus }
				};

				db.DirectoryUsers.Add(user);
				db.SaveChanges();

				return user;
			}
		}

		public void EditDirectoryUser(int userId, string userName, string password, DirectoryUserStatus userStatus)
		{
			using (var db = GetContext())
			{
				var user = db.DirectoryUsers.Find(userId);

				user.UserName = userName;

				var prevCurrentUserStatus = user.CurrentUserStatus;

				user.CurrentUserStatus = new CurrentUserStatus { DirectoryUserStatus = userStatus };

				db.SaveChanges();

				db.CurrentUserStatuses.Remove(prevCurrentUserStatus);
				db.SaveChanges();
			}
		}

		public void RemoveDirectoryUser(DirectoryUser user)
		{
			using (var db = GetContext())
			{
				db.DirectoryUsers.Remove(user);

				db.SaveChanges();
			}
		}

		public DirectoryUserStatusPrivilege GetDirectoryUserStatusPrivilege(string privilegeName)
		{
			using (var db = GetContext())
			{
				return db.DirectoryUserStatusPrivileges.First(p => p.Name == privilegeName);
			}
		}

		public string[] GetPrivileges(int userId)
		{
			using (var db = GetContext())
			{
				int currentUserStatusId = db.DirectoryUsers.Find(userId).CurrentUserStatusId;
				int directoryUserStatusId = db.CurrentUserStatuses.Find(currentUserStatusId).DirectoryUserStatusId;
				var directoryUserStatusPrivilegeIds =
					db.CurrentUserStatusPrivileges.Where(p => p.DirectoryUserStatusId == directoryUserStatusId)
						.Select(p => p.DirectoryUserStatusPrivilegeId);
				var privileges =
					db.DirectoryUserStatusPrivileges.Where(p => directoryUserStatusPrivilegeIds.Contains(p.Id))
						.Select(p => p.Name)
						.ToList();
				return privileges.ToArray();
			}
		}

		#region Auth

		public bool LoginUser(int userId, string password)
		{
			using (var db = GetContext())
			{
				var auth = db.Auths.FirstOrDefault(s => s.DirectoryUserId == userId);
				if (auth == null)
				{
					return false;
				}

				var hash = CryptoHelper.GetHash(password + auth.Salt);

				return auth.Hash == hash;
			}
		}

		#endregion
	}
}
