using System;
using System.ComponentModel;
using SQLite;

namespace WTONewProject.Model
{
    public class TokenModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string token { get; set; }
        public string url { get; set; }
        public string sid { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public DateTime lastTime { get; set; }
    }
}
