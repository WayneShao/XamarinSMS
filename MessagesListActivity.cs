using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using XamarinSMS.Data;
using XamarinSMS.Models;
using XamarinSMS.Receivers;
using Message = XamarinSMS.Models.Message;

namespace XamarinSMS
{
    [Activity(Label = "MessagesListActivity")]
    public class MessagesListActivity : Activity
    {
        private List<Message> _messages;
        private ListView _listView;
        private MessagesListType? Type;
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MessagesList);
            _listView = FindViewById<ListView>(Resource.Id.List);

            var type = Intent?.GetSerializableExtra("Type");
            var typeEnum = Type = Enum.Parse(typeof(MessagesListType), type?.ToString() ?? string.Empty) as MessagesListType?;
            switch (typeEnum)
            {
                case MessagesListType.Inbox:
                    _messages = ContentResolver.GetInboxSms();
                    break;
                case MessagesListType.Deleted:
                    _messages = await ContentResolver.GetDeletedSms();
                    break;
                default:
                    _messages = new List<Message>();
                    break;
            }

            if (_listView == null) return;
            _listView.Adapter = new MessageAdapter(this, _messages);
            _listView.ItemClick += OnListItemClick;
            _listView.ItemLongClick += (sender, args) =>
            {
                var message = _messages[args.Position];

                Toast.MakeText(Application.Context, $"Try to delete message id:{message.MsgId}.", ToastLength.Short)?.Show();
                var url = Android.Net.Uri.Parse("content://sms/" + message.MsgId);

                var ret = ContentResolver!.Delete(url, null, null);
                if (ret > 0)
                {
                    Toast.MakeText(Application.Context, $"Message id:{message.MsgId} deleted.", ToastLength.Short)?.Show();
                    new MessageDatabase().SaveMessageAsync(_messages[args.Position]);
                    _messages.RemoveAt(args.Position);
                    _listView.Adapter = new MessageAdapter(this, _messages);
                }
                else
                {
                    Toast.MakeText(Application.Context, $"Message id:{message.MsgId} not deleted. \r\nPlease check if the current APP is set as the default SMS program", ToastLength.Long)?.Show();
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

    internal enum MessagesListType
    {
        Inbox,
        Deleted
    }

    public class MessageAdapter : BaseAdapter<Message>
    {
        private readonly List<Message> _items;
        private readonly Activity _context;
        public MessageAdapter(Activity context, List<Message> items)
            : base()
        {
            _context = context;
            _items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Message this[int position] => _items[position];

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