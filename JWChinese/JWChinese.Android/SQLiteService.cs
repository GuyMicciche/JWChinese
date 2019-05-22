using System;
using System.IO;
using Xamarin.Forms;
using JWChinese.Droid;
using SQLite;

[assembly: Dependency(typeof(SQLiteService))]
namespace JWChinese.Droid
{
    public class SQLiteService : ISQLiteService
    {
        public SQLiteService() { }

        private static string GetDatabasePath()
        {
            const string sqliteFilename = "storehouse.db3";

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);

            return path;
        }

        public SQLiteConnection GetConnection()
        {
            var dbPath = GetDatabasePath();

            return new SQLiteConnection(dbPath);
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            var dbPath = GetDatabasePath();

            return new SQLiteAsyncConnection(dbPath);
        }
    }
}