using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Yugioh.Draw.Builders;
using Yugioh.Draw.Repositories;

namespace Yugioh.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            string resourcesPath = Path.GetFullPath(@"..\..\..\..\resources");
            if (!Directory.Exists(resourcesPath))
            {
                throw new FileNotFoundException($"Resources directory not found at {resourcesPath}");
            }

            IResourceRepository resourceRepository = new ResourceRepository(resourcesPath);

            new CardBuilder(resourceRepository, Frame.Normal,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__DARK_MAGICIAN_001.png")
                .AddAttribute(Attribute.Dark)
                .AddName("Dark Magician", Brushes.Black)
                .AddLevel(Level.Seven)
                .AddNumber("YGLD-ENA03", Brushes.Black)
                .AddMonsterType(new List<string>() { "Spellcaster", "Normal" })
                .AddDescription("The ultimate wizard in terms of attack and defense.", Brushes.Black)
                .AddAtkAndDef("2500", "2100", Brushes.Black)
                .AddPasscode("46986414", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__DARK_MAGICIAN.png", ImageFormat.Png);

            new CardBuilder(resourceRepository, Frame.Normal,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__BLUE_EYES_WHITE_DRAGON_001.png")
                .AddAttribute(Attribute.Light)
                .AddName("Blue-Eyes White Dragon", Brushes.Black)
                .AddLevel(Level.Eight)
                .AddNumber("SDK-001", Brushes.Black)
                .AddMonsterType(new List<string>() { "Dragon", "Normal" })
                .AddDescription("This legendary dragon is a powerful engine of destruction. Virtually invincible, very few have faced this awesome creature and lived to tell the tale.", Brushes.Black)
                .AddAtkAndDef("3000", "2500", Brushes.Black)
                .AddPasscode("89631139", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__BLUE_EYES_WHITE_DRAGON.png", ImageFormat.Png);

            new CardBuilder(resourceRepository, Frame.Normal,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__GUNKAN_SUSHIP_SHARI_001.png")
                .AddAttribute(Attribute.Fire)
                .AddName("Gunkan Suship Shari", Brushes.Black)
                .AddLevel(Level.Four)
                .AddNumber("YS17-EN005", Brushes.Black)
                .AddMonsterType(new List<string>() { "Aqua", "Normal" })
                .AddDescription("Finally got to visit that harbor specializing in Gunkan Suships that I've been curious about for a while! The premium \"Shari\" here is limited to 2000 Suships a year, and uses specially developed smooth aged rice, giving it extra boldness not found anywhere else. The classy atmosphere made my heart sing, too. The Gunkan Suship served had a perfect balance of vinegar, nigiri, shine, and shape, demonstrating exquisite artisanship. The owner told me, \"We are introducing rich yet mellow scented EDO-FRONT red vinegar in the near future,\" which I'm really looking forward to. However, I was disappointed the surrounding seas were a little noisy... so, giving it 4 stars with hope for improvements in the future.", Brushes.Black)
                .AddAtkAndDef("2000", "0", Brushes.Black)
                .AddPasscode("24639891", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__GUNKAN_SUSHIP_SHARI.png", ImageFormat.Png);

            new CardBuilder(resourceRepository, Frame.Effect,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__BLACK_LUSTER_SOLDIER_ENVOY_OF_THE_BEGINNING_001.png")
                .AddAttribute(Attribute.Light)
                .AddName("Black Luster Soldier - Envoy of the Beginning", Brushes.Black)
                .AddLevel(Level.Eight)
                .AddNumber("DUSA-EN053", Brushes.Black)
                .AddMonsterType(new List<string>() { "Warrior", "Normal" })
                .AddDescription("Cannot be Normal Summoned/Set. Must first be Special Summoned (from your hand) by banishing 1 LIGHT and 1 DARK monster from your GY. Once per turn, you can activate 1 of these effects.\r\n● Target 1 monster on the field; banish it. This card cannot attack the turn this effect is activated.\r\n● If this attacking card destroys an opponent's monster by battle: It can make a second attack in a row.", Brushes.Black)
                .AddAtkAndDef("3000", "2500", Brushes.Black)
                .AddPasscode("72989439", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__BLACK_LUSTER_SOLDIER_ENVOY_OF_THE_BEGINNING.png", ImageFormat.Png);

            new CardBuilder(resourceRepository, 
                Frame.Effect,  
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__RAVIEL_LORD_OF_PHANTASMS_001.png")
                .AddAttribute(Attribute.Dark)
                .AddName("Raviel, Lord of Phantasms", Brushes.Black)
                .AddLevel(Level.Ten)
                .AddNumber("SDSA-EN044", Brushes.Black)
                .AddMonsterType(new List<string>() { "Fiend", "Effect" })
                .AddDescription("Cannot be Normal Summoned/Set. Must be Special Summoned (from your hand) by Tributing 3 Fiend monsters. Each time your opponent Normal Summons a monster: Special Summon 1 \"Phantasm Token\" (Fiend/DARK/Level 1/ATK 1000/DEF 1000), but it cannot declare an attack. Once per turn: You can Tribute 1 monster; this card gains ATK equal to the Tributed monster's original ATK, until the end of this turn.", Brushes.Black)
                .AddAtkAndDef("4000", "4000", Brushes.Black)
                .AddPasscode("69890967", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__RAVIEL_LORD_OF_PHANTASMS.png", ImageFormat.Png);

            new CardBuilder(resourceRepository,
                Frame.Effect,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__DESTINY_HERO_PLASMA_001.png")
                .AddAttribute(Attribute.Dark)
                .AddName("Destiny HERO - Plasma", Brushes.Black)
                .AddLevel(Level.Eight)
                .AddNumber("LEHD-ENA02", Brushes.Black)
                .AddMonsterType(new List<string>() { "Warrior", "Effect" })
                .AddDescription("Cannot be Normal Summoned/Set. Must be Special Summoned (from your hand) by Tributing 3 monsters. Negate the effects of face-up monsters while your opponent controls them. Once per turn: You can target 1 monster your opponent controls; equip that target to this card (max. 1). This card gains ATK equal to half the original ATK of the monster equipped to it by this effect.", Brushes.Black)
                .AddAtkAndDef("1900", "600", Brushes.Black)
                .AddPasscode("83965310", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__DESTINY_HERO_PLASMA.png", ImageFormat.Png);

            new CardBuilder(resourceRepository, 
                Frame.Ritual,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__RELINQUISHED_001.png")
                .AddAttribute(Attribute.Dark)
                .AddName("Relinquished", Brushes.Black)
                .AddLevel(Level.One)
                .AddNumber("LDS1-EN047", Brushes.Black)
                .AddMonsterType(new List<string>() { "Spellcaster", "Ritual", "Effect" })
                .AddDescription("You can Ritual Summon this card with \"Black Illusion Ritual\". Once per turn: You can target 1 monster your opponent controls; equip that target to this card (max. 1). This card's ATK/DEF become equal to that equipped monster's. If this card would be destroyed by battle, destroy that equipped monster instead. While equipped with that monster, any battle damage you take from battles involving this card inflicts equal effect damage to your opponent.", Brushes.Black)
                .AddAtkAndDef("0", "0", Brushes.Black)
                .AddPasscode("64631466", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__RELINQUISHED.png", ImageFormat.Png);

            new CardBuilder(resourceRepository, 
                Frame.Fusion,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__EVIL_HERO_DARK_GAIA_001.png")
                .AddAttribute(Attribute.Earth)
                .AddName("Evil HERO Dark Gaia", Brushes.Black)
                .AddLevel(Level.Eight)
                .AddNumber("LCGX-EN069", Brushes.Black)
                .AddMonsterType(new List<string>() { "Fiend", "Fusion", "Effect" })
                .AddDescription("1 Fiend-Type monster + 1 Rock-Type monster\r\nMust be Special Summoned with \"Dark Fusion\" and cannot be Special Summoned by other ways. The original ATK of this card is equal to the combined original ATK of the Fusion Material Monsters used to Fusion Summon it. When this card declares an attack: You can change all Defense Position monsters your opponent controls to face-up Attack Position. (Flip Effects are not activated at this time.)", Brushes.Black)
                .AddAtkAndDef("?", "0", Brushes.Black)
                .AddPasscode("58332301", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__EVIL_HERO_DARK_GAIA.png", ImageFormat.Png);

            new CardBuilder(resourceRepository, Frame.Synchro,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__STARDUST_DRAGON_001.png")
                .AddAttribute(Attribute.Wind)
                .AddName("Stardust Dragon", Brushes.Black)
                .AddLevel(Level.Eight)
                .AddNumber("DP08-EN014", Brushes.Black)
                .AddMonsterType(new List<string>() { "Dragon", "Synchro", "Effect" })
                .AddDescription("1 Tuner + 1+ non-Tuner monsters\r\nWhen a card or effect is activated that would destroy a card(s) on the field(Quick Effect): You can Tribute this card; negate the activation, and if you do, destroy it. During the End Phase, if this effect was activated this turn(and was not negated): You can Special Summon this card from your GY.", Brushes.Black)
                .AddAtkAndDef("2500", "2000", Brushes.Black)
                .AddPasscode("44508094", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__STARDUST_DRAGON.png", ImageFormat.Png);

            new CardBuilder(resourceRepository, 
                Frame.DarkSynchro,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__HUNDRED_EYES_DRAGON_001.png")
                .AddAttribute(Attribute.Dark)
                .AddName("Hundred Eyes Dragon", Brushes.White)
                .AddLevel(Level.MinusEight)
                .AddNumber("LC5D-EN154", Brushes.White)
                .AddMonsterType(new List<string>() { "Dragon", "DarkSynchro", "Effect" })
                .AddDescription("1 non-Tuner monster - 1 DARK Tuner\r\nThis card can only be Synchro Summoned if the Level of the non-Tuner Synchro Material Monster minus the Level of the Dark Tuner Synchro Material Monster equals -8.While this card is face-up on the field, it gains the effects of all \"Infernity\" monsters in your Graveyard. If this card is destroyed while on the field, select 1 card from your Deck and add it to your hand.", Brushes.Black)
                .AddAtkAndDef("3000", "2500", Brushes.Black)
                .AddPasscode("95453143", Brushes.White)
                .AddEditionAndHologram(Edition.First, Brushes.White)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.White)
                .Build()
                .Save($"EXAMPLE__HUNDRED_EYES_DRAGON.png", ImageFormat.Png);

            new CardBuilder(resourceRepository, Frame.Xyz,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__NEO_GALAXY_EYES_PHOTON_DRAGON_001.png")
                .AddAttribute(Attribute.Light)
                .AddName("Neo Galaxy-Eyes Photon Dragon", new LinearGradientBrush(Point.Empty, new Point(100, 100), Color.Gold, Color.White))
                .AddRank(Rank.Eight)
                .AddNumber("BLLR-EN064", Brushes.White)
                .AddMonsterType(new List<string>() { "Dragon", "Xyz", "Effect" })
                .AddDescription("3 Level 8 monsters\r\nIf this card is Xyz Summoned using \"Galaxy-Eyes Photon Dragon\" as any of its materials: All other face-up cards currently on the field have their effects negated. Once per turn: You can detach 1 material from this card; detach all materials from monsters your opponent controls, then this card gains 500 ATK for each, also it can attack up to that many times during each Battle Phase this turn.", Brushes.Black)
                .AddAtkAndDef("4500", "3000", Brushes.Black)
                .AddPasscode("39272762", Brushes.White)
                .AddEditionAndHologram(Edition.First, Brushes.White)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.White)
                .Build()
                .Save($"EXAMPLE__NEO_GALAXY_EYES_PHOTON_DRAGON.png", ImageFormat.Png);

            new CardBuilder(resourceRepository,
                Frame.PendulumEffect, 
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__ODD_EYES_PENDULUM_DRAGON_001.png")
                .AddAttribute(Attribute.Dark)
                .AddName("Odd-Eyes Pendulum Dragon", Brushes.Black)
                .AddLevel(Level.Seven)
                .AddPendulumScale(PendulumScale.Four)
                .AddNumber("DUPO-EN105", Brushes.Black)
                .AddMonsterType(new List<string>() { "Dragon", "Pendulum", "Effect" })
                .AddDescription("If this card battles an opponent's monster, any battle damage this card inflicts to your opponent is doubled.", Brushes.Black)
                .AddPendulumDescription("You can reduce the battle damage you take from an attack involving a Pendulum Monster you control to 0. During your End Phase: You can destroy this card, and if you do, add 1 Pendulum Monster with 1500 or less ATK from your Deck to your hand. You can only use each Pendulum Effect of \"Odd-Eyes Pendulum Dragon\" once per turn.", Brushes.Black)
                .AddAtkAndDef("2500", "2000", Brushes.Black)
                .AddPasscode("16178681", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__ODD_EYES_PENDULUM_DRAGON.png", ImageFormat.Png);

            new CardBuilder(resourceRepository,
                Frame.PendulumNormal,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__DRAGONPULSE_MAGICIAN_001.png")
                .AddAttribute(Attribute.Earth)
                .AddName("Dragonpulse Magician", Brushes.Black)
                .AddLevel(Level.Four)
                .AddPendulumScale(PendulumScale.One)
                .AddNumber("PEVO-EN013", Brushes.Black)
                .AddMonsterType(new List<string>() { "Spellcaster", "Pendulum" })
                .AddDescription("This boy magician has the gift of seeing the natural lines of energy that run through the earth, which his people call the Pulse of the Dragon. His exuberance and skill put him in high regard with his mentor, the \"Dragonpit Magician\".", Brushes.Black)
                .AddPendulumDescription("Once per turn, if you have a \"Magician\" card in your other Pendulum Zone: You can discard 1 Pendulum Monster, then target 1 face-up monster on the field; destroy it.", Brushes.Black)
                .AddAtkAndDef("1800", "900", Brushes.Black)
                .AddPasscode("15146890", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__DRAGONPULSE_MAGICIAN.png", ImageFormat.Png);

            new CardBuilder(resourceRepository,
                Frame.PendulumFusion,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__DDD_SUPER_DOOM_KING_PURPLE_ARMAGEDDON_001.png")
                .AddAttribute(Attribute.Dark)
                .AddName("D/D/D Super Doom King Purple Armageddon", Brushes.Black)
                .AddLevel(Level.Ten)
                .AddPendulumScale(PendulumScale.One)
                .AddNumber("SOFU-EN096", Brushes.Black)
                .AddMonsterType(new List<string>() { "Fiend", "Fusion", "Pendulum", "Effect" })
                .AddDescription("2 \"D/D/D\" monsters\r\nYou can target 1 Attack Position monster your opponent controls; destroy it, and if you do, inflict damage to your opponent equal to half its original ATK.You can only use this effect of \"D/D/D Super Doom King Purple Armageddon\" once per turn.Before damage calculation, if an opponent's monster battles: You can make its ATK become equal to its original ATK until the end of the Damage Step. If this card in the Monster Zone is destroyed: You can place this card in your Pendulum Zone.", Brushes.Black)
                .AddPendulumDescription("Once per turn, before damage calculation, if your \"D/D/D\" Fusion Monster battles an opponent's monster: You can make that opponent's monster lose 1000 ATK until the end of this turn (even if this card leaves the field).", Brushes.Black)
                .AddAtkAndDef("3500", "3000", Brushes.Black)
                .AddPasscode("84569886", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__DDD_SUPER_DOOM_KING_PURPLE_ARMAGEDDON.png", ImageFormat.Png);

            new CardBuilder(resourceRepository,
                Frame.Link,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__GAIA_SABER_THE_LIGHTNING_SHADOW_001.png")
                .AddAttribute(Attribute.Earth)
                .AddName("Gaia Saber, the Lightning Shadow", Brushes.Black)
                .AddNumber("BLRR-EN087", Brushes.Black)
                .AddMonsterType(new List<string>() { "Machine", "Link" })
                .AddDescription("2+ monsters", Brushes.Black)
                .AddLinkArrows(new List<LinkArrow>() { LinkArrow.Left, LinkArrow.Right, LinkArrow.Bottom })
                .AddAtkAndLinkRating("2600", 3, Brushes.Black)
                .AddPasscode("67598234", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__GAIA_SABER_THE_LIGHTNING_SHADOW.png", ImageFormat.Png);

            new CardBuilder(resourceRepository,
                Frame.Link,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__FIREWALL_DRAGON_001.png")
                .AddAttribute(Attribute.Light)
                .AddName("Firewall Dragon", Brushes.Black)
                .AddNumber("GFTP-EN131", Brushes.Black)
                .AddMonsterType(new List<string>() { "Cyberse", "Link", "Effect" })
                .AddDescription("2+ monsters\r\nwhile face-up on the field (Quick Effect): You can target monsters on the field and/or GY up to the number of monsters co-linked to this card; return them to the hand. If a monster this card points to is destroyed by battle or sent to the GY: You can Special Summon 1 Cyberse monster from your hand. You can only use each effect of \"Firewall Dragon\" once per turn.", Brushes.Black)
                .AddLinkArrows(new List<LinkArrow>() { LinkArrow.Left, LinkArrow.Top, LinkArrow.Right, LinkArrow.Bottom })
                .AddAtkAndLinkRating("2500", 4, Brushes.Black)
                .AddPasscode("05043010", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__FIREWALL_DRAGON.png", ImageFormat.Png);

            new CardBuilder(resourceRepository,
                Frame.Link,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__KNIGHTMARE_PHOENIX_001.png")
                .AddAttribute(Attribute.Fire)
                .AddName("Knightmare Phoenix", Brushes.Black)
                .AddNumber("MP19-EN027", Brushes.Black)
                .AddMonsterType(new List<string>() { "Fiend", "Link", "Effect" })
                .AddDescription("2 monsters with different names\r\nIf this card is Link Summoned: You can discard 1 card, then target 1 Spell/Trap your opponent controls; destroy it, then, if this card was co-linked when this effect was activated, you can draw 1 card. You can only use this effect of \"Knightmare Phoenix\" once per turn. Co-linked monsters you control cannot be destroyed by battle.", Brushes.Black)
                .AddLinkArrows(new List<LinkArrow>() { LinkArrow.Top, LinkArrow.Right })
                .AddAtkAndLinkRating("1900", 2, Brushes.Black)
                .AddPasscode("02857636", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__KNIGHTMARE_PHOENIX.png", ImageFormat.Png);

            new CardBuilder(resourceRepository,
                Frame.Spell,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__DIMENSION_FUSION_DESTRUCTION_001.png")
                .AddAttribute(Attribute.Spell)
                .AddName("Dimension Fusion Destruction", Brushes.Black)
                .AddProperty(Property.QuickPlay)
                .AddNumber("DP01-EN036", Brushes.Black)
                .AddDescription("Banish from your hand, field, and/or GY, the Fusion Materials that are listed on a \"Phantasm\" Fusion Monster, then Special Summon that Fusion Monster from your Extra Deck, ignoring its Summoning conditions. You take no battle damage from attacks involving the monster Special Summoned by this effect. If you control \"Uria, Lord of Searing Flames\", \"Hamon, Lord of Striking Thunder\", or \"Raviel, Lord of Phantasms\", your opponent cannot activate cards or effects in response to this card's activation.", Brushes.Black)
                .AddPasscode("67598234", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__DIMENSION_FUSION_DESTRUCTION.png", ImageFormat.Png);

            new CardBuilder(resourceRepository,
                Frame.Trap,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__MIRROR_FORCE_001.png")
                .AddAttribute(Attribute.Trap)
                .AddName("Mirror Force", Brushes.Black)
                .AddProperty(Property.Normal)
                .AddNumber("YS18-EN036", Brushes.Black)
                .AddDescription("When an opponent's monster declares an attack: Destroy all your opponent's Attack Position monsters.", Brushes.Black)
                .AddPasscode("44095762", Brushes.Black)
                .AddEditionAndHologram(Edition.First, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__MIRROR_FORCE.png", ImageFormat.Png);

            new CardBuilder(resourceRepository, Frame.EgyptianGodRed,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__SLIFER_THE_SKY_DRAGON_001.png")
                .AddAttribute(Attribute.Divine)
                .AddName("Slifer the Sky Dragon", Brushes.Black)
                .AddLevel(Level.Ten)
                .AddNumber("YGLD-ENG01", Brushes.Black)
                .AddMonsterType(new List<string>() { "Divine-Beast" })
                .AddDescription("The heavens twist and thunder roars, signaling the coming of this ancient creature, and the dawn of true power.", Brushes.Black)
                .AddAtkAndDef("X000", "X000", Brushes.Black)
                .AddEditionAndHologram(Edition.Unlimited, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__SLIFER_THE_SKY_DRAGON.png", ImageFormat.Png);

            new CardBuilder(resourceRepository, Frame.EgyptianGodBlue,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__OBELISK_THE_TORMENTOR_001.png")
                .AddAttribute(Attribute.Divine)
                .AddName("Obelisk the Tormentor", Brushes.Black)
                .AddLevel(Level.Ten)
                .AddNumber("YGLD-ENG02", Brushes.Black)
                .AddMonsterType(new List<string>() { "Divine-Beast" })
                .AddDescription("The descent of this mighty creature shall be heralded by burning winds and twisted land. And with the coming of this horror, those who draw breath shall know the true meaning of eternal slumber.", Brushes.Black)
                .AddAtkAndDef("4000", "4000", Brushes.Black)
                .AddEditionAndHologram(Edition.Unlimited, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__OBELISK_THE_TORMENTOR.png", ImageFormat.Png);

            new CardBuilder(resourceRepository, Frame.EgyptianGodYellow,
                @"C:\Users\Admin\Documents\Projects\Yugioh.Draw\resources\images\ARTWORK__THE_WINGED_DRAGON_OF_RA_001.png")
                .AddAttribute(Attribute.Divine)
                .AddName("The Winged Dragon of Ra", Brushes.Black)
                .AddLevel(Level.Ten)
                .AddNumber("YGLD-ENG03", Brushes.Black)
                .AddMonsterType(new List<string>() { "Divine-Beast" })
                .AddDescription("Spirits sing of a powerful creature that rules over all that is mystic.", Brushes.Black)
                .AddAtkAndDef("?", "?", Brushes.Black)
                .AddEditionAndHologram(Edition.Unlimited, Brushes.Black)
                .AddCreator("1996 KAZUKI TAKAHASHI", Brushes.Black)
                .Build()
                .Save($"EXAMPLE__THE_WINGED_DRAGON_OF_RA.png", ImageFormat.Png); 
        }
    }
}
