using System;
using System.Linq;
using Yugioh.Data.Entities;
using Yugioh.Sync.Utils;

namespace Yugioh.Sync.Factories
{
    public class CardFactory
    {
        public CardEntity CreateCard(Konami.Entities.CardEntity konamiCardEntity, string passcode)
        {
            var cardEntity = new CardEntity();

            switch (konamiCardEntity.Attribute)
            {
                case "SPELL":
                    cardEntity.CardId = konamiCardEntity.CardId;
                    cardEntity.Type = Data.Entities.Type.Spell;
                    cardEntity.Name = konamiCardEntity.Name;
                    cardEntity.Attribute = Data.Entities.Attribute.Spell;
                    cardEntity.Property = (Data.Entities.Property)Enum.Parse(typeof(Data.Entities.Property), konamiCardEntity.Property.Replace('-', '_'));
                    cardEntity.Description = konamiCardEntity.Description;
                    break;
                case "TRAP":
                    cardEntity.CardId = konamiCardEntity.CardId;
                    cardEntity.Type = Data.Entities.Type.Trap;
                    cardEntity.Name = konamiCardEntity.Name;
                    cardEntity.Attribute = Data.Entities.Attribute.Trap;
                    cardEntity.Property = (Data.Entities.Property)Enum.Parse(typeof(Data.Entities.Property), konamiCardEntity.Property.Replace('-', '_'));
                    cardEntity.Description = konamiCardEntity.Description;
                    break;
                default:
                    cardEntity.CardId = konamiCardEntity.CardId;
                    cardEntity.Type = Data.Entities.Type.Monster;
                    cardEntity.Name = konamiCardEntity.Name;
                    cardEntity.Attribute = (Data.Entities.Attribute)Enum.Parse(typeof(Data.Entities.Attribute), StringUtil.Capitalize(konamiCardEntity.Attribute.ToLower()));
                    cardEntity.Race = konamiCardEntity.MonsterTypes.First();
                    cardEntity.MonsterTypes = 0;
                    cardEntity.Abilities = 0;
                    cardEntity.Attack = konamiCardEntity.Atk;
                    cardEntity.Description = konamiCardEntity.Description;

                    if (konamiCardEntity.Level != null)
                    {
                        cardEntity.Level = Convert.ToByte(konamiCardEntity.Level);
                        cardEntity.Defense = konamiCardEntity.Def;
                    }
                    else if (konamiCardEntity.Rank != null)
                    {
                        cardEntity.Rank = Convert.ToByte(konamiCardEntity.Rank);
                        cardEntity.Defense = konamiCardEntity.Def;
                    }
                    else if (konamiCardEntity.Link != null)
                    {
                        cardEntity.LinkRating = Convert.ToByte(konamiCardEntity.Link);
                        cardEntity.LinkArrows = 0;

                        foreach (var linkMarker in konamiCardEntity.LinkMarkers)
                        {
                            cardEntity.LinkArrows = cardEntity.LinkArrows | (Data.Entities.LinkArrows)Enum.Parse(typeof(Data.Entities.LinkArrows), linkMarker.ToString());
                        }
                    }

                    if (konamiCardEntity.PendulumScale != null)
                    {
                        cardEntity.PendulumScale = Convert.ToByte(konamiCardEntity.PendulumScale);
                        cardEntity.PendulumDescription = konamiCardEntity.PendulumDescription == null ? "" : konamiCardEntity.PendulumDescription;
                    }

                    foreach (string monsterType in Enum.GetNames(typeof(Data.Entities.MonsterTypes)))
                    {
                        if (konamiCardEntity.MonsterTypes.Contains(monsterType))
                        {
                            cardEntity.MonsterTypes = cardEntity.MonsterTypes | (Data.Entities.MonsterTypes)Enum.Parse(typeof(Data.Entities.MonsterTypes), monsterType);
                        }
                    }

                    foreach (string ability in Enum.GetNames(typeof(Data.Entities.Abilities)))
                    {
                        if (konamiCardEntity.MonsterTypes.Contains(ability))
                        {
                            cardEntity.Abilities = cardEntity.Abilities | (Data.Entities.Abilities)Enum.Parse(typeof(Data.Entities.Abilities), ability);
                        }
                    }

                    if (cardEntity.Description.StartsWith("FLIP:"))
                    {
                        cardEntity.Abilities = cardEntity.Abilities | Data.Entities.Abilities.Flip;
                    }
                    break;
            }

            return cardEntity;
        }
    }
}
