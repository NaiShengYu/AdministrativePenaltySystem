
using System;
using System.Collections.Generic;
using WTONewProject.Comment;
using Xamarin.Forms;
using Newtonsoft.Json;

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

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            web.CCallJs("你好啊");

            //web.Eval("pushCode('你好','不好')");

        }

        void Handle_clickYi(object sender, System.EventArgs e)
        {
            Dictionary<string, object> dic = sender as Dictionary<string, object>;
            Console.WriteLine(dic["goodsId"]);

          }
    }
}
