
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using WTONewProject.Model;
using WTONewProject.View;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WTONewProject.Renderer
{
    public class HyBridWebView : WebView
    {
        public string AzuraCookie { get; set; }
        public string userid { get; set; }
        public event EventHandler<EventArgs> pushCode;
        public event EventHandler<EventArgs> clickYi;

        public void clickOne(object sss)
        {
            //Dictionary<string, object> dic = sss as Dictionary<string,object>;
            //Console.WriteLine(dic);
            clickYi.Invoke(sss, new EventArgs());
        }

        public void CCallJs(object sss)
        {
            //pushCode.Invoke(sss,new EventArgs());

        }

        public event EventHandler<EventArgs> getLocation;

        public async void getLngLat(object sss)
        {
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
            if (currentLocation ==null)
            {
                currentLocation = new Location(34.754626, 113.735763);
            }
            Console.WriteLine("location success: lat="+ currentLocation.Latitude + " lng="+currentLocation.Longitude);
            //getLocation.Invoke(currentLocation.Latitude + "," + currentLocation.Longitude, new EventArgs());
        }

        public async void logOut()
        {


            ////循环删除所存的数据
            List<TokenModel> tokenModels = await App.Database.GetTokenModelAsync();
            if (tokenModels != null && tokenModels.Count > 0)
            {
                foreach (var item in tokenModels)
                {
                    await App.Database.DeleteTokenModelAsync(item);
                }
            }

            FrameWorkURL URLModel = null;
            List<FrameWorkURL> URLModels = await App.Database.GetURLModelAsync();
            if (URLModels != null && URLModels.Count > 0) URLModel = URLModels[0];
            if (URLModel != null)
            {
                //App.FrameworkURL = URLModel.frameURL;
            }

            //获取存储文件下的内容
            LoginModel userModel = null;
            List<LoginModel> userModels = await App.Database.GetUserModelAsync();
            if (userModels != null && userModels.Count > 0) userModel = userModels[0];

            if (userModel != null)
            {
                App.Current.MainPage = new NavigationPage(new LoginWithNullPage(userModel.userNameOrEmailAddress, userModel.password, userModel.tenancyName));
            }
            else
            {
                App.Current.MainPage = new NavigationPage(new LoginWithNullPage());
            }
        }
    }
}
