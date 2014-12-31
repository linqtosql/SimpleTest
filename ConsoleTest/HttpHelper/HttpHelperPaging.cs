using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleTest
{
    public class HttpHelperPaging
    {
        private ConcurrentQueue<string> _urls;
        private object _lock = new object();
        private int _total;
        public HttpHelperPaging(string pageUrl, int pageCount, int pageStart, int pageSize)
        {
            PageUrl = pageUrl;
            PageCount = pageCount;
            PageStart = pageStart;
            PageSize = pageSize;
            _total = pageCount*pageSize;
            _urls = new ConcurrentQueue<string>();
            StartRequest();
        }

        private volatile int _currentPage;

        public int CurrentPage
        {
            get { return _currentPage; }
        }

        public string PageUrl { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int PageStart { get; set; }
        public Func<string, IEnumerable<string>> UrlsAction { get; set; }

        private void StartRequest()
        {
            for (int i = PageStart; i <= PageCount; i++)
            {
                var url = string.Format(PageUrl, i);
                var httpHelper = new HttpHelper(url);
                httpHelper.OpenReadAsync(httpHelper);
                httpHelper.OpenReadCompleted += HttpHelperOnOpenReadCompleted;
            }
        }

        private void HttpHelperOnOpenReadCompleted(object sender, OpenReadCompletedEventArgs args)
        {
            var httpHelper = args.UserState as HttpHelper;
            if (httpHelper != null)
                httpHelper.HttpWebRequest.Abort();
            _currentPage++;
            Console.WriteLine(_currentPage);

            //lock (_lock)
            //{
            //    if (args.Error != null)
            //    {
            //        _total -= PageSize;
            //        Console.WriteLine("_currentPage:{0} faild!", _currentPage);
            //    }
            //    else Console.WriteLine("_currentPage:{0} successed!", _currentPage);
            //    _currentPage++;
            //}

            //using (var streamReader = new StreamReader(args.Result))
            //{
            //    var html = streamReader.ReadToEnd();
            //    var urls = UrlsAction(html);
            //    foreach (string url in urls) _urls.Enqueue(url);
            //}

            //if (_currentPage == PageCount)
            //{
            //    lock (_lock)
            //    {
            //        if (_total == _urls.Count)
            //        {
            //            Console.WriteLine("_urls:{0},_currentPage:{1}", _urls.Count, _currentPage);
            //        }
            //    }
            //}
            if (_currentPage == PageCount)
            {
                Console.WriteLine("结束");
            }

            //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        }
    }
}
