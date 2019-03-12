using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Java.Interop;
using WTONewProject.Droid.Renderer;
using WTONewProject.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WebView = Xamarin.Forms.WebView;

[assembly: ExportRenderer(typeof(HyBridWebView), typeof(HybridWebViewRenderer))]
namespace WTONewProject.Droid.Renderer
{
    public class HybridWebViewRenderer: WebViewRenderer
    {
        const string JavaScriptFunction = "function invokeCSharpAction(data){jsBridge.invokeAction(data);}";

        public HyBridWebView webView;

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("jsBridge");
            }
            if (e.NewElement != null)
            {
                webView = e.NewElement as HybridWebView;

                Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
                InjectJS(JavaScriptFunction);
            }

            void InjectJS(string script)
            {
                if (Control != null)
                {
                    Control.LoadUrl(string.Format("javascript: {0}", script));
                }
            }



        }

        public class JSBridge : Java.Lang.Object
        {
            readonly WeakReference<HybridWebViewRenderer> hybridWebViewRenderer;

            public JSBridge(HybridWebViewRenderer hybridRenderer)
            {
                hybridWebViewRenderer = new WeakReference<HybridWebViewRenderer>(hybridRenderer);
            }

            [JavascriptInterface]
            [Export("invokeAction")]
            public void InvokeAction(string data)
            {
                HybridWebViewRenderer hybridRenderer;

                if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
                {
                    hybridRenderer.webView.SendClick(data);
                }
            }
        }



    }
