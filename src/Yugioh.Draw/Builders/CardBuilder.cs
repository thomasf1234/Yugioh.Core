using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Resources;
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
        PendulumWickedBeast,
        PendulumXyz,
        Ritual,
        SacredBeastBlue,
        SacredBeastRed,
        SacredBeastYellow,
        Spell,
        Synchro,
        Token,
        Trap,
        WickedBeast,
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
        Trap = 9
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

    public enum TextJustification
    {
        Left,
        Right,
        Center,
        Full
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

            int nameWidthUpperLimit = 610;
            Bitmap nameBitmap = ImageUtil.RenderText(name, font, brush, g.MeasureString(name, font));
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
            // Cannot draw greater than 8
            if (passcode.Length != 8)
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
            // TODO : Revisit
            int fontHeight = 16;
            FontFamily fontFamily = _resourceRepository.GetFontFamily("StoneSerif LT");
            Font font = new Font(fontFamily, fontHeight);

            RectangleF rec = new RectangleF(470, 1147, 500, 1200);
            g.DrawString(creator, font, brush, rec);

            RectangleF rec2 = new RectangleF(449, 1145, 40, 1200);
            Font copyrightFont = new Font(fontFamily, 20);
            g.DrawString("©", copyrightFont, brush, rec2);

            return this;
        }

        public CardBuilder AddDescription(string description, Brush brush)
        {
            string fontFamilyName = _isNormalMonster ? "Yu-Gi-Oh! StoneSerif LT" : "Yu-Gi-Oh! Matrix Book";
            var targetRect = _isMonster ? new RectangleF(65, 936, 700, 154) : new RectangleF(62, 904, 703, 225);
            
            AddDescription(description, fontFamilyName, brush, targetRect);

            return this;
        }

        public CardBuilder AddPendulumDescription(string description, Brush brush)
        {
            string fontFamilyName = "Yu-Gi-Oh! Matrix Book";
            var targetRect = new RectangleF(124, 753, 580, 134);

            AddDescription(description, fontFamilyName, brush, targetRect);

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

        private void AddDescription(string description, string fontFamilyName, Brush brush, RectangleF rect)
        {
            FontFamily fontFamily = _resourceRepository.GetFontFamily(fontFamilyName);
            float lineSpacing = 0.8f;

            DrawParagraphs(g, rect, fontFamily, brush, description, TextJustification.Full, lineSpacing, 0, 0);
        }

        // http://csharphelper.com/blog/2014/10/fully-justify-paragraphs-of-text-in-c/
        // Draw justified text on the Graphics object in the indicated Rectangle.
        private void DrawParagraphs(Graphics gr, RectangleF rect,
            FontFamily fontFamily, Brush brush, string text,
            TextJustification justification, float lineSpacing,
            float indent, float paragraphSpacing)
        {
            // Split the text into paragraphs.
            string[] paragraphs = text.Replace("\r\n", "\n").Split(new[] { "\n" }, StringSplitOptions.None);
            float defaultFontHeight = 18;
            float fontHeight = defaultFontHeight;
            List<List<string>> paragraphLines = new List<List<string>>();

            bool fitsRectangle = false;
            while (!fitsRectangle)
            {
                Font font = new Font(fontFamily, fontHeight);

                // Reset our lines
                paragraphLines.Clear();

                // Get the coordinates for the first line.
                float y = rect.Top;

                int paragraphCount = paragraphs.Length;

                for (int i = 0; i < paragraphCount; ++i)
                {
                    string paragraph = paragraphs[i];
                    var lines = new List<string>();

                    // Break the text into words.
                    string[] words = paragraph.Split(' ');
                    int startWordIndex = 0;

                    // Repeat until we run out of text or room.
                    for (; ; )
                    {
                        // See how many words will fit.
                        // Start with just the next word.
                        string line = words[startWordIndex];

                        // Add more words until the line won't fit.
                        int endWordIndex = startWordIndex + 1;
                        while (endWordIndex < words.Length)
                        {
                            // See if the next word fits.
                            string trialLine = line + " " + words[endWordIndex];
                            SizeF line_size = gr.MeasureString(trialLine, font);
                            if (line_size.Width > rect.Width)
                            {
                                // The line is too wide. Don't use the last word.
                                endWordIndex--;
                                break;
                            }
                            else
                            {
                                // The word fits. Save the test line.
                                line = trialLine;
                            }

                            // Try the next word.
                            endWordIndex++;
                        }

                        lines.Add(line);

                        // Move down to draw the next line.
                        y += font.Height * lineSpacing;

                        // Make sure there's room for another line.
                        if (font.Size > rect.Height) break;

                        // Start the next line at the next word.
                        startWordIndex = endWordIndex + 1;
                        if (startWordIndex >= words.Length) break;

                        // Don't indent subsequent lines in this paragraph.
                        indent = 0;
                    }

                    paragraphLines.Add(lines);

                    if (i < (paragraphCount - 1))
                    {
                        // Add a gap after the paragraph.
                        y += font.Height * paragraphSpacing;
                    }
                }

                // Return a RectangleF representing any unused
                // space in the original RectangleF.
                float height = rect.Bottom - y;
                if (height < 0)
                {
                    // Try the next size down
                    fontHeight -= 0.05f;
                }
                else
                {
                    fitsRectangle = true;
                }
            }

            Font finalFont = new Font(fontFamily, fontHeight);
            float y2 = rect.Top;

            foreach (List<string> lines in paragraphLines)
            {
                int lineCount = lines.Count;
                for (int i = 0; i < lineCount; ++i)
                {
                    string line = lines[i];
                    if (i == (lineCount - 1) &&
                (justification == TextJustification.Full))
                    {
                        // This is the last line. Don't justify it.
                        DrawLine(gr, line, finalFont, brush,
                            rect.Left + indent,
                            y2,
                            rect.Width - indent,
                            TextJustification.Left);
                    }
                    else
                    {
                        // This is not the last line. Justify it.
                        DrawLine(gr, line, finalFont, brush,
                            rect.Left + indent,
                            y2,
                            rect.Width - indent,
                            justification);
                    }

                    // Move down to draw the next line.
                    y2 += finalFont.Height * lineSpacing;
                }

                // Add a gap after the paragraph.
                y2 += finalFont.Height * paragraphSpacing;
            }
        }

        // Draw a line of text.
        private void DrawLine(Graphics gr, string line, Font font,
            Brush brush, float x, float y, float width,
            TextJustification justification)
        {
            // Make a rectangle to hold the text.
            RectangleF rect = new RectangleF(x, y, width, font.Height);

            // See if we should use full justification.
            if (justification == TextJustification.Full)
            {
                // Justify the text.
                DrawJustifiedLine(gr, rect, font, brush, line);
            }
            else
            {
                // Make a StringFormat to align the text.
                using (StringFormat sf = new StringFormat())
                {
                    // Use the appropriate alignment.
                    switch (justification)
                    {
                        case TextJustification.Left:
                            sf.Alignment = StringAlignment.Near;
                            break;
                        case TextJustification.Right:
                            sf.Alignment = StringAlignment.Far;
                            break;
                        case TextJustification.Center:
                            sf.Alignment = StringAlignment.Center;
                            break;
                    }

                    gr.DrawString(line, font, brush, rect, sf);
                }
            }
        }

        // Draw justified text on the Graphics object
        // in the indicated Rectangle.
        private void DrawJustifiedLine(Graphics gr, RectangleF rect,
            Font font, Brush brush, string text)
        {
            // Break the text into words.
            string[] words = text.Split(' ');

            // Add a space to each word and get their lengths.
            float[] word_width = new float[words.Length];
            float total_width = 0;
            for (int i = 0; i < words.Length; i++)
            {
                // See how wide this word is.
                SizeF size = gr.MeasureString(words[i], font);
                word_width[i] = size.Width;
                total_width += word_width[i];
            }

            // Get the additional spacing between words.
            float extra_space = rect.Width - total_width;
            int num_spaces = words.Length - 1;
            if (words.Length > 1) extra_space /= num_spaces;

            // Draw the words.
            float x = rect.Left;
            float y = rect.Top;
            for (int i = 0; i < words.Length; i++)
            {
                // Draw the word.
                gr.DrawString(words[i], font, brush, x, y);

                // Move right to draw the next word.
                x += word_width[i] + extra_space;
            }
        }
    }
}
