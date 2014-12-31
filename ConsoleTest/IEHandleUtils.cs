//#region 文件注释

////  /*************************************************** 
////   	* Copyright (C) 2013 Spring
////   	* 版权所有。 
////   	* 
////   	* 文件名:IEHandleUtils.cs
////   	* 文件功能简述：
//// 	*	这里填写简要描述
////   	* 作者: Spring
////   	* 创建日期:2013/12/17
////   	* 修改历史:
////   	*     R1:
////   	*         修改人:Spring
////   	*         修改时间:2014/01/07
////   	*         修改内容:
////   	*            此处只作主要说明,具体说明在具体方法处
//// ***************************************************/

//#endregion

//#region 引用

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Net;
//using System.Reflection;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Web;

//#endregion

//namespace ConsoleTest
//{
//    public class IEHandleUtils
//    {

//        #region 异步请求

//        private const int TIMEOUT = 60*1000;
//        private readonly static object lockObj = new object();
//        public class RequestState
//        {
//            public string requestData;

//            public HttpWebRequest request;
//            public HttpWebResponse response;
//            public Stream streamResponse;
//            public AsyncRequestCallBack callBack;
//            public byte[] postBuff = null;
//            public bool HasError = false;
//            public Exception Error = null;
//            public HttpContext context;
//            public int tryCount = 0; //已重发的次数
//            public int maxTryCount = 3; //请求失败后重发次数最大值

//            public RequestState()
//            {
//                request = null;
//                streamResponse = null;
//            }
//        }

//        public delegate void AsyncRequestCallBack(RequestState state);

//        public static void BeginGetHtml(RequestState state)
//        {
//            var isPost = state.postBuff != null;
//            //ServicePointManager.Expect100Continue = false;
//            //ServicePointManager.DefaultConnectionLimit = HttpHelper.DefaultConnectionLimit;
//            ////设置并发连接数限制上额 
//            //HttpHelper.DefaultConnectionLimit++;

//            var httpWebRequest = (HttpWebRequest) WebRequest.Create(state.request.RequestUri.ToString());
//            httpWebRequest.Method = isPost ? "POST" : "GET";
//            httpWebRequest.AllowAutoRedirect = true;
//            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
//            httpWebRequest.Accept = "*/*";
//            httpWebRequest.Timeout = TIMEOUT;
//            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
//            httpWebRequest.CookieContainer = state.request.CookieContainer;

//            state.request = httpWebRequest;
//            if (isPost)
//            {
//                httpWebRequest.ContentLength = state.postBuff.Length;
//                httpWebRequest.BeginGetRequestStream(RequCallback, state);
//            }
//            else
//            {
//                httpWebRequest.BeginGetResponse(RespCallback, state);
//            }
//        }

//        public static void BeginGetHtml(string url, string postString, CookieContainer cookie, AsyncRequestCallBack callback)
//        {
//            var myState = new RequestState();

//            var isPost = !string.IsNullOrEmpty(postString);
//            //ServicePointManager.Expect100Continue = false;
//            //ServicePointManager.DefaultConnectionLimit = HttpHelper.DefaultConnectionLimit;
//            ////设置并发连接数限制上额 
//            //HttpHelper.DefaultConnectionLimit++;

//            var httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
//            httpWebRequest.Method = isPost ? "POST" : "GET";
//            httpWebRequest.AllowAutoRedirect = true;
//            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
//            httpWebRequest.Accept = "*/*";
//            httpWebRequest.Timeout = TIMEOUT;
//            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
//            httpWebRequest.CookieContainer = cookie;

//            myState.request = httpWebRequest;
//            myState.callBack = callback;
//            myState.context = HttpContext.Current;
//            if (isPost)
//            {
//                var byteRequest = Encoding.UTF8.GetBytes(postString);
//                myState.postBuff = byteRequest;
//                httpWebRequest.ContentLength = byteRequest.Length;
//                httpWebRequest.BeginGetRequestStream(RequCallback, myState);
//            }
//            else
//            {
//                httpWebRequest.BeginGetResponse(RespCallback, myState);
//            }
//        }

//        private static void RequCallback(IAsyncResult asynchronousResult)
//        {
//            var myRequestState = (RequestState) asynchronousResult.AsyncState;
//            try
//            {
//                var postStream = myRequestState.request.EndGetRequestStream(asynchronousResult);
//                postStream.Write(myRequestState.postBuff, 0, myRequestState.postBuff.Length);
//                postStream.Close();

//                myRequestState.request.BeginGetResponse(RespCallback, myRequestState);
//            }
//            catch (Exception e)
//            {
//                myRequestState.HasError = true;
//                myRequestState.Error = e;
//                if (myRequestState.callBack != null)
//                    myRequestState.callBack(myRequestState);
//            }
//        }

//        private static int count = 0;
//        private static void RespCallback(IAsyncResult asynchronousResult)
//        {
//            // State of request is asynchronous.

//            var myRequestState = (RequestState) asynchronousResult.AsyncState;
//            try
//            {
//                var myHttpWebRequest = myRequestState.request;
//                myRequestState.response = (HttpWebResponse) myHttpWebRequest.EndGetResponse(asynchronousResult);
                
//                // Read the response into a Stream object.
//                var responseStream = myRequestState.response.GetResponseStream();

//                myRequestState.streamResponse = responseStream;
//                if (responseStream == null)
//                    return;
//                // Begin the Reading of the contents of the HTML page and print it to the console.
//                //IAsyncResult asynchronousInputRead = responseStream.BeginRead(myRequestState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);

//                using (var streamReader = new StreamReader(responseStream, Encoding.UTF8))
//                {
//                    myRequestState.requestData = streamReader.ReadToEnd();

//                    if (myRequestState.callBack != null)
//                        myRequestState.callBack(myRequestState);
//                    myRequestState.request.Abort();
//                }
//            }
//            catch (WebException web)
//            {
//                myRequestState.Error = web;
//                ReRequest(myRequestState);
//                //Logger.Error("\r\n\r\n重发\r\n" + myRequestState.request.RequestUri + "\r\n请求" + web);
//            }
//            catch (IOException ioe)
//            {
//                myRequestState.Error = ioe;
//                ReRequest(myRequestState);
//                //Logger.Error("\r\n\r\n重发\r\n" + myRequestState.request.RequestUri + "\r\n请求" + ioe);
//            }
//            catch (Exception e)
//            {
//                //Logger.Error("\r\n\r\n" + myRequestState.request.RequestUri + "\r\n" + e);
//            }
//            finally
//            {
//                if (myRequestState.streamResponse != null) myRequestState.streamResponse.Dispose();
//                if (myRequestState.response != null) myRequestState.response.Dispose();
//            }
//        }

//        /// <summary>
//        /// 重发请求
//        /// </summary>
//        /// <param name="myRequestState"></param>
//        private static void ReRequest(RequestState myRequestState)
//        {
//            if (myRequestState.tryCount == myRequestState.maxTryCount)
//            {
                
//                myRequestState.HasError = true;
//                if (myRequestState.callBack != null)
//                    myRequestState.callBack(myRequestState);
//                return;
//            }

//            myRequestState.tryCount++;
//            BeginGetHtml(myRequestState);
//        }

//        #endregion

//    }
//}