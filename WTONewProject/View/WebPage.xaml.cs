
using Newtonsoft.Json;
using System.Net;
using WTONewProject.Models;
using WTONewProject.Services;
using WTONewProject.Tools;
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
            source.Url = Constants.URL_WEB;
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
            HTTPResponse res = await EasyWebRequest.SendHTTPRequestAsync(Constants.URL_GET_USER, "", "POST", _cookie);
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
