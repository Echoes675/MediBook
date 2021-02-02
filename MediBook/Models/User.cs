namespace MediBook.Core.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using MediBook.Core.DTOs;
    using MediBook.Core.Enums;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The UserAccount class
    /// </summary>
    [Index(nameof(EmployeeId))]
    [Index(nameof(JobDescriptionId))]
    public class User : IDbEntity
    {
        /// <summary>
        /// Initializes a new instance of <see cref="User"/> class
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="UserFullDetailsDto"/> class
        /// </summary>
        public User(UserFullDetailsDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            Id = dto.UserId;
            AccountGuid = dto.AccountGuid;
            Username = dto.Username;
            JobDescription.Description = dto.JobDescription;
            JobDescription.Role = dto.Role;
            State = dto.State;
            EmployeeDetails.Id = dto.EmployeeId;
            EmployeeDetails.Title = dto.Title;
            EmployeeDetails.Firstname = dto.Firstname;
            EmployeeDetails.Lastname = dto.Lastname;
        }

        /// <summary>
        /// The entity's Id in the Users table
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Account Guid
        /// </summary>
        public string AccountGuid { get; set; } = new Guid().ToString();

        /// <summary>
        /// The Username
        /// </summary>
        [Required, MaxLength(50), MinLength(3)]
        public string Username { get; set; }

        /// <summary>
        /// The PasswordHash
        /// </summary>
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// The PasswordSalt
        /// </summary>
        public byte[] PasswordSalt { get; set; }

        /// <summary>
        /// The Account State
        /// </summary>
        [Required]
        public AccountState State { get; set; }

        /// <summary>
        /// The Id of the Job Description
        /// </summary>
        public int JobDescriptionId { get; set; }

        /// <summary>
        /// The navigation property for the user's Job Description
        /// </summary>
        [Required]
        public JobDescription JobDescription { get; set; }

        /// <summary>
        /// The Employee Id
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Navigation property to the Employee
        /// </summary>
        [Required]
        public Employee EmployeeDetails { get; set; }
    }
}