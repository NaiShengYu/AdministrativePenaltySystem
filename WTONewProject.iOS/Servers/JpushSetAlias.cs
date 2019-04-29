using System;
using WTONewProject.iOS.Servers;
using WTONewProject.Services;
using JPush.Binding.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(JpushSetAlias))]
namespace WTONewProject.iOS.Servers

{
    class JpushSetAlias: IJpushSetAlias
    {
        public void setAliasWithName(string name){
            JPUSHService.SetAlias("", (arg0, arg1, arg2) => { }, new nint());
        }
    }
}
