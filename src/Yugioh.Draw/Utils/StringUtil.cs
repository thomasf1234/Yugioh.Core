using System.Linq;

namespace Yugioh.Draw.Utils
{
    public static class StringUtil
    {
        public static string ToUnderscoreUpperCase(string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();
        }
    }
}
