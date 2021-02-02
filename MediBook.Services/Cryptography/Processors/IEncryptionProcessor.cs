namespace MediBook.Services.Cryptography.Processors
{
    public interface IEncryptionProcessor
    {
        /// <summary>
        /// Encrypts a string using the default configuration
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Encrypt(string data);

        /// <summary>
        /// Decrypts a string using the default configuration
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Decrypt(string data);
    }
}