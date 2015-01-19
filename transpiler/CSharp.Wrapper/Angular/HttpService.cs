using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Wrapper.Angular
{
    public class HttpService
    {
        public HttpPromise get(string url, dynamic RequestConfig = null) { return null; }
        public HttpPromise delete(string url, dynamic RequestConfig = null) { return null; }
        public HttpPromise head(string url, dynamic RequestConfig = null) { return null; }
        public HttpPromise jsonp(string url, dynamic RequestConfig = null) { return null; }
        public HttpPromise post(string url, dynamic data, dynamic RequestConfig = null) { return null; }
        public HttpPromise put(string url, dynamic data, dynamic RequestConfig = null) { return null; }
    }

    public class HttpPromise {
        public HttpPromise success(HttpCallback callbackFn) { return null; }
        public HttpPromise error(HttpCallback callbackFn) { return null; }
        public void then(HttpCallback successCallbackFn, HttpCallback errorCallbackFn){}
    }
    public class RequestConfig {
        public string method { get; set; }
        public string url { get; set; }
        public string paramss { get; set; }
        public string headers { get; set; }
        public string cache { get; set; }
        public bool withCredentials { get; set; }
        public dynamic data { get; set; }
        public dynamic transformRequest { get; set; }
        public dynamic transformResponse { get; set; }
        public dynamic timeout { get; set; }
    }
}
