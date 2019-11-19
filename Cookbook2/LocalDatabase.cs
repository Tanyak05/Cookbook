using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using Environment = System.Environment;

namespace Cookbook2
{

    public interface IDatabaseType
    {
        string Id { get; set; }
    }

    public class LocalDatabase
    {

        private static LocalDatabase database;

        public static LocalDatabase Database =>
            database ?? (database = new LocalDatabase(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "CoocbookSQLite.db3")));

        readonly SQLiteAsyncConnection connectionAsync;

        public LocalDatabase(string dbPath)
        {
            connectionAsync = new SQLiteAsyncConnection(dbPath);
            connectionAsync.Trace = true;
            //connection = new SQLiteConnection(dbPath);
        }

        public void CreateTable<DBType>() where DBType : IDatabaseType, new()
        {
            connectionAsync.CreateTableAsync<DBType>().Wait();
        }

        public Task<List<DBType>> GetItemsAsync<DBType>() where DBType : IDatabaseType, new()
        {
            return connectionAsync.Table<DBType>().ToListAsync();
        }

        //public Task<List<DBType>> GetItemsNotDoneAsync<DBType>() where DBType : IDatabaseType, new()
        //{
        //    return connectionAsync.QueryAsync<DBType>("SELECT * FROM [DBType] WHERE [Done] = 0");
        //}

        public Task<DBType> GetItemAsync<DBType>(string id) where DBType : IDatabaseType, new()
        {
            return connectionAsync.Table<DBType>().Where(i => i.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync<DBType>(DBType item) where DBType : IDatabaseType, new()
        {
            if (string.IsNullOrEmpty(item.Id))
            {
                return connectionAsync.UpdateAsync(item);
            }
            else
            {
                return connectionAsync.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync<DBType>(DBType item)
        {
            return connectionAsync.DeleteAsync(item);
        }
    }



}
