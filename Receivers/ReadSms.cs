using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using Android.Provider;
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
            var reqCols = new[]
            {
                //Telephony.Sms.Inbox.InterfaceConsts.Count,
                Telephony.Sms.Inbox.InterfaceConsts.Id,
                Telephony.Sms.Inbox.InterfaceConsts.Address,
                Telephony.Sms.Inbox.InterfaceConsts.Body,
                Telephony.Sms.Inbox.InterfaceConsts.Creator,
                Telephony.Sms.Inbox.InterfaceConsts.Date,
                Telephony.Sms.Inbox.InterfaceConsts.DateSent,
                //Telephony.Sms.Inbox.InterfaceConsts.ErrorCode,
               // Telephony.Sms.Inbox.InterfaceConsts.Locked,
                Telephony.Sms.Inbox.InterfaceConsts.Person,
                Telephony.Sms.Inbox.InterfaceConsts.Protocol,
                Telephony.Sms.Inbox.InterfaceConsts.Read,
                //Telephony.Sms.Inbox.InterfaceConsts.ReplyPathPresent,
                Telephony.Sms.Inbox.InterfaceConsts.Seen,
                Telephony.Sms.Inbox.InterfaceConsts.ServiceCenter,
                Telephony.Sms.Inbox.InterfaceConsts.Status,
                Telephony.Sms.Inbox.InterfaceConsts.Subject,
                Telephony.Sms.Inbox.InterfaceConsts.SubscriptionId,
                Telephony.Sms.Inbox.InterfaceConsts.ThreadId,
                Telephony.Sms.Inbox.InterfaceConsts.Type
            };
            var cursor = resolver?.Query(Telephony.Sms.Inbox.ContentUri!, reqCols, null, null, null);

            if (cursor == null || !cursor.MoveToFirst()) return items;
            do
            {
                var id = cursor.GetLong(0);
                //var count = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.Count));
                var messageId = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.Id));
                var address = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.Address));
                var msg = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.Body));
                var creator = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.Creator));
                var date = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.Date));
                var dateTime = cursor.GetLong(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.Date)).ToDateTime();
                var dateSent = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.DateSent));
                var dateSentTime = cursor.GetLong(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.DateSent)).ToDateTime();
                //var errorCode = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.ErrorCode));
                //var locked = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.Locked));
                var name = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.Person));
                var protocol = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.Protocol));
                var read = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.Read));
                //var replayPathPresent = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.ReplyPathPresent));
                var seen = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.Seen));
                var serviceCenter = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.ServiceCenter));
                var status = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.Status));
                System.Enum.TryParse(status, out SmsStatus statusEnum);
                var subject = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.Subject));
                var subscriptionId = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.SubscriptionId));
                var threadId = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.ThreadId));
                var type = cursor.GetString(cursor.GetColumnIndex(Telephony.Sms.Inbox.InterfaceConsts.Type));
                System.Enum.TryParse(type, out SmsMessageType typeEnum);

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