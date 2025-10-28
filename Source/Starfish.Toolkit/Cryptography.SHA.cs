using System.Security.Cryptography;

namespace Nerosoft.Starfish.Toolkit;

internal partial class Cryptography
{
    /// <summary>
    /// The SHA encryption class.
    /// </summary>
    public class SHA
    {
        /// <summary>
        /// Encrypts the specified string using the SHA-256 hashing algorithm.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Encrypt(string source)
        {
            var bytes = Encoding.UTF8.GetBytes(source);

#if NETSTANDARD2_1
			byte[] output;
			using (var sha = SHA256.Create())
			{
				output = sha.ComputeHash(bytes);
			}
#else
            var output = SHA256.HashData(bytes);
#endif
            return Convert.ToBase64String(output);
        }
    }
}