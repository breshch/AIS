using System.Collections.Generic;

namespace AIS_Enterprise_Global.Helpers
{
    public delegate List<int> GettingYears(BusinessContext bc);

    public delegate List<int> GettingMonthes(BusinessContext bc, int year);
}
