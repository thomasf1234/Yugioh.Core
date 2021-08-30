using System.Drawing;
using System.Drawing.Text;
using System.IO;

namespace Yugioh.Draw.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly string _resourcesPath;
        private readonly PrivateFontCollection _fontCollection;
        public ResourceRepository(string resourcesPath)
        {
            _resourcesPath = resourcesPath;
            _fontCollection = new PrivateFontCollection();

            string[] fontFilePaths = Directory.GetFiles(Path.Combine(_resourcesPath, "fonts"), "*", SearchOption.AllDirectories);
            foreach (string fontPath in fontFilePaths)
            {
                _fontCollection.AddFontFile(fontPath);
            }
        }

        // https://www.cardmaker.net/forums/topic/308603-fonts-for-yu-gi-oh-card-making-with-multilingual-support/
        public FontFamily GetFontFamily(string name)
        {
            return new FontFamily(name, _fontCollection);
        }

        public Image GetImage(string reference)
        {
            string imagePath = Path.Combine(_resourcesPath, $"images/{reference}.png");
            return Image.FromFile(imagePath);
        }
    }
}
