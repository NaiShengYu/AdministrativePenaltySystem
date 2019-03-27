using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

[assembly: ExportRenderer (typeof(HyBridWebView), typeof(HybridWebViewRenderer))]
namespace WTONewProject.iOS.Renderer
{
    public class HybridWebViewRenderer : ViewRenderer<HyBridWebView, WKWebView>, IWKScriptMessageHandler,IWKNavigationDelegate,IWKUIDelegate
    {
        
        WKWebView webView;
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
               userController = new WKUserContentController();
                //给webView添加Cookie
                if (!string.IsNullOrWhiteSpace(Element.AzuraCookie))
                {
                    NSString cookieSource = new NSString("document.cookie ='AzuraCookie=" + Element.AzuraCookie + "';");
                    WKUserScript cookieScript = new WKUserScript(cookieSource, WKUserScriptInjectionTime.AtDocumentStart, false);
                    userController.AddUserScript(cookieScript);
                }

                userController.AddUserScript(new WKUserScript(new NSString(rename1), WKUserScriptInjectionTime.AtDocumentEnd, false));
                userController.AddUserScript(new WKUserScript(new NSString(rename2), WKUserScriptInjectionTime.AtDocumentEnd, false));
                //userController.AddUserScript(new WKUserScript(new NSString(rename3), WKUserScriptInjectionTime.AtDocumentEnd, false));
                userController.AddScriptMessageHandler(this, "getLocation");
                userController.AddScriptMessageHandler(this, "logOut");
                userController.AddScriptMessageHandler(this, "secondClick");
                //userController.AddScriptMessageHandler(this, "console");


                var config = new WKWebViewConfiguration { UserContentController = userController };
                webView = new WKWebView(Frame, config);
                webView.ScrollView.Bounces = false;//禁止超过边框滑动
                webView.UIDelegate = this;
                webView.NavigationDelegate = this;
                SetNativeControl(webView);
            }
            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
            }
            if (e.NewElement != null)
            {
                UrlWebViewSource source = e.NewElement.Source as UrlWebViewSource;
                Control.LoadRequest(new NSUrlRequest(new NSUrl(source.Url)));
                //加载本地必须用下面的
                //NSUrl url = NSBundle.MainBundle.GetUrlForResource("index.html", "");
                //Control.LoadRequest(new NSUrlRequest(url));
            }

        }

        public async void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {

            Console.WriteLine(message.Body);
            //获取经纬度
            if (message.Name.Equals("getLocation"))
            {
                Console.WriteLine(message.Body);
               var currentLocation = await Geolocation.GetLastKnownLocationAsync();
                if (currentLocation == null)
                {
                    currentLocation = new Location(34.754626, 113.735763);
                }
                string js = "setLocation('" + currentLocation.Latitude + "','" + currentLocation.Longitude + "')";
                webView.EvaluateJavaScript(js,(NSObject result, NSError error) => { 
                
                });
            }
            if (message.Name.Equals("logOut"))
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).LastOrDefault();
                var account1 = AccountStore.Create().FindAccountsForService(App.siteURL).LastOrDefault();

                if (account1 != null)
                    App.FrameworkURL = account1.Username;
                if (account == null)
                    App.Current.MainPage = new LoginWithNullPage();
                else
                {
                    App.Current.MainPage = new LoginWithNullPage(account.Username, account.Properties["pwd"]);
                    App.userName = account.Username;
                    App.pwd = account.Properties["pwd"];
                }
            }
            if (message.Name.Equals("invokeAction"))
            {

            }


        }



    }
}