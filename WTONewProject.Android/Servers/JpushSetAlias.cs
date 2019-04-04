using Android.App;
using CN.Jpush.Android.Api;
using WTONewProject.Droid.Servers;
using WTONewProject.Services;

[assembly: Xamarin.Forms.Dependency(typeof(JpushSetAlias))]
namespace WTONewProject.Droid.Servers
{
    class JpushSetAlias : IJpushSetAlias
    {
        public void setAliasWithName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Replace("-", "");
                JPushInterface.SetAlias(Application.Context, 0, name);
            }
            else
            {
                JPushInterface.SetAlias(Application.Context, 0, "");
            }
        }
    }
}
