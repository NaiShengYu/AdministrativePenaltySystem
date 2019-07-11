using System;
using System.IO;
using WTONewProject.Services;
using Xamarin.Forms;

namespace WTONewProject.Tools
{
    public class FileUtils
    {
        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="content"></param>
        public static void SaveLogFile(String content)
        {
            String fileName = "ehsoon_logging_" + TimeUtils.DateTime2YMD(DateTime.Now) + ".txt";
            String path = DependencyService.Get<IFileService>().GetExtrnalStoragePath(Constants.STORAGE_TYPE_DOC);
            Console.WriteLine("=== log file path:" + path);
            SaveFile(content, path, fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="path"></param> 存储路径，包含文件名
        /// <param name="fileName"></param> 文件名（带后缀）
        public static void SaveFile(string content, string path, string fileName)
        {
            if (string.IsNullOrWhiteSpace(content) || string.IsNullOrWhiteSpace(path))
            {
                return;
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            String fullName = Path.Combine(path, fileName);
            if (!File.Exists(fullName))
            {
                File.Create(fullName).Dispose();
            }
            File.AppendAllText(fullName, "\r\n" + DateTime.Now.ToString());
            File.AppendAllText(fullName, content);
            File.AppendAllText(fullName, "\r\n");
        }

    }
}
