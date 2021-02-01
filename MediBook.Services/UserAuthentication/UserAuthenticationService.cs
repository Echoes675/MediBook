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
        public async Task<ClaimsPrincipal> Login(string username, string password)
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

            // if not found return null
            if (user == null)
            {
                _log.LogInformation($"User not found. \"Username\"={username}");
                return null;
            }

            // user found, check the password is correct and build the ClaimsPrincipal, if not, return null
            if (!_cryptographyService.VerifyPasswordHash(user.PasswordSalt, user.PasswordHash, password))
            {
                return null;
            }
            
            var claimsPrincipal = BuildClaimsPrincipal(user);

            if (claimsPrincipal != null)
            {
                return claimsPrincipal;
            }
            
            _log.LogError($"Failed to build ClaimsPrincipal as malformed User received from Db. \"Username\"={username}");
            return null;
        }

        // Build a claims principal from authenticated user
        private ClaimsPrincipal BuildClaimsPrincipal(User user)
        {

            if (string.IsNullOrEmpty(user.Username) || user.Id < 1 || user.Role == UserRole.Unknown)
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
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            // build principal using claims
            return new ClaimsPrincipal(claims);
        }
    }
}