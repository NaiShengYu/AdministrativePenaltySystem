﻿using System;

using Android.App;
using Android.Content.PM;
using Plugin.CurrentActivity;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using CN.Jpush.Android.Api;

namespace WTONewProject.Droid
{
    [Activity(Label = "EHSOON", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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