using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yugioh.Data.Entities;

namespace Yugioh.Data.Repositories
{
    public interface ICardRepository
    {
        Task<List<int>> AllCardIdsAsync(SqliteConnection connection, SqliteTransaction transaction);
        Task<CardEntity> FindCardAsync(int cardId, SqliteConnection connection, SqliteTransaction transaction);
        Task InsertCardAsync(CardEntity cardEntity, SqliteConnection connection, SqliteTransaction transaction);
    }
}
