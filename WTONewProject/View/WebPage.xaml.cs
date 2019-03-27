using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WTONewProject.View
{
    public partial class WebPage : ContentPage
    {
        public WebPage(string cookie)
        {
            InitializeComponent();
            var source = new UrlWebViewSource();
            //source.Url = "http://192.168.2.111:8081";
            source.Url = "http://sx.azuratech.com:20001/Mobile/index";
            web.Source = source;
            web.AzuraCookie = cookie;
        }
    }
}
