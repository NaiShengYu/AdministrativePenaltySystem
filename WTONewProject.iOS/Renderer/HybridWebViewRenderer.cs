using Foundation;
using System;
using UIKit;
using WebKit;
using WTONewProject.iOS.Renderer;
using WTONewProject.Renderer;
using WTONewProject.Tools;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(HyBridWebView), typeof(HybridWebViewRenderer))]
namespace WTONewProject.iOS.Renderer
{
    public class HybridWebViewRenderer : ViewRenderer<HyBridWebView, WKWebView>, IWKScriptMessageHandler
    {
        HyBridWebView hyBridWebView; WKWebView _webView; WKUserContentController userController;
        /// <summary>
        /// ⽬目前不不⽤用此⽅方法实现js被调⽤用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<HyBridWebView> e)
        {
            //给JS⽅方法重命名(多参数需要放在⼀一个字典⾥里里⾯面)
            const string rename1 = "function getLocation(data){window.webkit.messageHandlers.getLocation.postMessage(data);}";
            const string rename2 = "function logOut(data){window.webkit.messageHandlers.logOut.postMessage(data);}";
            //const string rename3 = "function console(data){window.webkit.messageHandlers.console.postMessage(data);}"; base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
                userController.RemoveScriptMessageHandler("logOut");
            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    userController = new WKUserContentController();
                    //var script = new WKUserScript(new NSString(rename2), WKUserScriptInjectionTime.AtDocumentEnd, false); 
                    //userController.AddUserScript(script);
                    userController.AddScriptMessageHandler(this, "logOut");
                    //var script1 = new WKUserScript(new NSString(rename1), WKUserScriptInjectionTime.AtDocumentEnd, false); 
                    //userController.AddUserScript(script1);
                    userController.AddScriptMessageHandler(this, "getLocation");
                    var cookiestring = new NSString("document.cookie = 'AzuraCookie=" + Element.AzuraCookie + ";userId="+Element.userid+ "'");
                    Console.WriteLine("cookie===" + cookiestring);
                    var cookieScript = new WKUserScript(cookiestring, WKUserScriptInjectionTime.AtDocumentStart, false); userController.AddUserScript(cookieScript);
                    var config = new WKWebViewConfiguration { UserContentController = userController };
                    var webView = new WKWebView(Frame, config);
                    _webView = webView;
                    SetNativeControl(webView);
                }
                hyBridWebView = e.NewElement as HyBridWebView;
                UrlWebViewSource source = e.NewElement.Source as UrlWebViewSource; NSMutableUrlRequest request = new NSMutableUrlRequest(new NSUrl(source.Url)); NSMutableDictionary headers = new NSMutableDictionary();
                headers.Add(new NSString("AzuraCookie"), new NSString(Element.AzuraCookie));
                headers.Add(new NSString("userId"), new NSString(Element.userid));
                request.Headers = headers;
                Control.LoadRequest(new NSUrlRequest(new NSUrl(source.Url)));
            }
        }
        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            if (message.Name == "logOut")
            {
                Console.WriteLine("退出登录");
                hyBridWebView.logOut();
            }
            else if (message.Name == "getLocation")
            {
                Console.WriteLine("获取经纬度");
                setlocation();
            }

        }
        async void setlocation()
        {
            var currentLocation = await Geolocation.GetLastKnownLocationAsync(); if (currentLocation == null)
            {
                currentLocation = new Location(34.754626, 113.735763);
            }
            string js = "setLocation('" + currentLocation.Latitude + "','" + currentLocation.Longitude + "')";
            _webView.EvaluateJavaScript(new NSString(js), (result, error) => { });
        }
    }
}
