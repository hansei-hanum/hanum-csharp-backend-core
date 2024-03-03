
namespace Hanum.Core.Helpers;

public static class StringExtensions {
    public static string Truncate(this string value, int maxLength, string suffix = "...") =>
        value.Length <= maxLength ? value : value[..maxLength] + suffix;
}

