using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Br.Com.BiscoitinhosVovoLiva.Intranet.Controllers;

namespace Br.Com.BiscoitinhosVovoLiva.Intranet.App_Start
{
    public class IntranetBootstrapper
    {
        public static void Inicializar()
        {
            var builder = Bootstrapper.Bootstrapper.ObterContainer();

            #region Controllers
            
            // Registrando no container os controllers
            var controllers = typeof(HomeController).Assembly;
            builder.RegisterAssemblyTypes(controllers)
                .Where(t => t.Namespace == "Br.Com.BiscoitinhosVovoLiva.Intranet.Controllers")
                .AsSelf()
                .PropertiesAutowired();

            #endregion

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            System.Web.HttpContext.Current.Application["DependencyResolver"] = DependencyResolver.Current;
        }
    }
}