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
        static TodoItemDatabase database;
        public static TokenModel tokenModel = null;
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
            if (tokenModel != null && !string.IsNullOrWhiteSpace(tokenModel.accessToken))
            {
                MainPage = new WebPage(tokenModel.accessToken, tokenModel.userId);
            }
            else
            {
                GetUserNameAndPassword();
            }
        }

        public async void GetUserNameAndPassword() {

            //获取存储文件下的内容
            LoginModel userModel = null;
            List<LoginModel> userModels =await App.Database.GetUserModelAsync();
            if (userModels != null && userModels.Count > 0) userModel = userModels[0];
            if (userModel != null)
            {
                NavigationPage navigationPage = new NavigationPage(new LoginWithNullPage(userModel.userNameOrEmailAddress, userModel.password, userModel.tenancyName))
                {
                    BarTextColor = Color.Black,
                };
                NavigationPage.SetBackButtonTitle(this, "");
                MainPage = navigationPage;
            }
            else
            {
                MainPage = new NavigationPage(new LoginWithNullPage())
                {
                    BarTextColor = Color.Black,
                };
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
        public async Task<bool> LoginAsync(string username, string password,string TenantName, bool issavePassword) {


            _loginResultModel = await GetFrameworkTokenAsync(username, password, TenantName,issavePassword);
           
            if (_loginResultModel == null || _loginResultModel.result ==null) 
            {
                return false;
            }

            else
            {
                tokenModel = new TokenModel() { ID = 0 };
                tokenModel.accessToken = _loginResultModel.result.accessToken;
                tokenModel.encryptedAccessToken = _loginResultModel.result.encryptedAccessToken;
                tokenModel.expireInSeconds = _loginResultModel.result.expireInSeconds;
                tokenModel.userId = _loginResultModel.result.userId;
                saveToken();
                MainPage = new WebPage(_loginResultModel.result.accessToken,_loginResultModel.result.userId);
                return true;
            }
        }
        /// <summary>
        /// Get an access token from the framework server with the provided credential
        /// </summary>
        /// <param name="username">User Name</param>
        /// <param name="password">Password</param>
        /// <returns>A FrameworkToken structure that contains the access token for all subsequence requests</returns>
        private async Task<LoginResultModel> GetFrameworkTokenAsync(string username, string password, string TenantName, bool issavePassword)
        {
            try
            {
                string url = Constants.URL_GET_ACCESSTOKEN;
                Dictionary<string, object> map = new Dictionary<string, object>();
                map.Add("UserNameOrEmailAddress", username);
                map.Add("Password", password);
                map.Add("TenancyName", TenantName);
                map.Add("rememberClient", true);
                string param = JsonConvert.SerializeObject(map);

                HTTPResponse res = await EasyWebRequest.SendHTTPRequestAsync(url, param, "POST", null);
                LoginResultModel loginResultModel = null;
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    loginResultModel = JsonConvert.DeserializeObject<LoginResultModel>(res.Results);
                }
                if (loginResultModel !=null && loginResultModel.result !=null)
                {
                    deleteData(username, password, TenantName, issavePassword);
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

        private async void deleteData(string username,string passWord,string tenancyName, bool isSavePassword)
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
                    userNameOrEmailAddress = username,
                    password = passWord,
                    tenancyName = tenancyName
                };
                await App.Database.SaveUserModelAsync(loginModel);
            }
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
