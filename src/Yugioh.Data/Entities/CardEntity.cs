using System;
using System.Collections.Generic;
using System.Linq;

namespace Yugioh.Data.Entities
{
    public enum Type
    {
        Monster = 0,
        Spell = 1,
        Trap = 2
    }

    public enum Attribute
    {
        Dark = 0,
        Divine = 1,
        Earth = 2,
        Fire = 3,
        Light = 4,
        Water = 5,
        Wind = 6,
        Laugh = 7,
        Spell = 8,
        Trap = 9
    }

    public enum Property
    {
        Normal = 0,
        Continuous = 1,
        Equip = 2,
        Field = 3,
        Quick_Play = 4,
        Ritual = 5,
        Counter = 6,
    }

    [Flags]
    public enum MonsterTypes : byte
    {
        Normal   = 0b_00000001,
        Effect   = 0b_00000010,
        Ritual   = 0b_00000100,
        Fusion   = 0b_00001000,
        Synchro  = 0b_00010000,
        Xyz      = 0b_00100000,
        Pendulum = 0b_01000000,
        Link     = 0b_10000000
    }

    [Flags]
    public enum LinkArrows : byte
    {
        TopLeft     = 0b_00000001,
        Top         = 0b_00000010,
        TopRight    = 0b_00000100,
        Right       = 0b_00001000,
        BottomRight = 0b_00010000,
        Bottom      = 0b_00100000,
        BottomLeft  = 0b_01000000,
        Left        = 0b_10000000,
    }

    [Flags]
    public enum Abilities : byte
    {
        Flip      = 0b_00000001,
        Gemini    = 0b_00000010,
        Spirit    = 0b_00000100,
        Union     = 0b_00001000,
        Toon      = 0b_00010000,
        Tuner     = 0b_00100000,
        DarkTuner = 0b_01000000,
    }

    public class CardEntity
    {
        public int CardId { get; set; }
        public Type Type { get; set; }
        public string Name { get; set; }
        public Attribute Attribute { get; set; }
        public byte? Level { get; set; }
        public byte? Rank { get; set; }
        public byte? PendulumScale { get; set; }
        public byte? LinkRating { get; set; }
        public LinkArrows? LinkArrows { get; set; }
        public Property? Property { get; set; }
        public MonsterTypes? MonsterTypes { get; set; }
        public string Race { get; set; }
        public Abilities? Abilities { get; set; }
        public string Attack { get; set; }
        public string Defense { get; set; }
        public string Description { get; set; }
        public string PendulumDescription { get; set; }

        public List<string> GetDisplayedTypes()
        {
            var types = new List<string>();
            types.Add(Race);

            if (MonsterTypes != null)
            {
                var monsterTypes = (MonsterTypes)MonsterTypes;

                var nonNormalEffectTypes = monsterTypes & ~(Entities.MonsterTypes.Normal | Entities.MonsterTypes.Effect);

                if (nonNormalEffectTypes > 0)
                {
                    types.AddRange(nonNormalEffectTypes.ToString().Split(',').Select(e => e.Trim()));
                }

                if (Abilities != null && Abilities > 0)
                {
                    var abilities = Abilities.ToString().Split(',').Select(e => e.Trim());
                    types.AddRange(abilities);
                }

                var normalEffectTypes = monsterTypes & (Entities.MonsterTypes.Normal | Entities.MonsterTypes.Effect);
                
                if (normalEffectTypes > 0)
                {
                    types.AddRange(normalEffectTypes.ToString().Split(',').Select(e => e.Trim()));
                }
            }

            return types;
        }
    }
}
