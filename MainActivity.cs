using System;
using Android;
using Android.App;
using Android.App.Roles;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Content;
using Android.Widget;
using Android.Content;
using Android.Provider;
using Android.Runtime;

namespace XamarinSMS
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);



            var getPermissionBtn = FindViewById<Button>(Resource.Id.GetPermissionBtn);
            var readSmsBtn = FindViewById<Button>(Resource.Id.ReadSmsBtn);
            var setDefaultBtn = FindViewById<Button>(Resource.Id.SetDefaultBtn);

            if (getPermissionBtn != null)
                getPermissionBtn.Click += (sender, e) =>
                {
                    if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadSms) == (int)Permission.Granted &&
                        ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReceiveSms) == (int)Permission.Granted &&
                        ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteSms) == (int)Permission.Granted)
                    {
                        Toast.MakeText(Application.Context, "Permission has been obtained", ToastLength.Short)?.Show();
                    }
                    else
                    {
                        Toast.MakeText(Application.Context, "Try to get permission.", ToastLength.Short)?.Show();
                        RequestPermissions(new[] { Manifest.Permission.ReadSms, Manifest.Permission.ReceiveSms, Manifest.Permission.WriteSms }, 0);
                        Toast.MakeText(Application.Context, "Permission obtained successfully", ToastLength.Short)?.Show();
                    }
                };

            if (readSmsBtn != null)
                readSmsBtn.Click += (sender, e) =>
                {
                    var intent = new Intent(this, typeof(MessagesListActivity));
                    StartActivity(intent);
                };
            if (setDefaultBtn != null)
                setDefaultBtn.Click += (sender, e) =>
                {
                    if (double.TryParse(Build.VERSION.Release, out var sdk) && sdk >= 10)
                    {
                        var roleManager = GetSystemService(RoleService) as RoleManager;
                        if (roleManager == null)
                        {

                        }
                        else if (roleManager.IsRoleAvailable(RoleManager.RoleSms))
                        {
                            int makeDefaultAppRequest = 1;
                            var intent = roleManager.CreateRequestRoleIntent(RoleManager.RoleSms);
                            StartActivityForResult(intent, makeDefaultAppRequest);
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        var setSmsAppIntent = new Intent(Telephony.Sms.Intents.ActionChangeDefault);
                        setSmsAppIntent.PutExtra(Telephony.Sms.Intents.ExtraPackageName,
                            Application.Context.PackageName);
                        StartActivity(setSmsAppIntent);
                    }
                };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == 0)
            {
                if (grantResults.Length == 1 && grantResults[0] == Permission.Granted)
                    Toast.MakeText(Application.Context, $"{permissions[0]} Permission has been granted.", ToastLength.Short)?.Show();
                else
                    Toast.MakeText(Application.Context, $"Failed to obtain {permissions[0]} permission.", ToastLength.Short)?.Show();
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }
        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}