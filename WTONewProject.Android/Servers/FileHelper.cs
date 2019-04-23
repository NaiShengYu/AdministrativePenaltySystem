using System;
using WTONewProject.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]

namespace WTONewProject.Droid.Servers
{
    public interface FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }

    }
}
