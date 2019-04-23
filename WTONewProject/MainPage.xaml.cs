using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Auth;
using WTONewProject.Comment;

namespace WTONewProject
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var source = new UrlWebViewSource();
            //source.Url = "http://sx.azuratech.com:20001/Mobile/index";
            //source.Url = "https://www.baidu.com";
            source.ClearValue(UrlWebViewSource.UrlProperty);
            source.Url = "https://www.baidu.com/s?wd=vue%20&rsv_spt=1&rsv_iqid=0x9f0483900008fec5&issp=1&f=8&rsv_bp=1&rsv_idx=2&ie=utf-8&rqlang=cn&tn=baiduhome_pg&rsv_enter=1&oq=vue%2520%25E4%25B8%258D%25E5%2590%258C%25E7%25AB%25AF%25E5%258F%25A3&rsv_t=30d7B8XrSPfZGLlS4UJFhLPstmy9lOD%2FEP282DwdVxSt1zWjlTne24q6oS0QP1xOa1NC&inputT=245&rsv_pq=e6e8d29000006037&rsv_sug3=483&rsv_sug1=259&rsv_sug7=100&rsv_sug2=0&rsv_sug4=1446";
            var rootPath = DependencyService.Get<IBaseUrl>().Get();
            //source.Url = System.IO.Path.Combine(rootPath, "index.html");
            web.Source = source;

        }
    }
}
