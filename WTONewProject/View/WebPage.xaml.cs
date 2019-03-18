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
            source.Url = "http://sx.azuratech.com:20001/Mobile/index";
            //source.Url = "http://www.baidu.com";
            web.Source = source;
            web.AzuraCookie = cookie;
        }
    }
}
