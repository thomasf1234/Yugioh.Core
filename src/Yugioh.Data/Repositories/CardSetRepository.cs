using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using Yugioh.Data.Entities;

namespace Yugioh.Data.Repositories
{
    public class CardSetRepository : ICardSetRepository
    {
        public async Task InsertCardSetAsync(CardSetEntity cardSetEntity, SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText =
            $@"
INSERT INTO CardSet ({nameof(CardSetEntity.CardId)}, 
                  {nameof(CardSetEntity.ArtworkId)},
                  {nameof(CardSetEntity.Number)}
                  )
VALUES (@cardId,
        @artworkId,
        @number);";

            command.Parameters.AddWithValue("@cardId", cardSetEntity.CardId);
            command.Parameters.AddWithValue("@artworkId", cardSetEntity.ArtworkId);
            command.Parameters.AddWithValue("@number", cardSetEntity.Number);

            await command.ExecuteNonQueryAsync();
        }
    }
}
