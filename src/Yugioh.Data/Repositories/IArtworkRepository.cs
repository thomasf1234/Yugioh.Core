using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using Yugioh.Data.Entities;

namespace Yugioh.Data.Repositories
{
    public interface IArtworkRepository
    {
        Task<bool> HasArtworkAsync(int artworkId, SqliteConnection connection);
        Task<ArtworkEntity> FindArtworkAsync(int artworkId, SqliteConnection connection);
        Task InsertArtworkAsync(ArtworkEntity artworkEntity, SqliteConnection connection);
    }
}
