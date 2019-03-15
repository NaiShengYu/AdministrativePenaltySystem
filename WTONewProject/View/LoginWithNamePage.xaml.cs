using System;
using System.Collections.Generic;
using System.Linq;
using WTONewProject.Comment;
using Xamarin.Auth;
using Xamarin.Forms;

namespace WTONewProject.View
{
    public partial class LoginWithNamePage : ContentPage
    {

        bool _isSavePassword = true;
        string _userName = "";
        string _passWord = "";
        public LoginWithNamePage(string userName,string passWord)
        {
            InitializeComponent();
            _userName = userName;
            _passWord = passWord;
            password.Text = passWord;
            if (App.ScreenWidth > 400)
            {
                TrapezoidImg.WidthRequest = 300;
                TrapezoidImg.HeightRequest = 256;
                centerGrid.WidthRequest = 300;
                centerGrid.HeightRequest = 298;
                userFrame.Margin = new Thickness(0, 10, 0, 0);
            }
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                App.Current.MainPage = new LoginWithNullPage();
            };
            userImg.GestureRecognizers.Add(tapGestureRecognizer);

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
       async void Login_Clicked(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(password.Text))
            {
                await DisplayAlert("提示", "密码不能为空", "确定");
                return;
            }
            bool autologin = await(App.Current as App).LoginAsync(_userName, password.Text);

        }
        void ForgotPassWord_Clicked(object sender, System.EventArgs e)
        {

        }

        private void deleteData()
        {
#if !(DEBUG && __IOS__)
            //循环删除所存的数据
            IEnumerable<Account> outs = AccountStore.Create().FindAccountsForService(App.AppName);
            for (int i = 0; i < outs.Count(); i++)
            {
                AccountStore.Create().Delete(outs.ElementAt(i), App.AppName);
            }
            if (_isSavePassword)
            {
                Account count = new Account
                {
                    Username = _userName
                };
                count.Properties.Add("pwd", password.Text);
                AccountStore.Create().Save(count, App.AppName);
            }
#endif
        }
    }
}
