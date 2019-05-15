

using System;
using System.Collections.Generic;

namespace WTONewProject.Model
{
    public class LoginResultModel
    {
        public bool success { get; set; }
        public LoginResultModel_Error error { get; set; }
        public LoginResultModel_Result result { get; set; }
     
    }
    public class LoginResultModel_Result
    {
        public string accessToken { get; set; }
        public string encryptedAccessToken { get; set; }
        public string expireInSeconds { get; set; }
        public string userId { get; set; }
    }

    public class LoginResultModel_Error
    {
        public string code { get; set; }
        public string message { get; set; }
        public string details { get; set; }
        public string validationErrors { get; set; }
     
    }

}
