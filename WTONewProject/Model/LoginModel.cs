using System;
using System.ComponentModel;
using SQLite;

namespace WTONewProject.Model
{
    public class LoginModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }
}
