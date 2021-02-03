namespace MediBook.Core.DTOs
{
    using System;
    using MediBook.Core.Enums;
    using MediBook.Core.Models;

    public class UserFullDetailsDto
    {
        /// <summary>
        /// Initializes an instance of the <see cref="UserFullDetailsDto"/> class
        /// </summary>
        public UserFullDetailsDto()
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="UserFullDetailsDto"/> class
        /// </summary>
        public UserFullDetailsDto(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.JobDescription == null)
            {
                throw new ArgumentNullException(nameof(user.JobDescription));
            }

            if (user.EmployeeDetails == null)
            {
                throw new ArgumentNullException(nameof(user.EmployeeDetails));
            }

            UserId = user.Id;
            Username = user.Username;
            JobDescription = user.JobDescription.Description;
            Role = user.JobDescription.Role;
            State = user.State;
            EmployeeId = user.EmployeeDetails.Id;
            Title = user.EmployeeDetails.Title;
            Firstname = user.EmployeeDetails.Firstname;
            Lastname = user.EmployeeDetails.Lastname;
        }

        /// <summary>
        /// The entity's Id in the Users table
        /// </summary>
        public int UserId { get; set; }

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
    }
}