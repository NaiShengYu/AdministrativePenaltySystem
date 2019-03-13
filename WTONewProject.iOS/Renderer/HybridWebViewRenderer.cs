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
        protected override void OnElementChanged(ElementChangedEventArgs<HyBridWebView> e)
        {
            //给JS方法重命名(多参数需要放在一个字典里面)
           const string rename1= "function ZTHTestParameteroneAndParametertwo(data){window.webkit.messageHandlers.invokeAction.postMessage(data);}";
            base.OnElementChanged(e);
            if (Control == null)
            {
               var userController = new WKUserContentController();
                userController.AddUserScript(new WKUserScript(new NSString(rename1), WKUserScriptInjectionTime.AtDocumentEnd, false));
                 userController.AddScriptMessageHandler(this, "filstClick");
                userController.AddScriptMessageHandler(this, "secondClick");
                userController.AddScriptMessageHandler(this, "invokeAction");

                var config = new WKWebViewConfiguration { UserContentController = userController };
                webView = new WKWebView(Frame, config);
                webView.UIDelegate = this;
                webView.NavigationDelegate = this;
                SetNativeControl(webView);
            }

            if (e.NewElement != null)
            {
                UrlWebViewSource source = e.NewElement.Source as UrlWebViewSource;
                Control.LoadRequest(new NSUrlRequest(new NSUrl(source.Url, false)));
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

        }



    }
}