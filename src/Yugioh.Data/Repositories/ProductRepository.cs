using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yugioh.Data.Constants;
using Yugioh.Data.Entities;

namespace Yugioh.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public async Task<List<ProductEntity>> AllProductsAsync(SqliteConnection connection, SqliteTransaction transaction)
        {
            var productEntities = new List<ProductEntity>();

            string queryString =
            $@"
SELECT {nameof(ProductEntity.ProductId)},
       {nameof(ProductEntity.Title)}, 
       {nameof(ProductEntity.SetSize)},
       {nameof(ProductEntity.LaunchDate)}
FROM {TableConstants.Product}";

            using (var command = new SqliteCommand(queryString, connection, transaction))
            {
                using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string productId = reader.GetString(0);
                        string title = reader.GetString(1);
                        int setSize = reader.GetInt32(2);
                        DateTime launchDate = reader.GetDateTime(3);

                        var productEntity = new ProductEntity()
                        {
                            ProductId = productId,
                            Title = title,
                            SetSize = setSize,
                            LaunchDate = launchDate
                        };

                        productEntities.Add(productEntity);
                    }
                }
            }

            return productEntities;
        }

        public async Task<ProductEntity> FindProductAsync(string productId, SqliteConnection connection, SqliteTransaction transaction)
        {
            string queryString =
            $@"
SELECT {nameof(ProductEntity.Title)}, 
       {nameof(ProductEntity.SetSize)},
       {nameof(ProductEntity.LaunchDate)}
FROM {TableConstants.Product} 
WHERE {nameof(ProductEntity.ProductId)} = @productId;";

            using (var command = new SqliteCommand(queryString, connection, transaction))
            {
                command.Parameters.AddWithValue("@productId", productId);

                using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        string title = reader.GetString(0);
                        int setSize = reader.GetInt32(1);
                        DateTime launchDate = reader.GetDateTime(2);

                        return new ProductEntity()
                        {
                            ProductId = productId,
                            Title = title,
                            SetSize = setSize,
                            LaunchDate = launchDate
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public async Task InsertProductAsync(ProductEntity productEntity, SqliteConnection connection, SqliteTransaction transaction)
        {
            string queryString =
            $@"
INSERT INTO {TableConstants.Product}  (
                  {nameof(ProductEntity.ProductId)},
                  {nameof(ProductEntity.Title)}, 
                  {nameof(ProductEntity.SetSize)},
                  {nameof(ProductEntity.LaunchDate)}
                  )
VALUES (@productId,
        @title,
        @setSize,
        @launchDate);";

            using (var command = new SqliteCommand(queryString, connection, transaction))
            {
                command.Parameters.AddWithValue("@productId", productEntity.ProductId);
                command.Parameters.AddWithValue("@title", productEntity.Title);
                command.Parameters.AddWithValue("@setSize", productEntity.SetSize);
                command.Parameters.AddWithValue("@launchDate", productEntity.LaunchDate);

                await command.ExecuteNonQueryAsync();
            }  
        }
    }
}
