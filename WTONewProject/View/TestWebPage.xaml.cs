using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace WTONewProject.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestWebPage : ContentPage
	{
		public TestWebPage ()
		{
			InitializeComponent ();
            var source = new UrlWebViewSource();
            source.Url = "http://sx.azuratech.com:20001/Mobile/index";
            if (Device.RuntimePlatform == Device.Android)
            {
                web.On<Xamarin.Forms.PlatformConfiguration.Android>().SetMixedContentMode(MixedContentHandling.AlwaysAllow);
            }
            web.Source = source;
        }
	}
}