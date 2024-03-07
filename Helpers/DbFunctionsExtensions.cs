using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace Hanum.Core.Helpers;

public static partial class DbFunctionsExtensions {
    public static string GetExtendedMatchPattern(this DbFunctions _, string pattern) {
        var words = pattern.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < words.Length; i++) {
            var isNegated = words[i].StartsWith('-');
            var word = isNegated ? words[i][1..] : words[i];

            words[i] = $"{(isNegated ? "-" : "+")}\"*{word}*\"";
        }

        pattern = string.Join(" ", words);

        return pattern;
    }
}
