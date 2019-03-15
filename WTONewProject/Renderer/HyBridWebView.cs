using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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


    }
}
