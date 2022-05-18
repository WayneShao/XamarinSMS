using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BuiltInViews
{
    public class MessageItem
    {
        public long Id { get; set; }
        public string From { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public string ThreadId { get; set; }
        public string Address { get; set; }
        public string Person { get; set; }
        public string Date { get; set; }
        public string Body { get; set; }
        public string Type { get; set; }
    }
}