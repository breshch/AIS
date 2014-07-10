using AIS_Enterprise_Global.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Helpers
{
    public delegate List<int> GettingYears(BusinessContext bc);

    public delegate List<int> GettingMonthes(BusinessContext bc, int year);
}
