namespace MediBook.Services.UserAdministration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
    using MediBook.Core.Models;
    using MediBook.Data.Repositories;
    using MediBook.Services.Cryptography;
    using MediBook.Services.Enums;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The User Administration Service
    /// </summary>
    public class UserAdministrationService : IUserAdministrationService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<UserAdministrationService> _logger;

        /// <summary>
        /// The Cryptography Service
        /// </summary>
        private readonly ICryptographyService _cryptoSvc;

        /// <summary>
        /// The UserDal
        /// </summary>
        private readonly IUserDal _userDal;

        /// <summary>
        /// Initializes an instance of the <see cref="UserAdministrationService"/> class
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userDal"></param>
        /// <param name="cryptographyService"></param>
        public UserAdministrationService(ILogger<UserAdministrationService> logger, IUserDal userDal, ICryptographyService cryptographyService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userDal = userDal ?? throw new ArgumentNullException(nameof(userDal));
            _cryptoSvc = cryptographyService ?? throw new ArgumentNullException(nameof(cryptographyService));
        }

        /// <summary>
        /// Return a collection of summaries for all accounts
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserAccountDetailsDto>> LoadUserAccounts()
        {
            var users = await _userDal.GetAllAsync();
            return users.Select(user => new UserAccountDetailsDto(user)).ToList();
        }

        /// <summary>
        /// Creates a new User and Employee from the details provided
        /// </summary>
        /// <param name="newUserDetails"></param>
        /// <returns></returns>
        public async Task<UserFullDetailsDto> CreateUserAsync(UserForRegistrationDto newUserDetails)
        {
            if (newUserDetails == null)
            {
                throw new ArgumentNullException(nameof(newUserDetails));
            }

            // map elements of the UserForRegistrationDto to JobDescription and Employee
            var newJobDescription = new JobDescription()
            {
                Description = newUserDetails.JobDescription,
                Role = newUserDetails.Role
            };

            var newEmployeeDetails = new Employee()
            {
                Firstname = newUserDetails.FirstName,
                Lastname = newUserDetails.LastName,
                Title = newUserDetails.Title,
            };

            // Create the Password Hash and Salt
            _cryptoSvc.CreateHash(newUserDetails.Password, out var passwordHash, out var passwordSalt);

            // Combine elements to make the new user
            var newUser = new User()
            {
                Username = newUserDetails.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                State = newUserDetails.State,
                JobDescription = newJobDescription,
                EmployeeDetails = newEmployeeDetails
            };

            // Save to the database
            var result = await _userDal.AddAsync(newUser);

            return result != null ? new UserFullDetailsDto(result) : null;
        }

        /// <summary>
        /// Returns the Users full account and employee details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserFullDetailsDto> GetUserFullDetailsAsync(int id)
        {
            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), $"Id cannot be less than 1. \"Id\"={id}");
            }

            var account = await _userDal.GetEntityAsync(id);

            return new UserFullDetailsDto(account);
        }

        /// <summary>
        /// Saves the updated user details
        /// </summary>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResultStatusCode> UpdateUserDetailsAsync(UserFullDetailsDto userDetails)
        {
            if (userDetails == null)
            {
                throw new ArgumentNullException(nameof(userDetails));
            }

            var dbUser = await _userDal.GetEntityAsync(userDetails.UserId);
            if (dbUser == null)
            {
                return ServiceResultStatusCode.NotFound;
            }

            UpdateUserProperties(ref dbUser, userDetails);

            return await _userDal.UpdateAsync(dbUser) != null ? ServiceResultStatusCode.Success : ServiceResultStatusCode.Failed;
        }

        /// <summary>
        /// Soft-Delete the user account associated to the Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResultStatusCode> DeleteUserAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), $"Id provided cannot be less than 1 \"Id\"={id}");
            }

            if (await _userDal.DeleteAsync(id))
            {
                return ServiceResultStatusCode.Success;
            }

            return ServiceResultStatusCode.Failed;
        }

        /// <summary>
        /// Reset the user account password
        /// </summary>
        /// <param name="userPasswordReset"></param>
        /// <returns></returns>
        public async Task<ServiceResultStatusCode> ResetPasswordAsync(UserPasswordResetDto userPasswordReset)
        {
            if (userPasswordReset == null)
            {
                throw new ArgumentNullException(nameof(userPasswordReset));
            }

            var user = await _userDal.GetEntityAsync(userPasswordReset.Id);
            if (user == null)
            {
                return ServiceResultStatusCode.NotFound;
            }

            // Generate the password hash and salt
            _cryptoSvc.CreateHash(userPasswordReset.Password, out var hashPassword, out var saltPassword);

            // update the user account
            user.PasswordHash = hashPassword;
            user.PasswordSalt = saltPassword;

            return await _userDal.UpdateAsync(user) != null ? ServiceResultStatusCode.Success : ServiceResultStatusCode.Failed;
        }

        private void UpdateUserProperties(ref User user, UserFullDetailsDto updatedDetails)
        {
            user.State = updatedDetails.State;
            user.JobDescription.Description = updatedDetails.JobDescription;
            user.JobDescription.Role = updatedDetails.Role;
            user.EmployeeDetails.Firstname = updatedDetails.Firstname;
            user.EmployeeDetails.Lastname = updatedDetails.Lastname;
        }
    }
}