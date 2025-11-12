using System.Security.Cryptography;

namespace Nerosoft.Starfish.Toolkit;

/// <summary>
/// Utility class for generating random passwords with specified complexity and length.
/// </summary>
public static class PasswordGenerator
{
    private const string LOWERCASE_CHARS = "abcdefghijklmnopqrstuvwxyz";
    private const string UPPERCASE_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string DIGIT_CHARS = "0123456789";
    private const string SYMBOL_CHARS = "!@#$%^&*()_+-=[]{}|;:,.<>?";

    public static string GeneratePassword(Complexity complexity, int minLength = 8, int maxLength = 16)
    {
        // Check length constraints
        if (maxLength < 0)
        {
            throw new ArgumentException("The max length must be non-negative.", nameof(maxLength));
        }

        if (maxLength < minLength)
        {
            maxLength = minLength;
        }

        var length = RandomNumberGenerator.GetInt32(minLength, maxLength + 1);

        // Check complexity constraints
        if (complexity == Complexity.None)
        {
            return GenerateSimplePassword(length);
        }

        // Build required character sets
        var requiredChars = new List<string>();
        if (complexity.HasFlag(Complexity.ContainsLowercase))
        {
            requiredChars.Add(LOWERCASE_CHARS);
        }

        if (complexity.HasFlag(Complexity.ContainsUppercase))
        {
            requiredChars.Add(UPPERCASE_CHARS);
        }

        if (complexity.HasFlag(Complexity.ContainsDigit))
        {
            requiredChars.Add(DIGIT_CHARS);
        }

        if (complexity.HasFlag(Complexity.ContainsSymbol))
        {
            requiredChars.Add(SYMBOL_CHARS);
        }

        // if no specific requirements, use all character types
        if (requiredChars.Count == 0)
        {
            requiredChars.Add(LOWERCASE_CHARS + UPPERCASE_CHARS + DIGIT_CHARS);
        }

        // Validate required character sets
        if (requiredChars.Count == 0)
        {
            throw new ArgumentException("Complexity must specify at least one character type.", nameof(complexity));
        }

        var charPool = requiredChars.SelectMany(s => s).Distinct().ToList();

        // Validate character pool
        if (charPool.Count == 0)
        {
            throw new Exception("字符池为空，无法生成密码");
        }

        // Generate password
        var passwordChars = new char[length];
        var charSetUsed = new bool[requiredChars.Count];
        var setsToSatisfy = 0;

        using (var rng = RandomNumberGenerator.Create())
        {
            // Ensure each required character set is represented at least once
            for (var i = 0; i < length && setsToSatisfy < requiredChars.Count; i++)
            {
                // Choose a character set that has not been used yet
                var setIndex = -1;
                for (var j = 0; j < requiredChars.Count; j++)
                {
                    if (charSetUsed[j])
                    {
                        continue;
                    }

                    if (setIndex == -1 || j < setIndex)
                    {
                        setIndex = j;
                    }
                }

                if (setIndex != -1)
                {
                    // Choose a random character from the selected set
                    var setChars = requiredChars[setIndex];
                    int charIndex;
                    do
                    {
                        charIndex = GetRandomCharIndex(rng, setChars.Length);
                    }
                    while (charSetUsed[setIndex] && i < length); // Ensure we don't overwrite already set characters

                    char c = setChars[charIndex];
                    passwordChars[i] = c;
                    charSetUsed[setIndex] = true;
                    setsToSatisfy++;
                }
                else
                {
                    // If all sets are used, fill with random characters from the pool
                    var charIndex = GetRandomCharIndex(rng, charPool.Count);
                    passwordChars[i] = charPool[charIndex];
                }
            }

            // Fill the remaining characters randomly from the full pool
            for (var i = setsToSatisfy; i < length; i++)
            {
                var charIndex = GetRandomCharIndex(rng, charPool.Count);
                passwordChars[i] = charPool[charIndex];
            }
        }

        return new string(passwordChars);
    }

    private static string GenerateSimplePassword(int length)
    {
        using var rng = RandomNumberGenerator.Create();
        const string chars = LOWERCASE_CHARS + UPPERCASE_CHARS + DIGIT_CHARS;
        var charPool = chars.ToCharArray();
        var password = new char[length];

        for (var index = 0; index < length; index++)
        {
            var charIndex = GetRandomCharIndex(rng, chars.Length);
            password[index] = charPool[charIndex];
        }

        return new string(password);
    }

    private static int GetRandomCharIndex(RandomNumberGenerator rng, int max)
    {
        var buffer = new byte[4];
        rng.GetBytes(buffer);
        var result = BitConverter.ToInt32(buffer, 0) & int.MaxValue;
        return result % max;
    }

    [Flags]
    public enum Complexity
    {
        None = 0,
        ContainsLowercase = 1,
        ContainsUppercase = 2,
        ContainsDigit = 4,
        ContainsSymbol = 8,
        All = ContainsLowercase | ContainsUppercase | ContainsDigit | ContainsSymbol
    }
}