namespace MediBook.Services.UserAdministration
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
    using MediBook.Services.Enums;

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
        Task<UserFullDetailsDto> CreateUserAsync(UserForRegistrationDto newUserDetails);

        /// <summary>
        /// Returns the Users full account and employee details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserFullDetailsDto> GetUserFullDetailsAsync(int id);

        /// <summary>
        /// Saves the updated user details
        /// </summary>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        Task<ServiceResultStatusCode> UpdateUserDetailsAsync(UserFullDetailsDto userDetails);

        /// <summary>
        /// Soft-Delete the user account associated to the Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResultStatusCode> DeleteUserAsync(int id);

        /// <summary>
        /// Reset the user account password
        /// </summary>
        /// <param name="userPasswordReset"></param>
        /// <returns></returns>
        Task<ServiceResultStatusCode> ResetPasswordAsync(UserPasswordResetDto userPasswordReset);
    }
}