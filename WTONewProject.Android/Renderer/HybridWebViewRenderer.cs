using Android.Content;
using Android.Net.Http;
using Android.Webkit;
using Java.Interop;
using Plugin.Media.Abstractions;
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
        const string JavaScriptGetLocation = "function getLocation(data){jsBridge.getLocation(data);}";
        const string JavaScriptLogOut = "function logOut(data){jsBridge.logOut(data);}";
        const string JavaScriptSms = "function sendSms(data){jsBridge.sendSms(data);}";
        const string JavaScriptPhone = "function phone(data){jsBridge.phone(data);}";
        const string JavaScriptTakeMedia = "function takeMedia(type){jsBridge.takeMedia(type);}";

        Context _context;
        Android.Webkit.WebView androidWebView;
        bool videoFlag = false;


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
                Control.SetWebViewClient(new JavascriptWebViewClient(
                    $"javascript: {JavaScriptGetLocation}{JavaScriptLogOut}{JavaScriptSms}{JavaScriptPhone}{JavaScriptTakeMedia}",
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
            webView.SetLayerType(Android.Views.LayerType.Software, null);
            WebSettings set = webView.Settings;
            set.JavaScriptEnabled = true;
            set.JavaScriptCanOpenWindowsAutomatically = true;
            set.SetSupportZoom(true);
            set.BuiltInZoomControls = true;
            set.UseWideViewPort = true;
            set.AllowFileAccess = true;
            set.CacheMode = CacheModes.NoCache;
            set.SetLayoutAlgorithm(LayoutAlgorithm.SingleColumn);
            set.LoadWithOverviewMode = true;
            set.SetAppCacheEnabled(true);
            set.SetGeolocationEnabled(true);
            set.DomStorageEnabled = true;
            set.MixedContentMode = MixedContentHandling.AlwaysAllow;
            set.SetPluginState(PluginState.On);
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

        public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, string url)
        {
            Console.WriteLine("===ShouldOverrideUrlLoading:" + url);
            view.LoadUrl(url);
            return true;
        }

        public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, IWebResourceRequest request)
        {
            if (request.Url != null)
            {
                Console.WriteLine("===ShouldOverrideUrlLoading2:" + request.Url.Path);
            }
            if (request.Url != null && !string.IsNullOrWhiteSpace(request.Url.Path) && request.Url.Path.Contains("video"))
            {
                _hyBridWebView._videoFlag = true;
            }
            else
            {
                _hyBridWebView._videoFlag = false;
            }
            //view.LoadUrl(request.Url.Path);
            return base.ShouldOverrideUrlLoading(view, request);
        }
    }

    public class HybridWebChromeClient : WebChromeClient
    {
        private IValueCallback fileCallback;
        HyBridWebView _hyBridWebView;
        Context _context;

        public HybridWebChromeClient(Context context, HyBridWebView hyBridWebView)
        {
            _hyBridWebView = hyBridWebView;
            _context = context;
        }


        [Android.Runtime.Register("onShowFileChooser", "(Landroid/webkit/WebView;Landroid/webkit/ValueCallback;Landroid/webkit/WebChromeClient$FileChooserParams;)Z", "GetOnShowFileChooser_Landroid_webkit_WebView_Landroid_webkit_ValueCallback_Landroid_webkit_WebChromeClient_FileChooserParams_Handler")]
        public override bool OnShowFileChooser(Android.Webkit.WebView webView, IValueCallback filePathCallback, FileChooserParams fileChooserParams)
        {
            fileCallback = filePathCallback;

            if (_hyBridWebView._videoFlag)
            {
                recordVideo();
            }
            else
            {
                takePhoto(_context);
            }
            //return base.OnShowFileChooser(webView, filePathCallback, fileChooserParams);
            return true;
        }

        /// <summary>
        /// 拍照
        /// </summary>
        private async void takePhoto(Context _context)
        {
            MediaFile file = null;
            if (fileCallback != null && file != null)
            {
                Android.Net.Uri[] uris = new Android.Net.Uri[1];
                Android.Net.Uri u = AndroidFileUtils.GetImageContentUri(_context, file.Path);
                uris[0] = u;
                fileCallback.OnReceiveValue(uris);
                //fileCallback = null;
                file.Dispose();
            }


        }

        /// <summary>
        /// 拍视频
        /// </summary>
        private void recordVideo()
        {

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

        [Export("getLocation")]
        [JavascriptInterface]
        public async void getLocation(string data)
        {
            Xamarin.Essentials.Location currentLocation;
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
                currentLocation = new Xamarin.Essentials.Location(34.754626, 113.735763);
            }
            Console.WriteLine("=== android location success: lat=" + currentLocation.Latitude + " lng=" + currentLocation.Longitude);
            string js = "setLocation('" + currentLocation.Latitude + "','" + currentLocation.Longitude + "')";
            callWebJs(js);
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

        [Export("sendSms")]
        [JavascriptInterface]
        public void sendSms(string data)
        {
            HyBridWebView hyBridWeb;

            if (_hyBridWeb != null && _hyBridWeb.IsAlive)
            {
                hyBridWeb = _hyBridWeb.Target as HyBridWebView;
                if (hyBridWeb != null)
                {
                    hyBridWeb.sendSms(data);
                }
            }
        }


        [Export("phone")]
        [JavascriptInterface]
        public void phone(string data)
        {
            HyBridWebView hyBridWeb;

            if (_hyBridWeb != null && _hyBridWeb.IsAlive)
            {
                hyBridWeb = _hyBridWeb.Target as HyBridWebView;
                if (hyBridWeb != null)
                {
                    hyBridWeb.phone(data);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">0:拍照  1:拍视频</param>
        [Export("takeMedia")]
        [JavascriptInterface]
        public async void takeMedia(string type)
        {
            Java.IO.File file = null;
            if (type == "0")
            {
                file = await new MediaUtil().TakePhoto(false, 50);
                sendMediaData(file, "img");
            }
            else if (type == "1")
            {
                MediaUtil u = new MediaUtil();
                u.TakeVideo();
                u.SaveVideoPath += async (arg1, arg2) =>
                {
                    string path = arg1 as string;
                    Console.WriteLine("=====video result=====" + path);
                    if (string.IsNullOrWhiteSpace(path)) return;
                    file = new Java.IO.File(path);
                    sendMediaData(file, "video");
                };
            }

        }

        //发送媒体数据
        private void sendMediaData(Java.IO.File file, string mediaType)
        {
            if (file != null)
            {
                byte[] imgBytes = new AndroidFileUtils().GetBytesFromFile(file);
                string base64Data = Convert.ToBase64String(imgBytes);

                string js = "setMedia('" + mediaType + "','" + base64Data + "')";
                Console.WriteLine("=====base64=====" + base64Data);
                callWebJs(js);
            }
        }

        /// <summary>
        /// 调用web网页的js
        /// </summary>
        /// <param name="js"></param>
        private void callWebJs(string js)
        {
            Android.Webkit.WebView androidWeb;

            if (string.IsNullOrWhiteSpace(js))
            {
                return;
            }
            if (_androidWeb != null && _androidWeb.IsAlive)
            {
                androidWeb = _androidWeb.Target as Android.Webkit.WebView;
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

    }

}
