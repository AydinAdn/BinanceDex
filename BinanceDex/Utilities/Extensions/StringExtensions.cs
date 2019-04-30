namespace BinanceDex.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static string Prepend(this string text, string textToPrepend)
        {
            return textToPrepend + text;
        }

        public static bool IsNullOrWhiteSpace(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        public static string ToCamelCase(this string v)
        {
            string firstChar = v[0].ToString().ToLower();
            return firstChar + v.Substring(1);
        }
    }
}
