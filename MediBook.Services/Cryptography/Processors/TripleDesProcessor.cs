namespace MediBook.Services.Cryptography.Processors
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The Triple Des processor
    /// </summary>
    public class TripleDesProcessor : IEncryptionProcessor
    {
        /// <summary>
        /// The Cryptography key
        /// </summary>
        private readonly string _cryptographyKey;

        /// <summary>
        /// Initializes an instance of the <see cref="TripleDesProcessor"/> class
        /// </summary>
        /// <param name="cryptographyKey"></param>
        public TripleDesProcessor(string cryptographyKey)
        {
            _cryptographyKey = cryptographyKey ?? throw new ArgumentNullException(nameof(cryptographyKey));
        }

        /// <summary>
        /// Encrypts a string using Triple Des encryption
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Encrypt(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            var encryptedArray = Encoding.UTF8.GetBytes(data);

            using (var md5CryptoService = new MD5CryptoServiceProvider())
            {
                var keyArray = md5CryptoService.ComputeHash(Encoding.UTF8.GetBytes(_cryptographyKey));
                md5CryptoService.Clear();

                using (var tripleDesCryptoService = new TripleDESCryptoServiceProvider())
                {
                    tripleDesCryptoService.Key = keyArray;
                    tripleDesCryptoService.Mode = CipherMode.ECB;
                    tripleDesCryptoService.Padding = PaddingMode.PKCS7;
                    var cryptoTransform = tripleDesCryptoService.CreateEncryptor();

                    var resultArray = cryptoTransform.TransformFinalBlock(encryptedArray, 0, encryptedArray.Length);

                    tripleDesCryptoService.Clear();

                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
            }
        }

        /// <summary>
        /// Decrypts a string using Triple Des encryption
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Decrypt(string data)
        {
            var decryptArray = Convert.FromBase64String(data);

            using (var mD5CryptoService = new MD5CryptoServiceProvider())
            {
                var securityKeyArray = mD5CryptoService.ComputeHash(Encoding.UTF8.GetBytes(_cryptographyKey));
                mD5CryptoService.Clear();

                using (var tripleDesCryptoService = new TripleDESCryptoServiceProvider())
                {
                    tripleDesCryptoService.Key = securityKeyArray;
                    tripleDesCryptoService.Mode = CipherMode.ECB;
                    tripleDesCryptoService.Padding = PaddingMode.PKCS7;

                    var cryptoTransform = tripleDesCryptoService.CreateDecryptor();
                    var resultArray = cryptoTransform.TransformFinalBlock(decryptArray, 0, decryptArray.Length);
                    tripleDesCryptoService.Clear();

                    return Encoding.UTF8.GetString(resultArray);
                } 
            }
        }
    }
}