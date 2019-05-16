using Android.Content;
using Android.Net.Http;
using Android.Webkit;
using Java.Interop;
using System;
using WTONewProject.Droid.Renderer;
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
        const string JavaScriptGetLocation = "function getLocation(data){jsBridge.getLocation(data);}";
        const string JavaScriptLogOut = "function logOut(data){jsBridge.logOut(data);}";
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
                if (!string.IsNullOrWhiteSpace(WebPage._cookie))
                {
                    synCookies(_context, Constants.WEB_SOURCE, "accessToken=" + WebPage._cookie + ";");
                }
                if (App.tokenModel != null && !string.IsNullOrWhiteSpace(App.tokenModel.userId))
                {
                    synCookies(_context, Constants.WEB_SOURCE, "userId=" + App.tokenModel.userId + ";");
                }
                setSettings(Control);
                Control.SetWebViewClient(new JavascriptWebViewClient($"javascript: {JavaScriptGetLocation}{JavaScriptLogOut}", BridWebView, this));
            }
            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("jsBridge");
                var hybridWebView = e.OldElement as HyBridWebView;
            }
            if (e.NewElement != null)
            {
                Control.AddJavascriptInterface(new JSBridge(this, BridWebView, androidWebView), "jsBridge");
                UrlWebViewSource source = e.NewElement.Source as UrlWebViewSource;
                Control.LoadUrl(source.Url);
            }
        }

        //配置webview
        private void setSettings(Android.Webkit.WebView webView)
        {
            androidWebView = webView;
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
            //cookieManager.RemoveAllCookie();
            cookieManager.SetAcceptCookie(true);
            cookieManager.SetCookie(url, cookie);
            CookieSyncManager.Instance.Sync();
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
        readonly WeakReference<HybridWebViewRenderer> _hybridWebViewRenderer;
        WeakReference _hyBridWeb;
        WeakReference _androidWeb;

        public JSBridge(HybridWebViewRenderer hybridRenderer, HyBridWebView hyBridWeb, Android.Webkit.WebView webView)
        {
            _hybridWebViewRenderer = new WeakReference<HybridWebViewRenderer>(hybridRenderer);
            _hyBridWeb = new WeakReference(hyBridWeb);
            _androidWeb = new WeakReference(webView);
        }

        [Export("getLocation")]
        [JavascriptInterface]
        public async void getLocation(string data)
        {
            Android.Webkit.WebView androidWeb;
            if (_androidWeb != null && _androidWeb.IsAlive)
            {
                androidWeb = _androidWeb.Target as Android.Webkit.WebView;
                Location currentLocation;
                if (Device.RuntimePlatform == Device.iOS)
                {
                    currentLocation = await Geolocation.GetLastKnownLocationAsync();
                }
                else
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                    currentLocation = await Geolocation.GetLocationAsync(request);
                }
                if (currentLocation == null)
                {
                    currentLocation = new Location(34.754626, 113.735763);
                }
                Console.WriteLine("=== android location success: lat=" + currentLocation.Latitude + " lng=" + currentLocation.Longitude);
                string js = "setLocation('" + currentLocation.Latitude + "','" + currentLocation.Longitude + "')";
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        if (androidWeb != null && androidWeb.IsAttachedToWindow)
                        {
                            androidWeb.EvaluateJavascript(js, null);
                        }
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }

        [Export("logOut")]
        [JavascriptInterface]
        public void logOut(string data)
        {
            HyBridWebView hyBridWeb;

            if (_hyBridWeb != null && _hyBridWeb.IsAlive)
            {
                hyBridWeb = _hyBridWeb.Target as HyBridWebView;
                if (hyBridWeb != null)
                {
                    hyBridWeb.logOut();
                }
            }
        }

    }

}
