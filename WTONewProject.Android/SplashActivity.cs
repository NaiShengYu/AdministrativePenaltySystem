﻿
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;

namespace WTONewProject.Droid
{
    [Activity(Label = "EHSOON", Icon = "@drawable/icon", Theme = "@style/splashscreen", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));
        }
    }
}