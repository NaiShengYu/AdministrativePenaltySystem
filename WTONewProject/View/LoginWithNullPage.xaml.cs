
using System;
using System.Collections.Generic;
using WTONewProject.Comment;
using Xamarin.Forms;
using Newtonsoft.Json;

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
        void Login_Clicked(object sender, System.EventArgs e)
        {

        }
        void ForgotPassWord_Clicked(object sender, System.EventArgs e)
        {

        }
    }
}
