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

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WTONewProject
{
    public partial class App : Application
    {
        public FrameworkToken frameworkToken = null;
        public static string FrameworkURL = "";
        public static string AppName = "CloudWTO";
        public static string siteURL = "siteURL";
        public App()
        {
            InitializeComponent();
            var account = AccountStore.Create().FindAccountsForService(AppName).LastOrDefault();
            var account1 = AccountStore.Create().FindAccountsForService(siteURL).LastOrDefault();

            if (account1 != null)
                FrameworkURL = account1.Username;
            if (account == null)
                MainPage = new LoginWithNullPage();
            else {
                MainPage = new LoginWithNullPage(account.Username, account.Properties["pwd"]);
            }

            //MainPage = new MainPage();


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

        private void deleteData(string username,string passWord,bool isSavePassword)
        {
            //#if !(DEBUG && __IOS__)
            //循环删除所存的数据
            IEnumerable<Account> outs = AccountStore.Create().FindAccountsForService(App.AppName);
            for (int i = 0; i < outs.Count(); i++)
            {
                AccountStore.Create().Delete(outs.ElementAt(i), App.AppName);
            }
            if (isSavePassword)
            {
                Account count = new Account
                {
                    Username = username
                };
                count.Properties.Add("pwd", passWord);
                count.Properties.Add("sourceURL", App.FrameworkURL);
                AccountStore.Create().Save(count, App.AppName);
            }
            //#endif
        }

        private void saveSiteURL()
        {
            //#if !(DEBUG && __IOS__)
            //循环删除所存的数据
            IEnumerable<Account> outs = AccountStore.Create().FindAccountsForService(App.siteURL);
            for (int i = 0; i < outs.Count(); i++)
            {
                AccountStore.Create().Delete(outs.ElementAt(i), App.siteURL);
            }
                Account count = new Account
                {
                    Username = FrameworkURL
                };
                AccountStore.Create().Save(count, App.siteURL);
            //#endif
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
