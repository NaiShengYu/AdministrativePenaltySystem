﻿using System;

using Android.App;
using Android.Content.PM;
using Plugin.CurrentActivity;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using CN.Jpush.Android.Api;
using Android.Content;

namespace WTONewProject.Droid
{
    [Activity(Label = "运维平台", Icon = "@drawable/logo", Theme = "@style/MainTheme", MainLauncher = false, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            base.OnCreate(savedInstanceState);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                //状态栏透明（白色title时状态栏也可见）
                //Window.AddFlags(WindowManagerFlags.TranslucentStatus);
                //状态栏透明（白色title时状态栏字体颜色也白色了
                Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
                //底部导航栏透明
                Window.AddFlags(WindowManagerFlags.TranslucentNavigation);
            }
            else if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat && Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                Window.AddFlags(WindowManagerFlags.TranslucentStatus);
                //Window.AddFlags(WindowManagerFlags.TranslucentNavigation);
            }


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            InitJPush();
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        /// <summary>
        /// init JPush
        /// </summary>
        private void InitJPush()
        {
            JPushInterface.SetDebugMode(true);
            JPushInterface.Init(Application.Context);
            BasicPushNotificationBuilder builder = new BasicPushNotificationBuilder(this);
            builder.StatusBarDrawable = Resource.Drawable.jpush_notification_icon;
            JPushInterface.SetPushNotificationBuilder(new Java.Lang.Integer(1), builder);
        }


        protected override void OnResume()
        {
            base.OnResume();

            JPushInterface.OnResume(this);
        }

        protected override void OnPause()
        {
            base.OnPause();

            JPushInterface.OnPause(this);
        }


    }
}