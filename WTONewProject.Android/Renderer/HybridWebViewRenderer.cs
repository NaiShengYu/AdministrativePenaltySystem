using Android.Content;
using Android.Graphics;
using Android.Net.Http;
using Android.Webkit;
using Java.Interop;
using System;
using WTONewProject.Droid.Renderer;
using WTONewProject.Renderer;
using WTONewProject.View;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Webkit.WebSettings;

[assembly: ExportRenderer(typeof(HyBridWebView), typeof(HybridWebViewRenderer))]
namespace WTONewProject.Droid.Renderer
{
    public class HybridWebViewRenderer : WebViewRenderer
    {
        const string JavaScriptFunction = "function ZTHTestParameteroneAndParametertwo(data,data2){jsBridge.invokeAction(data,data2);}";
        const string JavaScriptFunction1 = "function secondClick(data,data2){jsBridge.secondClick(data,data2);}";
        Context _context;

        public HyBridWebView BridWebView
        {
            get { return Element as HyBridWebView; }
        }

        public HybridWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            //if (Control == null)
            //{
            //    var webView = new Android.Webkit.WebView(_context);
            //    WebSettings set = webView.Settings;
            //    set.JavaScriptEnabled = true;
            //    //多个脚本在后面{name}{name}
            //    webView.SetWebViewClient(new JavascriptWebViewClient($"javascript: {JavaScriptFunction}{JavaScriptFunction1}"));
            //    SetNativeControl(webView);
            //}
            //if (e.OldElement != null)
            //{
            //    //Control.RemoveJavascriptInterface("jsBridge");
            //    //var hybridWebView = e.OldElement as HyBridWebView;
            //}
            //if (e.NewElement != null)
            //{
            //    //Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
            //    UrlWebViewSource source = e.NewElement.Source as UrlWebViewSource;
            //    if (!string.IsNullOrWhiteSpace(Element.AzuraCookie))
            //    {
            //        //synCookies(_context, source.Url, "AzuraCookie=" + Element.AzuraCookie + "");
            //    }
            //    //Control.LoadUrl(source.Url);
            //}


            if (e.OldElement == null)
            {
                setSettings(Control);
                Control.SetWebViewClient(new JavascriptWebViewClient($"javascript: {JavaScriptFunction}{JavaScriptFunction1}", BridWebView));
            }
            if (e.NewElement != null)
            {
                Control.AddJavascriptInterface(new JSBridge(this, BridWebView), "jsBridge");
                UrlWebViewSource source = e.NewElement.Source as UrlWebViewSource;
                if (!string.IsNullOrWhiteSpace(WebPage._cookie))
                {
                    synCookies(_context, source.Url, "AzuraCookie=" + WebPage._cookie + "");
                }
                Control.LoadUrl(source.Url);
            }
        }

        //配置webview
        private void setSettings(Android.Webkit.WebView webView)
        {
            WebSettings set = webView.Settings;
            set.JavaScriptEnabled = true;
            set.JavaScriptCanOpenWindowsAutomatically = true;
            set.SetSupportZoom(true);
            set.BuiltInZoomControls = true;
            set.UseWideViewPort = true;
            set.CacheMode = CacheModes.Default;
            set.SetLayoutAlgorithm(LayoutAlgorithm.SingleColumn);
            set.LoadWithOverviewMode = true;
            set.SetAppCacheEnabled(true);
            set.SetGeolocationEnabled(true);
            set.DomStorageEnabled = true;
            set.MixedContentMode = MixedContentHandling.AlwaysAllow;
        }

        //设置Cookie
        public void synCookies(Context context, String url, String cookie)
        {
            CookieSyncManager.CreateInstance(context);
            CookieManager cookieManager = CookieManager.Instance;
            cookieManager.RemoveAllCookie();
            cookieManager.SetAcceptCookie(true);
            cookieManager.SetCookie(url, cookie);//cookies是在HttpClient中获得的cookie
            CookieSyncManager.Instance.Sync();
        }


    }

    public class JavascriptWebViewClient : WebViewClient
    {
        string _javascript;
        HyBridWebView _hyBridWebView;

        public JavascriptWebViewClient(string javascript, HyBridWebView hyBridWebView)
        {
            _javascript = javascript;
            _hyBridWebView = hyBridWebView;
        }

        public override void OnPageStarted(Android.Webkit.WebView view, string url, Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);
        }

        public override void OnPageFinished(Android.Webkit.WebView view, string url)
        {
            base.OnPageFinished(view, url);
            view.EvaluateJavascript(_javascript, null);
        }

        //public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, string url)
        //{
        //    view.LoadUrl(url);
        //    return true;
        //}

        //public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, IWebResourceRequest request)
        //{
        //    return false;
        //}

        public override void OnReceivedSslError(Android.Webkit.WebView view, SslErrorHandler handler, SslError error)
        {
            handler.Proceed();
        }
    }


    public class JSBridge : Java.Lang.Object
    {
        readonly WeakReference<HybridWebViewRenderer> hybridWebViewRenderer;
        HyBridWebView _hyBridWeb;

        public JSBridge(HybridWebViewRenderer hybridRenderer, HyBridWebView hyBridWeb)
        {
            hybridWebViewRenderer = new WeakReference<HybridWebViewRenderer>(hybridRenderer);
            _hyBridWeb = hyBridWeb;
        }

        [JavascriptInterface]
        [Export("invokeAction")]
        public void InvokeAction(string data, string data2)
        {
            HybridWebViewRenderer hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                _hyBridWeb.CCallJs(data);
            }
        }

        [JavascriptInterface]
        [Export("secondClick")]
        public void clickTow(string data, string data2)
        {
            HybridWebViewRenderer hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                _hyBridWeb.ReturnValue(data, data2);
            }
        }



    }

}
