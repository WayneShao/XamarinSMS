using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Telephony;
using Android.Widget;

namespace XamarinSMS.Receivers
{
    [BroadcastReceiver]
    [IntentFilter(new[] { "android.provider.Telephony.SMS_RECEIVED" }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class SmsListener: BroadcastReceiver
    {
        protected string message, address = string.Empty;
        public override void OnReceive(Context context, Intent intent)
        {
            
            if (intent.Action.Equals("android.provider.Telephony.SMS_RECEIVED"))
            {
                Bundle bundle = intent.Extras;
                if (bundle != null)
                {
                    try
                    {
                        var smsArray = (Java.Lang.Object[])bundle.Get("pdus");

                        foreach (var item in smsArray)
                        {
#pragma warning disable CS0618
                            var sms = SmsMessage.CreateFromPdu((byte[])item);
#pragma warning restore CS0618
                            address = sms.OriginatingAddress;
                            message = sms.MessageBody;
                            Console.WriteLine($"Newly received SMS, the content is{address}:{message}");
                            Toast.MakeText(Application.Context, $"Newly received SMS from {address}, the content is \r\n{message}", ToastLength.Long)?.Show();

                        }
                    }
                    catch (Exception)
                    {
                        //Something went wrong.
                    }
                }
            }
        }
    }
}