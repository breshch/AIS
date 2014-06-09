using AIS_Enterprise_Global.Models.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Models.Currents
{
    public class CurrentUserStatus
    {
        public int Id { get; set; }
        public int DirectoryUserStatusId { get; set; }
        public virtual DirectoryUserStatus DirectoryUserStatus { get; set; }
    }
}
