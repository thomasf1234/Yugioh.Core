using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using Yugioh.Data.Entities;

namespace Yugioh.Data.Repositories
{
    public interface ICardImageRepository
    {
        Task<bool> HasCardImageAsync(string code, SqliteConnection connection, SqliteTransaction transaction);
        Task<CardImageEntity> FindCardImageAsync(string code, SqliteConnection connection, SqliteTransaction transaction);
        Task InsertCardImageAsync(CardImageEntity cardImageEntity, SqliteConnection connection, SqliteTransaction transaction);
    }
}
