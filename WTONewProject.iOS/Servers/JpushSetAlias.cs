using System;
using WTONewProject.iOS.Servers;
using WTONewProject.Services;
using JPush.Binding.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(JpushSetAlias))]
namespace WTONewProject.iOS.Servers

{
    class JpushSetAlias: IJpushSetAlias
    {
        public void setAliasWithName(string userid)
        {

            if (!string.IsNullOrWhiteSpace(userid))
            {
                string alias = userid.Replace("-", "");
                JPUSHService.SetAlias(alias, (arg0, arg1, arg2) => { }, 1);

                //string[] tags = App.FrameworkURL.Split(":");
                //string tag = "";
                //if (tags.Count() > 1)
                //{
                //    tag = tags[1];
                //    tag = tag.Replace("//", "");
                //    tag = tags[0] + tag;
                //}
                //Console.WriteLine(" ios SetAlias userid = " + userid);
                //NSSet<NSString> nSSet = new NSSet<NSString>(new NSString[] { (NSString)tag });
                //JPUSHService.SetTags(nSSet, (arg0, arg1, arg2) => { }, 1);
            }
            else
            {
                JPUSHService.DeleteAlias((arg0, arg1, arg2) => { }, 1);
            }
        }
    }
}
