using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using Yugioh.Data.Entities;

namespace Yugioh.Data.Repositories
{
    public interface ICardSetRepository
    {
        Task InsertCardSetAsync(CardSetEntity cardSetEntity, SqliteConnection connection);
    }
}
