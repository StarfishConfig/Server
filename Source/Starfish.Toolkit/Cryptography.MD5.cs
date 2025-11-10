namespace Nerosoft.Starfish.Toolkit;

public partial class Cryptography
{
    /// <summary>
    /// The MD5 encryption class.
    /// </summary>
    public class MD5
    {
        /// <summary>
        /// Encrypts the specified string using the MD5 hashing algorithm.
        /// </summary>
        /// <param name="value">The origin string to be encrypted.</param>
        /// <returns>The MD5 string.</returns>
        public static string Encrypt(string value)
        {
#if NETSTANDARD2_1
			using var md5 = System.Security.Cryptography.MD5.Create();
			var bytes = md5.ComputeHash(Encoding.Default.GetBytes(value));
#elif NET6_0_OR_GREATER
            var bytes = System.Security.Cryptography.MD5.HashData(Encoding.Default.GetBytes(value));
#endif
            var builder = new StringBuilder();
            foreach (var @byte in bytes)
            {
                builder.Append(@byte.ToString("x").PadLeft(2, '0'));
            }

            return builder.ToString();
        }
    }
}