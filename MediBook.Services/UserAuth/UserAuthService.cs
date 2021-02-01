namespace MediBook.Services.UserAuth
{
    using System;
    using System.Threading.Tasks;
    using MediBook.Core.Models;
    using MediBook.Data.Repositories;
    using MediBook.Services.Cryptography;
    using Microsoft.Extensions.Logging;

    public class UserAuthService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private ILogger _log;

        /// <summary>
        /// The Cryptography Service
        /// </summary>
        private readonly ICryptographyService _cryptographyService;

        /// <summary>
        /// The database context
        /// </summary>
        private readonly IUserDal _userDal;

        /// <summary>
        /// Initializes an instance of the <see cref="UserAuthService"/> class.
        /// </summary>
        /// <param name="userDal"></param>
        /// <param name="logger"></param>
        /// <param name="cryptographyService"></param>
        public UserAuthService(IUserDal userDal, ILogger logger, ICryptographyService cryptographyService)
        {
            _userDal = userDal ?? throw new ArgumentNullException(nameof(userDal));
            _log = logger ?? throw new ArgumentNullException(nameof(logger));
            _cryptographyService = cryptographyService ?? throw new ArgumentNullException(nameof(cryptographyService));
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

            // user found, check the password is correct, if not, return null
            return _cryptographyService.VerifyPasswordHash(user.PasswordSalt, user.PasswordHash, password) ? 
                user : null;
        }
    }
}