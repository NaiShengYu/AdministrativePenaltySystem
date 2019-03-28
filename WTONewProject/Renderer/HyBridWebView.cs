
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using WTONewProject.View;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WTONewProject.Renderer
{
    public class HyBridWebView : WebView
    {
        public string AzuraCookie { get; set; }
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

        public void logOut()
        {
            var account = AccountStore.Create().FindAccountsForService(App.AppName).LastOrDefault();
            var account1 = AccountStore.Create().FindAccountsForService(App.siteURL).LastOrDefault();

            if (account1 != null)
                App.FrameworkURL = account1.Username;
            if (account == null)
                App.Current.MainPage = new LoginWithNullPage();
            else
            {
                App.Current.MainPage = new LoginWithNullPage(account.Username, account.Properties["pwd"]);
                App.userName = account.Username;
                App.pwd = account.Properties["pwd"];
            }
        }
    }
}
