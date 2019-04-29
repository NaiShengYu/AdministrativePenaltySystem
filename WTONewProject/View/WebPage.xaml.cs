
using Newtonsoft.Json;
using System.Net;
using WTONewProject.Models;
using WTONewProject.Services;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace WTONewProject.View
{
    public partial class WebPage : ContentPage
    {
        public static string _cookie;

        public WebPage(string cookie)
        {
            InitializeComponent();
            _cookie = cookie;
            var source = new UrlWebViewSource();
            //source.Url = "http://192.168.2.111:8081";
            source.Url = "http://39.97.104.173:801/Mobile/index";
            if (Device.RuntimePlatform == Device.Android)
            {
                web.On<Xamarin.Forms.PlatformConfiguration.Android>().SetMixedContentMode(MixedContentHandling.AlwaysAllow);
            }
            web.Source = source;
            web.AzuraCookie = cookie;
            GetUserInfo();
        }

        protected override bool OnBackButtonPressed()
        {
            if (web.CanGoBack)
            {
                web.GoBack();
                return true;
            }
            return base.OnBackButtonPressed();
        }

        private async void GetUserInfo()
        {
            if (string.IsNullOrWhiteSpace(_cookie))
            {
                return;
            }
            string url = "http://sx.azuratech.com:20001/api/Account/GetUserInfo";
            HTTPResponse res = await EasyWebRequest.SendHTTPRequestAsync(url, "", "POST", _cookie);
            if (res.StatusCode == HttpStatusCode.OK)
            {
                UserInfo user = JsonConvert.DeserializeObject<UserInfo>(res.Results);
                if (user != null)
                {
                    DependencyService.Get<IJpushSetAlias>().setAliasWithName(user.userInf_id);
                }
            }
        }
    }
}
