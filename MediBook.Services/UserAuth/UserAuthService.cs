namespace MediBook.Services.UserAuth
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using MediBook.Core.Models;
    using MediBook.Data.Repositories;
    using Microsoft.Extensions.Logging;

    public class UserAuthService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private ILogger _log;

        /// <summary>
        /// The database context
        /// </summary>
        private readonly IUserDal _userDal;

        /// <summary>
        /// Initializes an instance of the <see cref="UserAuthService"/> class.
        /// </summary>
        /// <param name="userDal"></param>
        /// <param name="logger"></param>
        public UserAuthService(IUserDal userDal, ILogger logger)
        {
            _userDal = userDal ?? throw new ArgumentNullException(nameof(userDal));
            _log = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Method to process login functionality
        /// </summary>
        /// <param name = "username" > String of the username</param>
        /// <param name = "password" > String of the submitted password</param>
        /// <returns>User object if authentication successful</returns>
        public async Task<User> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }
            // check that the user exists in the db
            var user = await _userDal.GetUserAsync(username);

            // if not found return null
            if (user == null)
            {
                return null;
            }

            // user found, check the password is correct; if not, return null.
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            // if password is also correct, return the user object
            return user;
        }

        /// <summary>
        /// Method to take password string, hash and salt and check whether the password string, when hashed, matches the 
        /// password has stored against the user account
        /// </summary>
        /// <param name="password">String of entered password</param>
        /// <param name="passwordHash">Byte array of hashed password stored in database</param>
        /// <param name="passwordSalt">Byte array of password salt to be used to hash the password string</param>
        /// <returns></returns>
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // the using keyword means anything inside the brackets will be disposed of when complete
            // pass the passwordSalt from the db to obtain the secure key
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                // password converted to a byte array so it can be hashed using the salt key above
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

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