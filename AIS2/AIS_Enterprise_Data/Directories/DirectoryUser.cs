﻿using AIS_Enterprise_Data.Currents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace AIS_Enterprise_Data.Directories
{
    public class DirectoryUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string TranscriptionName { get; set; }

        public int CurrentUserStatusId { get; set; }
        public virtual CurrentUserStatus CurrentUserStatus { get; set; }

        private static List<string> _privileges = new List<string>();

        public static List<string> Privileges
        {
            get
            {
                return _privileges;
            }
        }


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

        public static void ChangeUserId(BusinessContext bc, int userId, string userName)
        {
            _currentUserId = userId;
            _currentUserName = userName;

             _privileges = bc.GetPrivileges(_currentUserId);
        }
    }
}
