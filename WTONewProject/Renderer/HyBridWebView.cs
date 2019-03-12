using System;
using Xamarin.Forms;

namespace WTONewProject.Renderer
{
    public class HyBridWebView:WebView
    {
        public event EventHandler<EventArgs> CallAction;
        public void SendClick(string data)
        {
            CallAction?.Invoke(this, new EventArgs());
        }
    }
}
