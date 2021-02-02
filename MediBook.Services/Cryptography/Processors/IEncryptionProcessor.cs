namespace MediBook.Services.Cryptography.Processors
{
    public interface IEncryptionProcessor
    {
        /// <summary>
        /// Encrypts a string using the default configuration
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Encrypt(string data);

        /// <summary>
        /// Decrypts a string using the default configuration
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Decrypt(byte[] data);
    }
}