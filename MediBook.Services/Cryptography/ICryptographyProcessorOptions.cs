namespace MediBook.Services.Cryptography
{
    /// <summary>
    /// The options for the Cryptography service
    /// </summary>
    public interface ICryptographyProcessorOptions
    {
        /// <summary>
        /// The CryptographyKey from the configuration
        /// </summary>
        string CryptographyKey { get; }

        /// <summary>
        /// The EncryptionAlgorithm
        /// </summary>
        string EncryptionAlgorithm { get; }

        /// <summary>
        /// The HashingAlgorithm
        /// </summary>
        string HashingAlgorithm { get; }
    }
}