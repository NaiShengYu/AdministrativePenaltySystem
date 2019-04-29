using Android.Content;
using System;
using System.IO;
using WTONewProject.Droid.Servers;
using WTONewProject.Services;
using WTONewProject.Tools;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]

namespace WTONewProject.Droid.Servers
{
    public class FileService : IFileService
    {
        public string GetDatabasePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }

        public string GetExtrnalStoragePath(string type)
        {
            Context c = Android.App.Application.Context;
            Java.IO.File f = c.GetExternalFilesDir(null);
            if (Constants.STORAGE_TYPE_DOC.Equals(type))
            {
                f = c.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
            }
            else if (Constants.STORAGE_TYPE_DOWNLOAD.Equals(type))
            {
                f = c.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads);
            }
            return f.AbsolutePath;
        }
    }
}
