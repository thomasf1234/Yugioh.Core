using System.Drawing;
using System.IO;

namespace Yugioh.Data.Utils
{
    public static class ImageUtil
    {
        public static byte[] ToBytes(Image image)
        {
            using (var mStream = new MemoryStream())
            {
                image.Save(mStream, image.RawFormat);
                return mStream.ToArray();
            }
        }

        public static Image FromBytes(byte[] imageBytes)
        {
            using (var mStream = new MemoryStream(imageBytes))
            {
                return Image.FromStream(mStream);
            }
        }
    }
}
