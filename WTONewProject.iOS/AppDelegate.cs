using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using JPush.Binding.iOS;
using UIKit;
using UserNotifications;
using WTONewProject.iOS.Notification.JPush;

namespace WTONewProject.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
          
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        JPushInterface jPushRegister { get; set; }
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            App.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;
            App.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;

            //Xamarin.Calabash.Start();
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            ////注册apns远程推送
            if (options == null) options = new NSDictionary();
            jPushRegister = new JPushInterface();
            jPushRegister.Register(this, options);
            this.RegistLogin(options);
            return base.FinishedLaunching(app, options);
        }



        /// <summary>
        /// 注册apns远程推送
        /// </summary>
        /// <param name="launchOptions"></param>
        protected void RegistLogin(NSDictionary launchOptions)
        {
            string systemVersion = UIDevice.CurrentDevice.SystemVersion.Split('.')[0];
            Console.WriteLine("System Version: " + UIDevice.CurrentDevice.SystemVersion);

            //iOS10以上的注册方式
            if (float.Parse(systemVersion) >= 10.0)
            {
                UNUserNotificationCenter center = UNUserNotificationCenter.Current;
                center.RequestAuthorization((UNAuthorizationOptions.CarPlay | UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge), (bool arg1, NSError arg2) =>
                {
                    if (arg1)
                        Console.WriteLine("ios 10 request notification success");
                    else
                        Console.WriteLine("IOS 10 request notification failed");
                });
            }
            //iOS8以上的注册方式
            else if (float.Parse(systemVersion) >= 8.0)
            {
                UIUserNotificationSettings notiSettings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Badge | UIUserNotificationType.Sound | UIUserNotificationType.Alert, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(notiSettings);
            }
            //iOS8以下的注册方式，这里我们最低版本是7.0以上
            else
            {
                UIRemoteNotificationType myTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Sound | UIRemoteNotificationType.Badge;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(myTypes);
            }
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
            if (launchOptions != null)
            {
                NSDictionary remoteNotification = (NSDictionary)(launchOptions.ObjectForKey(UIApplication.LaunchOptionsRemoteNotificationKey));
                if (remoteNotification != null)
                {
                    Console.WriteLine(remoteNotification);
                    //这里是跳转页面用的
                    //this.goToMessageViewControllerWith(remoteNotification);
                }
            }
        }
        /// <summary>
        /// 给极光deviceToken
        /// </summary>
        /// <param name="application">Application.</param>
        /// <param name="deviceToken">Device token.</param>
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Console.WriteLine("给极光token");
            JPUSHService.RegisterDeviceToken(deviceToken);
        }
        public void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            //这个是在前台
            if (application.ApplicationState == UIApplicationState.Active)
            {
            }
            completionHandler(UIBackgroundFetchResult.NewData);
        }

        public override void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings)
        {
            application.RegisterForRemoteNotifications();
        }
        /// <summary>
        /// 注册token失败
        /// </summary>
        /// <param name="application"></param>
        /// <param name="error"></param>
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            new UIAlertView("注册通知失败", error.LocalizedDescription, null, "OK", null).Show();
        }

        public void GetRegistrationID(int resCode, NSString registrationID)
        {
            if (resCode == 0)
            {
                Console.WriteLine("RegistrationID Successed: {0}", registrationID);

                //App1.ViewModel.UserCenterPageViewModel.Instance.RegistId = registrationID;
            }
            else
                Console.WriteLine("RegistrationID Failed. ResultCode:{0}", resCode);
        }


    }


}
