using System;
using System.Linq;


namespace Acr.Utilities
{
    public static class StringUtils
    {

        public static bool IsEmpty(this string @string)
        {
            return String.IsNullOrWhiteSpace(@string);
        }


        public static byte[] FromHex(this string hex)
        {
            hex = hex
                .Replace("-", String.Empty)
                .Replace(" ", String.Empty);

            return Enumerable
                .Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }


        public static string ToHexString(this byte[] bytes)
        {
            return String.Concat(bytes.Select(b => b.ToString("X2")));
        }
    }
}
