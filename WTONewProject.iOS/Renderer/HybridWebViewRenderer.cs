using Foundation;
using JPush.Binding.iOS;
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
            const string rename1 = "function UserId(data){window.webkit.messageHandlers.UserId.postMessage(data);}";
            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
                userController.RemoveScriptMessageHandler("UserId");
            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    userController = new WKUserContentController();
                    userController.AddScriptMessageHandler(this, "UserId");
                    var config = new WKWebViewConfiguration { UserContentController = userController };
                    var webView = new WKWebView(Frame, config);
                    _webView = webView;
                    SetNativeControl(webView);
                }
                hyBridWebView = e.NewElement as HyBridWebView;
                UrlWebViewSource source = e.NewElement.Source as UrlWebViewSource; 
                NSMutableUrlRequest request = new NSMutableUrlRequest(new NSUrl(source.Url)); 
                Control.LoadRequest(new NSUrlRequest(new NSUrl(source.Url)));
            }
        }
        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            if (message.Name == "UserId")
            {
                Console.WriteLine("用户id"+message.Body);
                string alias = message.Body.ToString().Replace("-", "").ToLower();
                if (string.IsNullOrWhiteSpace(alias))
                {
                    JPUSHService.DeleteAlias((arg0, arg1, arg2) => { },1);
                }
                else {
                    JPUSHService.SetAlias(alias, (arg0, arg1, arg2) => {
                    }, 1);
                }
               

            }
        }

    }
}
