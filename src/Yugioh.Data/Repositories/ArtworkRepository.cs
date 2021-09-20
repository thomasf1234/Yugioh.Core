using Microsoft.Data.Sqlite;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using Yugioh.Data.Entities;
using Yugioh.Data.Utils;

namespace Yugioh.Data.Repositories
{
    public class ArtworkRepository : IArtworkRepository
    {
        public async Task<bool> HasArtworkAsync(int artworkId, SqliteConnection connection)
        {
            string queryString =
            $@"
SELECT COUNT(*) FROM Artwork 
WHERE {nameof(ArtworkEntity.ArtworkId)} = @artworkId;";

            using (var command = new SqliteCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@artworkId", artworkId);

                long artworkCount = (long)await command.ExecuteScalarAsync();

                return artworkCount > 0;
            }
        }

        public async Task<ArtworkEntity> FindArtworkAsync(int artworkId, SqliteConnection connection)
        {
            string queryString =
            $@"
SELECT {nameof(ArtworkEntity.CardId)},
       {nameof(ArtworkEntity.Alternate)},
       {nameof(ArtworkEntity.Image)}
FROM Artwork 
WHERE {nameof(ArtworkEntity.ArtworkId)} = @artworkId;";

            using (var command = new SqliteCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@artworkId", artworkId);

                using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        int cardId = reader.GetInt32(0);
                        bool alternate = reader.GetBoolean(1);
                        Image image;

                        using (var readStream = reader.GetStream(2))
                        {
                            image = Image.FromStream(reader.GetStream(2));
                        }

                        return new ArtworkEntity()
                        {
                            ArtworkId = artworkId,
                            CardId = cardId,
                            Alternate = alternate,
                            Image = image
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public async Task InsertArtworkAsync(ArtworkEntity artworkEntity, SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText =
            $@"
INSERT INTO Artwork ({nameof(ArtworkEntity.ArtworkId)}, 
                     {nameof(ArtworkEntity.CardId)},
                     {nameof(ArtworkEntity.Alternate)},
                     {nameof(ArtworkEntity.Image)}
                     )
VALUES (@artworkId,
        @cardId,
        @alternate,
        @image);";

            command.Parameters.AddWithValue("@artworkId", artworkEntity.ArtworkId);
            command.Parameters.AddWithValue("@cardId", artworkEntity.CardId);
            command.Parameters.AddWithValue("@alternate", artworkEntity.Alternate);

            byte[] imageBytes = ImageUtil.ToBytes(artworkEntity.Image);
            var imageParam = new SqliteParameter("@image", imageBytes);
            imageParam.Size = imageBytes.Length;
            imageParam.DbType = DbType.Binary;

            command.Parameters.Add(imageParam);

            await command.ExecuteNonQueryAsync();
        }
    }
}
