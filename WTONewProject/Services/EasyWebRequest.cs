using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Plugin.Hud;

namespace WTONewProject.Services
{

    public class HTTPResponse
    {
        public string Results { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
    public class EasyWebRequest
    {

        public static async Task<HTTPResponse> SendHTTPRequestAsync(string url, string param, string method = "GET", string token = null, string contenttype = "json")
        {
            CrossHud.Current.Show("请求中...");
            HttpWebResponse res = null;
            string result = null;
            try
            {
                Console.WriteLine("请求URL：" + url);
                Console.WriteLine("请求token：" + "Bearer " + token);
                Console.WriteLine("请求参数：" + param);
                ServicePointManager.ServerCertificateValidationCallback = MyCertHandler;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    req.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);//给请求添加权限
                }
                req.ContentType = "application/json";
                if (method.Equals("GET"))
                {
                    req.Method = "GET";
                }
                else
                {
                    byte[] bs = Encoding.UTF8.GetBytes(param);

                    req.Method = method;

                    if (contenttype == "json")
                    {
                        req.ContentType = "application/json";
                    }
                    else
                    {
                        req.ContentType = "application/x-www-form-urlencoded";
                    }

                    req.ContentLength = bs.Length;
                    try
                    {
                        Stream requestStream = await req.GetRequestStreamAsync();
                        await requestStream.WriteAsync(bs, 0, bs.Length);
                        requestStream.Close();
                    }
                    catch (WebException ex)
                    {
                    }
                }
                WebResponse wr = await req.GetResponseAsync();
                res = wr as HttpWebResponse;
                StreamReader sr = new StreamReader(res.GetResponseStream());
                result = await sr.ReadToEndAsync();
                sr.Close();
            }
            catch (Exception ex)
            {
                result = ex == null ? "" : ex.Message;
                Console.WriteLine("错误信息：====" + ex + "错误的URL" + url);
                return new HTTPResponse { Results = result, StatusCode = HttpStatusCode.ExpectationFailed };
            }
            Console.WriteLine(url + "===ex:" + result);
            CrossHud.Current.Dismiss();
            return new HTTPResponse { Results = result, StatusCode = res.StatusCode };
        }


        static bool MyCertHandler(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors error)
        {
            Console.WriteLine("hi");
            // Ignore errors
            return true;
        }

    }
}
