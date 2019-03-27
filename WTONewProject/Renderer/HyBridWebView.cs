using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WTONewProject.Renderer
{
    public class HyBridWebView:WebView
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
            getLocation.Invoke(currentLocation.Latitude + "," + currentLocation.Longitude, new EventArgs());
        }
        

    }
}
