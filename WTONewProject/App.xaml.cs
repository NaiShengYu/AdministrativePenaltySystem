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

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WTONewProject
{
    public partial class App : Application
    {
        public FrameworkToken frameworkToken = null;
        public static string FrameworkURL = "http://dev.azuratech.com:22000/token";
        public static string AppName = "CloudWTO";
        public App()
        {
            InitializeComponent();
            var account = AccountStore.Create().FindAccountsForService(AppName).LastOrDefault();
            if (account == null)
                MainPage = new LoginWithNullPage();
            else
            MainPage = new LoginWithNamePage(account.Username,account.Properties["pwd"]);

             //MainPage = new MainPage();


        }
        protected override void OnStart()
        {
            // Handle when your app starts
            


        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public async Task<bool> LoginAsync(string username, string password) {

            frameworkToken = await GetFrameworkTokenAsync(username, password);
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
        private async Task<FrameworkToken> GetFrameworkTokenAsync(string username, string password)
        {
            try
            {
                string url = FrameworkURL;
                string param = "username=" + username + "&password=" + password + "&grant_type=password";

                //string url = "http://dev2.azuratech.com:30000/token";
                //Dictionary<string, object> map = new Dictionary<string, object>();
                //map.Add("userid", username);
                //map.Add("password", password);
                //map.Add("grant_type", "password");
                //string param = JsonConvert.SerializeObject(map);

                HTTPResponse res = await EasyWebRequest.SendHTTPRequestAsync(url, param, "POST", null);
                FrameworkToken ft = null;
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    ft = JsonConvert.DeserializeObject<FrameworkToken>(res.Results);
                }
                return ft;
            }
            catch
            {
                return null;
            }
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
