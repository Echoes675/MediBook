namespace MediBook.Web.Controllers
{
    using MediBook.Core.DTOs;
    using MediBook.Web.Enums;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using MediBook.Services.UserAuthentication;
    using Microsoft.Extensions.Logging;

    public class AuthController : ControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// The User Auth service
        /// </summary>
        private readonly IUserAuthenticationService _svc;

        /// <summary>
        /// Initializes an instance of the <see cref="AuthController"/>
        /// </summary>
        /// <param name="svc"></param>
        public AuthController(ILogger<AuthController> logger, IUserAuthenticationService svc)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));
        }

        [AllowAnonymous]
        //GET /auth/
        [HttpGet]
        public IActionResult Index()
        {
            // if anyone attempts to browse to /auth/ directly, redirect to home page
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        //GET /auth/Login
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View(new UserForAuthenticationDto());
        }

        [AllowAnonymous]
        // POST /auth/login
        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] UserForAuthenticationDto userForLogin)
        {
            if (userForLogin == null)
            {
                throw new ArgumentNullException(nameof(UserForAuthenticationDto));
            }

            // check user exists in the db and that the details provided are valid for login purposes
            var claimsPrincipal = await _svc.Login(userForLogin.Username.Trim(), userForLogin.Password).ConfigureAwait(false);

            // if user does not exist return Unauthorized
            if (claimsPrincipal == null)
            {
                // if username or password not recognised, produce an alert and reload the view
                var logMessage = $"Username and password combination not recognised. \"Username\"={userForLogin.Username}";
                _logger.LogDebug(logMessage);

                Alert(logMessage, AlertType.Warning);
                return View();
            }

            // sign user in using cookie authentication to store principal
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal).ConfigureAwait(false);
            
            // record the login event in system logs and return to the Index page
            _logger.LogInformation($"User logged in. \"Username\"={userForLogin.Username}");

            //await _repo.UpdateUserLoginTimeAsync(claimsPrincipal).ConfigureAwait(false);
            return RedirectToAction("Index", "Home");
        }

        //GET auth/logout
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            // delete the login cookie and redirect to login page
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
            return RedirectToAction(nameof(Login), "Auth");
        }


    }
}
