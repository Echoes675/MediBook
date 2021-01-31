namespace MediBook.Services.Cryptography
{
    using System.Collections.Generic;

    /// <summary>
    /// The CryptographyService interface
    /// </summary>
    public interface ICryptographyService
    {
        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string Encrypt(string data);

        /// <summary>
        /// Encrypt multiple strings at a time
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        List<string> Encrypt(List<string> data);

        /// <summary>
        /// Decrypts a string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string Decrypt(string data);

        /// <summary>
        /// Decrypt multiple strings at a time
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        List<string> Decrypt(List<string> data);

        /// <summary>
        /// Generates new hash and salt from plain text string
        /// </summary>
        /// <param name="data"></param>
        /// <param name="hash"></param>
        /// <param name="salt"></param>
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