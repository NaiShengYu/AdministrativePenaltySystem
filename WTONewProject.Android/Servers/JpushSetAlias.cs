using System;
using Android.App;
using CN.Jpush.Android.Api;
using WTONewProject.Droid.Servers;
using WTONewProject.Services;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseService))]
namespace WTONewProject.Droid.Servers
{
    class DatabaseService : IJpushSetAlias
    {
        public void setAliasWithName(string name)
        {
            JPushInterface.SetAlias(Application.Context, 0, name);
        }
    }
}
