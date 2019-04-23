
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
        string _userName = "";
        string _passWord = "";
        string _sourceURL = "";
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


        public LoginWithNullPage(string userName, string passWord) :this()
        {
            _isSavePassword = true; 
            _userName = userName;
            _passWord = passWord;
            password.Text = passWord;
            account.Text = userName;
            saveBut.Image = ImageSource.FromFile("select.png") as FileImageSource;
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
            string[] xx = account.Text.Split(new string[] { "@" }, StringSplitOptions.None);
            string username = "";
            string siteURL = "";
            username = xx[0];
            if (xx.Count()==2)
            {
                siteURL = xx[1];
            }
            bool autologin = await (App.Current as App).LoginAsync(username, password.Text,siteURL,_isSavePassword);
            if(!autologin)
                await DisplayAlert("提示", "登录失败", "确定");

        }
        void ForgotPassWord_Clicked(object sender, System.EventArgs e)
        {

        }


       
    }
}
