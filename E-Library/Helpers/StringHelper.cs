namespace E_Library.Helpers
{
    public class StringHelper
    {
        public static bool NullOrEmptyChecker(params string[] values)
        {
            return values.Any(string.IsNullOrEmpty);
        }
    }
}
