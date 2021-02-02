namespace MediBook.Services.Cryptography.Processors
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The AES encryption processor
    /// </summary>
    public class AesProcessor : IEncryptionProcessor
    {
        /// <summary>
        /// The Cryptography key
        /// </summary>
        private readonly string _cryptographyKey;

        /// <summary>
        /// Initializes an instance of the <see cref="TripleDesProcessor"/> class
        /// </summary>
        /// <param name="cryptographyKey"></param>
        public AesProcessor(string cryptographyKey)
        {
            if (string.IsNullOrEmpty(cryptographyKey))
            {
                throw new ArgumentNullException(nameof(cryptographyKey));
            }

            _cryptographyKey = cryptographyKey;
        }

        /// <summary>
        /// Encrypts a string using AES encryption
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Encrypt(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_cryptographyKey);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(data);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            //return Convert.ToBase64String(array);
            return array;
        }

        /// <summary>
        /// Decrypts a string using AES encryption
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Decrypt(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            byte[] iv = new byte[16];
           // byte[] buffer = Convert.FromBase64String(data);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_cryptographyKey);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

               // using (MemoryStream memoryStream = new MemoryStream(buffer))
                using (MemoryStream memoryStream = new MemoryStream(data))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}