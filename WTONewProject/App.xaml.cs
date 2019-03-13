using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Auth;
using System.Linq;
using WTONewProject.View;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WTONewProject
{
    public partial class App : Application
    {
       
        public string AppName = "CloudWTO";
        public App()
        {
            InitializeComponent();
            Account account = AccountStore.Create().FindAccountsForService(AppName).LastOrDefault();
            if (account == null)
                MainPage = new LoginWithNullPage();
            else
                MainPage = new LoginWithNamePage();

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


        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }
    }
}
