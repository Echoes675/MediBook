namespace MediBook.Services.UserAdministration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
    using MediBook.Data.Repositories;
    using Microsoft.Extensions.Logging;

    public interface IUserAdministrationService
    {
        /// <summary>
        /// Return a collection of summaries for all accounts
        /// </summary>
        /// <returns></returns>
        Task<List<UserAccountDetailsDto>> LoadUserAccounts();

        /// <summary>
        /// Creates a new User and Employee from the details provided
        /// </summary>
        /// <param name="newUserDetails"></param>
        /// <returns></returns>
        Task CreateUser(UserForRegistrationDto newUserDetails);

        /// <summary>
        /// Returns the Users full account and employee details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserFullDetailsDto> GetUserFullDetailsAsync(int id);
    }

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
        /// The User account Dal
        /// </summary>
        private readonly IUserDal _userDal;

        /// <summary>
        /// The Employee details Dal
        /// </summary>
        private readonly IEmployeeDal _employeeDal;

        /// <summary>
        /// Initializes an instance of the <see cref="UserAdministrationService"/> class
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userDal"></param>
        /// <param name="employeeDal"></param>
        public UserAdministrationService(ILogger<UserAdministrationService> logger, IUserDal userDal, IEmployeeDal employeeDal)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userDal = userDal ?? throw new ArgumentNullException(nameof(userDal));
            _employeeDal = employeeDal ?? throw new ArgumentNullException(nameof(employeeDal));
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
        public async Task CreateUser(UserForRegistrationDto newUserDetails)
        {

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
                throw new ArgumentOutOfRangeException(nameof(id),$"Id cannot be less than 1. \"Id\"={id}");
            }

            var account = await _userDal.GetUserFullDetailsAsync(id);

            return new UserFullDetailsDto(account);
        }

    }
}