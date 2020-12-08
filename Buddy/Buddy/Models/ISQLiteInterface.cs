using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteConnection))]

namespace Buddy.Models
{
    public interface ISQLiteInterface
    {
        SQLiteConnection GetConnection();
    }
}