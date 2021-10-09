using System.Collections.Generic;
using System.Drawing;

namespace Yugioh.Draw.Utils
{
    public static class TextUtil
    {
        public enum TextJustification
        {
            Left,
            Right,
            Center,
            Full
        }

        public static Bitmap RenderLines(Font font, Brush brush, 
            List<string> lines, RectangleF rect,
            TextJustification justification, float lineSpacing,
            float indent)
        {
            var bitmap = new Bitmap((int)rect.Width, (int)rect.Height);

            using (Graphics g = ImageUtil.GetGraphics(bitmap))
            {
                g.Clear(Color.Transparent);

                DrawLines(g, rect, font,
                    brush, lines, justification,
                    lineSpacing, indent);
            }

            bitmap = ImageUtil.PadBottom(ImageUtil.TrimRight(ImageUtil.TrimBottom(bitmap)), 1);

            return bitmap;
        }

        public static void DrawLines(Graphics gr, RectangleF rect,
            Font font, Brush brush, List<string> lines,
            TextJustification justification, float lineSpacing,
            float indent)
        {
            float y = 0;

            int lineCount = lines.Count;
            for (int i = 0; i < lineCount; ++i)
            {
                string line = lines[i];

                if (i == (lineCount - 1) &&
            (justification == TextUtil.TextJustification.Full))
                {
                    // This is the last line. Don't justify it.
                    TextUtil.DrawLine(gr, line, font, brush,
                        0,
                        y,
                        rect.Width - indent,
                        TextUtil.TextJustification.Left);
                }
                else
                {
                    // This is not the last line. Justify it.
                    TextUtil.DrawLine(gr, line, font, brush,
                        0,
                        y,
                        rect.Width - indent,
                        justification);
                }

                // Move down to draw the next line.
                //y += font.Height * lineSpacing;
                y += gr.MeasureString(line, font).Height * 0.7f;
            }
        } 

        // Draw a line of text.
        public static void DrawLine(Graphics gr, string line, Font font,
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
        public static void DrawJustifiedLine(Graphics gr, RectangleF rect,
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
