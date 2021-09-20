using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using Yugioh.Draw.Utils;

namespace Yugioh.Draw.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly ResourceManager _componentsResourceManager;
        private readonly ResourceManager _fontsResourceManager;
        private readonly PrivateFontCollection _fontCollection;
        private readonly Dictionary<string, Image> _componentImages;
        private readonly Dictionary<string, FontFamily> _fontFamilities;
        public ResourceRepository()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            _componentsResourceManager = new ResourceManager("Yugioh.Draw.Resources.Components", assembly);
            _fontsResourceManager = new ResourceManager("Yugioh.Draw.Resources.Fonts", assembly);
            _fontCollection = new PrivateFontCollection();
            _fontFamilities = new Dictionary<string, FontFamily>();

            // Load fonts
            ResourceSet fontsResourceSet = _fontsResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in fontsResourceSet)
            {
                byte[] fontBytes = (byte[])entry.Value;

                var handle = GCHandle.Alloc(fontBytes, GCHandleType.Pinned);
                IntPtr pointer = handle.AddrOfPinnedObject();
                try
                {
                    _fontCollection.AddMemoryFont(pointer, fontBytes.Length);
                }
                finally
                {
                    handle.Free();
                }
            }

            // Load components
            _componentImages = new Dictionary<string, Image>();
            ResourceSet componentsResourceSet = _componentsResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in componentsResourceSet)
            {
                string reference = (string)entry.Key;
                byte[] imageBytes = (byte[])entry.Value;
                Image componentImage = ImageUtil.FromBytes(imageBytes);
                _componentImages.Add(reference, componentImage);
            }
        }

        // https://www.cardmaker.net/forums/topic/308603-fonts-for-yu-gi-oh-card-making-with-multilingual-support/
        public FontFamily GetFontFamily(string name)
        {
            if (name == "Yu-Gi-Oh! Matrix Small Caps 1")
            {
                ;
            }
            // Caching - strange bug were the font familiy fails to be found on subsequent calls (2-4)
            if (_fontFamilities.ContainsKey(name))
            {
                return _fontFamilities[name];
            }

            var fontFamily = new FontFamily(name, _fontCollection);
            _fontFamilities.Add(name, fontFamily);

            return fontFamily;
        }

        public Image GetImage(string reference)
        {
            return _componentImages[reference];
        }
    }
}
