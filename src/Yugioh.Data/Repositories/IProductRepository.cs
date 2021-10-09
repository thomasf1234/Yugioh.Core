using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yugioh.Data.Entities;

namespace Yugioh.Data.Repositories
{
    public interface IProductRepository
    {
        Task<List<ProductEntity>> AllProductsAsync(SqliteConnection connection, SqliteTransaction transaction);
        Task<ProductEntity> FindProductAsync(string productId, SqliteConnection connection, SqliteTransaction transaction);
        Task InsertProductAsync(ProductEntity productEntity, SqliteConnection connection, SqliteTransaction transaction);
    }
}
