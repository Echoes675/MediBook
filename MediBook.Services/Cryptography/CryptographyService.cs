namespace MediBook.Services.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MediBook.Services.Cryptography.Processors;

    /// <summary>
    /// The Cryptography Service
    /// </summary>
    public class CryptographyService : ICryptographyService
    {
        /// <summary>
        /// The EncryptionProcessor
        /// </summary>
        private readonly IEncryptionProcessor _encryptionProcessor;

        /// <summary>
        /// The HashingProcessor
        /// </summary>
        private readonly IHashingProcessor _hashingProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyService"/> class
        /// </summary>
        public CryptographyService(ICryptographyProcessorFactory processorFactory)
        {
            if (processorFactory == null)
            {
                throw new ArgumentNullException(nameof(processorFactory));
            }

            _encryptionProcessor = processorFactory.GetEncryptionProcessor();
            _hashingProcessor = processorFactory.GetHashingProcessor();
        }

        /// <summary>
        /// Encrypts a string using the default configuration
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Encrypt(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            return _encryptionProcessor.Encrypt(data);
        }

        /// <summary>
        /// Encrypt multiple strings at a time
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<string> Encrypt(List<string> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return data.Select(item => _encryptionProcessor.Encrypt(item)).ToList();
        }

        /// <summary>
        /// Decrypt multiple strings at a time
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Decrypt(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            return _encryptionProcessor.Decrypt(data);
        }

        /// <summary>
        /// Decrypt multiple strings at a time
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<string> Decrypt(List<string> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return data.Select(item => _encryptionProcessor.Decrypt(item)).ToList();
        }

        /// <summary>
        /// Generates new hash and salt from plain text string
        /// </summary>
        /// <param name="data"></param>
        /// <param name="hash"></param>
        /// <param name="salt"></param>
        public void CreateHash(string data, out byte[] hash, out byte[] salt)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            _hashingProcessor.CreateHash(data, out hash, out salt);
        }

        /// <summary>
        /// Verifies whether a plain text password matches the hashed password
        /// </summary>
        /// <param name="passwordSalt"></param>
        /// <param name="plainTextPassword"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public bool VerifyPasswordHash(byte[] passwordHash, byte[] passwordSalt, string plainTextPassword)
        {
            if (passwordHash == null)
            {
                throw new ArgumentNullException(nameof(passwordHash));
            }

            if (passwordSalt == null)
            {
                throw new ArgumentNullException(nameof(passwordSalt));
            }

            if (string.IsNullOrEmpty(plainTextPassword))
            {
                throw new ArgumentNullException(nameof(plainTextPassword));
            }

            return _hashingProcessor.VerifyPasswordHash(passwordHash, passwordSalt, plainTextPassword);
        }
    }
}