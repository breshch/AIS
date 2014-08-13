using AIS_Enterprise_Global.Models.Currents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Models.Directories
{
    public class DirectoryUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string TranscriptionName { get; set; }

        public int CurrentUserStatusId { get; set; }
        public virtual CurrentUserStatus CurrentUserStatus { get; set; }


        private static int _currentUserId = 0;
        private static string _currentUserName = "";

        static DirectoryUser()
        {
        }

        public static int CurrentUserId
        {
            get
            {
                return _currentUserId;
            }
        }

        public static string CurrentUserName
        {
            get
            {
                return _currentUserName;
            }
        }

        public static void ChangeUserId(int userId, string userName)
        {
            _currentUserId = userId;
            _currentUserName = userName;

        }
    }
}
