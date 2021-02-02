namespace MediBook.Web.Models
{
    using System;
    using System.Collections.Generic;
    using MediBook.Core.DTOs;
    using MediBook.Core.Enums;
    using MediBook.Web.Helpers;
    using Microsoft.AspNetCore.Mvc.Rendering;

    /// <summary>
    /// Model to represent the User's details in the view
    /// </summary>
    public class UserDetailsViewModel
    {
        /// <summary>
        /// Initializes an instance of the <see cref="UserDetailsViewModel"/> class
        /// </summary>
        public UserDetailsViewModel()
        {

        }

        /// <summary>
        /// Initializes an instance of the <see cref="UserDetailsViewModel"/> class
        /// </summary>
        public UserDetailsViewModel(UserFullDetailsDto userFullDetails)
        {
            if (userFullDetails == null)
            {
                throw new ArgumentNullException(nameof(userFullDetails));
            }

            UserId = userFullDetails.UserId;
            AccountGuid = userFullDetails.AccountGuid;
            Username = userFullDetails.Username;
            JobDescription = userFullDetails.JobDescription ?? throw new ArgumentNullException(nameof(userFullDetails.JobDescription));
            Role = userFullDetails.Role;
            State = userFullDetails.State;
            EmployeeId = userFullDetails.EmployeeId;
            Title = userFullDetails.Title;
            Firstname = userFullDetails.Firstname;
            Lastname = userFullDetails.Lastname;

            // Gets the list of Roles for the drop down
            TitlesSelectListItems = SelectListHelpers.GetTitlesSelectList(Title);

            // Gets the list of Roles for the drop down
            RolesSelectListItems = SelectListHelpers.GetRolesSelectList(Role);

            // Gets the list of Account States for the drop down
            AccountStatesSelectListItems = SelectListHelpers.GetAccountStatesSelectList(State);
        }

        /// <summary>
        /// List of SelectListItems to be used as a dropdown of Titles in the EditUser view
        /// </summary>
        public List<SelectListItem> TitlesSelectListItems { get; }

        /// <summary>
        /// List of SelectListItems to be used as a dropdown of Roles in the EditUser view
        /// </summary>
        public List<SelectListItem> RolesSelectListItems { get; }

        /// <summary>
        /// List of SelectListItems to be used as a dropdown of Account States in the EditUser view
        /// </summary>
        public List<SelectListItem> AccountStatesSelectListItems { get; }

        /// <summary>
        /// The entity's Id in the Users table
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The Account Guid
        /// </summary>
        public string AccountGuid { get; set; }

        /// <summary>
        /// The Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The user's role
        /// </summary>
        public UserRole Role { get; set; }

        /// <summary>
        /// The Account State
        /// </summary>
        public AccountState State { get; set; }

        /// <summary>
        /// The Employee Id
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// The user's title
        /// </summary>
        public Title Title { get; set; }

        /// <summary>
        /// The user's firstname
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// The user's lastname
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// The user's Job Description
        /// </summary>
        public string JobDescription { get; set; }

        public UserFullDetailsDto MapToUserFullDetailsDtoDto()
        {
            return new UserFullDetailsDto()
            {
                UserId = UserId,
                AccountGuid = AccountGuid,
                Username = Username,
                Role = Role,
                State = State,
                EmployeeId = EmployeeId,
                Title = Title,
                Firstname = Firstname,
                Lastname = Lastname,
                JobDescription = JobDescription
            };
        }
    }
}