using System.Security.Cryptography;

namespace Nerosoft.Starfish.Toolkit;

/// <summary>
/// Provides cryptographic functionalities and utilities.
/// </summary>
internal partial class Cryptography
{
	/// <summary>
	/// Class for RSA encryption and decryption.
	/// </summary>
    public class RSA
	{
		private readonly System.Security.Cryptography.RSA _privateKeyRsaProvider;
		private readonly System.Security.Cryptography.RSA _publicKeyRsaProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="RSA"/> class.
		/// </summary>
		/// <param name="privateKey">私钥</param>
		/// <param name="publicKey">公钥</param>
		public RSA(string privateKey, string publicKey = null)
		{
			if (!string.IsNullOrEmpty(privateKey))
			{
				_privateKeyRsaProvider = CreateRsaProviderFromPrivateKey(privateKey);
			}

			if (!string.IsNullOrEmpty(publicKey))
			{
				_publicKeyRsaProvider = CreateRsaProviderFromPublicKey(publicKey);
			}
		}

		/// <summary>
		/// Encrypt the specified source string.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public string Encrypt(string source)
		{
			if (_publicKeyRsaProvider == null)
			{
				throw new Exception("_publicKeyRsaProvider is null");
			}

			return Convert.ToBase64String(_publicKeyRsaProvider.Encrypt(Encoding.UTF8.GetBytes(source), RSAEncryptionPadding.Pkcs1));
		}

		/// <summary>
		/// Decrypt the specified source string.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public string Decrypt(string source)
		{
			if (_privateKeyRsaProvider == null)
			{
				throw new Exception("_privateKeyRsaProvider is null");
			}

			return Encoding.UTF8.GetString(_privateKeyRsaProvider.Decrypt(Convert.FromBase64String(source), RSAEncryptionPadding.Pkcs1));
		}

		private static System.Security.Cryptography.RSA CreateRsaProviderFromPrivateKey(string key)
		{
			var privateKeyBits = Convert.FromBase64String(key);

			var rsa = System.Security.Cryptography.RSA.Create();
			var parameters = new RSAParameters();

			using (var reader = new BinaryReader(new MemoryStream(privateKeyBits)))
			{
				var twobytes = reader.ReadUInt16();
				switch (twobytes)
				{
					case 0x8130:
						reader.ReadByte();
						break;
					case 0x8230:
						reader.ReadInt16();
						break;
					default:
						throw new Exception("Unexpected value read reader.ReadUInt16()");
				}

				twobytes = reader.ReadUInt16();
				if (twobytes != 0x0102)
				{
					throw new Exception("Unexpected version");
				}

				var bt = reader.ReadByte();
				if (bt != 0x00)
				{
					throw new Exception("Unexpected value read reader.ReadByte()");
				}

				parameters.Modulus = reader.ReadBytes(GetIntegerSize(reader));
				parameters.Exponent = reader.ReadBytes(GetIntegerSize(reader));
				parameters.D = reader.ReadBytes(GetIntegerSize(reader));
				parameters.P = reader.ReadBytes(GetIntegerSize(reader));
				parameters.Q = reader.ReadBytes(GetIntegerSize(reader));
				parameters.DP = reader.ReadBytes(GetIntegerSize(reader));
				parameters.DQ = reader.ReadBytes(GetIntegerSize(reader));
				parameters.InverseQ = reader.ReadBytes(GetIntegerSize(reader));
			}

			rsa.ImportParameters(parameters);
			return rsa;
		}

		private static System.Security.Cryptography.RSA CreateRsaProviderFromPublicKey(string key)
		{
			byte[] seqOid = [0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00];

			var x509Key = Convert.FromBase64String(key);

			using var mem = new MemoryStream(x509Key);
			using var reader = new BinaryReader(mem);
			var twobytes = reader.ReadUInt16();
			switch (twobytes)
			{
				case 0x8130:
					reader.ReadByte();
					break;
				case 0x8230:
					reader.ReadInt16();
					break;
				default:
					return null;
			}

			var seq = reader.ReadBytes(15);
			if (!CompareBytes(seq, seqOid)) //make sure Sequence for OID is correct
			{
				return null;
			}

			twobytes = reader.ReadUInt16();
			switch (twobytes)
			{
				//data read as little endian order (actual data order for Bit String is 03 81)
				case 0x8103:
					reader.ReadByte(); //advance 1 byte
					break;
				case 0x8203:
					reader.ReadInt16(); //advance 2 bytes
					break;
				default:
					return null;
			}

			var bt = reader.ReadByte();
			if (bt != 0x00) //expect null byte next
				return null;

			twobytes = reader.ReadUInt16();
			switch (twobytes)
			{
				//data read as little endian order (actual data order for Sequence is 30 81)
				case 0x8130:
					reader.ReadByte(); //advance 1 byte
					break;
				case 0x8230:
					reader.ReadInt16(); //advance 2 bytes
					break;
				default:
					return null;
			}

			twobytes = reader.ReadUInt16();
			byte lowByte;
			byte highByte = 0x00;

			switch (twobytes)
			{
				//data read as little endian order (actual data order for Integer is 02 81)
				case 0x8102:
					lowByte = reader.ReadByte(); // read next bytes which is bytes in modulus
					break;
				case 0x8202:
					highByte = reader.ReadByte(); //advance 2 bytes
					lowByte = reader.ReadByte();
					break;
				default:
					return null;
			}

			byte[] value = { lowByte, highByte, 0x00, 0x00 }; //reverse byte order since asn.1 key uses big endian order
			var modulusSize = BitConverter.ToInt32(value, 0);

			int firstbyte = reader.PeekChar();
			if (firstbyte == 0x00)
			{
				//if first byte (highest order) of modulus is zero, don't include it
				reader.ReadByte(); //skip this null byte
				modulusSize -= 1; //reduce modulus buffer size by 1
			}

			var modulus = reader.ReadBytes(modulusSize);

			if (reader.ReadByte() != 0x02)
			{
				return null;
			}

			int exponentCount = reader.ReadByte();
			var exponent = reader.ReadBytes(exponentCount);

			// ------- create RSACryptoServiceProvider instance and initialize with public key -----
			var rsa = System.Security.Cryptography.RSA.Create();
			var rsaKeyInfo = new RSAParameters
			{
				Modulus = modulus,
				Exponent = exponent
			};
			rsa.ImportParameters(rsaKeyInfo);

			return rsa;
		}

		private static int GetIntegerSize(BinaryReader reader)
		{
			int count;
			var @byte = reader.ReadByte();
			if (@byte != 0x02)
			{
				return 0;
			}

			@byte = reader.ReadByte();

			switch (@byte)
			{
				case 0x81:
					count = reader.ReadByte();
					break;
				case 0x82:
				{
					var highByte = reader.ReadByte();
					var lowByte = reader.ReadByte();
					byte[] value = { lowByte, highByte, 0x00, 0x00 };
					count = BitConverter.ToInt32(value, 0);
					break;
				}
				default:
					count = @byte;
					break;
			}

			while (reader.ReadByte() == 0x00)
			{
				count -= 1;
			}

			reader.BaseStream.Seek(-1, SeekOrigin.Current);
			return count;
		}

		private static bool CompareBytes(byte[] source, byte[] target)
		{
			if (source.Length != target.Length)
			{
				return false;
			}

			var i = 0;
			foreach (var @byte in source)
			{
				if (@byte != target[i])
				{
					return false;
				}

				i++;
			}

			return true;
		}
	}
	
	
}