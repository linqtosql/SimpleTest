using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace ConsoleTest
{
    public class Program
    {
        public int count = 0;
        Mutex mutex = new Mutex();
        public int Count { get { return count; } set { count = value; } }
        private static void Main(string[] args)
        {
            //IDbCommand dbCommand = new SqlCommand("");
            //dbCommand.CommandTimeout = 5000;
            //var list = new List<Program>();
            //list.Add(new Program());
            //var p = list[0];
            //list.RemoveAt(0);
            //var html = Razor.Parse("<h2>这是视图内容 @Model.Name</h2>", new {Name = "张三"});

            //var path = Environment.CurrentDirectory;
            //path = Path.Combine(path, "Views\\Index.cshtml");
            //Console.WriteLine(path);

            //var html = GetRazorViewAsString(new {}, path);
            //var assembly1 = Assembly.LoadFile(@"E:\Demo\TestLucene\bin\Debug\TestLucene.exe");
            ////var types = assembly1.GetTypes();
            //var obj = assembly1.CreateInstance("TestLucene.TestStatic");
            //var type = obj.GetType();
            //type.GetMethod("Add").Invoke(obj, new object[] {5});

            //var assembly2 = Assembly.LoadFile(@"E:\Demo\TestLucene\bin\Debug\TestLucene.exe");
            ////var types = assembly1.GetTypes();
            //var obj2 = assembly2.CreateInstance("TestLucene.TestStatic");
            //var type2 = obj2.GetType();
            ////var method = type2.GetMethod("Add");
            //type2.GetMethod("Add").Invoke(obj2, new object[] { 2 });
            ////type.GetMethod("Add").Invoke(obj, new object[] { 5 });

            //int i=(int) type.GetMethod("Get").Invoke(obj, null);
            //int i2=(int)type2.GetMethod("Get").Invoke(obj2, null);


            //bool b = type == type2;
            //type2.GetMethod("Add").Invoke(obj2, new object[] { 3 });

            //var stack = new ConcurrentStack<string>();

            //var queue = new ConcurrentQueue<string>();

            //var httpHelper = new HttpHelper("http://www.baidu.com/");
            //httpHelper.OpenReadAsync(null);
            //httpHelper.OpenReadCompleted += httpHelper_OpenReadCompleted;
            //Console.ReadLine();

            //var pageHelper = new HttpHelperPaging("http://www.globalmarket.com/hot-products/led-{0}-38.html", 100, 1, 38);
            //pageHelper.UrlsAction = html =>
            //                            {
            //                                var htmlDoc = new HtmlDocument();
            //                                htmlDoc.LoadHtml(html);
            //                                var nodes =
            //                                    htmlDoc.DocumentNode.SelectNodes(".//div[@class='col desc']/h3/a");
            //                                return nodes.Select(n => n.Attributes["href"].Value);
            //                           };


            //var query = new WebQuery("李强");
            //query.StartIndex.Value = 1;
            //query.HostLangauge.Value = Languages.Chinese_Simplified;
            //IGoogleResultSet<GoogleWebResult> resultSet = GoogleService.Instance.Search<GoogleWebResult>(query);
            //for (int i = 0; i < 100; i++)
            //{
            //    Thread.Sleep(1000);
            //    var client = new GwebSearchClient("www.hao123.com");
            //    IList<IWebResult> results = client.Search(i.ToString(), 10);
            //    if (results.Count > 0)
            //        Console.WriteLine("{0}:{1}", i, results[0].Title);
            //}

            //var url = "https://www.google.com/search?q={0}";

            //var httpHelper = new HttpHelper(string.Format(url, "Ed Bedrick Autographs"));
            //using (var stream = httpHelper.OpenRead())
            //{
            //    var streamReader = new StreamReader(stream);
            //    var html = streamReader.ReadToEnd();
            //    var htmlDoc = new HtmlDocument();
            //    htmlDoc.LoadHtml(html);
            //    var nodes = htmlDoc.DocumentNode.SelectNodes(".//li[@class='g']//h3/a");
            //    Console.WriteLine("{0}:{1},{2}", 0, nodes.Count, nodes.Count > 0 ? nodes[0].InnerText : "");

            //}
            //httpHelper.HttpWebRequest.Abort();
            //var a = System.Net.CredentialCache.DefaultNetworkCredentials;

            try
            {

                var p = new Program();
                var t1 = new Thread(()=>
                                           {
                                               for (int i = 0; i < 10000; i++)
                                               {
                                                   p.SetCount();
                                               }
                                           });
                var t2 = new Thread(()=>
                                           {
                                               for (int i = 0; i < 10000; i++)
                                               {
                                                   p.SetCount();
                                               }
                                           });
                var t3 = new Thread(() =>
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        p.SetCount();
                    }
                });
                t1.Start();
                t2.Start();
                t3.Start();

                var t4 = new Thread(()=>
                                        {
                                            while (true)
                                            {
                                                Thread.Sleep(1000);
                                                Console.WriteLine(p.count);
                                            }
                                        });
                t4.Start();
                Console.ReadLine();
                //dynamic ddd = obj;
                //var path = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                //if (NetworkInterface.GetIsNetworkAvailable())
                //{
                //    NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                //    foreach (NetworkInterface Interface in interfaces)
                //    {
                //        if (Interface.OperationalStatus == OperationalStatus.Up)
                //        {
                //            if ((Interface.NetworkInterfaceType == NetworkInterfaceType.Ppp) && (Interface.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                //            {
                //                IPv4InterfaceStatistics statistics = Interface.GetIPv4Statistics();
                //                Console.WriteLine(Interface.Name + " " + Interface.NetworkInterfaceType.ToString() + " " + Interface.Description);
                //            }
                //            else
                //            {
                //                Console.WriteLine("VPN Connection is lost!");
                //            }

                //        }
                //    }
                //}
                //var arr = new string[] {};
                //if(arr.Any())
                //{
                //    Console.WriteLine(true);
                //}
                //var proxy = WebRequest.GetSystemWebProxy();
                //var url = new Uri("https://www.google.com");
                //var b = proxy.GetProxy(url);
                //var c = url == b;
                //Console.WriteLine(b);
                //var httpClient = (HttpWebRequest)WebRequest.Create("https://www.google.com");
                //httpClient.Proxy = WebRequest.DefaultWebProxy;
                //httpClient.UseDefaultCredentials = true;
                //httpClient.CookieContainer = new CookieContainer();
                ////var httpClient = (HttpWebRequest)WebRequest.Create("http://www.baidu.com");
                //if (httpClient.Proxy != null)
                //{
                //    var b = httpClient.Proxy.GetProxy(httpClient.RequestUri);
                //    Console.WriteLine(b);
                //}
                //else
                //{
                //WebProxy proxy = new WebProxy("210.14.152.91", 88);

                //    httpClient.Proxy = proxy;
                //}
                //httpClient.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36";
                //var reponse = httpClient.GetResponse();
                //var stram = reponse.GetResponseStream();
                //StreamReader reader = new StreamReader(stram);
                //var str = reader.ReadToEnd();
                //reader.Close();
                //stram.Close();
                //httpClient.Abort();
                //Console.WriteLine(str);
                //Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        public void SetCount()
        {
            mutex.WaitOne();
            Count++;
            mutex.ReleaseMutex();
        }

        public void Write(string str)
        {
            var s = string.Format("这是线程：{0}", str);
            Console.WriteLine(s);
        }


        static void httpHelper_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            var reader = new StreamReader(e.Result);
            var str = reader.ReadToEnd();
        }

        public static HttpContext FakeHttpContext()
        {
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


        //public static DataTable GetTable(object model)
        //{
        //    var dataTable = new DataTable();

        //    var props =
        //        model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(
        //            p => p.CanRead).ToArray();

        //    var vals = new List<object>();
        //    foreach (PropertyInfo property in props)
        //    {
        //        vals.Add(property.GetValue(model));
        //    }
        //    dataTable.Rows.Add(vals);
        //    return dataTable;
        //}


    }

    public class HomeController : Controller
    {

    }

    public class MyVirtualPathProvider : VirtualPathProvider
    {
        public override string GetCacheKey(string virtualPath)
        {
            return virtualPath;
        }
    }
}
