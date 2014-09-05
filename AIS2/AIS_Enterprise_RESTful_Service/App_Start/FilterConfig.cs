using System.Web;
using System.Web.Mvc;

namespace AIS_Enterprise_RESTful_Service
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
