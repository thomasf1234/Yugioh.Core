using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yugioh.Data.Entities;

namespace Yugioh.Data.Repositories
{
    public interface IProductCardRepository
    {
        Task<List<ProductCardEntity>> FindByCardIdAsync(int cardId, SqliteConnection connection, SqliteTransaction transaction);
        Task<List<ProductCardEntity>> FindProductCardsAsync(string productId, SqliteConnection connection, SqliteTransaction transaction);
        Task<ProductCardEntity> FindProductCardAsync(string productId, SqliteConnection connection, SqliteTransaction transaction);
        Task InsertProductCardAsync(ProductCardEntity productCardEntity, SqliteConnection connection, SqliteTransaction transaction);
    }
}
