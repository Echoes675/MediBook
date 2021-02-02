namespace MediBook.Services.Cryptography
{
    using System;
    using Microsoft.Extensions.Configuration;

    public class DefaultCryptographyProcessorOptions : ICryptographyProcessorOptions
    {
        public DefaultCryptographyProcessorOptions(IConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            CryptographyKey = config.GetSection("DefaultCryptographyOptions:CryptographyKey").Value;
            EncryptionAlgorithm = config.GetSection("DefaultCryptographyOptions:EncryptionAlgorithm").Value;
            HashingAlgorithm = config.GetSection("DefaultCryptographyOptions:HashingAlgorithm").Value;
        }

        /// <summary>
        /// The CryptographyKey from the configuration
        /// </summary>
        public string CryptographyKey { get; }

        /// <summary>
        /// The EncryptionAlgorithm
        /// </summary>
        public string EncryptionAlgorithm { get; }

        /// <summary>
        /// The HashingAlgorithm
        /// </summary>
        public string HashingAlgorithm { get; }
    }
}