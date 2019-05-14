

using System;
using System.Collections.Generic;

namespace WTONewProject.Model
{
    public class LoginResultModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public List<LoginResultModel_ModList> modList { get; set; }
        public LoginResultModel_Profil profile { get; set; }
     
    }
    public class LoginResultModel_ModList
    {
        public string id { get; set; }
        public string index { get; set; }
        public string url { get; set; }
        public string status { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

    public class LoginResultModel_Profil
    {
        public string sid { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public DateTime auth_time { get; set; }
        public DateTime expires_at { get; set; }

    }

}
