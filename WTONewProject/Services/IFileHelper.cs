using System;
namespace WTONewProject.Services
{
    public interface IFileService
    {
        string GetDatabasePath(string filename);
        string GetExtrnalStoragePath(string type);
    }
}
