using System;
using SQLite;

namespace XamarinSMS.Models
{
    public class Message
    {

        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public long MsgId { get; set; }
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