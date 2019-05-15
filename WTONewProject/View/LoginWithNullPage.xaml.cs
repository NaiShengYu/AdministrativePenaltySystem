
using System;
using System.Collections.Generic;
using WTONewProject.Comment;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Auth;
using System.Linq;
using WTONewProject.Services;

namespace WTONewProject.View
{
    public partial class LoginWithNullPage : ContentPage
    {

        bool _isSavePassword = true;
        string _userName = "";
        string _passWord = "";
        string _tenancyName = "";
        public LoginWithNullPage()
        {
            InitializeComponent();
            DependencyService.Get<IJpushSetAlias>().setAliasWithName("");
        }


        public LoginWithNullPage(string userName, string passWord,string tenancyName) :this()
        {
            _isSavePassword = true; 
            _userName = userName;
            _passWord = passWord;
            password.Text = passWord;
            account.Text = userName;
            _tenancyName = tenancyName;
            saveBut.Image = ImageSource.FromFile("icon-select.png") as FileImageSource;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetBackButtonTitle(this, "");
            NavigationPage.SetHasNavigationBar(this, false);
            //or 
        }

        void ShowPassWord_Clicke(object sender, System.EventArgs e)
        {
            password.IsPassword = !password.IsPassword;
            Button button = sender as Button;
            button.Image = password.IsPassword ? ImageSource.FromFile("Group6.png") as FileImageSource : ImageSource.FromFile("show.png") as FileImageSource;
        }

        void SavePassWord_Clicked(object sender, System.EventArgs e)
        {
            _isSavePassword = !_isSavePassword;

            saveBut.Image = _isSavePassword ? ImageSource.FromFile("icon-select.png") as FileImageSource : ImageSource.FromFile("icon-unselect.png") as FileImageSource;
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
            string tenantName = "";
            username = xx[0];
            if (xx.Count() == 2)
            {
                tenantName = xx[1];
            }
            if (!string.IsNullOrWhiteSpace(tenantName))
            {
                _tenancyName = tenantName;
            }
            bool autologin = await (App.Current as App).LoginAsync(username, password.Text, _tenancyName, _isSavePassword);
            if(!autologin)
                await DisplayAlert("提示", "登陆失败", "确定");

        }
        void ForgotPassWord_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ForgotPasswordPage(_userName));

        }


       
    }
}
