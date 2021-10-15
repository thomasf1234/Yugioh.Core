using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yugioh.Data.Constants;
using Yugioh.Data.Entities;

namespace Yugioh.Data.Repositories
{
    public class ProductCardRepository : IProductCardRepository
    {
        public async Task<List<ProductCardEntity>> FindProductCardsAsync(string productId, SqliteConnection connection, SqliteTransaction transaction)
        {
            var productCardEntities = new List<ProductCardEntity>();

            string queryString =
            $@"
SELECT {nameof(ProductCardEntity.Code)}, 
       {nameof(ProductCardEntity.CardId)},
       {nameof(ProductCardEntity.ArtworkOrdinal)},
       {nameof(ProductCardEntity.Rarity)},
       {nameof(ProductCardEntity.Passcode)}
FROM {TableConstants.ProductCard} 
WHERE {nameof(ProductCardEntity.ProductId)} = @productId
ORDER BY {nameof(ProductCardEntity.Code)} ASC;";

            using (var command = new SqliteCommand(queryString, connection, transaction))
            {
                command.Parameters.AddWithValue("@productId", productId);

                using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string code = reader.GetString(0);
                        int cardId = reader.GetInt32(1);
                        int artworkOrdinal = reader.GetInt32(2);
                        string rarity = reader.GetString(3);
                        string passcode = reader[nameof(ProductCardEntity.Passcode)] == DBNull.Value ? null : reader.GetString(reader.GetOrdinal(nameof(ProductCardEntity.Passcode)));

                        var productCardEntity = new ProductCardEntity()
                        {
                            Code = code,
                            ProductId = productId,
                            CardId = cardId,
                            ArtworkOrdinal = artworkOrdinal,
                            Rarity = rarity,
                            Passcode = passcode
                        };

                        productCardEntities.Add(productCardEntity);
                    }
                }
            }

            return productCardEntities;
        }

        public async Task<ProductCardEntity> FindProductCardAsync(string code, SqliteConnection connection, SqliteTransaction transaction)
        {
            string queryString =
            $@"
SELECT {nameof(ProductCardEntity.ProductId)}, 
       {nameof(ProductCardEntity.CardId)},
       {nameof(ProductCardEntity.ArtworkOrdinal)},
       {nameof(ProductCardEntity.Rarity)}
FROM {TableConstants.ProductCard} 
WHERE {nameof(ProductCardEntity.Code)} = @code;";

            using (var command = new SqliteCommand(queryString, connection, transaction))
            {
                command.Parameters.AddWithValue("@code", code);

                using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        string productId = reader.GetString(0);
                        int cardId = reader.GetInt32(1);
                        int artworkOrdinal = reader.GetInt32(2);
                        string rarity = reader.GetString(3);
                        string passcode = reader[nameof(ProductCardEntity.Passcode)] == DBNull.Value ? null : reader.GetString(reader.GetOrdinal(nameof(ProductCardEntity.Passcode)));

                        return new ProductCardEntity()
                        {
                            Code = code,
                            ProductId = productId,
                            CardId = cardId,
                            ArtworkOrdinal = artworkOrdinal,
                            Rarity = rarity,
                            Passcode = passcode
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public async Task InsertProductCardAsync(ProductCardEntity productCardEntity, SqliteConnection connection, SqliteTransaction transaction)
        {
            string queryString =
            $@"
INSERT INTO {TableConstants.ProductCard}  (
                  {nameof(ProductCardEntity.Code)}, 
                  {nameof(ProductCardEntity.ProductId)}, 
                  {nameof(ProductCardEntity.CardId)},
                  {nameof(ProductCardEntity.ArtworkOrdinal)},
                  {nameof(ProductCardEntity.Rarity)},
                  {nameof(ProductCardEntity.Passcode)}
                  )
VALUES (@code,
        @productId,
        @cardId,
        @artworkOrdinal,
        @rarity,
        @passcode);";

            using (var command = new SqliteCommand(queryString, connection, transaction))
            {
                command.Parameters.AddWithValue("@code", productCardEntity.Code);
                command.Parameters.AddWithValue("@productId", productCardEntity.ProductId);
                command.Parameters.AddWithValue("@cardId", productCardEntity.CardId);
                command.Parameters.AddWithValue("@artworkOrdinal", productCardEntity.ArtworkOrdinal);
                command.Parameters.AddWithValue("@rarity", productCardEntity.Rarity);
                command.Parameters.AddWithValue("@passcode", productCardEntity.Passcode);

                await command.ExecuteNonQueryAsync();
            }           
        }

        public async Task<List<ProductCardEntity>> FindByCardIdAsync(int cardId, SqliteConnection connection, SqliteTransaction transaction)
        {
            var productCardEntities = new List<ProductCardEntity>();

            string queryString =
            $@"
SELECT {nameof(ProductCardEntity.Code)}, 
       {nameof(ProductCardEntity.ProductId)},
       {nameof(ProductCardEntity.ArtworkOrdinal)},
       {nameof(ProductCardEntity.Rarity)},
       {nameof(ProductCardEntity.Passcode)}
FROM {TableConstants.ProductCard} 
WHERE {nameof(ProductCardEntity.CardId)} = @cardId
ORDER BY {nameof(ProductCardEntity.Code)} ASC;";

            using (var command = new SqliteCommand(queryString, connection, transaction))
            {
                command.Parameters.AddWithValue("@cardId", cardId);

                using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string code = reader.GetString(0);
                        string productId = reader.GetString(1);
                        int artworkOrdinal = reader.GetInt32(2);
                        string rarity = reader.GetString(3);
                        string passcode = reader[nameof(ProductCardEntity.Passcode)] == DBNull.Value ? null : reader.GetString(reader.GetOrdinal(nameof(ProductCardEntity.Passcode)));

                        var productCardEntity = new ProductCardEntity()
                        {
                            Code = code,
                            ProductId = productId,
                            CardId = cardId,
                            ArtworkOrdinal = artworkOrdinal,
                            Rarity = rarity,
                            Passcode = passcode
                        };

                        productCardEntities.Add(productCardEntity);
                    }
                }
            }

            return productCardEntities;
        }
    }
}
