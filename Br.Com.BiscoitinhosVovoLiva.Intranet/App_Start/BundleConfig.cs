using System.Web;
using System.Web.Optimization;

namespace Br.Com.BiscoitinhosVovoLiva.Intranet
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

#if DEBUG
            #region Bundles JS jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery/js").Include(
                        "~/Scripts/jquery-{version}.js"));
            #endregion

            #region Bundles JS Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap/js").Include(
                        "~/Scripts/bootstrap-3.2.0/bootstrap.js"));
            #endregion

            #region Bundles JS jQuery-Bootgrid
            bundles.Add(new ScriptBundle("~/bundles/jquery-bootgrid/js").Include(
                        "~/Scripts/jquery.bootgrid-1.1.0/jquery.bootgrid.js"));
            #endregion

            #region Bundles CSS Bootstrap
            bundles.Add(new StyleBundle("~/bundles/bootstrap/css")
                .Include("~/Content/bootstrap-3.2.0/css/bootstrap.css")
                .Include("~/Content/bootstrap-3.2.0/css/bootstrap-theme.css")
            );
            #endregion


#else
            #region Bundles JS jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery/js").Include(
                        "~/Scripts/jquery-{version}.min.js"));
            #endregion

            #region Bundles JS Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap/js").Include(
                        "~/Scripts/bootstrap-3.2.0/bootstrap.min.js"));
             #endregion

            #region Bundles JS jQuery-Bootgrid
            bundles.Add(new ScriptBundle("~/bundles/jquery-bootgrid/js").Include(
                        "~/Scripts/jquery.bootgrid-1.1.0/jquery.bootgrid.min.js"));
            #endregion

            #region Bundles CSS Bootstrap
            bundles.Add(new StyleBundle("~/bundles/bootstrap/css")
                .Include("~/Content/bootstrap-3.2.0/css/bootstrap.min.css")
                .Include("~/Content/bootstrap-3.2.0/css/bootstrap-theme.min.css")
            );
            #endregion
#endif

            #region Bundles CSS Comuns
            bundles.Add(new StyleBundle("~/bundles/css")
                .Include("~/Content/css/carousel.css"));
            #endregion

            #region Bundles CSS jQuery-Bootgrid
            bundles.Add(new StyleBundle("~/bundles/jquery-bootgrid/css")
                .Include("~/Content/css/jquery.bootgrid-1.1.0/jquery.bootgrid.css"));
            #endregion
        }
    }
}