using Android.Widget;
using Sample;
using WTONewProject.Droid.Servers;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToastService))]
namespace WTONewProject.Droid.Servers
{
    public class ToastService : IToast
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();

        }
    }
}