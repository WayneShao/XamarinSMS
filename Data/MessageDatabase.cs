using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android.Provider;
using SQLite;
using XamarinSMS.Models;

namespace XamarinSMS.Data
{
    public class MessageDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public MessageDatabase()
        {
            _database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Messages.db3"));
            _database.CreateTableAsync<Message>().Wait();
        }

        public Task<List<Message>> GetMessagesAsync()
        {
            //Get all messages.
            return _database.Table<Message>().ToListAsync();
        }

        public Task<Message> GetMessageAsync(long id)
        {
            // Get a specific message.
            return _database.Table<Message>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveMessageAsync(Message message)
        {
            if (message.Id != 0)
            {
                // Update an existing message.
                return _database.UpdateAsync(message);
            }
            else
            {
                // Save a new message.
                return _database.InsertAsync(message);
            }
        }

        public Task<int> DeleteMessageAsync(Message message)
        {
            // Delete a message.
            return _database.DeleteAsync(message);
        }
    }
}