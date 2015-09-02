using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AIS_Enterprise_Data;
using ReportTool.Reports;

namespace ReportTool
{
	class Program
	{
		static void Main(string[] args)
		{
			Configuration.SetLocalization();

			CashReports.MonthCashReportMinsk();
		}
	}
}
