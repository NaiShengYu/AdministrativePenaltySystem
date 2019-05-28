using System;
namespace WTONewProject.Models
{
    public class ResetPasswordModel
    {
        public bool success { get; set; }
        public Result result { get; set; }
    }



    public class Result
    {
        public bool successed { get; set; }
        public string error { get; set; }//登录账号

    }
}
