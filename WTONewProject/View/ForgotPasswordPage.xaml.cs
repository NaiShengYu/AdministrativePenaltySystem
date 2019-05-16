using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WTONewProject.View
{
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage(string username)
        {
            InitializeComponent();

        }

        void SendCord_Clicked(object sender, System.EventArgs e)
        {
            int i = 60;
            sendCordImage.Source = ImageSource.FromFile("cantEdit.png");
            sendCordButton.IsEnabled = false;
            sendCordButton.Text = i + "s后发送";
            Device.StartTimer(new TimeSpan(0, 0, 1), () => {
                i -= 1;
                if (i >0)
                {
                    sendCordImage.Source = ImageSource.FromFile("cantEdit.png");
                    sendCordButton.IsEnabled = false;
                    sendCordButton.Text =i + "s后发送";
                    return true;
                }
                else
                {
                    sendCordImage.Source = ImageSource.FromFile("canEdit.png");
                    sendCordButton.IsEnabled = true;
                    sendCordButton.Text = "发送验证码";
                    return false;
                }
            });
        
        
        
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

    }
}
