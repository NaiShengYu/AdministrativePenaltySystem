
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

        public WebPage()
        {
            InitializeComponent();

       
            var source = new UrlWebViewSource();
            source.Url = Constants.WEB_SOURCE;
            web.Source = source;
        }

     

    }
}
