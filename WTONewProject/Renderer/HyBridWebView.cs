
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using WTONewProject.Model;
using WTONewProject.Tools;
using WTONewProject.View;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WTONewProject.Renderer
{
    public class HyBridWebView : WebView
    {
        public bool _videoFlag { get; set; }
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
         
        }

        public async void logOut()
        {


          
        }


        public void sendSms(string number)
        {
            DeviceUtils.sms(number);
        }

        public void phone(string number)
        {
            DeviceUtils.phone(number);
        }
    }
}
