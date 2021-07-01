using System.Data.SQLite;

namespace PoEAA_ActiveRecord
{
    public static class DbManager
    {
        public static SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection("Data Source=poeaa_activerecord.db");
        }
    }
}