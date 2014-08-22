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
        MenuVisibility_Costs_DayCosts,
        MenuVisibility_Costs_MonthCosts,
        MenuVisibility_Costs_DefaultCosts,
        MenuVisibility_Costs_Safe,
        MenuVisibility_Reports_ReportSalaryPrint,
        MenuVisibility_Reports_ReportSalaryMinsk,
        MenuVisibility_Reports_ReportCosts,
        MenuVisibility_Reports_ReportCash,
        MenuVisibility_AdminPanel_UserStatuses,
        MenuVisibility_AdminPanel_Users,
        MenuVisibility_AdminPanel_Logs,

        MonthTimeSheetColumnsVisibility_FullName,
        MonthTimeSheetColumnsVisibility_PostName,
        MonthTimeSheetColumnsVisibility_SalaryInHour,
        MonthTimeSheetColumnsVisibility_Hours,
        MonthTimeSheetColumnsVisibility_OverTime,
        MonthTimeSheetColumnsVisibility_VocationDays,
        MonthTimeSheetColumnsVisibility_SickDays,
        MonthTimeSheetColumnsVisibility_MissDays,
        MonthTimeSheetColumnsVisibility_PrepaymentCash,
        MonthTimeSheetColumnsVisibility_PrepaymentBankTransaction,
        MonthTimeSheetColumnsVisibility_Compensation,
        MonthTimeSheetColumnsVisibility_VocationPayment,
        MonthTimeSheetColumnsVisibility_CardAV,
        MonthTimeSheetColumnsVisibility_CardFenox,
        MonthTimeSheetColumnsVisibility_Panalty,
        MonthTimeSheetColumnsVisibility_Inventory,
        MonthTimeSheetColumnsVisibility_BirthDays,
        MonthTimeSheetColumnsVisibility_Bonus,
        MonthTimeSheetColumnsVisibility_FinalSalary,

        MonthTimeSheetColumnsNotReadOnly_Hours,
        MonthTimeSheetColumnsNotReadOnly_PrepaymentCash,
        MonthTimeSheetColumnsNotReadOnly_PrepaymentBankTransaction,
        MonthTimeSheetColumnsNotReadOnly_Compensation,
        MonthTimeSheetColumnsNotReadOnly_VocationPayment,
        MonthTimeSheetColumnsNotReadOnly_CardAV,
        MonthTimeSheetColumnsNotReadOnly_CardFenox,
        MonthTimeSheetColumnsNotReadOnly_Inventory,
        MonthTimeSheetColumnsNotReadOnly_Bonus,

        Salary_AdminSalary,
        
        WorkersVisibility_DeadSpirit,
        WorkersVisibility_Office,

        Workers_FireWorkers,

        ButtonsVisibility_AdminButtons,

        CostsVisibility_IsNotTransportOnly
    }

    public enum Rules
    {
        MenuVisibility,
        MonthTimeSheetColumnsVisibility,
        MonthTimeSheetColumnsNotReadOnly,
        Salary,
        WorkersVisibility,
        Workers,
        ButtonsVisibility,
        CostsVisibility
    }

    public enum LoggingOptions
    {
        Info,
        Fatal
    }

    public enum CashType
    {
        Наличка,
        Карточка
    }
}