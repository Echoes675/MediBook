namespace MediBook.Services.UserAuthentication
{
    using System;
    using System.Globalization;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using MediBook.Core.Enums;
    using MediBook.Core.Models;
    using MediBook.Data.Repositories;
    using MediBook.Services.Cryptography;
    using MediBook.Services.Enums;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.Extensions.Logging;

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

            // user found, check the password is correct and build the ClaimsPrincipal, if not, return null
            if (!_cryptographyService.VerifyPasswordHash(user.PasswordHash, user.PasswordSalt, password))
            {
                message = $"Account login failed. Username or password not recognised. \"Username\"={username}";
                _log.LogInformation(message);
                return new UserLoginResult(ServiceResultStatusCode.Failed, message, null);
            }
            
            var claimsPrincipal = BuildClaimsPrincipal(user);

            if (claimsPrincipal != null)
            {
                message = "Account login Success.";
                _log.LogInformation(message);
                return new UserLoginResult(ServiceResultStatusCode.Success, message, claimsPrincipal);
            }
            
            _log.LogError($"Failed to build ClaimsPrincipal as malformed User received from Db. \"Username\"={username}");
            message = $"Account login failed. Internal error. \"Username\"={username}";
            return new UserLoginResult(ServiceResultStatusCode.Failed, message, null);
        }

        // Build a claims principal from authenticated user
        private ClaimsPrincipal BuildClaimsPrincipal(User user)
        {

            if (string.IsNullOrEmpty(user.Username) || user.Id < 1 || 
                user.JobDescription == null || user.JobDescription.Role == UserRole.Unknown)
            {
                return null;
            }

            // define user claims including a custom claim for user Id
            // this would be useful if any future queries/actions required
            // user Id to be submitted with requests
            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("Id", user.Id.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.Role, user.JobDescription.Role.ToString())
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            // build principal using claims
            return new ClaimsPrincipal(claims);
        }
    }
}