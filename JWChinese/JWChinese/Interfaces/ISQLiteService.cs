using SQLite;

namespace JWChinese
{
    public interface ISQLiteService
    {
        SQLiteConnection GetConnection();
        SQLiteAsyncConnection GetAsyncConnection();
    }
}