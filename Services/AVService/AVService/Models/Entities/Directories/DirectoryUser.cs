using AVService.Models.Entities.Currents;

namespace AVService.Models.Entities.Directories
{
    public class DirectoryUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public int CurrentUserStatusId { get; set; }
        public virtual CurrentUserStatus CurrentUserStatus { get; set; }

		//private static List<string> _privileges = new List<string>();

		//public static List<string> Privileges
		//{
		//	get
		//	{
		//		return _privileges;
		//	}
		//}

		//public static void ChangeUserId(BusinessContext bc, int userId, string userName)
		//{
		//	_currentUserId = userId;
		//	_currentUserName = userName;

		//	 _privileges = bc.GetPrivileges(_currentUserId);
		//}
    }
}
