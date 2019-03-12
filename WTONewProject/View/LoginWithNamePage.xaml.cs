using System;
using System.Collections.Generic;
using WTONewProject.Comment;
using Xamarin.Forms;

namespace WTONewProject.View
{
    public partial class LoginWithNamePage : ContentPage
    {
        public LoginWithNamePage()
        {
            InitializeComponent();
            var source = new UrlWebViewSource();
            var rootPath = DependencyService.Get<IBaseUrl>().Get();
            source.Url = System.IO.Path.Combine(rootPath, "index.html");
            web.Source = source;
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {

            web.Eval("pushCode('你好','不好')");
        }
    }
}
