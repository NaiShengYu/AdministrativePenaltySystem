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
        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }
        public App()
        {
            InitializeComponent();
            MainPage = new WebPage();
            //GetLastToken();
            //AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;
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

       
   

     

    }
}
