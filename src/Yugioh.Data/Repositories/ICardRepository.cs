using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using Yugioh.Data.Entities;

namespace Yugioh.Data.Repositories
{
    public interface ICardRepository
    {
        Task<CardEntity> FindCardAsync(int cardId, SqliteConnection connection);
        Task InsertCardAsync(CardEntity cardEntity, SqliteConnection connection);
    }
}
