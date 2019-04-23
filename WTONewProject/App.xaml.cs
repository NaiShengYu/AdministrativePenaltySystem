using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Auth;
using System.Linq;
using WTONewProject.View;
using System.Threading.Tasks;
using WTONewProject.Services;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using WTONewProject.Model;
using System.IO;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WTONewProject
{
    public partial class App : Application
    {
        public FrameworkToken frameworkToken = null;
        public static string FrameworkURL = "";
        static TodoItemDatabase database;

        public App()
        {
            InitializeComponent();
            MainPage = new MyPage();

            GetUserNameAndPassword();
        }

        public async void GetUserNameAndPassword() {
            //FrameWorkURL URLModel = null;
            //List<FrameWorkURL> URLModels = await App.Database.GetURLModelAsync();
            //if (URLModels != null && URLModels.Count > 0) URLModel = URLModels[0];
            //if (URLModel !=null)
            //{
            //    App.FrameworkURL = URLModel.frameURL;
            //}

            //获取存储文件下的内容
            LoginModel userModel = null;
            List<LoginModel> userModels =await App.Database.GetUserModelAsync();
            if (userModels != null && userModels.Count > 0) userModel = userModels[0];

            if (userModel != null)
            {
                MainPage = new LoginWithNullPage(userModel.userName, userModel.password);
            }
            else
            {
                MainPage = new LoginWithNullPage();
            }

        }

           public static TodoItemDatabase Database
        {
            get
            {
                if (database == null)
                {
                    string path = DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3");
                    database = new TodoItemDatabase(path);
                }
                return database;
             }
        }


        protected override void OnStart()
        {



        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public async Task<bool> LoginAsync(string username, string password,string siteurl,bool issavePassword) {

            if (string.IsNullOrWhiteSpace(siteurl))
            {
                if (string.IsNullOrWhiteSpace(FrameworkURL))
                {
                    return false;
                }else
                    frameworkToken = await GetFrameworkTokenAsync(username, password, FrameworkURL, issavePassword);
            }
            else
            {
               var frameworkToken1 = await GetFrameworkTokenAsync(username, password, "http://"+siteurl+"/token", issavePassword);
                if(frameworkToken1 != null)
                {
                    frameworkToken = frameworkToken1;
                    FrameworkURL = "http://" + siteurl + "/token";
                    saveSiteURL();
                }
                var frameworkToken2 = await GetFrameworkTokenAsync(username, password, "https://" + siteurl + "/token", issavePassword);
                if (frameworkToken2 != null)
                {
                    frameworkToken = frameworkToken2;
                    FrameworkURL = "https://" + siteurl + "/token";
                    saveSiteURL();
                }
            }


            if (frameworkToken == null) return false;
            else
            {
                MainPage = new WebPage(frameworkToken.access_token);
                //MainPage = new TestWebPage();
                return true;
            }
        }
        /// <summary>
        /// Get an access token from the framework server with the provided credential
        /// </summary>
        /// <param name="username">User Name</param>
        /// <param name="password">Password</param>
        /// <returns>A FrameworkToken structure that contains the access token for all subsequence requests</returns>
        private async Task<FrameworkToken> GetFrameworkTokenAsync(string username, string password, string siteurl, bool issavePassword)
        {
            try
            {
                string url = siteurl;
                string param = "username=" + username + "&password=" + password + "&grant_type=password";
                HTTPResponse res = await EasyWebRequest.SendHTTPRequestAsync(url, param, "POST", null);
                FrameworkToken ft = null;
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    ft = JsonConvert.DeserializeObject<FrameworkToken>(res.Results);
                    deleteData(username, password, issavePassword);
                }
                return ft;
            }
            catch
            {
                return null;
            }
        }

        private async void deleteData(string username,string passWord,bool isSavePassword)
        {
            ////循环删除所存的数据
            List<LoginModel> userModels = await App.Database.GetUserModelAsync();
            if (userModels != null && userModels.Count > 0)
                foreach (var item in userModels)
                {
                    await App.Database.DeleteUserModelAsync(item);
                }

            if (isSavePassword == true)
            {
                LoginModel loginModel = new LoginModel
                {
                    ID = 0,
                    userName = username,
                    password = passWord
                };
                await App.Database.SaveUserModelAsync(loginModel);
            }
        }

        private async void saveSiteURL()
        {
            ////循环删除所存的数据
            List<FrameWorkURL> userModels = await App.Database.GetURLModelAsync();
            if (userModels != null && userModels.Count > 0)
                foreach (var item in userModels)
                {
                    await App.Database.DeleteURLModelAsync(item);
                }


            FrameWorkURL URLModel = new FrameWorkURL
            {
                    ID = 0,
                    frameURL = FrameworkURL,
            };
                await App.Database.SaveURLModelAsync(URLModel);

        }

        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }

        public class FrameworkToken
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string expires_in { get; set; }
            public string refresh_token { get; set; }
            public string userName { get; set; }
        }
    }
}
