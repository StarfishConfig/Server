namespace Nerosoft.Starfish.Toolkit;

internal partial class Cryptography
{
    /// <summary>
    /// The Base64 encoding and decoding class.
    /// </summary>
    public class Base64
    {
        /// <summary>
        /// Encodes the specified string to Base64 format.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Encrypt(string source)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
        }

        /// <summary>
        /// Decodes the specified Base64 encoded string.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Decrypt(string source)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(source));
        }
    }
}