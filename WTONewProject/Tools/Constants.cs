using System;
using System.Collections.Generic;
using System.Text;

namespace WTONewProject.Tools
{
   public  class Constants
    {

        public static string WEB_SOURCE = "http://amp.azuratech.com:8088/mobile.html";
        //public static string WEB_SOURCE = "http://amp.azuratech.com:8090";
        public static string URL_ROOT = "http://amp.azuratech.com:5080/";
        public static string URL_WEB = URL_ROOT + "/Mobile/index";
        public static string URL_GET_USER = URL_ROOT + "/api/Account/GetUserInfo";
        public static string URL_GET_ACCESSTOKEN = URL_ROOT + "api/TokenAuth/Authenticate";//登陆
        public static string URL_GET_IDENTIFYINGCODE = URL_ROOT + "api/services/app/User/SendIdentifyingCode";//获取验证码
        public static string URL_RESETOWNPASSWORD = URL_ROOT + "api/services/app/User/ResetOwnPassword";//重新设置密码

        public static string STORAGE_TYPE_DOC = "document";
        public static string STORAGE_TYPE_DOWNLOAD = "download";
    }
}
