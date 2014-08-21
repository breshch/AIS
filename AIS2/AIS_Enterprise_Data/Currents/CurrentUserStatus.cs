using AIS_Enterprise_Data.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Currents
{
    public class CurrentUserStatus
    {
        public int Id { get; set; }
        public int DirectoryUserStatusId { get; set; }
        public virtual DirectoryUserStatus DirectoryUserStatus { get; set; }
    }
}
