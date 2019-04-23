using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;//导入的库是sqlite-net-pcl
using WTONewProject.Model;

namespace WTONewProject.Services
{
    public class TodoItemDatabase
    {
        readonly SQLiteAsyncConnection database;
        public TodoItemDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<LoginModel>().Wait();
            database.CreateTableAsync<FrameWorkURL>().Wait();
            Console.WriteLine("表创建成功");
        }

        //查
        public Task<List<LoginModel>> GetUserModelAsync()
        {
            return database.Table<LoginModel>().ToListAsync();
        }

        //删
         public Task<int> DeleteUserModelAsync(LoginModel item)
        {
            return database.DeleteAsync(item);
        }

        //增，改
        public Task<int> SaveUserModelAsync(LoginModel item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }


        //查
        public Task<List<FrameWorkURL>> GetURLModelAsync()
        {
            return database.Table<FrameWorkURL>().ToListAsync();
        }

        //删
        public Task<int> DeleteURLModelAsync(FrameWorkURL item)
        {
            return database.DeleteAsync(item);
        }

        //增，改
        public Task<int> SaveURLModelAsync(FrameWorkURL item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

    }
}
