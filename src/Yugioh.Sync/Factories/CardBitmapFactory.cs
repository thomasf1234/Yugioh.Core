using System;
using System.Drawing;
using System.Threading.Tasks;
using Yugioh.Data.Entities;
using Yugioh.Draw.Builders;
using Yugioh.Draw.Repositories;

namespace Yugioh.Sync.Factories
{
    public class CardBitmapFactory
    {
        private readonly IResourceRepository _resourceRepository;
        public CardBitmapFactory(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }
        public async Task<Bitmap> CreateCardBitmapAsync(CardEntity cardEntity, string cardNumber, string passcode, ArtworkEntity artworkEntity)
        {
            if (cardEntity.Type != Data.Entities.Type.Monster)
            {
                var frame = cardEntity.Type == Data.Entities.Type.Spell ? Frame.Spell : Frame.Trap;
                
                return new CardBuilder(_resourceRepository, frame, artworkEntity.Image)
                .AddAttribute((Draw.Builders.Attribute)cardEntity.Attribute)
                .AddName(cardEntity.Name, Brushes.White)
                .AddProperty((Draw.Builders.Property)cardEntity.Property)
                .AddNumber(cardNumber, Brushes.Black)
                .AddDescription(cardEntity.Description, Brushes.Black)
                .AddPasscode(passcode, Brushes.Black)
                .AddEditionAndHologram(Edition.Unlimited, Brushes.Black)
                .AddCreator(Creator.StudioDice, Brushes.Black)
                .Build();
            }

            var monsterTypes = (Data.Entities.MonsterTypes)cardEntity.MonsterTypes;

            if (cardEntity.PendulumScale != null)
            {
                Frame frame;

                if (monsterTypes.HasFlag(MonsterTypes.Fusion))
                {
                    return new CardBuilder(_resourceRepository, Frame.PendulumFusion, artworkEntity.Image)
                        .AddAttribute((Draw.Builders.Attribute)cardEntity.Attribute)
                        .AddName(cardEntity.Name, Brushes.Black)
                        .AddLevel((Draw.Builders.Level)cardEntity.Level)
                        .AddPendulumScale((Draw.Builders.PendulumScale)cardEntity.PendulumScale)
                        .AddNumber(cardNumber, Brushes.Black)
                        .AddMonsterType(cardEntity.GetDisplayedTypes())
                        .AddDescription(cardEntity.Description, Brushes.Black, true)
                        .AddPendulumDescription(cardEntity.PendulumDescription, Brushes.Black)
                        .AddAtkAndDef(cardEntity.Attack, cardEntity.Defense, Brushes.Black)
                        .AddPasscode(passcode, Brushes.Black)
                        .AddEditionAndHologram(Edition.Unlimited, Brushes.Black)
                        .AddCreator(Creator.StudioDice, Brushes.Black)
                        .Build();
                }
                else if (monsterTypes.HasFlag(MonsterTypes.Ritual))
                {
                    return new CardBuilder(_resourceRepository, Frame.PendulumRitual, artworkEntity.Image)
                        .AddAttribute((Draw.Builders.Attribute)cardEntity.Attribute)
                        .AddName(cardEntity.Name, Brushes.Black)
                        .AddLevel((Draw.Builders.Level)cardEntity.Level)
                        .AddPendulumScale((Draw.Builders.PendulumScale)cardEntity.PendulumScale)
                        .AddNumber(cardNumber, Brushes.Black)
                        .AddMonsterType(cardEntity.GetDisplayedTypes())
                        .AddDescription(cardEntity.Description, Brushes.Black)
                        .AddPendulumDescription(cardEntity.PendulumDescription, Brushes.Black)
                        .AddAtkAndDef(cardEntity.Attack, cardEntity.Defense, Brushes.Black)
                        .AddPasscode(passcode, Brushes.Black)
                        .AddEditionAndHologram(Edition.Unlimited, Brushes.Black)
                        .AddCreator(Creator.StudioDice, Brushes.Black)
                        .Build();
                }
                else if (monsterTypes.HasFlag(MonsterTypes.Synchro))
                {
                    return new CardBuilder(_resourceRepository, Frame.PendulumSynchro, artworkEntity.Image)
                        .AddAttribute((Draw.Builders.Attribute)cardEntity.Attribute)
                        .AddName(cardEntity.Name, Brushes.Black)
                        .AddLevel((Draw.Builders.Level)cardEntity.Level)
                        .AddPendulumScale((Draw.Builders.PendulumScale)cardEntity.PendulumScale)
                        .AddNumber(cardNumber, Brushes.Black)
                        .AddMonsterType(cardEntity.GetDisplayedTypes())
                        .AddDescription(cardEntity.Description, Brushes.Black, true)
                        .AddPendulumDescription(cardEntity.PendulumDescription, Brushes.Black)
                        .AddAtkAndDef(cardEntity.Attack, cardEntity.Defense, Brushes.Black)
                        .AddPasscode(passcode, Brushes.Black)
                        .AddEditionAndHologram(Edition.Unlimited, Brushes.Black)
                        .AddCreator(Creator.StudioDice, Brushes.Black)
                        .Build();
                }
                else if (monsterTypes.HasFlag(MonsterTypes.Xyz))
                {
                    return new CardBuilder(_resourceRepository, Frame.PendulumXyz, artworkEntity.Image)
                        .AddAttribute((Draw.Builders.Attribute)cardEntity.Attribute)
                        .AddName(cardEntity.Name, Brushes.Black)
                        .AddRank((Draw.Builders.Rank)cardEntity.Rank)
                        .AddPendulumScale((Draw.Builders.PendulumScale)cardEntity.PendulumScale)
                        .AddNumber(cardNumber, Brushes.Black)
                        .AddMonsterType(cardEntity.GetDisplayedTypes())
                        .AddDescription(cardEntity.Description, Brushes.Black, true)
                        .AddPendulumDescription(cardEntity.PendulumDescription, Brushes.Black)
                        .AddAtkAndDef(cardEntity.Attack, cardEntity.Defense, Brushes.Black)
                        .AddPasscode(passcode, Brushes.Black)
                        .AddEditionAndHologram(Edition.Unlimited, Brushes.Black)
                        .AddCreator(Creator.StudioDice, Brushes.Black)
                        .Build();
                }
                else if (monsterTypes.HasFlag(MonsterTypes.Link))
                {
                    throw new Exception("Pendulum Link monsters not supported");
                }
                else if (monsterTypes.HasFlag(MonsterTypes.Effect))
                {
                    return new CardBuilder(_resourceRepository, Frame.PendulumEffect, artworkEntity.Image)
                        .AddAttribute((Draw.Builders.Attribute)cardEntity.Attribute)
                        .AddName(cardEntity.Name, Brushes.Black)
                        .AddLevel((Draw.Builders.Level)cardEntity.Level)
                        .AddPendulumScale((Draw.Builders.PendulumScale)cardEntity.PendulumScale)
                        .AddNumber(cardNumber, Brushes.Black)
                        .AddMonsterType(cardEntity.GetDisplayedTypes())
                        .AddDescription(cardEntity.Description, Brushes.Black)
                        .AddPendulumDescription(cardEntity.PendulumDescription, Brushes.Black)
                        .AddAtkAndDef(cardEntity.Attack, cardEntity.Defense, Brushes.Black)
                        .AddPasscode(passcode, Brushes.Black)
                        .AddEditionAndHologram(Edition.Unlimited, Brushes.Black)
                        .AddCreator(Creator.StudioDice, Brushes.Black)
                        .Build();
                }
                else
                {
                    return new CardBuilder(_resourceRepository, Frame.PendulumNormal, artworkEntity.Image)
                        .AddAttribute((Draw.Builders.Attribute)cardEntity.Attribute)
                        .AddName(cardEntity.Name, Brushes.Black)
                        .AddLevel((Draw.Builders.Level)cardEntity.Level)
                        .AddPendulumScale((Draw.Builders.PendulumScale)cardEntity.PendulumScale)
                        .AddNumber(cardNumber, Brushes.Black)
                        .AddMonsterType(cardEntity.GetDisplayedTypes())
                        .AddDescription(cardEntity.Description, Brushes.Black)
                        .AddPendulumDescription(cardEntity.PendulumDescription, Brushes.Black)
                        .AddAtkAndDef(cardEntity.Attack, cardEntity.Defense, Brushes.Black)
                        .AddPasscode(passcode, Brushes.Black)
                        .AddEditionAndHologram(Edition.Unlimited, Brushes.Black)
                        .AddCreator(Creator.StudioDice, Brushes.Black)
                        .Build();
                }
            }

            if (monsterTypes.HasFlag(MonsterTypes.Fusion))
            {
                // Fusion monster
                return new CardBuilder(_resourceRepository, Frame.Fusion, artworkEntity.Image)
                .AddAttribute((Draw.Builders.Attribute)cardEntity.Attribute)
                .AddName(cardEntity.Name, Brushes.Black)
                .AddLevel((Draw.Builders.Level)cardEntity.Level)
                .AddNumber(cardNumber, Brushes.Black)
                .AddMonsterType(cardEntity.GetDisplayedTypes())
                .AddDescription(cardEntity.Description, Brushes.Black, true)
                .AddAtkAndDef(cardEntity.Attack, cardEntity.Defense, Brushes.Black)
                .AddPasscode(passcode, Brushes.Black)
                .AddEditionAndHologram(Edition.Unlimited, Brushes.Black)
                .AddCreator(Creator.StudioDice, Brushes.Black)
                .Build();
            }

            if (monsterTypes.HasFlag(MonsterTypes.Ritual))
            {
                return new CardBuilder(_resourceRepository, Frame.Ritual, artworkEntity.Image)
                .AddAttribute((Draw.Builders.Attribute)cardEntity.Attribute)
                .AddName(cardEntity.Name, Brushes.Black)
                .AddLevel((Draw.Builders.Level)cardEntity.Level)
                .AddNumber(cardNumber, Brushes.Black)
                .AddMonsterType(cardEntity.GetDisplayedTypes())
                .AddDescription(cardEntity.Description, Brushes.Black)
                .AddAtkAndDef(cardEntity.Attack, cardEntity.Defense, Brushes.Black)
                .AddPasscode(passcode, Brushes.Black)
                .AddEditionAndHologram(Edition.Unlimited, Brushes.Black)
                .AddCreator(Creator.StudioDice, Brushes.Black)
                .Build();
            }

            if (monsterTypes.HasFlag(MonsterTypes.Synchro))
            {
                return new CardBuilder(_resourceRepository, Frame.Synchro, artworkEntity.Image)
                .AddAttribute((Draw.Builders.Attribute)cardEntity.Attribute)
                .AddName(cardEntity.Name, Brushes.Black)
                .AddLevel((Draw.Builders.Level)cardEntity.Level)
                .AddNumber(cardNumber, Brushes.Black)
                .AddMonsterType(cardEntity.GetDisplayedTypes())
                .AddDescription(cardEntity.Description, Brushes.Black, true)
                .AddAtkAndDef(cardEntity.Attack, cardEntity.Defense, Brushes.Black)
                .AddPasscode(passcode, Brushes.Black)
                .AddEditionAndHologram(Edition.Unlimited, Brushes.Black)
                .AddCreator(Creator.StudioDice, Brushes.Black)
                .Build();
            }

            if (monsterTypes.HasFlag(MonsterTypes.Xyz))
            {
                return new CardBuilder(_resourceRepository, Frame.Xyz, artworkEntity.Image)
                .AddAttribute((Draw.Builders.Attribute)cardEntity.Attribute)
                .AddName(cardEntity.Name, Brushes.White)
                .AddRank((Draw.Builders.Rank)cardEntity.Rank)
                .AddNumber(cardNumber, Brushes.Black)
                .AddMonsterType(cardEntity.GetDisplayedTypes())
                .AddDescription(cardEntity.Description, Brushes.Black, true)
                .AddAtkAndDef(cardEntity.Attack, cardEntity.Defense, Brushes.Black)
                .AddPasscode(passcode, Brushes.White)
                .AddEditionAndHologram(Edition.Unlimited, Brushes.White)
                .AddCreator(Creator.StudioDice, Brushes.White)
                .Build();
            }

            if (monsterTypes.HasFlag(MonsterTypes.Link))
            {
                return new CardBuilder(_resourceRepository, Frame.Link, artworkEntity.Image)
                .AddAttribute((Draw.Builders.Attribute)cardEntity.Attribute)
                .AddName(cardEntity.Name, Brushes.White)
                .AddNumber(cardNumber, Brushes.Black)
                .AddMonsterType(cardEntity.GetDisplayedTypes())
                .AddDescription(cardEntity.Description, Brushes.Black, true)
                .AddAtkAndLinkRating(cardEntity.Attack, (int)cardEntity.LinkRating, Brushes.Black)
                .AddLinkArrows((Draw.Builders.LinkArrows)cardEntity.LinkArrows)
                .AddPasscode(passcode, Brushes.Black)
                .AddEditionAndHologram(Edition.Unlimited, Brushes.Black)
                .AddCreator(Creator.StudioDice, Brushes.Black)
                .Build();
            }

            if (monsterTypes.HasFlag(MonsterTypes.Effect))
            {
                return new CardBuilder(_resourceRepository, Frame.Effect, artworkEntity.Image)
                .AddAttribute((Draw.Builders.Attribute)cardEntity.Attribute)
                .AddName(cardEntity.Name, Brushes.Black)
                .AddLevel((Draw.Builders.Level)cardEntity.Level)
                .AddNumber(cardNumber, Brushes.Black)
                .AddMonsterType(cardEntity.GetDisplayedTypes())
                .AddDescription(cardEntity.Description, Brushes.Black)
                .AddAtkAndDef(cardEntity.Attack, cardEntity.Defense, Brushes.Black)
                .AddPasscode(passcode, Brushes.Black)
                .AddEditionAndHologram(Edition.Unlimited, Brushes.Black)
                .AddCreator(Creator.StudioDice, Brushes.Black)
                .Build();
            }

            if (monsterTypes.HasFlag(MonsterTypes.Normal))
            {
                return new CardBuilder(_resourceRepository, Frame.Normal, artworkEntity.Image)
                .AddAttribute((Draw.Builders.Attribute)cardEntity.Attribute)
                .AddName(cardEntity.Name, Brushes.Black)
                .AddLevel((Draw.Builders.Level)cardEntity.Level)
                .AddNumber(cardNumber, Brushes.Black)
                .AddMonsterType(cardEntity.GetDisplayedTypes())
                .AddDescription(cardEntity.Description, Brushes.Black)
                .AddAtkAndDef(cardEntity.Attack, cardEntity.Defense, Brushes.Black)
                .AddPasscode(passcode, Brushes.Black)
                .AddEditionAndHologram(Edition.Unlimited, Brushes.Black)
                .AddCreator(Creator.StudioDice, Brushes.Black)
                .Build();
            }

            return null;
        }
    }
}
