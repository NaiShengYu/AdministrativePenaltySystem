using System;
using Foundation;
using WTONewProject.Comment;
using WTONewProject.iOS.Comment;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl))]
namespace WTONewProject.iOS.Comment
{
    public class BaseUrl :IBaseUrl
    {
        public string Get()
        {
            return NSBundle.MainBundle.BundlePath;
        }
    }
}
