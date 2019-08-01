namespace WTONewProject.Tools
{
    public class Constants
    {
        //公司内网
        public static string WEB_SOURCE = "http://amp.azuratech.com:8088/mobile.html";
        public static string URL_ROOT = "http://amp.azuratech.com:5080/";

        //延庆外网
        //public static string WEB_SOURCE = "http://demo.azuratech.com:51008/mobile.html";
        //public static string URL_ROOT = "http://demo.azuratech.com:51007/";

        //public static string WEB_SOURCE = "http://192.168.2.98:8180/";
        //public static string URL_ROOT = "http://192.168.2.98:8180/";

        public static string URL_GET_USER = URL_ROOT + "/api/Account/GetUserInfo";
        public static string URL_GET_ACCESSTOKEN = URL_ROOT + "api/TokenAuth/Authenticate";//登陆
        public static string URL_GET_IDENTIFYINGCODE = URL_ROOT + "api/services/app/User/SendIdentifyingCode";//获取验证码
        public static string URL_RESETOWNPASSWORD = URL_ROOT + "api/services/app/User/ResetOwnPassword";//重新设置密码

        public static string STORAGE_TYPE_DOC = "document";
        public static string STORAGE_TYPE_DOWNLOAD = "download";
        public static string STORAGE_TYPE_MOVIES = "movie";
    }
}
