namespace MediBook.Services.Cryptography
{
    using System;
    using MediBook.Services.Cryptography.Processors;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The CryptographyProcessor factory
    /// </summary>
    public class CryptographyProcessorFactory : ICryptographyProcessorFactory
    {
        private readonly string _cryptographyKey;
        private readonly HashingAlgorithm _hashAlgorithm;
        private readonly EncryptionAlgorithm _encryptionAmAlgorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyProcessorFactory"/> class
        /// </summary>
        public CryptographyProcessorFactory(IConfiguration config) : this(new DefaultCryptographyProcessorOptions(config))
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyProcessorFactory"/> class
        /// </summary>
        public CryptographyProcessorFactory(ICryptographyProcessorOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrEmpty(options.HashingAlgorithm))
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrEmpty(options.CryptographyKey))
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrEmpty(options.EncryptionAlgorithm))
            {
                throw new ArgumentNullException(nameof(options));
            }

            _cryptographyKey = options.CryptographyKey.Trim();

            _hashAlgorithm = 
                Enum.TryParse<HashingAlgorithm>(options.HashingAlgorithm.Trim(), true, out var parseHashResult) ? 
                    parseHashResult : 
                    throw new NotSupportedException($"Unsupported Hashing algorithm found in options. \"HashAlgorithm\"={options.HashingAlgorithm}");

            _encryptionAmAlgorithm =
                Enum.TryParse<EncryptionAlgorithm>(options.EncryptionAlgorithm.Trim(), true, out var parseEncryptAlgorithmResult) ?
                    parseEncryptAlgorithmResult :
                    throw new NotSupportedException($"Unsupported Encryption algorithm found in options. \"EncryptionAlgorithm\"={options.EncryptionAlgorithm}");

        }

        /// <summary>
        /// Returns an instance of the EncryptionProcessor specified in the options
        /// </summary>
        /// <returns></returns>
        public IEncryptionProcessor GetEncryptionProcessor()
        {
            switch (_encryptionAmAlgorithm)
            {
                case EncryptionAlgorithm.TripleDes:
                    return new TripleDesProcessor(_cryptographyKey);
                    
                case EncryptionAlgorithm.AES:
                    return new AesProcessor(_cryptographyKey);

                default:
                    throw new ArgumentOutOfRangeException(nameof(_encryptionAmAlgorithm));
            }
        }

        /// <summary>
        /// Returns an instance of the HashingProcessor specified in the options
        /// </summary>
        /// <returns></returns>
        public IHashingProcessor GetHashingProcessor()
        {
            switch (_hashAlgorithm)
            {
                case HashingAlgorithm.SHA256:
                    return new Sha256Processor();

                case HashingAlgorithm.SHA512:
                    return new Sha512Processor();

                default:
                    throw new ArgumentOutOfRangeException(nameof(_hashAlgorithm));
            }
        }
    }
}