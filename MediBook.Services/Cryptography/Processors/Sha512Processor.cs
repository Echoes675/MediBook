namespace MediBook.Services.Cryptography.Processors
{
    using System;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The Sha512 hashing processor
    /// </summary>
    public class Sha512Processor : IHashingProcessor
    {
        /// <summary>
        /// Gets the hash of the supplied string
        /// </summary>
        /// <param name="data"></param>
        /// <param name="hash"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public void CreateHash(string data, out byte[] hash, out byte[] salt)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            }
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
            // pass the passwordSalt to obtain the secure key
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                // password converted to a byte array so it can be hashed using the salt key above
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(plainTextPassword));

                // compare each element in the computedHash byte array
                if (computedHash.Where((t, i) => t != passwordHash[i]).Any())
                {
                    return false;
                }
            }

            // if all correct, return true
            return true;
        }
    }
}
