namespace MediBook.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
    using MediBook.Services.Enums;
    using MediBook.Services.UserAdministration;
    using MediBook.Web.Enums;
    using MediBook.Web.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The User Administration Controller
    /// </summary>
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserAdminController : ControllerBase
    {
        /// <summary>
        /// The Logger
        /// </summary>
        private readonly ILogger<UserAdminController> _logger;

        /// <summary>
        /// The User Administration service
        /// </summary>
        private readonly IUserAdministrationService _userAdminSvc;

        /// <summary>
        /// Initialize an instance of the <see cref="UserAdminController"/> class
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userAdminSvc"></param>
        public UserAdminController(ILogger<UserAdminController> logger, IUserAdministrationService userAdminSvc)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userAdminSvc = userAdminSvc ?? throw new ArgumentNullException(nameof(userAdminSvc));
        }

        [HttpGet]
        //[Authorize(Roles = "PracticeAdmin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userAdminSvc.LoadUserAccounts().ConfigureAwait(false);
            if (users == null)
            {
                var message = $"Failed to load users from the database";
                _logger.LogError(message);
                Alert(message, AlertType.danger);
                return RedirectToPage(nameof(Index), nameof(HomeController));
            }

            var usersViewModel = new UsersViewModel(users);

            return View(usersViewModel);
        }

        // GET auth/CreateUser
        //[Authorize(Roles = "PracticeAdmin")]
        [HttpGet("CreateUser")]
        public IActionResult CreateUser()
        {
            return View();
        }

        // POST auth/EditUser
        //[Authorize(Roles = "PracticeAdmin")]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromForm] UserForRegistrationDto userRegistrationDetails)
        {
            if (userRegistrationDetails == null)
            {
                throw new ArgumentNullException(nameof(userRegistrationDetails));
            }

            var result = await _userAdminSvc.CreateUserAsync(userRegistrationDetails).ConfigureAwait(false);

            string message;
            if (result != null)
            {
                message = $"User successfully created. \"Username\"={userRegistrationDetails.Username}, \"UserId\"={result.UserId}";
                _logger.LogInformation(message);
                Alert(message, AlertType.success);
                return RedirectToAction("Index");
            }

            message =
                $"An error occurred. Could not create user. \"Username\"={userRegistrationDetails.Username}";
            _logger.LogError(message);
            Alert(message, AlertType.danger);
            return RedirectToAction("Index");
        }

        // GET auth/EditUser
        //[Authorize(Roles = "PracticeAdmin")]
        [HttpGet("EditUser")]
        public async Task<IActionResult> EditUser(int id)
        {
            // load the user from the database
            var userFromDb = await _userAdminSvc.GetUserFullDetailsAsync(id).ConfigureAwait(false);
            var userDetailsViewModel = new UserDetailsViewModel(userFromDb);

            // if the requested user is not found, load the EditUser view
            if (userFromDb != null)
            {
                return View(userDetailsViewModel);
            }

            // if the requested user is not found, return an error
            var message = $"Cannot find user for editing. \"Id\"={id}";
            _logger.LogError(message);
            Alert(message, AlertType.danger);
            return RedirectToAction("Index", "UserAdmin");
        }

        // POST auth/EditUser
        //[Authorize(Roles = "PracticeAdmin")]
        [HttpPost("EditUser")]
        public async Task<IActionResult> EditUser([FromForm] UserDetailsViewModel userDetailsView)
        {
            if (userDetailsView == null)
            {
                throw new ArgumentNullException(nameof(userDetailsView));
            }

            var userDetailsDto = userDetailsView.MapToUserFullDetailsDtoDto();
            var result = await _userAdminSvc.UpdateUserDetailsAsync(userDetailsDto).ConfigureAwait(false);

            string message;
            switch (result)
            {
                case ServiceResultStatusCode.Success:
                    message = $"User details successfully updated. \"Username\"={userDetailsDto.Username}, \"UserId\"={userDetailsDto.UserId}, \"ServiceResultStatusCode\"={result}";
                    _logger.LogInformation(message);
                    Alert(message, AlertType.success);
                    return RedirectToAction("Index");

                default:
                    message = $"An error occurred. Could not update user details. \"Username\"={userDetailsDto.Username}, \"UserId\"={userDetailsDto.UserId}, \"ServiceResultStatusCode\"={result}";
                    _logger.LogInformation(message);
                    Alert(message, AlertType.danger);
                    return RedirectToAction("Index");
            }
        }

        // GET auth/ResetPassword
        //[Authorize(Roles = "PracticeAdmin")]
        [HttpGet("ResetPassword")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            // load the user from the database
            var userFromDb = await _userAdminSvc.GetUserFullDetailsAsync(id).ConfigureAwait(false);

            // if the requested user is not found, load the EditUser view
            if (userFromDb != null)
            {
                return View(new UserPasswordResetDto()
                {
                    Id = id,
                    Username = userFromDb.Username
                });
            }

            // if the requested user is not found, return an error
            var message = $"Cannot find user load for editing. \"Id\"={id}";
            _logger.LogError(message);
            Alert(message, AlertType.danger);
            return RedirectToAction("Index", "UserAdmin");
        }

        // POST auth/ResetPassword
        //[Authorize(Roles = "PracticeAdmin")]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] UserPasswordResetDto passwordResetDetails)
        {
            if (passwordResetDetails == null)
            {
                throw new ArgumentNullException(nameof(passwordResetDetails));
            }

            var result = await _userAdminSvc.ResetPasswordAsync(passwordResetDetails);

            string message;
            switch (result)
            {
                case ServiceResultStatusCode.Success:
                    message = $"User password successfully updated. \"Username\"={passwordResetDetails.Username}, \"UserId\"={passwordResetDetails.Id}";
                    _logger.LogInformation(message + $", \"ServiceResultStatusCode\"={result}");
                    Alert(message, AlertType.success);
                    return RedirectToAction("Index");

                default:
                    message = $"An error occurred. Could not update user password. \"Username\"={passwordResetDetails.Username}, \"UserId\"={passwordResetDetails.Id}";
                    _logger.LogInformation(message + $", \"ServiceResultStatusCode\"={result}");
                    Alert(message, AlertType.danger);
                    return RedirectToAction("Index");
            }
        }

        // POST userauth/delete
        //[Authorize(Roles = "Admin")]
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var result = await _userAdminSvc.DeleteUserAsync(id);
            string message;
            switch (result)
            {
                case ServiceResultStatusCode.Success:
                    message = $"User successfully deleted. \"UserId\"={id}";
                    _logger.LogInformation(message + $", \"ServiceResultStatusCode\"={result}");
                    Alert(message, AlertType.success);
                    return RedirectToAction("Index");

                default:
                    message = $"An error occurred. Could not delete user. \"UserId\"={id}";
                    _logger.LogInformation(message + $", \"ServiceResultStatusCode\"={result}");
                    Alert(message, AlertType.danger);
                    return RedirectToAction("Index");
            }
        }
    }
}
