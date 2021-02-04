namespace MediBook.Services.UserAuthentication
{
    using MediBook.Core.DTOs;
    using MediBook.Core.Enums;
    using MediBook.Data.Repositories;
    using MediBook.Services.Cryptography;
    using MediBook.Services.Enums;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public class UserAuthenticationService : IUserAuthenticationService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private ILogger<UserAuthenticationService> _log;

        /// <summary>
        /// The Cryptography Service
        /// </summary>
        private readonly ICryptographyService _cryptographyService;

        /// <summary>
        /// The database context
        /// </summary>
        private readonly IUserDal _userDal;

        /// <summary>
        /// Initializes an instance of the <see cref="UserAuthenticationService"/> class.
        /// </summary>
        /// <param name="userDal"></param>
        /// <param name="logger"></param>
        /// <param name="cryptographyService"></param>
        public UserAuthenticationService(IUserDal userDal, ILogger<UserAuthenticationService> logger, ICryptographyService cryptographyService)
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
        public async Task<UserLoginResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            // attempt to retrieve a matching user from the db
            var user = await _userDal.GetUserAsync(username);

            string message;
            // if not found return null
            if (user == null)
            {
                message = $"User not found. \"Username\"={username}";
                _log.LogInformation(message);
                return new UserLoginResult(ServiceResultStatusCode.NotFound, message, null);
            }

            if (user.State != AccountState.Active)
            {
                message = $"Account login failed. Account state is not active \"Username\"={username}, \"AccountState\"={user.State}";
                return new UserLoginResult(ServiceResultStatusCode.Failed, message, null);
            }

            if (user.JobDescription.Role == UserRole.Unknown)
            {
                message = $"Account login failed. Account role is unknown \"Username\"={username}, \"AccountRole\"={user.JobDescription.Role}";
                return new UserLoginResult(ServiceResultStatusCode.Failed, message, null);
            }

            // user found, check the password is correct and build the ClaimsPrincipal, if not, return null
            if (!_cryptographyService.VerifyPasswordHash(user.PasswordHash, user.PasswordSalt, password))
            {
                message = $"Account login failed. Username or password not recognised. \"Username\"={username}";
                _log.LogInformation(message);
                return new UserLoginResult(ServiceResultStatusCode.Failed, message, null);
            }

            var userAccountDetails = new UserAccountDetailsDto(user);
            message = "Account login Success.";
            _log.LogInformation(message);
            return new UserLoginResult(ServiceResultStatusCode.Success, message, userAccountDetails);
        }
    }
}