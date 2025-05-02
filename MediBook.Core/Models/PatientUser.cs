namespace MediBook.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PatientUser : User
    {
        /// <summary>
        /// The Patient Id
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// Navigation property to the Patient
        /// </summary>
        [Required]
        public Patient PatientDetails { get; set; }
    }
}