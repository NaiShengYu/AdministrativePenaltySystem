using System;
using System.IO;
using WTONewProject.iOS.Servers;
using WTONewProject.Services;
using WTONewProject.Tools;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace WTONewProject.iOS.Servers
{
    public class FileService : IFileService
    {

        public string GetDatabasePath(string filename)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))

            {

                Directory.CreateDirectory(libFolder);

            }

            return Path.Combine(libFolder, filename);

        }

        public string GetExtrnalStoragePath(string type)
        {
            string mainPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            if (!Directory.Exists(Path.Combine(mainPath, "Document")))
                Directory.CreateDirectory(Path.Combine(mainPath, "Document"));
            if (!Directory.Exists(Path.Combine(mainPath, "DownLoad")))
                Directory.CreateDirectory(Path.Combine(mainPath, "DownLoad"));
            if (Constants.STORAGE_TYPE_DOC.Equals(type))
            {
                mainPath = Path.Combine(mainPath, "Document");
            }
            else if (Constants.STORAGE_TYPE_DOWNLOAD.Equals(type))
            {
                mainPath = Path.Combine(mainPath, "DownLoad");
            }
            return mainPath;
        }
    }
}
