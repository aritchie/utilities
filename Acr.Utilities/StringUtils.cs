using System;


namespace Acr.Utilities
{
    public static class StringUtils
    {

        public static bool IsEmpty(this string @string)
        {
            return String.IsNullOrWhiteSpace(@string);
        }
    }
}
