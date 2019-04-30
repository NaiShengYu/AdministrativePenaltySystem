
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
<<<<<<< HEAD
            //source.Url = "http://192.168.2.111:8081";
            source.Url = App.tokenModel.url+"/Mobile/index";
=======
            source.Url = Constants.URL_WEB;
>>>>>>> 6fae9d48313bb501fabf49e6e5bb9e0007d4a7c1
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
<<<<<<< HEAD
             DependencyService.Get<IJpushSetAlias>().setAliasWithName(App.tokenModel.sid);
=======
            HTTPResponse res = await EasyWebRequest.SendHTTPRequestAsync(Constants.URL_GET_USER, "", "POST", _cookie);
            if (res.StatusCode == HttpStatusCode.OK)
            {
                UserInfo user = JsonConvert.DeserializeObject<UserInfo>(res.Results);
                if (user != null)
                {
                    DependencyService.Get<IJpushSetAlias>().setAliasWithName(user.userInf_id);
                }
            }
>>>>>>> 6fae9d48313bb501fabf49e6e5bb9e0007d4a7c1
        }

    }
}
