using Android.Content;
using Android.Net.Http;
using Android.Webkit;
using Java.Interop;
using System;
using WTONewProject.Droid.Renderer;
using WTONewProject.Droid.Tools;
using WTONewProject.Renderer;
using WTONewProject.Tools;
using WTONewProject.View;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Webkit.WebSettings;

[assembly: ExportRenderer(typeof(HyBridWebView), typeof(HybridWebViewRenderer))]
namespace WTONewProject.Droid.Renderer
{
    public class HybridWebViewRenderer : WebViewRenderer
    {
        const string JavaScriptUserId = "function UserId(data){jsBridge.UserId(data);}";

        Context _context;
        Android.Webkit.WebView androidWebView;


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

            if (e.OldElement == null)
            {
               
                setSettings(Control);
                Control.SetWebViewClient(new JavascriptWebViewClient($"javascript: {JavaScriptUserId}",
                    BridWebView,
                    this));
                //Control.SetWebChromeClient(new HybridWebChromeClient(_context, BridWebView));
            }
            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("jsBridge");
                var hybridWebView = e.OldElement as HyBridWebView;
            }
            if (e.NewElement != null)
            {
                Control.AddJavascriptInterface(new JSBridge(_context, this, BridWebView, androidWebView), "jsBridge");
                UrlWebViewSource source = e.NewElement.Source as UrlWebViewSource;
                Control.LoadUrl(source.Url);
            }
        }

        //配置webview
        private void setSettings(Android.Webkit.WebView webView)
        {
            androidWebView = webView;
            webView.SetLayerType(Android.Views.LayerType.Hardware, new Android.Graphics.Paint());
            WebSettings set = webView.Settings;
            set.JavaScriptEnabled = true;
            set.JavaScriptCanOpenWindowsAutomatically = true;
            set.SetSupportZoom(true);
            set.UseWideViewPort = true;
            set.SetPluginState(PluginState.On);
            set.PluginsEnabled = true;
            set.AllowFileAccess = true;
            set.CacheMode = CacheModes.NoCache;
            set.SetLayoutAlgorithm(LayoutAlgorithm.SingleColumn);
            set.LoadWithOverviewMode = true;
            set.BuiltInZoomControls = true;
            set.SetAppCacheEnabled(false);
            set.SetGeolocationEnabled(true);
            set.DomStorageEnabled = true;
            set.MixedContentMode = MixedContentHandling.AlwaysAllow;
            set.MediaPlaybackRequiresUserGesture = false;
        }


    }


    public class JavascriptWebViewClient : WebViewClient
    {
        string _javascript;
        HyBridWebView _hyBridWebView;
        WebViewRenderer _viewRenderer;

        public JavascriptWebViewClient(string javascript, HyBridWebView hyBridWebView, WebViewRenderer viewRenderer)
        {
            _javascript = javascript;
            _hyBridWebView = hyBridWebView;
            _viewRenderer = viewRenderer;
        }

        public override void OnPageFinished(Android.Webkit.WebView view, string url)
        {
            if (_viewRenderer == null || _viewRenderer.Element == null) return;
            ((IWebViewController)_viewRenderer.Element).CanGoBack = view.CanGoBack();
            ((IWebViewController)_viewRenderer.Element).CanGoForward = view.CanGoForward();
            base.OnPageFinished(view, url);
            view.EvaluateJavascript(_javascript, null);
        }

        public override void OnReceivedSslError(Android.Webkit.WebView view, SslErrorHandler handler, SslError error)
        {
            handler.Proceed();
        }
        
    }

    public class JSBridge : Java.Lang.Object
    {
        Context _context;
        readonly WeakReference<HybridWebViewRenderer> _hybridWebViewRenderer;
        WeakReference _hyBridWeb;
        WeakReference _androidWeb;

        public JSBridge(Context context, HybridWebViewRenderer hybridRenderer, HyBridWebView hyBridWeb, Android.Webkit.WebView webView)
        {
            _context = context;
            _hybridWebViewRenderer = new WeakReference<HybridWebViewRenderer>(hybridRenderer);
            _hyBridWeb = new WeakReference(hyBridWeb);
            _androidWeb = new WeakReference(webView);
        }

        [Export("UserId")]
        [JavascriptInterface]
        public void UserId(string data)
        {
            name = name.Replace("-", "");
            JPushInterface.SetAlias(Application.Context, 0, name);
        }

    }

}
