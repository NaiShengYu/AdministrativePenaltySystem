using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using WTONewProject.Model;
using WTONewProject.Models;
using WTONewProject.Services;
using WTONewProject.Tools;
using Xamarin.Forms;

namespace WTONewProject.View
{
    public partial class ForgotPasswordPage : ContentPage
    {


        string path = @"((^13[0-9]{1}[0-9]{8}|^15[0-9]{1}[0-9]{8}|^14[0-9]{1}[0-9]{8}|^16[0-9]{1}[0-9]{8}|^17[0-9]{1}[0-9]{8}|^18[0-9]{1}[0-9]{8}|^19[0-9]{1}[0-9]{8})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)";

        public ForgotPasswordPage(string username)
        {
            InitializeComponent();

        }

       async void SendCord_Clicked(object sender, System.EventArgs e)
        {


            bool isphone = Regex.IsMatch(account.Text, path);
            if (!isphone)
            {
                await DisplayAlert("提示", "请输入正确的手机号", "确定");
                return;
            }

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

            try
            {
                string url = Constants.URL_GET_IDENTIFYINGCODE+ "?phoneNumber="+ account.Text;

                HTTPResponse res = await EasyWebRequest.SendHTTPRequestAsync(url, "", "POST", null);
                if (res.StatusCode == HttpStatusCode.OK)
                {
                }
               
              
            }
            catch (Exception ex)
            {


            }

        }


       async void Clicked_SetNewPassWord(object sender, System.EventArgs e) {

            bool isphone = Regex.IsMatch(account.Text, path);
            if (!isphone)
            {
                await DisplayAlert("提示", "请输入正确的手机号", "确定");
                return;
            }
            if (string.IsNullOrWhiteSpace(code.Text))
            {
                await DisplayAlert("提示", "请输入验证码", "确定");
                return;
            }
            if (string.IsNullOrWhiteSpace(newpass.Text))
            {
                await DisplayAlert("提示", "请输入新密码", "确定");
                return;
            }
            if (newpass.Text != secondpass.Text)
            {
                await DisplayAlert("提示", "两次密码不一致", "确定");
                return;
            }
            try
            {
                string url = Constants.URL_RESETOWNPASSWORD;
                Dictionary<string, object> map = new Dictionary<string, object>();
                map.Add("phoneNumber", account.Text);
                map.Add("identifyingCode", code.Text);
                map.Add("newPassword", newpass.Text);
                map.Add("confirmNewPassword", secondpass.Text);
                string param = JsonConvert.SerializeObject(map);

                HTTPResponse res = await EasyWebRequest.SendHTTPRequestAsync(url, param, "POST", null);
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    ResetPasswordModel result = JsonConvert.DeserializeObject<ResetPasswordModel>(res.Results);
                    if (result.success == false)
                    {
                        await DisplayAlert("提示", "网络错误", "确定");
                    }
                    else
                    {
                        if (result.result.successed == true)
                        {
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("提示", result.result.error, "确定");

                        }
                    }
                  

                }


            }
            catch (Exception ex)
            {
                await DisplayAlert("提示", "网络错误", "确定");


            }





        }



        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

    }
}
