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
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HyBridWebView), typeof(HybridWebViewRenderer))]
namespace WTONewProject.Droid.Renderer
{
    public class HybridWebViewRenderer : ViewRenderer<HyBridWebView, Android.Webkit.WebView>
    {
        const string JavaScriptFunction = "function getLngLat(){jsBridge.getLngLat();}";
        const string JavaScriptFunction1 = "function secondClick(data,data2){jsBridge.secondClick(data,data2);}";
        Context _context;
        Android.Webkit.WebView webView;
        public HybridWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<HyBridWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                webView = new Android.Webkit.WebView(_context);
                webView.Settings.JavaScriptEnabled = true;
                //多个脚本在后面{name}{name}
                webView.SetWebViewClient(new JavascriptWebViewClient($"javascript: {JavaScriptFunction}{JavaScriptFunction1}"));
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
                if(!string.IsNullOrWhiteSpace(Element.AzuraCookie))
                    synCookies(_context, source.Url, "AzuraCookie=" + Element.AzuraCookie + "");
                Control.LoadUrl(source.Url);
            }
        }

        //设置Cookie
        public void synCookies(Context context, String url, String cookie)
        {
            CookieSyncManager.CreateInstance(context);
            CookieManager cookieManager = CookieManager.Instance;
            cookieManager.SetAcceptCookie(true);
            cookieManager.SetCookie(url, cookie);//cookies是在HttpClient中获得的cookie
            CookieSyncManager.Instance.Sync();
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

        public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, string url)
        {
            view.LoadUrl(url);
            return base.ShouldOverrideUrlLoading(view, url);
        }

        //public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, IWebResourceRequest request)
        //{
        //    view.LoadUrl(request.Url.ToString());
        //    return true;
        //}
    }


    public class JSBridge : Java.Lang.Object
    {
        readonly WeakReference<HybridWebViewRenderer> hybridWebViewRenderer;

        public JSBridge(HybridWebViewRenderer hybridRenderer)
        {
            hybridWebViewRenderer = new WeakReference<HybridWebViewRenderer>(hybridRenderer);
        }

        [JavascriptInterface]
        [Export("getLngLat")]
        public async void getLngLat(string data ,string data2)
        {
            HybridWebViewRenderer hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                var currentLocation = await Geolocation.GetLastKnownLocationAsync();
                if (currentLocation == null)
                {
                    currentLocation = new Location(34.754626, 113.735763);
                }

            }
        }

        [JavascriptInterface]
        [Export("secondClick")]
        public void clickTow(string data,string data2)
        {


        }



    }

}
