using System.Drawing;

namespace Yugioh.Draw.Repositories
{
    public interface IResourceRepository
    {
        Image GetImage(string imagePath);
        FontFamily GetFontFamily(string name);
    }
}

