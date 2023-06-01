using Newtonsoft.Json;
using System.Text;
namespace SuFood.Services
{
	public class AesService
	{
		//字串加密AES
		public string EncryptAES(byte[] source, string cryptoKey, string cryptoIV)
		{
			byte[] dataKey = Encoding.UTF8.GetBytes(cryptoKey);
			byte[] dataIV = Encoding.UTF8.GetBytes(cryptoIV);

			using (var aes = System.Security.Cryptography.Aes.Create())
			{
				aes.Mode = System.Security.Cryptography.CipherMode.CBC;
				aes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
				aes.Key = dataKey;
				aes.IV = dataIV;

				using (var encryptor = aes.CreateEncryptor())
				{
					byte[] encryptResult = encryptor.TransformFinalBlock(source, 0, source.Length);
					return BitConverter.ToString(encryptResult)?.Replace("-", "")?.ToLower();
				}
			}
		}

		//字串加密256
		public string EncryptSHA256(string source)
		{
			string result = string.Empty;

			using (System.Security.Cryptography.SHA256 algorithm = System.Security.Cryptography.SHA256.Create())
			{
				var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(source));

				if (hash != null)
				{
					result = BitConverter.ToString(hash)?.Replace("-", string.Empty)?.ToUpper();
				}
			}

			return result;
		}

		public byte[] RemovePKCS7Padding(byte[] data)
		{
			int indexLength = data[data.Length - 1];
			var outputData = new byte[data.Length - indexLength];
			Buffer.BlockCopy(data, 0, outputData, 0, outputData.Length);
			return outputData;
		}


		//AES解密
		public string DecryptAESHex(string source, string cryptoKey, string cryptoIV)
		{
			string result = string.Empty;

			if (!string.IsNullOrEmpty(source))
			{
				byte[] sourceBytes = ToByteArray(source);

				if (sourceBytes != null)
				{
					result = Encoding.UTF8.GetString(DecryptAES(sourceBytes, cryptoKey, cryptoIV)).Trim();
				}
			}
			return result;
		}

		public byte[] ToByteArray(string source)
		{
			byte[] result = null;

			if (!string.IsNullOrWhiteSpace(source))
			{
				var outputLength = source.Length / 2;
				var output = new byte[outputLength];

				for (var i = 0; i < outputLength; i++)
				{
					output[i] = Convert.ToByte(source.Substring(i * 2, 2), 16);
				}
				result = output;
			}
			return result;
		}

		public static byte[] DecryptAES(byte[] source, string cryptoKey, string cryptoIV)
		{
			byte[] dataKey = Encoding.UTF8.GetBytes(cryptoKey);
			byte[] dataIV = Encoding.UTF8.GetBytes(cryptoIV);

			using (var aes = System.Security.Cryptography.Aes.Create())
			{
				aes.Mode = System.Security.Cryptography.CipherMode.CBC;
				aes.Padding = System.Security.Cryptography.PaddingMode.None;
				aes.Key = dataKey;
				aes.IV = dataIV;

				using (var decryptor = aes.CreateDecryptor())
				{
					byte[] data = decryptor.TransformFinalBlock(source, 0, source.Length);
					int iLength = data[data.Length - 1];
					var output = new byte[data.Length - iLength];
					Buffer.BlockCopy(data, 0, output, 0, output.Length);
					return output;
				}
			}
		}
	}
}
