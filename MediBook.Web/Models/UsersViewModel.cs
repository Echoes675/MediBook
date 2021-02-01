namespace MediBook.Web.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using MediBook.Core.DTOs;
    using MediBook.Core.Enums;

    /// <summary>
    /// The Users ViewModel
    /// </summary>
    public class UsersViewModel
    {
        /// <summary>
        /// Initializes an instance of the <see cref="UsersViewModel"/>
        /// </summary>
        /// <param name="userAccountDetails"></param>
        public UsersViewModel(List<UserAccountDetailsDto> userAccountDetails)
        {
            ActiveUserAccounts = userAccountDetails.Where(x => x.State == AccountState.Active).ToList();
            InactiveUserAccounts = userAccountDetails.Where(x => x.State == AccountState.Inactive).ToList();
            DeletedUserAccounts = userAccountDetails.Where(x => x.State == AccountState.Deleted).ToList();
        }

        /// <summary>
        /// Collection of the active User Accounts
        /// </summary>
        public List<UserAccountDetailsDto> ActiveUserAccounts { get; set; }

        /// <summary>
        /// Collection of the inactive User Accounts
        /// </summary>
        public List<UserAccountDetailsDto> InactiveUserAccounts { get; set; }

        /// <summary>
        /// Collection of thedeleted User Accounts
        /// </summary>
        public List<UserAccountDetailsDto> DeletedUserAccounts { get; set; }
    }
}
