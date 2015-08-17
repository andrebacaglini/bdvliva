using System.Web.Mvc;

namespace Br.Com.BiscoitinhosVovoLiva.Intranet.Controllers
{
    [Authorize]
    public class BiscoitinhoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
