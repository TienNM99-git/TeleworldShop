using System.Web;
using System.Web.Optimization;
using TeleworldShop.Common;

namespace TeleworldShop.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/jquery").Include("~/Assets/client/js/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/js/plugins").Include(
                 "~/Assets/admin/libs/jquery-ui/jquery-ui.min.js",
                 "~/Assets/admin/libs/mustache/mustache.js",
                 "~/Assets/admin/libs/numeral/numeral.js",
                 "~/Assets/admin/libs/jquery-validation/jquery.validate.js",
                 "~/Assets/admin/libs/jquery-validation/additional-methods.min.js",
                 "~/Assets/client/js/common.js"
                ));
            bundles.Add(new ScriptBundle("~/js/forms").Include(
                "~/Assets/admin/libs/jquery/jquery.min.js",
                "~/Assets/admin/libs/bootstrap/js/bootstrap.bundle.min.js",
                "~/Assets/admin/js/adminlte.min.js"
               ));
            bundles.Add(new StyleBundle("~/css/advanced")
               .Include("~/Assets/admin/css/adminlte.min.css", new CssRewriteUrlTransform())
               .Include("~/Assets/admin/libs/fontawesome-free/css/all.min.css", new CssRewriteUrlTransform())
               .Include("~/Assets/admin/libs/icheck-bootstrap/icheck-bootstrap.min.css", new CssRewriteUrlTransform())
               .Include("~/Assets/client/css/custom.css", new CssRewriteUrlTransform())
               );
            bundles.Add(new StyleBundle("~/css/base")
                .Include("~/Assets/client/css/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/font-awesome-4.6.3/css/font-awesome.css", new CssRewriteUrlTransform())
                .Include("~/Assets/admin/libs/jquery-ui/themes/smoothness/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/css/style.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/css/custom.css", new CssRewriteUrlTransform())
                );
            BundleTable.EnableOptimizations = bool.Parse(ConfigHelper.GetByKey("EnableBundles"));
        }
    }
}
