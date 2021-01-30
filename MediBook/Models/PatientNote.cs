﻿namespace MediBook.Core.Models
{
    public class PatientNote : IDbEntity
    {
        /// <summary>
        /// The PatientNote Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The content of the PatientNote
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The Patient Id
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// The Patient navigation property
        /// </summary>
        public Patient Patient { get; set; }

        /// <summary>
        /// The Id of the associated Medical Practitioner User account
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The User navigation property
        /// </summary>
        public User User { get; set; }
    }
}