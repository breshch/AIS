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
        MenuVisibility_Directories_RCs,
        MenuVisibility_Directories_Posts_TypeOfPosts,
        MenuVisibility_Directories_Posts_ListOfPosts,
        MenuVisibility_Directories_Workers_AddingWorker,
        MenuVisibility_Directories_Workers_ListOfWorkers,
        
        MenuVisibility_Reports_Salary,

        MenuVisibility_AdminPanel_UserStatuses,
        MenuVisibility_AdminPanel_Users,
    }

    public enum Rules
    {
        MenuVisibility,
    }
}