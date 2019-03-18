using System;
using System.Collections.Generic;
using System.IO;
using CoreGraphics;
using Foundation;
using WebKit;
using WTONewProject.iOS.Renderer;
using WTONewProject.Renderer;
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
           const string rename1= "function ZTHTestParameteroneAndParametertwo(data){window.webkit.messageHandlers.invokeAction.postMessage(data);}";
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
                 userController.AddScriptMessageHandler(this, "filstClick");
                userController.AddScriptMessageHandler(this, "secondClick");
                userController.AddScriptMessageHandler(this, "invokeAction");

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
                Control.LoadRequest(new NSUrlRequest(new NSUrl("http://www.baidu.com")));
                //加载本地必须用下面的
                //NSUrl url = NSBundle.MainBundle.GetUrlForResource("index.html", "");
                //Control.LoadRequest(new NSUrlRequest(url));
            }
            Element.pushCode += Element_PushCode;

        }

        void Element_PushCode(object sender, EventArgs e)
        {
            Console.WriteLine("点击调用了");
            webView.EvaluateJavaScript("pushCode('你好','不好')",HandleWKJavascriptEvaluationResult);
        }
        //结果
        void HandleWKJavascriptEvaluationResult(NSObject result, NSError error)
        {



        }
        

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {


            if (message.Name.Equals("filstClick"))
            {
                Console.WriteLine("第一个按钮");
                //Element.clickOne(message)
            }
            if (message.Name.Equals("secondClick"))
            {
                Console.WriteLine("第2个按钮");
                NSDictionary dic = (NSDictionary)message.Body;
                Dictionary<string, object> sender = new Dictionary<string, object>();
                foreach (var key in dic.Keys)
                {
                    sender.Add(key.ToString(), dic[key]);
                }
                Element.clickOne(sender);
            }
            if (message.Name.Equals("invokeAction"))
            {

            }


        }



    }
}