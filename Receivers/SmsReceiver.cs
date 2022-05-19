using Android.App;
using Android.Content;

namespace XamarinSMS.Receivers
{
    [BroadcastReceiver(Enabled = true, Exported = true, Permission = "android.permission.BROADCAST_SMS")]
    [IntentFilter(new[] { "android.provider.Telephony.SMS_DELIVER" })]
    public class SmsReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            
        }
    }
}