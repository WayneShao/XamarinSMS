using Android.App;
using Android.Content;
using Android.Telephony;

namespace XamarinSMS.Receivers
{
    public class SendSms
    {
        
        public void Send(string address, string message)
        {
            var pendingIntent = PendingIntent.GetActivity(Android.App.Application.Context, 0, new Intent(Android.App.Application.Context, typeof(MainActivity)).AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask), PendingIntentFlags.NoCreate);

            SmsManager smsM = SmsManager.Default;
            smsM.SendTextMessage(address, null, message, pendingIntent, null);
        }
    }
}