
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
            source.Url = App.tokenModel.url+"/Mobile/index";
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
             DependencyService.Get<IJpushSetAlias>().setAliasWithName(App.tokenModel.sid);
        }

    }
}
