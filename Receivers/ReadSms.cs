using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using XamarinSMS.Data;
using XamarinSMS.Models;
using XamarinSMS.Utils;

namespace XamarinSMS.Receivers
{
    public static class ReadSms
    {
        public static async Task<List<Message>> GetDeletedSms(this ContentResolver resolver)
        {
            var items = new List<Message>();
            return await new MessageDatabase().GetMessagesAsync();
        }
        public static List<Message> GetInboxSms(this ContentResolver resolver)
        {

            var items = new List<Message>();
            const string inbox = "content://sms/inbox";
            var reqCols = new[] { "_id", "thread_id", "address", "person", "date", "body", "type" };
            var uri = Android.Net.Uri.Parse(inbox);
            var cursor = resolver?.Query(uri, reqCols, null, null, null);

            if (cursor == null || !cursor.MoveToFirst()) return items;
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
                var dateTime = cursor.GetLong(cursor.GetColumnIndex(reqCols[4])).ToDateTime();

                //_items.Add(messageId + "," + threadId + "," + address + "," + name + "," + date + " ," + msg + " ," + type);

                items.Add(new Message
                {
                    Content = msg,
                    Time = dateTime,
                    MsgId = id,
                    From = address,
                    Address = address,
                    Body = msg,
                    Date = date,
                    Person = name,
                    ThreadId = threadId,
                    Type = type
                });

                //ContentResolver.Delete(Android.Net.Uri.Parse("content://sms/" + messageId), null, null);
            } while (cursor.MoveToNext());


            return items;
        }

    }
}