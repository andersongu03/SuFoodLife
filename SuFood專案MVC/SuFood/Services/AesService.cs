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

				using (var encryptor  = aes.CreateEncryptor())
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

				if(hash != null)
				{
					result = BitConverter.ToString(hash)?.Replace("-", string.Empty)?.ToUpper();
				}
			}

			return result;
		}
	}
}
