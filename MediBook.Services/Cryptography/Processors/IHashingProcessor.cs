namespace MediBook.Services.Cryptography.Processors
{
    public interface IHashingProcessor
    {
        /// <summary>
        /// Gets the hash of the supplied string
        /// </summary>
        /// <param name="data"></param>
        /// <param name="hash"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        void CreateHash(string data, out byte[] hash, out byte[] salt);

        /// <summary>
        /// Verifies whether a plain text password matches the hashed password
        /// </summary>
        /// <param name="passwordSalt"></param>
        /// <param name="plainTextPassword"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        bool VerifyPasswordHash(byte[] passwordHash, byte[] passwordSalt, string plainTextPassword);
    }
}