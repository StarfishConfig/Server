using System.Security.Cryptography;

namespace Nerosoft.Starfish.Toolkit;

internal partial class Cryptography
{
    /// <summary>
    /// The DES encryption and decryption class.
    /// </summary>
    public class DES
    {
        /// <summary>
        /// The default salt value used for encryption and decryption.
        /// </summary>
        private static readonly byte[] _defaultSalt = [0x03, 0x0B, 0x13, 0x1B, 0x23, 0x2B, 0x33, 0x3B, 0x43, 0x4B, 0x9B, 0x93, 0x8B, 0x83, 0x7B, 0x73, 0x6B, 0x63, 0x5B, 0x53, 0xF3, 0xFB, 0xA3, 0xAB, 0xB3, 0xBB, 0xC3, 0xEB, 0xE3, 0xDB, 0xD3, 0xCB];

        #region Encryption

        /// <summary>
        /// Encrypts the specified source string using DES encryption.
        /// </summary>
        /// <param name="source">The source to be encrypted.</param>
        /// <returns>The result.</returns>
        public static string Encrypt(string source)
        {
            return Encrypt(source, _defaultSalt);
        }

        /// <summary>
        /// Encrypts the specified source string using DES encryption with the provided key.
        /// </summary>
        /// <param name="source">The source to be encrypted.</param>
        /// <param name="key">The key.</param>
        /// <returns>The result.</returns>
        public static string Encrypt(string source, byte[] key)
        {
            using SymmetricAlgorithm sa = Aes.Create(); //Rijndael.Create();
            sa.Key = key;
            sa.Mode = CipherMode.ECB;
            sa.Padding = PaddingMode.PKCS7;
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, sa.CreateEncryptor(), CryptoStreamMode.Write);
            var byt = Encoding.Unicode.GetBytes(source);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// Encrypt the specified source string with the given key.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string source, string key)
        {
            using var provider = System.Security.Cryptography.DES.Create();
            
            {
                provider.Mode = CipherMode.ECB;
                provider.Padding = PaddingMode.PKCS7;
            }

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var sourceBytes = Encoding.UTF8.GetBytes(source);
            using var memory = new MemoryStream();
            using var crypto = new CryptoStream(memory, provider.CreateEncryptor(keyBytes, keyBytes), CryptoStreamMode.Write);
            crypto.Write(sourceBytes, 0, sourceBytes.Length);
            crypto.FlushFinalBlock();
            return Convert.ToBase64String(memory.ToArray());
        }

        #endregion

        #region Decryption

        /// <summary>
        /// Decrypts the specified source string using DES decryption.
        /// </summary>
        /// <param name="source">The string to be decrypted.</param>
        /// <returns>The origin string.</returns>
        public static string Decrypt(string source)
        {
            return Decrypt(source, _defaultSalt);
        }

        /// <summary>
        /// Decrypts the specified source string using DES decryption with the provided key.
        /// </summary>
        /// <param name="source">The string to be decrypted.</param>
        /// <param name="key">The 32bits key.</param>
        /// <returns>The origin string.</returns>
        public static string Decrypt(string source, byte[] key)
        {
            using SymmetricAlgorithm sa = Aes.Create(); //Rijndael.Create();
            sa.Key = key;
            sa.Mode = CipherMode.ECB;
            sa.Padding = PaddingMode.PKCS7;
            var ct = sa.CreateDecryptor();
            var byt = Convert.FromBase64String(source);
            using var ms = new MemoryStream(byt);
            using var cs = new CryptoStream(ms, ct, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs, Encoding.Unicode);
            return sr.ReadToEnd();
        }

        #endregion
    }
}