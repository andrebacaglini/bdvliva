using System.Web;
using System.Web.Optimization;

namespace Br.Com.BiscoitinhosVovoLiva.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Bundles jQuery

            bundles.Add(new ScriptBundle("~/bundles/jquery/js").Include(
                        "~/Scripts/jquery-{version}.js"));

            #endregion

            #region Bundles do Bootstrap

            bundles.Add(new StyleBundle("~/bundles/bootstrap/css")
                .Include("~/Content/bootstrap/css/bootstrap-theme.css")
                );

            #endregion

            #region Bundles Comuns

            bundles.Add(new StyleBundle("~/bundles/css")
                .Include("~/Content/css/carousel.css"));

            #endregion
        }
    }
}