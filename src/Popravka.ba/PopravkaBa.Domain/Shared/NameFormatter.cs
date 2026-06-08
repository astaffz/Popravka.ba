using System.Globalization;

namespace PopravkaBa.Domain.Shared
{
    public static class NameFormatter
    {
        public static string NormalizeName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            var culture = new CultureInfo("bs-Latn-BA");
            return culture.TextInfo.ToTitleCase(input.Trim().ToLower(culture));
        }
    }
}
