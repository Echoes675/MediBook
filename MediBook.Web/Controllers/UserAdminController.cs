namespace MediBook.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using MediBook.Services.UserAdministration;
    using MediBook.Web.Enums;
    using MediBook.Web.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class UserAdminController : ControllerBase
    {
        /// <summary>
        /// The Logger
        /// </summary>
        private readonly ILogger<UserAdminController> _logger;

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
                Alert(message, AlertType.Error);
                return RedirectToPage(nameof(Index), nameof(HomeController));
            }

            var usersViewModel = new UsersViewModel(users);

            return View(usersViewModel);
        }

        // GET auth/EditUser
        //[Authorize(Roles = "PracticeAdmin")]
        [HttpGet("EditUser")]
        public async Task<IActionResult> EditUser(int id)
        {
            // load the user from the database
            var userFromDb = await _userAdminSvc.GetUserFullDetailsAsync(id).ConfigureAwait(false);

            // if the requested user is not found, load the EditUser view
            if (userFromDb != null)
            {
                return View(userFromDb);
            }

            // if the requested user is not found, return an error
            var message = $"Cannot find user load for editing. \"Id\"={id}";
            _logger.LogError(message);
            Alert(message, AlertType.Error);
            return RedirectToAction("Index", "UserAdmin");
        }

    }
}
