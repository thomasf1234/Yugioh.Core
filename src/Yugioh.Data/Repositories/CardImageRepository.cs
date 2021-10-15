using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using Yugioh.Data.Constants;
using Yugioh.Data.Entities;
using Yugioh.Data.Utils;

namespace Yugioh.Data.Repositories
{
    public class CardImageRepository : ICardImageRepository
    {
        public async Task<CardImageEntity> FindCardImageAsync(string code, SqliteConnection connection, SqliteTransaction transaction)
        {
            string queryString =
            $@"
SELECT {nameof(CardImageEntity.Image)}
FROM {TableConstants.CardImage} 
WHERE {nameof(CardImageEntity.Code)} = @code
LIMIT 1;";

            using (var command = new SqliteCommand(queryString, connection, transaction))
            {
                command.Parameters.AddWithValue("@code", code);

                using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        Image image;

                        using (var readStream = reader.GetStream(0))
                        {
                            image = new Bitmap(Image.FromStream(readStream));
                        }

                        return new CardImageEntity()
                        {
                            Code = code,
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

        public async Task<bool> HasCardImageAsync(string code, SqliteConnection connection, SqliteTransaction transaction)
        {
            string queryString =
            $@"
SELECT COUNT(*) 
FROM {TableConstants.CardImage} 
WHERE {nameof(CardImageEntity.Code)} = @code;";

            using (var command = new SqliteCommand(queryString, connection, transaction))
            {
                command.Parameters.AddWithValue("@code", code);

                long cardImageCount = (long)await command.ExecuteScalarAsync();

                return cardImageCount > 0;
            }
        }

        public async Task InsertCardImageAsync(CardImageEntity cardImageEntity, SqliteConnection connection, SqliteTransaction transaction)
        {
            string queryString =
            $@"
INSERT INTO {TableConstants.CardImage} 
(
{nameof(CardImageEntity.Code)},
{nameof(CardImageEntity.Image)}
)
VALUES (@code,
        @image);";

            using (var command = new SqliteCommand(queryString, connection, transaction))
            {
                command.Parameters.AddWithValue("@code", cardImageEntity.Code);

                byte[] imageBytes = ImageUtil.ToBytes(cardImageEntity.Image);
                var imageParam = new SqliteParameter("@image", imageBytes);
                imageParam.Size = imageBytes.Length;
                imageParam.DbType = DbType.Binary;

                command.Parameters.Add(imageParam);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
