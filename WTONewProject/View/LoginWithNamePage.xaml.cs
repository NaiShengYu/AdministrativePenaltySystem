using System;
using System.Collections.Generic;
using WTONewProject.Comment;
using Xamarin.Forms;

namespace WTONewProject.View
{
    public partial class LoginWithNamePage : ContentPage
    {

        bool _isSavePassword = false;

        public LoginWithNamePage()
        {
            InitializeComponent();
            //var source = new UrlWebViewSource();
            //var rootPath = DependencyService.Get<IBaseUrl>().Get();
            //source.Url = System.IO.Path.Combine(rootPath, "index.html");
            //web.Source = source;
            if (App.ScreenWidth > 400)
            {
                TrapezoidImg.WidthRequest = 300;
                TrapezoidImg.HeightRequest = 256;
                centerGrid.WidthRequest = 300;
                centerGrid.HeightRequest = 298;
                userFrame.Margin = new Thickness(0, 10, 0, 0);
            }
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            //web.CCallJs("你好啊");

            //web.Eval("pushCode('你好','不好')");

        }

        void Handle_clickYi(object sender, System.EventArgs e)
        {
            Dictionary<string, object> dic = sender as Dictionary<string, object>;
            Console.WriteLine(dic["goodsId"]);

        }


        void SavePassWord_Clicked(object sender, System.EventArgs e)
        {
            _isSavePassword = !_isSavePassword;
            saveBut.Image = _isSavePassword ? ImageSource.FromFile("select.png") as FileImageSource : ImageSource.FromFile("nomal.png") as FileImageSource;
        }

        void ChangePassWord_Clicked(object sender, System.EventArgs e)
        {

        }
        void Login_Clicked(object sender, System.EventArgs e)
        {

        }
        void ForgotPassWord_Clicked(object sender, System.EventArgs e)
        {

        }
    }
}
