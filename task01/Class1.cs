using System;

namespace task01;

public static class StringExtensions {
    public static bool IsPalindrome(this string input) {
        if (string.IsNullOrEmpty(input))
            return false;

        string cleaned = new([.. input.Where(c => !char.IsPunctuation(c) && !char.IsWhiteSpace(c)).Select(char.ToLower)]);

        if (cleaned.Length == 0)
            return false;

        return cleaned.SequenceEqual(cleaned.Reverse());
    }
}
