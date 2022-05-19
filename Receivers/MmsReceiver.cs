using Android.App;
using Android.Content;

namespace XamarinSMS.Receivers
{
    [BroadcastReceiver(Enabled = true, Exported = true, Permission = "android.permission.BROADCAST_WAP_PUSH")]
    [IntentFilter(new[] { "android.provider.Telephony.WAP_PUSH_DELIVER" })]
    public class MmsReceiver: BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            
        }
    }
}