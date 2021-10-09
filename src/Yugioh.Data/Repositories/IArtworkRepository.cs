using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using Yugioh.Data.Entities;

namespace Yugioh.Data.Repositories
{
    public interface IArtworkRepository
    {
        Task<bool> HasArtworkAsync(int cardId, int ordinal, SqliteConnection connection, SqliteTransaction transaction);
        Task<ArtworkEntity> FindArtworkAsync(int cardId, int ordinal, SqliteConnection connection, SqliteTransaction transaction);
        Task InsertArtworkAsync(ArtworkEntity artworkEntity, SqliteConnection connection, SqliteTransaction transaction);
    }
}
