using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UserNotifications;
using WTONewProject.iOS;
using JPush.Binding.iOS;

namespace WTONewProject.iOS.Notification.JPush
{
    public class JPushInterface : JPUSHRegisterDelegate
    {//ab4f6d9522395eb71a74c0cc
        internal static string JPushAppKey = "ab4f6d9522395eb71a74c0cc";
        internal static string Channel = "";
        JPushRegisterEntity entity { get; set; }
        public void Register(AppDelegate app, NSDictionary options)
        {
            //注册apns远程推送
            string advertisingId = AdSupport.ASIdentifierManager.SharedManager.AdvertisingIdentifier.AsString();
            this.entity = new JPushRegisterEntity();
            this.entity.Types = 1 | 2 | 3;//entity.Types = (nint)(JPAuthorizationOptions.Alert) | JPAuthorizationOptions.Badge | JPAuthorizationOptions.Sound;
            JPUSHService.RegisterForRemoteNotificationConfig(entity, this);
            JPUSHService.SetupWithOption(options, JPushAppKey, Channel, true, advertisingId);
            JPUSHService.RegistrationIDCompletionHandler(app.GetRegistrationID);
            NSSet<NSString> nSSet = new NSSet<NSString>(new NSString[] { (NSString)"jjououwoeur"});
            JPUSHService.AddTags(nSSet, (arg0, arg1, arg2) => { }, 1);
        }

        /// <summary>
        /// 前台收到通知,IOS10 support
        /// </summary>
        /// <param name="center"></param>
        /// <param name="notification"></param>
        /// <param name="completionHandler"></param>
        public override void WillPresentNotification(UserNotifications.UNUserNotificationCenter center, UserNotifications.UNNotification notification, Action<nint> completionHandler)
        {
            Console.WriteLine("WillPresentNotification:");
            var content = notification.Request.Content;
            var userInfo = notification.Request.Content.UserInfo;
            if (typeof(UserNotifications.UNPushNotificationTrigger) == notification.Request.Trigger.GetType())
            {//远程通知
                System.Console.WriteLine(" 收到远程通知,Title:{0} -SubTitle:{1}, -Body:{2}", content.Title, content.Subtitle, content.Body);
                this.AddNotificationToView(content);

                JPUSHService.HandleRemoteNotification(userInfo);
            }
            else
            {//本地通知

            }

            if (completionHandler != null)
            {
                completionHandler(4);//UNNotificationPresentationOptions： None = 0,Badge = 1,Sound = 2,Alert = 4,
            }
        }

        /// <summary>
        /// 后台收到通知
        /// </summary>
        /// <param name="center"></param>
        /// <param name="response"></param>
        /// <param name="completionHandler"></param>
        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            Console.WriteLine("DidReceiveNotificationResponse:");
            var content = response.Notification.Request.Content;
            var userInfo = response.Notification.Request.Content.UserInfo;
            if (typeof(UserNotifications.UNPushNotificationTrigger) == response.Notification.Request.Trigger.GetType())
            {//远程通知
                System.Console.WriteLine(" 收到远程通知,Title:{0} -SubTitle:{1}, -Body:{2}", content.Title, content.Subtitle, content.Body);
                this.AddNotificationToView(content);
                JPUSHService.HandleRemoteNotification(userInfo);
            }
            else
            {//本地通知

            }

            if (completionHandler != null)
            {
                completionHandler();
            }
        }

        /// <summary>
        /// 通知添加到视图
        /// </summary>
        /// <param name="content"></param>
        public void AddNotificationToView(UNNotificationContent content)
        {
            //App1.ViewModel.PushsPageViewModel.Item item = new ViewModel.PushsPageViewModel.Item()
            //{
            //    Id = content.CategoryIdentifier,
            //    Text = content.Title,
            //    Detail = content.Body,
            //};
            //App1.ViewModel.PushsPageViewModel.Instance.AddItem(item);
        }
    }
}
