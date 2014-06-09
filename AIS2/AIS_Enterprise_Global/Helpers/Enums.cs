using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Helpers
{
    public enum Gender
    {
        Male,
        Female
    }

    public enum DescriptionDay
    {
        Был,
        Б,
        О,
        ДО,
        П,
        С
    }

    public enum UserPrivileges
    {
        MenuVisibility_Directories_Companies,
        MenuVisibility_Directories_Posts_TypeOfPost,
        MenuVisibility_Directories_Posts_ListPosts,
    }

   

    public class Privileges
    {
        public enum Rules
        {
            MenuVisibility,
            MonthTimeSheetColumnsVisibility
        }

        public enum MenuVisibility
        {
            Directories,
            Reports
        }

        public enum MenuVisibilityDirectories
        {
            Companies,
            Posts
        }

        public enum MenuVisibilityDirectoriesPosts
        {
            TypeOfPost,
            ListPosts
        }
    }
}