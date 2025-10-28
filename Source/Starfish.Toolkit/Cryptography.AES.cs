using System.Security.Cryptography;

namespace Nerosoft.Starfish.Toolkit;

internal partial class Cryptography
{
    /// <summary>
    /// The AES encryption and decryption class.
    /// </summary>
    public class AES
    {
        /// <summary>
        /// The default salt value used for encryption and decryption.
        /// </summary>
        private static readonly byte[] _defaultSalt = { 0x03, 0x0B, 0x13, 0x1B, 0x23, 0x2B, 0x33, 0x3B, 0x43, 0x4B, 0x9B, 0x93, 0x8B, 0x83, 0x7B, 0x73, 0x6B, 0x63, 0x5B, 0x53, 0xF3, 0xFB, 0xA3, 0xAB, 0xB3, 0xBB, 0xC3, 0xEB, 0xE3, 0xDB, 0xD3, 0xCB };

        /// <summary>
        /// Encrypts the specified source string using AES encryption.
        /// </summary>
        /// <param name="source">The source string to be encrypted.</param>
        /// <returns></returns>
        public static string Encrypt(string source)
        {
            return Encrypt(source, _defaultSalt);
        }

        /// <summary>
        /// Encrypts the specified source string using AES encryption with the provided key.
        /// </summary>
        /// <param name="source">The source string to be encrypted.</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string source, string key)
        {
            return Encrypt(source, Encoding.UTF8.GetBytes(key));
        }

        /// <summary>
        /// Encrypts the specified source string using AES encryption with the provided key.
        /// </summary>
        /// <param name="source">The source string to be encrypted.</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string source, byte[] key)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }

            var bytes = Encoding.UTF8.GetBytes(source);

            using var rm = Aes.Create();
            rm.Key = key;
            rm.Mode = CipherMode.ECB;
            rm.Padding = PaddingMode.PKCS7;
            var encryptor = rm.CreateEncryptor();
            var result = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// Decrypts the specified source string using AES decryption.
        /// </summary>
        /// <param name="source">The string to be decrypted.</param>
        /// <returns>The origin string.</returns>
        public static string Decrypt(string source)
        {
            return Decrypt(source, _defaultSalt);
        }

        /// <summary>
        /// Decrypts the specified source string using AES decryption with the provided key.
        /// </summary>
        /// <param name="source">The string to be decrypted.</param>
        /// <param name="key"></param>
        /// <returns>The origin string.</returns>
        public static string Decrypt(string source, string key)
        {
            return Decrypt(source, Encoding.UTF8.GetBytes(key));
        }

        /// <summary>
        /// Decrypts the specified source string using AES decryption with the provided key.
        /// </summary>
        /// <param name="source">The string to be decrypted.</param>
        /// <param name="key"></param>
        /// <returns>The origin string.</returns>
        public static string Decrypt(string source, byte[] key)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }

            var bytes = Convert.FromBase64String(source);

            using var rm = Aes.Create();
            rm.Key = key;
            rm.Mode = CipherMode.ECB;
            rm.Padding = PaddingMode.PKCS7;

            var decryptor = rm.CreateDecryptor();
            var result = decryptor.TransformFinalBlock(bytes, 0, bytes.Length);

            return Encoding.UTF8.GetString(result);
        }
    }
}