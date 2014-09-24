using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Br.Com.BiscoitinhosVovoLiva.Intranet.App_Start;
using log4net;

namespace Br.Com.BiscoitinhosVovoLiva.Intranet
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static ILog Logger()
        {
            return LogManager.GetLogger("BDVLiva.LOG");
        }

        protected void Application_Start()
        {
            //Dispara configuração do Log4Net.
            log4net.Config.XmlConfigurator.Configure();

            try
            {
                AreaRegistration.RegisterAllAreas();
                WebApiConfig.Register(GlobalConfiguration.Configuration);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);

                IntranetBootstrapper.Inicializar();
            }
            catch (Exception ex)
            {
                Logger().Error("Ocorreu um erro durante a inicialização do sistema.", ex);
                throw;
            }
        }
    }
}