using System.Web;
using System.Web.Optimization;

namespace OrderManagement
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //1. jqeury
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 2. modernizr 适配不能使用css5 和 html5的浏览器
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // 3. bootstrap 
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                                    "~/bower_components/bootstrap/dist/js/bootstrap.min.js",
                                    "~/Scripts/respond.js"));

            // 4. bootstrap-table
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-table").Include(
                        // 注意这里的名字有点绕，都要引用进来！！！！！！
                        "~/bower_components/bootstrap3-editable/js/bootstrap-editable.js",
                        "~/bower_components/bootstrap-table/bootstrap-table.js",
                        "~/bower_components/bootstrap-table/locale/bootstrap-table-zh-CN.js",
                        "~/bower_components/bootstrap-table/extensions/editable/bootstrap-table-editable.js"));


            // 5. all css
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/bower_components/bootstrap/dist/css/bootstrap.min.css",
                      "~/bower_components/bootstrap3-editable/css/bootstrap-editable.css",
                      "~/bower_components/bootstrap-table/bootstrap-table.min.css",
                      "~/bower_components/metisMenu/dist/metisMenu.min.css",
                      "~/Content/timeline.css",
                      "~/Content/sb-admin-2.css"));
        }
    }
}
