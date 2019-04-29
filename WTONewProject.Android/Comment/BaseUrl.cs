using System;
using WTONewProject.Comment;
using WTONewProject.Droid.Comment;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl))]
namespace WTONewProject.Droid.Comment
{
    public class BaseUrl :IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/";
        }
    }
}
