using Microsoft.Data.Sqlite;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using Yugioh.Data.Constants;
using Yugioh.Data.Entities;
using Yugioh.Data.Utils;

namespace Yugioh.Data.Repositories
{
    public class ArtworkRepository : IArtworkRepository
    {
        public async Task<bool> HasArtworkAsync(int cardId, int ordinal, SqliteConnection connection, SqliteTransaction transaction)
        {
            string queryString =
            $@"
SELECT COUNT(*) 
FROM {TableConstants.Artwork} 
WHERE {nameof(ArtworkEntity.CardId)} = @cardId
AND {nameof(ArtworkEntity.Ordinal)} = @ordinal;";

            using (var command = new SqliteCommand(queryString, connection, transaction))
            {
                command.Parameters.AddWithValue("@cardId", cardId);
                command.Parameters.AddWithValue("@ordinal", ordinal);

                long artworkCount = (long)await command.ExecuteScalarAsync();

                return artworkCount > 0;
            }
        }

        public async Task<ArtworkEntity> FindArtworkAsync(int cardId, int ordinal, SqliteConnection connection, SqliteTransaction transaction)
        {
            string queryString =
            $@"
SELECT {nameof(ArtworkEntity.Image)}
FROM {TableConstants.Artwork} 
WHERE {nameof(ArtworkEntity.CardId)} = @cardId
AND {nameof(ArtworkEntity.Ordinal)} = @ordinal
LIMIT 1;";

            using (var command = new SqliteCommand(queryString, connection, transaction))
            {
                command.Parameters.AddWithValue("@cardId", cardId);
                command.Parameters.AddWithValue("@ordinal", ordinal);

                using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        Image image;

                        using (var readStream = reader.GetStream(0))
                        {
                            image = new Bitmap(Image.FromStream(readStream));
                        }

                        return new ArtworkEntity()
                        {
                            CardId = cardId,
                            Ordinal = ordinal,
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

        public async Task InsertArtworkAsync(ArtworkEntity artworkEntity, SqliteConnection connection, SqliteTransaction transaction)
        {
            string queryString =
            $@"
INSERT INTO {TableConstants.Artwork} 
(
{nameof(ArtworkEntity.CardId)},
{nameof(ArtworkEntity.Ordinal)},
{nameof(ArtworkEntity.Image)}
)
VALUES (@cardId,
        @ordinal,
        @image);";

            using (var command = new SqliteCommand(queryString, connection, transaction))
            {
                command.Parameters.AddWithValue("@cardId", artworkEntity.CardId);
                command.Parameters.AddWithValue("@ordinal", artworkEntity.Ordinal);

                byte[] imageBytes = ImageUtil.ToBytes(artworkEntity.Image);
                var imageParam = new SqliteParameter("@image", imageBytes);
                imageParam.Size = imageBytes.Length;
                imageParam.DbType = DbType.Binary;

                command.Parameters.Add(imageParam);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
