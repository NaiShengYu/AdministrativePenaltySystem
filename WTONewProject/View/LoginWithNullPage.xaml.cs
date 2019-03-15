
using System;
using System.Collections.Generic;
using WTONewProject.Comment;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Auth;
using System.Linq;

namespace WTONewProject.View
{
    public partial class LoginWithNullPage : ContentPage
    {

        bool _isSavePassword = false;
        public LoginWithNullPage()
        {
            InitializeComponent();
            if (App.ScreenWidth >400)
            {
                TrapezoidImg.WidthRequest = 300;
                TrapezoidImg.HeightRequest = 256;
                centerGrid.WidthRequest = 300;
                centerGrid.HeightRequest = 298;
                userFrame.Margin = new Thickness(0, 10, 0, 0);
            }          
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
            if (string.IsNullOrWhiteSpace(account.Text))
            {
                await DisplayAlert("提示", "账号不能为空", "确定");
                return;
            }
            if (string.IsNullOrEmpty(password.Text))
            {
                await DisplayAlert("提示", "密码不能为空", "确定");
                return;
            }
            deleteData();
            bool autologin = await (App.Current as App).LoginAsync(account.Text, password.Text);
        }
        void ForgotPassWord_Clicked(object sender, System.EventArgs e)
        {

        }


        private void deleteData()
        {
            //#if !(DEBUG && __IOS__)
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
                    Username = account.Text
                };
                count.Properties.Add("pwd", password.Text);
                AccountStore.Create().Save(count, App.AppName);
            }
            //#endif
        }
    }
}
