
using System;
using System.Collections.Generic;
using WTONewProject.Comment;
using Xamarin.Forms;

namespace WTONewProject.View
{
    public partial class LoginWithNullPage : ContentPage
    {
        public LoginWithNullPage()
        {
            InitializeComponent();
            var source = new UrlWebViewSource();
            var rootPath = DependencyService.Get<IBaseUrl>().Get();
            source.Url = System.IO.Path.Combine(rootPath, "index.html");
            web.Source = source;
        }
    }
}
