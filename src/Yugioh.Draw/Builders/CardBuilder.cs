using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Yugioh.Draw.Repositories;
using Yugioh.Draw.Utils;

namespace Yugioh.Draw.Builders
{
    public enum Frame
    {
        DarkSynchro,
        Effect,
        EgyptianGodBlue,
        EgyptianGodRed,
        EgyptianGodRed2,
        EgyptianGodYellow,
        Fusion,
        LegendaryDragon,
        Link,
        Normal,
        PendulumDarkSynchro,
        PendulumEffect,
        PendulumEgyptianGodBlue,
        PendulumEgyptianGodRed,
        PendulumEgyptianGodYellow,
        PendulumFusion,
        PendulumNormal,
        PendulumRitual,
        PendulumSacredBeastBlue,
        PendulumSacredBeastRed,
        PendulumSacredBeastYellow,
        PendulumSpell,
        PendulumSynchro,
        PendulumToken,
        PendulumTrap,
        PendulumWickedGod,
        PendulumXyz,
        Ritual,
        SacredBeastBlue,
        SacredBeastRed,
        SacredBeastYellow,
        Spell,
        Synchro,
        Token,
        Trap,
        WickedGod,
        Xyz
    }

    /*public enum Attribute
    {
        Dark,
        DarkDivine,
        Divine,
        Earth,
        Fire,
        Laugh,
        Light,
        Spell,
        Start,
        Trap,
        TrapSpell,
        Water,
        Wind
    }*/

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
        Trap = 9,
        DarkDivine = 10
    }

    public enum Level
    {
        MinusThirteen = -13,
        MinusTwelve = -12,
        MinusEleven = -11,
        MinusTen = -10,
        MinusNine = -9,
        MinusEight = -8,
        MinusSeven = -7,
        MinusSix = -6,
        MinusFive = -5,
        MinusFour = -4,
        MinusThree = -3,
        MinusTwo = -2,
        MinusOne = -1,
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Eleven = 11,
        Twelve = 12,
        Thirteen = 13
    }

    public enum Rank
    {
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Eleven = 11,
        Twelve = 12,
        Thirteen = 13
    }

    public enum PendulumScale
    {
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Eleven = 11,
        Twelve = 12,
        Thirteen = 13
    }

    [Flags]
    public enum LinkArrows : byte
    {
        TopLeft = 0b_00000001,
        Top = 0b_00000010,
        TopRight = 0b_00000100,
        Right = 0b_00001000,
        BottomRight = 0b_00010000,
        Bottom = 0b_00100000,
        BottomLeft = 0b_01000000,
        Left = 0b_10000000,
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

    public enum Edition
    {
        First,
        Limited,
        Unlimited,
        DuelTerminal
    }

    public enum EyeOfAnubisHologram
    {
        Silver,
        Gold
    }

    public static class Creator
    {
        public const string StudioDice = "2020 Studio Dice/SHUEISHA, TV TOKYO, KONAMI";
        public const string KazukiTakahashi = "1996 KAZUKI TAKAHASHI";
    }

    public class Paragraph
    {
        public string FontFamily { get; set; }
        public string RawText { get; set; }
        public bool FitOneLine { get; set; }
        public bool DrawAtBottom { get; set; }
        public List<string> DrawableLines { get; set; }
        public float TotalWidth { get; set; }
        public float ScaleWidth { get; set; }
    }

    public class CardBuilder : IBuilder<Bitmap>
    {
        private readonly Bitmap b;
        private readonly Graphics g;
        private readonly IResourceRepository _resourceRepository;

        private readonly bool _isMonster;
        private readonly bool _isSpell;
        private readonly bool _isNormalMonster;
        private readonly bool _isPendulumMonster;
        private readonly bool _isLinkMonster;

        public CardBuilder(IResourceRepository resourceRepository, Frame frame, Image artworkImage)
        {
            _resourceRepository = resourceRepository;

            // Initialise format of card
            switch (frame)
            {
                case Frame.Spell:
                    _isMonster = false;
                    _isSpell = true;
                    _isNormalMonster = false;
                    _isPendulumMonster = false;
                    _isLinkMonster = false;
                    break;
                case Frame.Trap:
                    _isMonster = false;
                    _isSpell = false;
                    _isNormalMonster = false;
                    _isPendulumMonster = false;
                    _isLinkMonster = false;
                    break;
                case Frame.Normal:
                    _isMonster = true;
                    _isSpell = false;
                    _isNormalMonster = true;
                    _isPendulumMonster = false;
                    _isLinkMonster = false;
                    break;
                case Frame.PendulumEffect:
                case Frame.PendulumFusion:
                case Frame.PendulumRitual:
                case Frame.PendulumSynchro:
                case Frame.PendulumXyz:
                    _isMonster = true;
                    _isSpell = false;
                    _isNormalMonster = false;
                    _isPendulumMonster = true;
                    _isLinkMonster = false;
                    break;
                case Frame.PendulumNormal:
                    _isMonster = true;
                    _isSpell = false;
                    _isNormalMonster = true;
                    _isPendulumMonster = true;
                    _isLinkMonster = false;
                    break;
                case Frame.Link:
                    _isMonster = true;
                    _isSpell = false;
                    _isNormalMonster = false;
                    _isPendulumMonster = false;
                    _isLinkMonster = true;
                    break;
                default:
                    _isMonster = true;
                    _isSpell = false;
                    _isNormalMonster = false;
                    _isPendulumMonster = false;
                    _isLinkMonster = false;
                    break;
            }

            // initial setup
            b = new Bitmap(826, 1204);
            g = ImageUtil.GetGraphics(b);
            g.Clear(Color.White);        
            AddArtwork(artworkImage);
            AddCardFrame(frame);
        }

        public Bitmap Build()
        {

            g.Dispose();

            return b;
        }

        public CardBuilder AddAttribute(Attribute attribute)
        {
            Image attributeImage = _resourceRepository.GetImage($"ATTRIBUTE__{attribute.ToString().ToUpper()}");

            g.DrawImage(attributeImage, 691, 55, 78, 78);

            return this;
        }

        public CardBuilder AddName(string name, Brush brush)
        {
            float fontHeight = 72f;
            FontFamily fontFamily = _resourceRepository.GetFontFamily("Yu-Gi-Oh! Matrix Small Caps 1");
            Font font = new Font(fontFamily, fontHeight);

            FontFamily atFontFamily = _resourceRepository.GetFontFamily("MatrixRegular");
            Font atFont = new Font(atFontFamily, 60);

            string atString = " @ ";

            var sections = name.Split('@');
            var bitmaps = new List<Bitmap>();
            for (int i=0; i< sections.Length; ++i)
            {
                string section = sections[i];
                if (i > 0)
                {
                    Bitmap atNameBitmap = ImageUtil.RenderText(atString, atFont, brush, g.MeasureString(atString, atFont));
                    bitmaps.Add(atNameBitmap);
                }

                Bitmap sectionBitmap = ImageUtil.RenderText(section, font, brush, g.MeasureString(section, font));
                bitmaps.Add(sectionBitmap);
            }

            int nameWidthUpperLimit = 610;

            Bitmap nameBitmap = new Bitmap(bitmaps.Sum(_b => _b.Width), bitmaps.Max(_b => _b.Height));
            using (var gr = Graphics.FromImage(nameBitmap))
            {
                int x = 0;
                foreach (var bitmap in bitmaps)
                {          
                    gr.DrawImage(bitmap, x, nameBitmap.Height - bitmap.Height);
                    x += bitmap.Width;
                }               
            }
            

            float targetWidth = nameBitmap.Width > nameWidthUpperLimit ? nameWidthUpperLimit : nameBitmap.Width;

            g.DrawImage(nameBitmap, 67, 47, targetWidth, nameBitmap.Height);

            return this;
        }

        public CardBuilder AddLevel(Level level)
        {
            int levelEndX = level == Level.Thirteen ? 710 : 685;
            int levelStartY = 147;
            int levelWidth = 51;
            int levelHeight = 51;
            int levelMarginX = 3;

            if ((int)level > 0)
            {
                Image levelImage = _resourceRepository.GetImage("LEVEL__STAR");

                for (int i = 0; i < (int)level; ++i)
                {
                    g.DrawImage(levelImage, levelEndX - (levelWidth + levelMarginX) * i, levelStartY, levelWidth, levelHeight);
                }
            }
            else
            {
                Image levelImage = _resourceRepository.GetImage("LEVEL__NEGATIVE_STAR");

                for (int i = (int)level; i < 0; ++i)
                {
                    g.DrawImage(levelImage, levelEndX - (levelWidth + levelMarginX) * (i + 12), levelStartY, levelWidth, levelHeight);
                }
            }

            return this;
        }

        public CardBuilder AddRank(Rank rank)
        {
            int rankStartX = rank == Rank.Thirteen ? 66 : 91;
            int rankStartY = 147;
            int rankWidth = 51;
            int rankHeight = 51;
            int rankMarginX = 3;
            Image rankImage = _resourceRepository.GetImage("RANK__STAR");

            for (int i = 0; i < (int)rank; ++i)
            {
                g.DrawImage(rankImage, rankStartX + (rankWidth + rankMarginX) * i, rankStartY, rankWidth, rankHeight);
            }

            return this;
        }

        public CardBuilder AddPendulumScale(PendulumScale pendulumScale)
        {
            Image pendulumScaleBlueImage = _resourceRepository.GetImage("PENDULUM__SCALE_BLUE");
            Image pendulumScaleRedImage = _resourceRepository.GetImage("PENDULUM__SCALE_RED");
            
            g.DrawImage(pendulumScaleBlueImage, 58, 784, pendulumScaleBlueImage.Width, pendulumScaleBlueImage.Height);
            g.DrawImage(pendulumScaleRedImage, 717, 784, pendulumScaleRedImage.Width, pendulumScaleRedImage.Height);

            string valueString = ((int)pendulumScale).ToString();
            int digits = valueString.Length;
            int scaleBlueStartX = digits > 1 ? 59 : 71;
            int scaleRedStartX = digits > 1 ? 715 : 727;
            int startY = 825;

            float fontHeight = 45f;
            FontFamily fontFamily = _resourceRepository.GetFontFamily("MatrixBoldSmallCaps");
            Font font = new Font(fontFamily, fontHeight);

            for (int i = 0; i < valueString.Length; ++i)
            {
                char c = valueString[i];
                Bitmap characterBitmap = ImageUtil.TrimRight(ImageUtil.RenderText(c.ToString(), font, Brushes.Black, g.MeasureString(c.ToString(), font)));

                g.DrawImage(characterBitmap, scaleBlueStartX, startY);
                g.DrawImage(characterBitmap, scaleRedStartX, startY);

                scaleBlueStartX += characterBitmap.Width;
                scaleRedStartX += characterBitmap.Width;
            }

            return this;
        }

        public CardBuilder AddProperty(Property property)
        {
            // TODO : Clean up
            bool isNormal = property == Property.Normal;
            string propertyText = _isSpell ? "Spell Card" : "Trap Card";

            float height = 31.5f;
            FontFamily fontFamily = _resourceRepository.GetFontFamily("Yu-Gi-Oh!ITCStoneSerifSmallCaps");
            Font font = new Font(fontFamily, height);

            int startX = 740;
            int startY = 139;

            // Using double left bracket to avoid a wierd spacing issue, and moving the square brackets 2px up
            SizeF lineSize = g.MeasureString(propertyText, font);

            using (Bitmap b = new Bitmap(400, (int)lineSize.Height))
            {
                using (Graphics gr = ImageUtil.GetGraphics(b))
                {
                    gr.Clear(Color.Transparent);

                    // Draw the text onto the bitmap
                    string rightSquareBracket = isNormal ? "]" : "   ]";

                    gr.DrawString("[", font, Brushes.Black, g.MeasureString("[", font).Width, 0, StringFormat.GenericTypographic);
                    gr.DrawString(propertyText, font, Brushes.Black, g.MeasureString("[[", font).Width + 2, 2, StringFormat.GenericTypographic);
                    gr.DrawString(rightSquareBracket, font, Brushes.Black, g.MeasureString("[[" + propertyText, font).Width, 0, StringFormat.GenericTypographic);

                    Bitmap trimmedBitmap = ImageUtil.PadRight(ImageUtil.TrimRight(b), 1);
                    g.DrawImage(trimmedBitmap, startX - trimmedBitmap.Width, startY);
                    g.DrawImage(trimmedBitmap, startX - trimmedBitmap.Width, startY);

                    if (!isNormal)
                    {
                        Image propertyImage = _resourceRepository.GetImage($"PROPERTY__{property.ToString().ToUpper()}");
                        var propertyIconRec = new RectangleF(683, 154, 44, 44);
                        g.DrawImage(propertyImage, propertyIconRec);
                    }
                }
            }

            return this;
        }

        public CardBuilder AddNumber(string number, Brush brush)
        {
            int fontHeight = 17;
            FontFamily fontFamily = _resourceRepository.GetFontFamily("StoneSerif LT");
            Font font = new Font(fontFamily, fontHeight);
            Bitmap numberBitmap;

            if (_isPendulumMonster)
            {
                numberBitmap = ImageUtil.RenderText(number, font, brush, g.MeasureString(number, font));
                g.DrawImage(numberBitmap, 69, 1102);
            }
            else if (_isLinkMonster)
            {
                numberBitmap = ImageUtil.RenderText(number, font, brush, g.MeasureString(number, font));
                g.DrawImage(numberBitmap, 740 - 65 - numberBitmap.Width, 865);
            }
            else
            {
                numberBitmap = ImageUtil.RenderText(number, font, brush, g.MeasureString(number, font));
                g.DrawImage(numberBitmap, 740 - numberBitmap.Width, 865);
            }
            
            return this;
        }

        public CardBuilder AddMonsterType(IList<string> types)
        {
            string text = string.Join("/", types);
            float height = 24.5f;
            FontFamily fontFamily = _resourceRepository.GetFontFamily("Yu-Gi-Oh!ITCStoneSerifSmallCaps");
            Font font = new Font(fontFamily, height);

            int startX = 37;
            int startY = 897;

            // Using double left bracket to avoid a wierd spacing issue, and moving the square brackets 2px up
            g.DrawString("[", font, Brushes.Black, startX + g.MeasureString("[", font).Width, startY - 2);
            g.DrawString(text, font, Brushes.Black, startX + g.MeasureString("[[", font).Width, startY);
            g.DrawString("]", font, Brushes.Black, startX + g.MeasureString("[[" + text, font).Width, startY - 2);

            return this;
        }

        public CardBuilder AddLinkArrows(LinkArrows linkArrows)
        {
            if (linkArrows.HasFlag(LinkArrows.TopLeft))
            {
                Image linkArrowImage = _resourceRepository.GetImage($"LINK_ARROW__{StringUtil.ToUnderscoreUpperCase(LinkArrows.TopLeft.ToString())}");
                g.DrawImage(linkArrowImage, 71, 190, linkArrowImage.Width, linkArrowImage.Height);
            }

            if (linkArrows.HasFlag(LinkArrows.Top))
            {
                Image linkArrowImage = _resourceRepository.GetImage($"LINK_ARROW__{StringUtil.ToUnderscoreUpperCase(LinkArrows.Top.ToString())}");
                g.DrawImage(linkArrowImage, 320, 176, linkArrowImage.Width, linkArrowImage.Height);
            }

            if (linkArrows.HasFlag(LinkArrows.TopRight))
            {
                Image linkArrowImage = _resourceRepository.GetImage($"LINK_ARROW__{StringUtil.ToUnderscoreUpperCase(LinkArrows.TopRight.ToString())}");
                g.DrawImage(linkArrowImage, 679, 190, linkArrowImage.Width, linkArrowImage.Height);
            }

            if (linkArrows.HasFlag(LinkArrows.Right))
            {
                Image linkArrowImage = _resourceRepository.GetImage($"LINK_ARROW__{StringUtil.ToUnderscoreUpperCase(LinkArrows.Right.ToString())}");
                g.DrawImage(linkArrowImage, 729, 439, linkArrowImage.Width, linkArrowImage.Height);
            }

            if (linkArrows.HasFlag(LinkArrows.BottomRight))
            {
                Image linkArrowImage = _resourceRepository.GetImage($"LINK_ARROW__{StringUtil.ToUnderscoreUpperCase(LinkArrows.BottomRight.ToString())}");
                g.DrawImage(linkArrowImage, 679, 799, linkArrowImage.Width, linkArrowImage.Height);
            }

            if (linkArrows.HasFlag(LinkArrows.Bottom))
            {
                Image linkArrowImage = _resourceRepository.GetImage($"LINK_ARROW__{StringUtil.ToUnderscoreUpperCase(LinkArrows.Bottom.ToString())}");
                g.DrawImage(linkArrowImage, 320, 849, linkArrowImage.Width, linkArrowImage.Height);
            }

            if (linkArrows.HasFlag(LinkArrows.BottomLeft))
            {
                Image linkArrowImage = _resourceRepository.GetImage($"LINK_ARROW__{StringUtil.ToUnderscoreUpperCase(LinkArrows.BottomLeft.ToString())}");
                g.DrawImage(linkArrowImage, 71, 800, linkArrowImage.Width, linkArrowImage.Height);
            }

            if (linkArrows.HasFlag(LinkArrows.Left))
            {
                Image linkArrowImage = _resourceRepository.GetImage($"LINK_ARROW__{StringUtil.ToUnderscoreUpperCase(LinkArrows.Left.ToString())}");
                g.DrawImage(linkArrowImage, 56, 438, linkArrowImage.Width, linkArrowImage.Height);
            }

            return this;
        }

        public CardBuilder AddAtkAndDef(string attack, string defense, Brush brush)
        {
            int endX = 757;
            int startY = 1100;

            // Add Atk and Def bar
            AddAtkDefBar();

            // Determine the number of digits needed for both
            int maxDigits = Math.Max(attack.Length, defense.Length);

            // Draw DEF section
            Point defCoords = AddAtkOrDef("DEF", defense, brush, endX, startY, maxDigits);

            // Draw ATK section
            int atkDefSpacing = 23;
            Point atkCoords = AddAtkOrDef("ATK", attack, brush, defCoords.X - atkDefSpacing, startY, maxDigits);

            return this;
        }

        public CardBuilder AddPasscode(string passcode, Brush brush)
        {
            // Can only draw if length is 8
            if (passcode == null || passcode.Length != 8)
                return this;

            int fontHeight = 17;
            FontFamily fontFamily = _resourceRepository.GetFontFamily("StoneSerif LT");
            Font font = new Font(fontFamily, fontHeight);

            RectangleF rec = new RectangleF(35, 1147, 150, 1200);
            g.DrawString(passcode, font, brush, rec);

            return this;
        }

        public CardBuilder AddAtkAndLinkRating(string attack, int rating, Brush brush)
        {
            int endX = 757;
            int startY = 1100;

            // Add Atk and Def bar
            AddAtkDefBar();

            // Determine the number of digits needed for both
            int maxDigits = attack.Length;

            // Draw Rating section
            Point ratingCoords = AddLinkRating(rating, brush, 612, endX, startY + 2);

            // Draw ATK section
            int atkDefSpacing = 23;
            Point atkCoords = AddAtkOrDef("ATK", attack, brush, ratingCoords.X - atkDefSpacing, startY, maxDigits);

            return this;
        }

        public CardBuilder AddEditionAndHologram(Edition edition, Brush brush)
        {
            switch (edition)
            {
                case Edition.First:
                    AddEdition(edition, brush);
                    AddEyeOfAnubisHologram(EyeOfAnubisHologram.Gold);
                    break;
                case Edition.Limited:
                    AddEdition(edition, brush);
                    AddEyeOfAnubisHologram(EyeOfAnubisHologram.Gold);
                    break;
                default:
                    AddEyeOfAnubisHologram(EyeOfAnubisHologram.Silver);
                    break;
            }

            return this;
        }

        public CardBuilder AddCreator(string creator, Brush brush)
        {
            FontFamily fontFamily = _resourceRepository.GetFontFamily("StoneSerif LT");

            int creatorFontHeight = 16;
            Font creatorfont = new Font(fontFamily, creatorFontHeight);            
            SizeF creatorSize = g.MeasureString(creator, creatorfont);

            string copyrightText = "©";
            int copyrightFontHeight = 20;
            Font copyrightFont = new Font(fontFamily, copyrightFontHeight);
            SizeF copyrightSize = g.MeasureString(copyrightText, copyrightFont);

            using (var b = new Bitmap((int) creatorSize.Width, (int) creatorSize.Height))
            {
                using (var gr = ImageUtil.GetGraphics(b))
                {
                    gr.DrawString(creator, creatorfont, brush, Point.Empty);

                    Bitmap trimmedCreatorBitmap = ImageUtil.TrimLeft(b);

                    // Squash when too large
                    int maxWidth = 366;
                    float scaledWidth = trimmedCreatorBitmap.Width > maxWidth ? maxWidth : trimmedCreatorBitmap.Width;
                    g.DrawImage(trimmedCreatorBitmap, 749 - scaledWidth, 1147, scaledWidth, trimmedCreatorBitmap.Height);

                    RectangleF copyrightRect = new RectangleF(749 - scaledWidth - 26, 1144, copyrightSize.Width, copyrightSize.Height);
                    g.DrawString(copyrightText, copyrightFont, brush, copyrightRect);
                }
            }

            return this;
        }

        public CardBuilder AddDescription(string description, Brush brush)
        {
            var targetRect = _isMonster ? new RectangleF(63, 936, 701, 160) : new RectangleF(62, 904, 703, 225);

            string[] rawParagraphs = description.Replace("\r\n", "\n").Split(new[] { "\n" }, StringSplitOptions.None)
                .Where(s => s.Trim() != "").ToArray();

            var _paragraphs = new List<Paragraph>();

            for (int i = 0; i < rawParagraphs.Length; ++i)
            {
                string rawParagraph = rawParagraphs[i];
                string fontFamilyName = _isNormalMonster ? "Yu-Gi-Oh! StoneSerif LT" : "Yu-Gi-Oh! Matrix Book";
                bool drawAtBottom = false;

                if (rawParagraph.Contains("(This card is always treated"))
                {
                    fontFamilyName = "Yu-Gi-Oh! Matrix Book";
                    drawAtBottom = _isNormalMonster;
                }
                        
                var paragraph = new Paragraph()
                {
                    RawText = rawParagraph,
                    FontFamily = fontFamilyName,
                    FitOneLine = false,
                    DrawAtBottom = drawAtBottom
                };
                _paragraphs.Add(paragraph);
            }

            DrawParagraphs(targetRect, brush, _paragraphs);

            return this;
        }

        public CardBuilder AddDescription(string description, Brush brush, bool hasMaterials)
        {
            int lineSizeCharLimit = 130;

            string fontFamilyName = "Yu-Gi-Oh! Matrix Book";
            var targetRect = new RectangleF(63, 934, 701, 160);
            string[] paragraphs = description.Replace("\r\n", "\n").Split(new[] { "\n" }, StringSplitOptions.None)
                .Where(s => s.Trim() != "").ToArray();

            var _paragraphs = new List<Paragraph>();

            for (int i = 0; i < paragraphs.Length; ++i)
            {
                string rawText = paragraphs[i];

                var paragraph = new Paragraph()
                {
                    RawText = rawText,
                    FontFamily = fontFamilyName,
                    FitOneLine = hasMaterials && i == 0 && rawText.Length < lineSizeCharLimit,
                    DrawAtBottom = false
                };
                _paragraphs.Add(paragraph);
            }

            DrawParagraphs(targetRect, brush, _paragraphs);

            return this;
        }

        public CardBuilder AddPendulumDescription(string description, Brush brush)
        {
            string fontFamilyName = "Yu-Gi-Oh! Matrix Book";
            var targetRect = new RectangleF(124, 753, 580, 134);
            string[] paragraphs = description.Replace("\r\n", "\n").Split(new[] { "\n" }, StringSplitOptions.None)
                .Where(s => s.Trim() != "").ToArray();

            var _paragraphs = new List<Paragraph>();

            for (int i = 0; i < paragraphs.Length; ++i)
            {
                var paragraph = new Paragraph()
                {
                    RawText = paragraphs[i],
                    FontFamily = fontFamilyName,
                    FitOneLine = false,
                    DrawAtBottom = false
                };
                _paragraphs.Add(paragraph);
            }

            DrawParagraphs(targetRect, brush, _paragraphs);

            return this;
        }

        private void AddCardFrame(Frame frame)
        {
            Image frameImage = _resourceRepository.GetImage($"FRAME__{StringUtil.ToUnderscoreUpperCase(frame.ToString())}");

            g.DrawImage(frameImage, 0, 0, b.Width, b.Height);
        }

        private void AddArtwork(Image artworkImage)
        {
            if (_isPendulumMonster)
            {
                int width = 717;
                double scaleFactor = ((double)width) / artworkImage.Width;
                int height = (int)(artworkImage.Height * scaleFactor);

                // stretch if too short
                height = height > 537 ? height : 537;
                g.DrawImage(artworkImage, 55, 213, width, height);
            }
            else
            {
                g.DrawImage(artworkImage, 98, 219, 632, 632);
            }
        }

        private void AddEdition(Edition edition, Brush brush)
        {
            string editionString;

            switch (edition)
            {
                case Edition.First:
                    editionString = "1\x02E2\x1D57 Edition";
                    break;
                case Edition.Limited:
                    editionString = "LIMITED EDITION";
                    break;
                case Edition.DuelTerminal:
                    editionString = "DUAL TERMINAL";
                    break;
                case Edition.Unlimited:
                    return;
                default:
                    return;
            }

            int fontHeight = 17;
            FontFamily fontFamily = _resourceRepository.GetFontFamily("StoneSerif-SemiBold");
            Font font = new Font(fontFamily, fontHeight);

            RectangleF rec = new RectangleF(150, 1146, 300, 1200);
            g.DrawString(editionString, font, brush, rec);
        }

        private void AddEyeOfAnubisHologram(EyeOfAnubisHologram eyeOfAnubisHologram)
        {
            string imageReference = $"EYE_OF_ANUBIS_HOLOGRAM__{eyeOfAnubisHologram.ToString().ToUpper()}";
            Image hologramImage = _resourceRepository.GetImage(imageReference);
            RectangleF rec = new RectangleF(758, 1135, 42, 42);
            g.DrawImage(hologramImage, rec);
        }

        private void AddAtkDefBar()
        {
            Image atkDefBarImage = _resourceRepository.GetImage("ATKDEF_BAR");
            g.DrawImage(atkDefBarImage, 63, 1092, atkDefBarImage.Width, atkDefBarImage.Height);
        }

        private Point AddAtkOrDef(string label, string value, Brush brush, int endX, int startY, int maxDigits)
        {
            float fontHeight = 27.5f;
            FontFamily fontFamily = _resourceRepository.GetFontFamily("MatrixBoldSmallCaps");
            Font font = new Font(fontFamily, fontHeight);

            string forwardSlash = "/";
            string questionMark = "?";

            int placeholderDigits = Math.Max(maxDigits, 4);
            string placeholderAtkDef = new string('0', placeholderDigits);

            // Get images
            Bitmap labelBitmap = ImageUtil.RenderText(label, font, brush, g.MeasureString(label, font));
            Bitmap valueBitmap = ImageUtil.RenderText(value, font, brush, g.MeasureString(value, font));
            Bitmap forwardSlashBitmap = ImageUtil.RenderText(forwardSlash, font, brush, g.MeasureString(forwardSlash, font));
            Bitmap placeholderAtkDefBitmap = ImageUtil.RenderText(placeholderAtkDef, font, brush, g.MeasureString(placeholderAtkDef, font));
            Bitmap questionMarkBitmap = ImageUtil.RenderText(questionMark, font, brush, g.MeasureString(questionMark, font));

            // Scale question mark to the height of the placeholder
            Bitmap trimmedQuestionMarkBitmap = ImageUtil.Trim(questionMarkBitmap);
            Bitmap trimmedVerticalPlaceholderAtkDef = ImageUtil.Trim(placeholderAtkDefBitmap, true, true, false, false);
            Bitmap scaledQuestionMarkBitmap = ImageUtil.Resize(trimmedQuestionMarkBitmap, trimmedQuestionMarkBitmap.Width, trimmedVerticalPlaceholderAtkDef.Height);

            // Add the top padding above the scaled question mark
            int topPad = ImageUtil.TrimBottom(placeholderAtkDefBitmap).Height - trimmedVerticalPlaceholderAtkDef.Height;
            Bitmap paddedScaledQuestionMarkBitmap = ImageUtil.PadTop(scaledQuestionMarkBitmap, topPad);

            if (value.Contains("?"))
            {
                valueBitmap = paddedScaledQuestionMarkBitmap;
            }

            // Spacing
            int labelForwardSlashSpacing = 4;

            // Draw DEF section
            g.DrawImage(valueBitmap, endX - valueBitmap.Width, startY);
            g.DrawImage(forwardSlashBitmap, endX - placeholderAtkDefBitmap.Width - forwardSlashBitmap.Width, startY);
            g.DrawImage(labelBitmap, endX - placeholderAtkDefBitmap.Width - forwardSlashBitmap.Width - labelBitmap.Width - labelForwardSlashSpacing, startY);

            int startX = endX - labelBitmap.Width - labelForwardSlashSpacing - forwardSlashBitmap.Width - placeholderAtkDefBitmap.Width;
            var coords = new Point(startX, startY);

            return coords;
        }

        private Point AddLinkRating(int rating, Brush brush, int startX, int endX, int startY)
        {
            float fontHeight = 20f;
            FontFamily fontFamily = _resourceRepository.GetFontFamily("Ro GSan Serif Std B");
            Font font = new Font(fontFamily, fontHeight);

            string ratingPrefixText = $"LINK";
            Bitmap ratingPrefixBitmap = ImageUtil.Trim(ImageUtil.RenderText(ratingPrefixText, font, brush, g.MeasureString(ratingPrefixText, font)));

            string hyphen = "-";
            Bitmap hyphenBitmap = ImageUtil.Trim(ImageUtil.RenderText(hyphen, font, brush, g.MeasureString(hyphen, font)));

            string ratingValueText = rating.ToString();
            Bitmap ratingValueBitmap = ImageUtil.Trim(ImageUtil.RenderText(ratingValueText, font, brush, g.MeasureString(ratingValueText, font)));

            float scaleX = 98 / (float)ratingPrefixBitmap.Width;
            float ratingPrefixBitmapWidth = scaleX * ratingPrefixBitmap.Width;
            g.DrawImage(ratingPrefixBitmap, startX, startY, ratingPrefixBitmapWidth, ratingPrefixBitmap.Height);

            float hyphenBitmapWidth = scaleX * hyphenBitmap.Width;
            g.DrawImage(hyphenBitmap, startX + ratingPrefixBitmapWidth + 2, startY + 9, hyphenBitmapWidth, hyphenBitmap.Height);

            float ratingValueBitmapWidth = scaleX * ratingValueBitmap.Width;
            g.DrawImage(ratingValueBitmap, endX - ratingValueBitmapWidth, startY, ratingValueBitmapWidth, ratingPrefixBitmap.Height);

            var coords = new Point(startX, startY);

            return coords;
        }

        private void DrawParagraphs(RectangleF rect, Brush brush, List<Paragraph> paragraphs)
        {      
            // Can't draw anything if the paragraphs are empty
            if (paragraphs.All(p => p.RawText.Trim().Length == 0))
            {
                return;
            }

            TextUtil.TextJustification justification = TextUtil.TextJustification.Full;
            float lineSpacing = 0.8f;
            float indent = 0;
            float paragraphSpacing = 0;

            float maxFontHeight = 18;
            float minimumFontHeight = 13;
            float fontHeight = maxFontHeight;
            float fontHeightDelta = 0.05f;
            bool bestFit = false;
            int maxRemainingY = 4;

            while (!bestFit)
            {
                if (fontHeight < minimumFontHeight)
                {
                    fontHeight = maxFontHeight;
                    maxRemainingY += 1;
                }
                //float remainingArea = CalculateAndSetDescriptionLayout(rect, brush, paragraphs, fontHeight, lineSpacing, paragraphSpacing);
                bestFit = CalculateAndSetDescriptionLayout(rect, brush, paragraphs, fontHeight, lineSpacing, paragraphSpacing, maxRemainingY);

                // Try the next font size down 
                fontHeight -= fontHeightDelta;
            }

            float y = 0;
            for (int k = 0; k < paragraphs.Count; ++k)
            {
                Paragraph paragraph = paragraphs[k];
                FontFamily fontFamily = _resourceRepository.GetFontFamily(paragraph.FontFamily);
                Font font = new Font(fontFamily, fontHeight);

                var stretchedRect = new RectangleF(0, 0, paragraph.TotalWidth, rect.Height);

                // Draw the lines onto a Bitmap
                Bitmap paragraphBitmap = TextUtil.RenderLines(font,
                        brush, paragraph.DrawableLines, stretchedRect, justification,
                        lineSpacing, indent);

                var paragraphWidth = paragraphBitmap.Width * paragraph.ScaleWidth;
                var paragraphHeight = paragraphBitmap.Height;

                // If last paragraph
                if (k == (paragraphs.Count - 1) && paragraph.DrawAtBottom)
                {
                    g.DrawImage(paragraphBitmap, rect.X, rect.Bottom - paragraphHeight, paragraphWidth, paragraphHeight);
                }
                else
                {
                    g.DrawImage(paragraphBitmap, rect.X, rect.Y + y, paragraphWidth, paragraphHeight); // TODO : need a paragraphHeight - y ?
                }

                // If we're not on the last paragraph
                if (k < paragraphs.Count - 1)
                {
                    // Add the line spacing.
                    foreach (var line in paragraph.DrawableLines)
                    {
                        y += g.MeasureString(line, font).Height * 0.7f;
                    }

                    // Add the paragraph spacing
                    y += (font.Height * paragraphSpacing);
                }
                else
                {
                    // Add the height of the last line
                    y += g.MeasureString(paragraph.DrawableLines.Last(), font).Height;
                }
            }
        }

        private bool CalculateAndSetDescriptionLayout(RectangleF rect, Brush brush, List<Paragraph> paragraphs, float fontHeight, float lineSpacing, float paragraphSpacing, int maxRemainingY)
        {
            float y = 0;
            int additionalWidth = 0;
            int maxAdditionalWidth = 290;
            int defaultFontHeight = 18;

            using (Bitmap b = new Bitmap((int)rect.Width + maxAdditionalWidth, (int)rect.Height))
            {
                using (Graphics gr = ImageUtil.GetGraphics(b))
                {
                    gr.Clear(Color.Transparent);

                    for (int i = 0; i < paragraphs.Count; ++i)
                    {
                        Paragraph paragraph = paragraphs[i];
                        paragraph.DrawableLines = new List<string>();
                        
                        FontFamily fontFamily = _resourceRepository.GetFontFamily(paragraph.FontFamily);
                        Font font = new Font(fontFamily, fontHeight);

                        if (paragraph.FitOneLine)
                        {
                            // set the entire paragraph as a single drawable line
                            string line = paragraph.RawText;
                            SizeF lineSize = gr.MeasureString(line, font);

                            paragraph.DrawableLines.Add(line);
                            paragraph.TotalWidth = lineSize.Width;
                            paragraph.ScaleWidth = lineSize.Width > rect.Width ? rect.Width / lineSize.Width : 1;

                            // Move down to draw the next line.
                            y += lineSize.Height * 0.7f;
                        }
                        else
                        {
                            int maxWidth = (int)rect.Width + additionalWidth;
                            // Break the text into words.
                            string[] words = paragraph.RawText.Split(' ');
                            int startWordIndex = 0;

                            // Repeat until we run out of text or room.
                            for (; ; )
                            {
                                // See how many words will fit.
                                // Start with just the next word.
                                string line = words[startWordIndex];
                                SizeF lineSize;

                                // Add more words until the line won't fit.
                                int endWordIndex = startWordIndex + 1;
                                while (endWordIndex < words.Length)
                                {
                                    // See if the next word fits.
                                    string trialLine = line + " " + words[endWordIndex];
                                    SizeF trialLineSize = gr.MeasureString(trialLine, font);

                                    // if the line is longer than our rectangle
                                    if (trialLineSize.Width > maxWidth)
                                    {
                                        // The line is too wide. Don't use the last word.
                                        endWordIndex--;
                                        break;
                                    }
                                    else
                                    {
                                        // The word fits. Save the test line.
                                        line = trialLine;
                                        lineSize = trialLineSize;
                                    }

                                    // Try the next word.
                                    endWordIndex++;
                                }

                                paragraph.DrawableLines.Add(line);
                                
                               
                                // Move down to draw the next line.
                                y += lineSize.Height * 0.7f;

                                // Start the next line at the next word.
                                startWordIndex = endWordIndex + 1;
                                if (startWordIndex >= words.Length) break;
                            }

                            paragraph.TotalWidth =  maxWidth;
                            paragraph.ScaleWidth = rect.Width / maxWidth;
                        }

                        // If last 
                        if (i == (paragraphs.Count - 1))
                        {
                            // If we were on the last word of the last paragraph then add the height of the text
                            string lastLine = paragraphs.Last().DrawableLines.Last();
                            SizeF lastLineSize = gr.MeasureString(lastLine, font);
                            y += lastLineSize.Height * 0.3f;

                            // Check how much space is left vertically
                            float remainingY = rect.Height - y;

                            if (!(font.Size == defaultFontHeight && additionalWidth == 0) && remainingY > maxRemainingY)
                            {
                                // We're too short
                                return false;
                            }
                            else if (remainingY < 0)
                            {
                                if (additionalWidth == maxAdditionalWidth)
                                {
                                    return false; 
                                }
                                else
                                {
                                    additionalWidth += 5;

                                    // reset looping through paragraph
                                    i = -1;
                                    y = 0;
                                    gr.Clear(Color.Transparent);
                                }             
                            }       
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            // Add a gap after the paragraph.
                            y += font.Height * paragraphSpacing;
                        }
                    }
                }
            }

            // shouldn't get here
            return false;
        }  
    }
}
