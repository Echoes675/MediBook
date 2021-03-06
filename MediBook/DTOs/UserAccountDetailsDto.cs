﻿namespace MediBook.Core.DTOs
{
    using System;
    using MediBook.Core.Enums;
    using MediBook.Core.Models;

    /// <summary>
    /// A summary of the User' account
    /// </summary>
    public class UserAccountDetailsDto
    {
        /// <summary>
        /// Initializes an instance of the <see cref="UserAccountDetailsDto"/> class
        /// </summary>
        public UserAccountDetailsDto()
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="UserAccountDetailsDto"/> class
        /// </summary>
        public UserAccountDetailsDto(User user)
        {
            if (user.EmployeeDetails?.Firstname == null)
            {
                throw new ArgumentNullException(nameof(user.EmployeeDetails.Firstname));
            }

            if (user.EmployeeDetails?.Lastname == null)
            {
                throw new ArgumentNullException(nameof(user.EmployeeDetails.Lastname));
            }

            if (user.JobDescription == null)
            {
                throw new ArgumentNullException(nameof(user.JobDescription));
            }

            Id = user.Id;
            Username = user.Username;
            Firstname = user.EmployeeDetails.Firstname;
            Lastname = user.EmployeeDetails.Lastname;
            JobDescription = user.JobDescription;
            State = user.State;
        }

        /// <summary>
        /// The entity's Id in the Users table
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The user's firstname
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// The user's lastname
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// The User's JobDescription
        /// </summary>
        public JobDescription JobDescription { get; set; }

        /// <summary>
        /// The Account State
        /// </summary>
        public AccountState State { get; set; }
    }
}