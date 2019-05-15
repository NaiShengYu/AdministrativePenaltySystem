using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CoreGraphics;
using Foundation;
using WebKit;
using WTONewProject.iOS.Renderer;
using WTONewProject.Renderer;
using WTONewProject.View;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using JavaScriptCore;
using System.Threading.Tasks;
using WTONewProject.Tools;

[assembly: ExportRenderer(typeof(HyBridWebView), typeof(HybridWebViewRenderer))]
namespace WTONewProject.iOS.Renderer
{
    public class HybridWebViewRenderer : ViewRenderer<HyBridWebView, UIWebView>, IUIWebViewDelegate
    {
        HyBridWebView hyBridWebView;
        UIWebView _webView;
        WKUserContentController userController;
        protected override void OnElementChanged(ElementChangedEventArgs<HyBridWebView> e)
        {
            //给JS方法重命名(多参数需要放在一个字典里面)
            const string rename1 = "function getLocation(data){window.webkit.messageHandlers.getLocation.postMessage(data);}";
            const string rename2 = "function logOut(data){window.webkit.messageHandlers.logOut.postMessage(data);}";
            //const string rename3 = "function console(data){window.webkit.messageHandlers.console.postMessage(data);}";
            base.OnElementChanged(e);
            if (Control == null)
            {
                _webView = new UIWebView(Frame);
                _webView.ShouldStartLoad += WebView_ShouldStartLoad;
                _webView.LoadError += (object sender, UIWebErrorArgs error) => {
                    hyBridWebView.logOut();
                };
                _webView.SuppressesIncrementalRendering = true;
                try
                {
                    _webView.ScrollView.ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never;
                }
                catch (Exception ex)
                {

                }
                _webView.ScrollView.Bounces = false;
                SetNativeControl(_webView);
            }
            if (e.OldElement != null)
            {
                //userController.RemoveAllUserScripts();
            }
            if (e.NewElement != null)
            {
                hyBridWebView = e.NewElement as HyBridWebView;
                UrlWebViewSource source = e.NewElement.Source as UrlWebViewSource;
                NSMutableUrlRequest request = new NSMutableUrlRequest(new NSUrl(source.Url));
                Control.LoadRequest(new NSUrlRequest(new NSUrl(source.Url)));
                //加载本地必须用下面的
                //NSUrl url = NSBundle.MainBundle.GetUrlForResource("index.html", "");
                //Control.LoadRequest(new NSUrlRequest(url));
            }
        }

        bool WebView_ShouldStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
        {

            string url = request.Url.AbsoluteString;
            Console.WriteLine("url=" + url);

            if (url.Contains("ios://") == true)
            {
                if (url.Contains("logOut") == true)
                {
                    Console.WriteLine("退出登录");
                    hyBridWebView.logOut();
                }
                if (url.Contains("getLocation") == true)
                {
                    setlocation();
                }
                return false;
            }

            NSHttpCookieStorage cookieStorage = NSHttpCookieStorage.SharedStorage;
            cookieStorage.AcceptPolicy = NSHttpCookieAcceptPolicy.Always;
            foreach (NSHttpCookie cookieItem in cookieStorage.Cookies)
            {
                cookieStorage.DeleteCookie(cookieItem);
            }
            NSMutableDictionary cookieProperties = new NSMutableDictionary();
            cookieProperties.Add(NSHttpCookie.KeyName, (NSString)"accessToken");
            string cooki = "Bearer " + Element.AzuraCookie;
            cookieProperties.Add(NSHttpCookie.KeyValue, (NSString)Element.AzuraCookie);
            cookieProperties.Add(NSHttpCookie.KeyOriginUrl, (NSString)Constants.WEB_SOURCE);
            cookieProperties.Add(NSHttpCookie.KeyPath, (NSString)"/");
            cookieProperties.Add(NSHttpCookie.KeyExpires, new NSDate().AddSeconds(30 * 24 * 3600));
            NSHttpCookie httpCookie = new NSHttpCookie(cookieProperties);
            cookieStorage.SetCookie(httpCookie);
            return true;
        }

        async void setlocation()
        {
            var currentLocation = await Geolocation.GetLastKnownLocationAsync();
            if (currentLocation == null)
            {
                currentLocation = new Location(34.754626, 113.735763);
            }
            string js = "setLocation('" + currentLocation.Latitude + "','" + currentLocation.Longitude + "')";
            _webView.EvaluateJavascript(js);
        }
    }



}