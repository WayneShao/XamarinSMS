using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using BuiltInViews;
using XamarinSMS.Receivers;

namespace XamarinSMS
{
    [Activity(Label = "MessagesListActivity")]
    public class MessagesListActivity : Activity
    {
        private List<MessageItem> _messages;
        private ListView listView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _messages = ContentResolver.GetAllSms();
            SetContentView(Resource.Layout.MessagesList);
            listView = FindViewById<ListView>(Resource.Id.List);


            if (listView == null) return;
            listView.Adapter = new MessageAdapter(this, _messages);
            listView.ItemClick += OnListItemClick;
            listView.ItemLongClick += (sender, args) =>
            {
                var message = _messages[args.Position];

                Toast.MakeText(Application.Context, $"Try to delete message id:{message.Id}.", ToastLength.Short)?.Show();
                var url = Android.Net.Uri.Parse("content://sms/" + message.Id);

                var ret = ContentResolver!.Delete(url, null, null);
                if (ret > 0)
                {
                    Toast.MakeText(Application.Context, $"Message id:{message.Id} deleted.", ToastLength.Short)?.Show();
                    _messages.RemoveAt(args.Position);
                    listView.Adapter = new MessageAdapter(this, _messages);
                }
                else
                {
                    Toast.MakeText(Application.Context, $"Message id:{message.Id} not deleted. \r\nPlease check if the current APP is set as the default SMS program", ToastLength.Long)?.Show();
                }
            };
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var t = _messages[e.Position];
            Toast.MakeText(this, $"{t.From}:{t.Content}", ToastLength.Long)?.Show();
            Console.WriteLine($"{t.From}:{t.Content}");
        }
    }

    public class MessageAdapter : BaseAdapter<MessageItem>
    {
        private List<MessageItem> _items;
        private Activity _context;
        public MessageAdapter(Activity context, List<MessageItem> items)
            : base()
        {
            _context = context;
            _items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override MessageItem this[int position] => _items[position];

        public override int Count => _items.Count;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _items[position];

            var view = _context.LayoutInflater?.Inflate(Resource.Layout.CustomView, null);
            if (view != null)
            {
                view.FindViewById<TextView>(Resource.Id.FromTxt)!.Text = item.From;
                view.FindViewById<TextView>(Resource.Id.ContentTxt)!.Text = item.Content;
                view.FindViewById<TextView>(Resource.Id.DateTxt)!.Text = item.Time.ToString("g");

            }
            return view;
        }
    }
}