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

[assembly: ExportRenderer(typeof(HyBridWebView), typeof(HybridWebViewRenderer))]
namespace WTONewProject.Droid.Renderer
{
    public class HybridWebViewRenderer : ViewRenderer<HyBridWebView, Android.Webkit.WebView>
    {
        const string JavaScriptFunction = "function ZTHTestParameteroneAndParametertwo(data){jsBridge.invokeAction(data);}";
        Context _context;

        public HybridWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<HyBridWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var webView = new Android.Webkit.WebView(_context);
                webView.Settings.JavaScriptEnabled = true;
                webView.SetWebViewClient(new JavascriptWebViewClient($"javascript: {JavaScriptFunction}"));
                SetNativeControl(webView);
            }
            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("jsBridge");
                var hybridWebView = e.OldElement as HyBridWebView;
            }
            if (e.NewElement != null)
            {
                Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
                UrlWebViewSource source = e.NewElement.Source as UrlWebViewSource;
                Control.LoadUrl(source.Url);
            }
        }

   
       
    }

    public class JavascriptWebViewClient : WebViewClient
    {
        string _javascript;

        public JavascriptWebViewClient(string javascript)
        {
            _javascript = javascript;
        }

        public override void OnPageFinished(Android.Webkit.WebView view, string url)
        {
            base.OnPageFinished(view, url);
            view.EvaluateJavascript(_javascript,null);
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
        [Export("invokeAction")]//有这句话编译不通过
        public void InvokeAction(object data)
        {
            HybridWebViewRenderer hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                hybridRenderer.Element.CCallJs(data);
            }
        }
    }

}
