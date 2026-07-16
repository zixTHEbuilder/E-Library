namespace E_Library.Helpers
{
    public class StringHelper
    {
        public static bool NullOrEmptyChecker(params string[] values)
        {
            return values.Any(string.IsNullOrEmpty);
        }
        public int? CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            var words = text.Split([' ', '\t', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries);

            return words.Length;
        }
        public string? TrimString(string text)
        {
            if (text is null) return null;
           return text.Trim();
        }
    }
}
