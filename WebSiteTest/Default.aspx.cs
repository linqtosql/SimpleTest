using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSiteTest
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //var v = HostingEnvironment.VirtualPathProvider;
            //var html = GetRazorViewAsString(new { }, "~/Views/Index.cshtml");
            
        }

        public static HttpContext FakeHttpContext()
        {
            if(HttpContext.Current != null)
                return HttpContext.Current;
            var httpRequest = new HttpRequest("", "http://stackoverflow/", "");
            var stringWriter = new StringWriter();
            var httpResponce = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponce);

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                                    new HttpStaticObjectsCollection(), 10, true,
                                                    HttpCookieMode.AutoDetect,
                                                    SessionStateMode.InProc, false);

            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                                        BindingFlags.NonPublic | BindingFlags.Instance,
                                        null, CallingConventions.Standard,
                                        new[] { typeof(HttpSessionStateContainer) },
                                        null)
                                .Invoke(new object[] { sessionContainer });
            
            return httpContext;
        }

        public static string GetRazorViewAsString(object model, string filePath)
        {
            var st = new StringWriter();
            var context = new HttpContextWrapper(FakeHttpContext());
            var routeData = new RouteData();
            var controllerContext = new ControllerContext(new RequestContext(context, routeData), new HomeController());

            var razor = new RazorView(controllerContext, filePath, null, false, null);


            razor.Render(
                new ViewContext(controllerContext, razor, new ViewDataDictionary(model), new TempDataDictionary(), st),
                st);

            return st.ToString();
        }
    }

    public class HomeController : Controller
    {

    }
}