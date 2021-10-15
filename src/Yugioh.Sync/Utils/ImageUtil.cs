using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Yugioh.Sync.Utils
{
    public static class ImageUtil
    {
        public static void ExportAsJpeg(Image image, string filePath, long jpegQuality)
        {
            ImageCodecInfo jgpEncoder = ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, jpegQuality);
            myEncoderParameters.Param[0] = myEncoderParameter;
            image.Save(filePath, jgpEncoder, myEncoderParameters);
        }
    }
}
