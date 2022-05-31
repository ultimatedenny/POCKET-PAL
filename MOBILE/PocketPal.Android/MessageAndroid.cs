using Android.App;
using Android.Widget;
using POCKETPAL.Droid;
using POCKETPAL.Classes;
[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace POCKETPAL.Droid
{
    public class MessageAndroid : IMessage
    {
        public MessageAndroid()
        {
        }
        public void ShortAlert(string message)
        {
            Android.Widget.Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
        public void LongAlert(string message)
        {
            Android.Widget.Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }
    }
}