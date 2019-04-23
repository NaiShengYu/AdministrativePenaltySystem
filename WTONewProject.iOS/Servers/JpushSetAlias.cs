using System;
using WTONewProject.iOS.Servers;
using WTONewProject.Services;
using JPush.Binding.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseService))]
namespace WTONewProject.iOS.Servers

{
    class DatabaseService: IJpushSetAlias
    {
        public void setAliasWithName(string name){
            JPUSHService.SetAlias("", (arg0, arg1, arg2) => { }, new nint());
        }
    }
}
