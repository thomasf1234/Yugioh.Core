using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yugioh.Data.Entities;
using Yugioh.Data.Extensions;

namespace Yugioh.Data.Repositories
{
    public class CardRepository : ICardRepository
    {
        public async Task<List<int>> AllCardIdsAsync(SqliteConnection connection, SqliteTransaction transaction)
        {
            string queryString =
            $@"
SELECT {nameof(CardEntity.CardId)}
FROM Card;";

            var cardIds = new List<int>();
            using (var command = new SqliteCommand(queryString, connection, transaction))
            {
                using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                {
                    while(await reader.ReadAsync())
                    {
                        int cardId = reader.GetInt32(0);

                        cardIds.Add(cardId);
                    }
                }
            }

            return cardIds;
        }

        public async Task<CardEntity> FindCardAsync(int cardId, SqliteConnection connection, SqliteTransaction transaction)
        {
            string queryString =
            $@"
SELECT {nameof(CardEntity.Type)},
       {nameof(CardEntity.Name)},
       {nameof(CardEntity.Attribute)},
       {nameof(CardEntity.Level)},
       {nameof(CardEntity.Rank)},
       {nameof(CardEntity.LinkRating)},
       {nameof(CardEntity.LinkArrows)},
       {nameof(CardEntity.PendulumScale)},
       {nameof(CardEntity.PendulumDescription)},
       {nameof(CardEntity.Property)},
       {nameof(CardEntity.MonsterTypes)},
       {nameof(CardEntity.Race)},
       {nameof(CardEntity.Abilities)},
       {nameof(CardEntity.Attack)},
       {nameof(CardEntity.Defense)},
       {nameof(CardEntity.Description)}
FROM Card 
WHERE {nameof(CardEntity.CardId)} = @cardId;";

            using (var command = new SqliteCommand(queryString, connection, transaction))
            {
                command.Parameters.AddWithValue("@cardId", cardId);

                using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        Entities.Type type = (Entities.Type)reader.GetByte(reader.GetOrdinal(nameof(CardEntity.Type)));
                        string name = reader.GetString(reader.GetOrdinal(nameof(CardEntity.Name)));
                        Entities.Attribute attribute = (Entities.Attribute)reader.GetByte(reader.GetOrdinal(nameof(CardEntity.Attribute)));
                        byte? level = reader[nameof(CardEntity.Level)] == DBNull.Value ? (byte?)null : reader.GetByte(reader.GetOrdinal(nameof(CardEntity.Level)));
                        byte? rank = reader[nameof(CardEntity.Rank)] == DBNull.Value ? (byte?)null : reader.GetByte(reader.GetOrdinal(nameof(CardEntity.Rank)));
                        byte? linkRating = reader[nameof(CardEntity.LinkRating)] == DBNull.Value ? (byte?)null : reader.GetByte(reader.GetOrdinal(nameof(CardEntity.LinkRating)));
                        Entities.LinkArrows? linkArrows = reader[nameof(CardEntity.LinkArrows)] == DBNull.Value ? (Entities.LinkArrows?)null : (Entities.LinkArrows)reader.GetByte(reader.GetOrdinal(nameof(CardEntity.LinkArrows)));
                        byte? pendulumScale = reader[nameof(CardEntity.PendulumScale)] == DBNull.Value ? (byte?)null : reader.GetByte(reader.GetOrdinal(nameof(CardEntity.PendulumScale)));
                        string pendulumDescription = reader[nameof(CardEntity.PendulumDescription)] == DBNull.Value ? null : reader.GetString(reader.GetOrdinal(nameof(CardEntity.PendulumDescription))).Replace("\n", Environment.NewLine); ;
                        Entities.Property? property = reader[nameof(CardEntity.Property)] == DBNull.Value ? (Entities.Property?)null : (Entities.Property)reader.GetByte(reader.GetOrdinal(nameof(CardEntity.Property)));
                        Entities.MonsterTypes? monsterTypes = reader[nameof(CardEntity.MonsterTypes)] == DBNull.Value ? (Entities.MonsterTypes?)null : (Entities.MonsterTypes)reader.GetByte(reader.GetOrdinal(nameof(CardEntity.MonsterTypes)));
                        string race = reader[nameof(CardEntity.Race)] == DBNull.Value ? null : reader.GetString(reader.GetOrdinal(nameof(CardEntity.Race)));
                        Entities.Abilities? abilities = reader[nameof(CardEntity.Abilities)] == DBNull.Value ? (Entities.Abilities?)null : (Entities.Abilities)reader.GetByte(reader.GetOrdinal(nameof(CardEntity.Abilities)));
                        string attack = reader[nameof(CardEntity.Attack)] == DBNull.Value ? null : reader.GetString(reader.GetOrdinal(nameof(CardEntity.Attack)));
                        string defense = reader[nameof(CardEntity.Defense)] == DBNull.Value ? null : reader.GetString(reader.GetOrdinal(nameof(CardEntity.Defense)));
                        string description = reader.GetString(reader.GetOrdinal(nameof(CardEntity.Description))).Replace("\n", Environment.NewLine);

                        return new CardEntity()
                        {
                            CardId = cardId,
                            Type = type,
                            Name = name,
                            Attribute = attribute,
                            Level = level,
                            Rank = rank,
                            LinkRating = linkRating,
                            LinkArrows = linkArrows,
                            PendulumScale = pendulumScale,
                            PendulumDescription = pendulumDescription,
                            Property = property,
                            MonsterTypes = monsterTypes,
                            Race = race,
                            Abilities = abilities,
                            Attack = attack,
                            Defense = defense,
                            Description = description
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public async Task InsertCardAsync(CardEntity cardEntity, SqliteConnection connection, SqliteTransaction transaction)
        {            
            string queryString =
            $@"
INSERT INTO Card ({nameof(CardEntity.CardId)}, 
                  {nameof(CardEntity.Type)},
                  {nameof(CardEntity.Name)},
                  {nameof(CardEntity.Attribute)},
                  {nameof(CardEntity.Level)},
                  {nameof(CardEntity.Rank)},
                  {nameof(CardEntity.LinkRating)},
                  {nameof(CardEntity.LinkArrows)},
                  {nameof(CardEntity.PendulumScale)},
                  {nameof(CardEntity.PendulumDescription)},
                  {nameof(CardEntity.Property)},
                  {nameof(CardEntity.MonsterTypes)},
                  {nameof(CardEntity.Race)},
                  {nameof(CardEntity.Abilities)},
                  {nameof(CardEntity.Attack)},
                  {nameof(CardEntity.Defense)},
                  {nameof(CardEntity.Description)}
                  )
VALUES (@cardId,
        @type,
        @name,
        @attribute,
        @level,
        @rank,
        @linkRating,
        @linkArrows,
        @pendulumScale,
        @pendulumDescription,
        @property,
        @monsterTypes,
        @race,
        @abilities,
        @attack,
        @defense,
        @description);";

            using (var command = new SqliteCommand(queryString, connection, transaction))
            {
                command.Parameters.AddWithValue("@cardId", cardEntity.CardId);
                command.Parameters.AddWithValue("@type", cardEntity.Type);
                command.Parameters.AddWithValue("@name", cardEntity.Name);
                command.Parameters.AddWithValue("@attribute", cardEntity.Attribute);
                command.Parameters.AddWithNullableValue("@level", cardEntity.Level);
                command.Parameters.AddWithNullableValue("@rank", cardEntity.Rank);
                command.Parameters.AddWithNullableValue("@linkRating", cardEntity.LinkRating);
                command.Parameters.AddWithNullableValue("@linkArrows", cardEntity.LinkArrows);
                command.Parameters.AddWithNullableValue("@pendulumScale", cardEntity.PendulumScale);

                if (cardEntity.PendulumDescription != null)
                {
                    cardEntity.PendulumDescription = cardEntity.PendulumDescription.Replace("\r\n", "\n");
                }

                command.Parameters.AddWithNullableValue("@pendulumDescription", cardEntity.PendulumDescription);
                command.Parameters.AddWithNullableValue("@property", cardEntity.Property);
                command.Parameters.AddWithNullableValue("@monsterTypes", cardEntity.MonsterTypes);
                command.Parameters.AddWithNullableValue("@race", cardEntity.Race);
                command.Parameters.AddWithNullableValue("@abilities", cardEntity.Abilities);
                command.Parameters.AddWithNullableValue("@attack", cardEntity.Attack);
                command.Parameters.AddWithNullableValue("@defense", cardEntity.Defense);
                command.Parameters.AddWithValue("@description", cardEntity.Description.Replace("\r\n", "\n"));

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
