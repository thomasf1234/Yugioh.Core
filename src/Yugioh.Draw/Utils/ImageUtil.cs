using System.Drawing;
using System.Drawing.Text;

namespace Yugioh.Draw.Utils
{
    public static class ImageUtil
    {
        public static Bitmap Resize(Bitmap originalBitmap, int width, int height)
        {
            Bitmap newBitmap = new Bitmap(width, height);
            using (Graphics g = GetGraphics(newBitmap))
            {
                g.DrawImage(originalBitmap, 0, 0, width, height);
            }

            return newBitmap;
        }

        public static Bitmap PadTop(Bitmap originalBitmap, int pad)
        {
            return Pad(originalBitmap, pad, 0, 0, 0);
        }

        public static Bitmap PadBottom(Bitmap originalBitmap, int pad)
        {
            return Pad(originalBitmap, 0, pad, 0, 0);
        }

        public static Bitmap PadLeft(Bitmap originalBitmap, int pad)
        {
            return Pad(originalBitmap, 0, 0, pad, 0);
        }

        public static Bitmap PadRight(Bitmap originalBitmap, int pad)
        {
            return Pad(originalBitmap, 0, 0, 0, pad);
        }

        public static Bitmap Pad(Bitmap originalBitmap, int topPad, int bottomPad, int leftPad, int rightPad)
        {
            Bitmap newBitmap = new Bitmap(originalBitmap.Width + leftPad + rightPad, originalBitmap.Height + topPad + bottomPad);
            using (Graphics g = GetGraphics(newBitmap))
            {
                g.DrawImage(originalBitmap, leftPad, topPad, originalBitmap.Width, originalBitmap.Height);
            }

            return newBitmap;
        }

        public static Graphics GetGraphics(Bitmap b)
        {
            Graphics g = Graphics.FromImage(b);
                
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            return g;
        }

        public static Bitmap RenderText(string text, Font font, Brush brush, SizeF size)
        {
            using (Bitmap b = new Bitmap((int) size.Width, (int) size.Height))
            {
                using (Graphics gr = ImageUtil.GetGraphics(b))
                {
                    gr.Clear(Color.Transparent);
                    gr.DrawString(text, font, brush, Point.Empty, StringFormat.GenericTypographic);
                    return PadRight(TrimRight(b), 1);
                }
            }
        }

        public static Bitmap Trim(Bitmap b)
        {
            return Trim(b, true, true, true, true);
        }

        public static Bitmap Trim(Bitmap b, bool trimTop, bool trimBottom, bool trimLeft, bool trimRight)
        {
            int width = b.Width;
            int height = b.Height;

            int startX = 0;
            int startY = 0;
            int endX = width;
            int endY = height;

            if (trimTop)
            {
                // Loop pixel rows from the top
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        // Get pixel
                        Color pixelColor = b.GetPixel(j, i);

                        // Check if not transparent
                        if (pixelColor.A > 0)
                        {
                            startY = i;

                            // finish searching
                            i = height;
                            j = width;
                        }
                    }
                }
            }
            

            if (trimBottom)
            {
                // Loop pixel rows from the bottom
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        // Get pixel
                        Color pixelColor = b.GetPixel(j, (height - 1) - i);

                        // Check if not transparent
                        if (pixelColor.A > 0)
                        {
                            endY = height - i;

                            // finish searching
                            i = height;
                            j = width;
                        }
                    }
                }
            }
            
            if (trimLeft)
            {
                // Loop pixel rows from the left
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        // Get pixel
                        Color pixelColor = b.GetPixel(i, j);

                        // Check if not transparent
                        if (pixelColor.A > 0)
                        {
                            startX = i;

                            // finish searching
                            i = width;
                            j = height;
                        }
                    }
                }
            }
            
            if (trimRight)
            {
                // Loop pixel rows from the right
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        // Get pixel
                        Color pixelColor = b.GetPixel((width - 1) - i, j);

                        // Check if not transparent
                        if (pixelColor.A > 0)
                        {
                            endX = width - i;

                            // finish searching
                            i = width;
                            j = height;
                        }
                    }
                }
            }
            
            Bitmap trimmedBitmap = Crop(b, new Rectangle(startX, startY, endX - startX, endY - startY)); 

            return trimmedBitmap;
        }

        public static Bitmap TrimTop(Bitmap b)
        {
            return Trim(b, true, false, false, false);
        }

        public static Bitmap TrimBottom(Bitmap b)
        {
            return Trim(b, false, true, false, false);
        }

        public static Bitmap TrimLeft(Bitmap b)
        {
            return Trim(b, false, false, true, false);
        }

        public static Bitmap TrimRight(Bitmap b)
        {
            return Trim(b, false, false, false, true);
        }

        public static Bitmap Crop(Bitmap b, Rectangle r)
        {
            Bitmap nb = new Bitmap(r.Width, r.Height);
            using (Graphics g = Graphics.FromImage(nb))
            {
                g.DrawImage(b, -r.X, -r.Y);
                return nb;
            }
        }
    }
}
