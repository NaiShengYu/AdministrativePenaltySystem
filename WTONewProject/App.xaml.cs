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
using WTONewProject.Tools;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WTONewProject
{
    public partial class App : Application
    {
        public  LoginResultModel _loginResultModel = null;
        public static string FrameworkURL = "";
        static TodoItemDatabase database;
        public static TokenModel tokenModel =null;
        public App()
        {
            InitializeComponent();
            MainPage = new MyPage();
            GetLastToken();
            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;
        }

        //crash收集
        protected void HandleUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            {
                if (args == null || args.ExceptionObject == null)
                    return;
            }
            Exception e = (Exception)args.ExceptionObject;
            String content = args.ExceptionObject.ToString();
            Console.WriteLine(content);
            FileUtils.SaveLogFile(content);
        }

        private async void GetLastToken() {


            //获取存储文件下的内容
            List<TokenModel> tokenModels = await App.Database.GetTokenModelAsync();
            if (tokenModels != null && tokenModels.Count > 0) tokenModel = tokenModels[0];
           int time =  DateTime.Compare(DateTime.Now, tokenModel.lastTime);
            if (tokenModel != null && !string.IsNullOrWhiteSpace(tokenModel.url) && !string.IsNullOrWhiteSpace(tokenModel.token) && time<0)
            {
                MainPage = new WebPage(tokenModel.token);
            }
            else
            {
                GetUserNameAndPassword();
            }
        }

        public async void GetUserNameAndPassword() {
            FrameWorkURL URLModel = null;
            List<FrameWorkURL> URLModels = await App.Database.GetURLModelAsync();
            if (URLModels != null && URLModels.Count > 0) URLModel = URLModels[0];
            if (URLModel !=null)
            {
                App.FrameworkURL = URLModel.frameURL;
            }

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
                    string path = DependencyService.Get<IFileService>().GetDatabasePath("TodoSQLite.db3");
                    database = new TodoItemDatabase(path);
                }
                return database;
             }
        }
        public async Task<bool> LoginAsync(string username, string password,string siteurl,bool issavePassword) {

            if (string.IsNullOrWhiteSpace(siteurl))
            {
                if (string.IsNullOrWhiteSpace(FrameworkURL))
                {
                    return false;
                }else
                    _loginResultModel = await GetFrameworkTokenAsync(username, password, FrameworkURL, issavePassword);
            }
            else
            {
               var frameworkToken1 = await GetFrameworkTokenAsync(username, password, "http://"+siteurl+"/token", issavePassword);
                if(frameworkToken1 != null)
                {
                    _loginResultModel = frameworkToken1;
                    FrameworkURL = "http://" + siteurl + "/token";
                    saveSiteURL();
                }
                else {
                    var frameworkToken2 = await GetFrameworkTokenAsync(username, password, "https://" + siteurl + "/token", issavePassword);
                    if (frameworkToken2 != null)
                    {
                        _loginResultModel = frameworkToken2;
                        FrameworkURL = "https://" + siteurl + "/token";
                        saveSiteURL();
                    }
                }

            }

            if (_loginResultModel == null) return false;
            else
            {
                tokenModel = new TokenModel() { ID = 0 };
                tokenModel.token = _loginResultModel.access_token;
                tokenModel.sid = _loginResultModel.profile.sid;
                tokenModel.name = _loginResultModel.profile.name;
                tokenModel.username = _loginResultModel.profile.username;
                tokenModel.lastTime = _loginResultModel.profile.expires_at.AddHours(8);
                if (_loginResultModel.modList.Count >0)
                {
                    tokenModel.url  = _loginResultModel.modList[0].url;
                }
                saveToken();
                MainPage = new WebPage(_loginResultModel.access_token);
                return true;
            }
        }
        /// <summary>
        /// Get an access token from the framework server with the provided credential
        /// </summary>
        /// <param name="username">User Name</param>
        /// <param name="password">Password</param>
        /// <returns>A FrameworkToken structure that contains the access token for all subsequence requests</returns>
        private async Task<LoginResultModel> GetFrameworkTokenAsync(string username, string password, string siteurl, bool issavePassword)
        {
            try
            {
                string url = siteurl;
                Dictionary<string, object> map = new Dictionary<string, object>();
                map.Add("userid", username);
                map.Add("password", password);
                map.Add("grant_type", "password");
                string param = JsonConvert.SerializeObject(map);

                HTTPResponse res = await EasyWebRequest.SendHTTPRequestAsync(url, param, "POST", null);
                LoginResultModel loginResultModel = null;
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    loginResultModel = JsonConvert.DeserializeObject<LoginResultModel>(res.Results);
                }
                if (loginResultModel !=null && !string.IsNullOrWhiteSpace(loginResultModel.access_token))
                {
                    deleteData(username, password, issavePassword);
                }
                else
                {
                    loginResultModel = null;
                }
                return loginResultModel;
            }
            catch(Exception ex)
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


        private async void saveToken()
        {
            ////循环删除所存的数据
            List<TokenModel> tokenModels = await App.Database.GetTokenModelAsync();
            if (tokenModels != null && tokenModels.Count > 0)
                foreach (var item in tokenModels)
                {
                    await App.Database.DeleteTokenModelAsync(item);
                }
          
            await App.Database.SaveTokenModelAsync(tokenModel);

        }

        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }

    }
}
