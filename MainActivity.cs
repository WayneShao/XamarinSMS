using System;
using System.Collections.Generic;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Widget;

namespace XamarinSMS
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Get our UI controls from the loaded layout
            var phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            var translatedPhoneWord = FindViewById<TextView>(Resource.Id.TranslatedPhoneword);
            var translateButton = FindViewById<Button>(Resource.Id.TranslateButton);

            // Add code to translate number
            if (translateButton != null)
                translateButton.Click += (sender, e) =>
                {
                    // Translate user's alphanumeric phone number to numeric
                    if (phoneNumberText == null) return;
                    var translatedNumber = PhoneWordTranslator.ToNumber(phoneNumberText.Text);
                    if (string.IsNullOrWhiteSpace(translatedNumber))
                    {
                        if (translatedPhoneWord != null) 
                            translatedPhoneWord.Text = string.Empty;
                    }
                    else
                    {
                        if (translatedPhoneWord != null) 
                            translatedPhoneWord.Text = translatedNumber;
                    }
                };
        }

        protected override void OnResume()
        {
            base.OnResume();


            var btnSend = FindViewById<Button>(Resource.Id.button1);
            btnSend.Click += (sender, e) =>
            {
                RequestPermissions(
                        new[] {
                        Manifest.Permission.ReadSms,
                        Manifest.Permission.BroadcastSms,
                        Manifest.Permission.ReceiveSms,
                        Manifest.Permission.SendSms,
                        Manifest.Permission.WriteSms }, 0);
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadSms) == (int)Permission.Granted)
                {
                    // We have permission, go ahead and use the camera.
                    Toast.MakeText(Application.Context, "Read Sms Check Success", ToastLength.Short)?.Show();
                }
                else
                {
                    // We have permission, go ahead and use the camera.
                    Toast.MakeText(Application.Context, "Read Sms Check Failed", ToastLength.Short)?.Show();
                    RequestPermissions(
                        new[] {
                        Manifest.Permission.ReadSms,
                        Manifest.Permission.BroadcastSms,
                        Manifest.Permission.ReceiveSms,
                        Manifest.Permission.SendSms,
                        Manifest.Permission.WriteSms }, 0);
                    // Camera permission is not granted. If necessary display rationale & request.
                }


                var intent = new Android.Content.Intent(Android.Content.Intent.ActionSend);
                intent.SetType("text/plain");
                intent.PutExtra(Android.Content.Intent.ExtraText, "This is a test message from Xamarin Android");
                StartActivity(intent);
                getAllSms();
            };
        }
        List<string> _items = new List<string>();
        public void getAllSms()
        {

            const string inbox = "content://sms/inbox";
            var reqCols = new[] { "_id", "thread_id", "address", "person", "date", "body", "type" };
            var uri = Android.Net.Uri.Parse(inbox);
            var cursor = ContentResolver?.Query(uri, reqCols, null, null, null);

            if (cursor == null || !cursor.MoveToFirst()) return;
            do
            {
                var id = cursor.GetLong(0);
                var messageId = cursor.GetString(cursor.GetColumnIndex(reqCols[0]));
                var threadId = cursor.GetString(cursor.GetColumnIndex(reqCols[1]));
                var address = cursor.GetString(cursor.GetColumnIndex(reqCols[2]));
                var name = cursor.GetString(cursor.GetColumnIndex(reqCols[3]));
                var date = cursor.GetString(cursor.GetColumnIndex(reqCols[4]));
                var msg = cursor.GetString(cursor.GetColumnIndex(reqCols[5]));
                var type = cursor.GetString(cursor.GetColumnIndex(reqCols[6]));

                _items.Add(messageId + "," + threadId + "," + address + "," + name + "," + date + " ," + msg + " ," + type);

                ContentResolver.Delete(Android.Net.Uri.Parse("content://sms/" + messageId), null, null);
            } while (cursor.MoveToNext());
        }
    }
}