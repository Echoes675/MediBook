namespace MediBook.Services.Cryptography
{
    using MediBook.Services.Cryptography.Processors;

    /// <summary>
    /// The CryptographyProcessor factory
    /// </summary>
    public interface ICryptographyProcessorFactory
    {
        /// <summary>
        /// Returns an instance of the EncryptionProcessor specified in the options
        /// </summary>
        /// <returns></returns>
        IEncryptionProcessor GetEncryptionProcessor();

        /// <summary>
        /// Returns an instance of the HashingProcessor specified in the options
        /// </summary>
        IHashingProcessor GetHashingProcessor();
    }
}