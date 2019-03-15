using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Auth;
using WTONewProject.Comment;

namespace WTONewProject
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var source = new UrlWebViewSource();
            var rootPath = DependencyService.Get<IBaseUrl>().Get();
            source.Url = System.IO.Path.Combine(rootPath, "index.html");
            web.Source = source;

        }
    }
}
