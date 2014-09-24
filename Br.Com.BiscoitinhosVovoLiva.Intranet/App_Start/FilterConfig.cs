using System.Web;
using System.Web.Mvc;

namespace Br.Com.BiscoitinhosVovoLiva.Intranet
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}