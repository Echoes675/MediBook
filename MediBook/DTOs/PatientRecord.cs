namespace MediBook.Core.DTOs
{
    using System.Collections.Generic;

    /// <summary>
    /// The Patient Record
    /// </summary>
    public class PatientRecord
    {
        /// <summary>
        /// The patient's firstname
        /// </summary>
        public string PatientFirstName { get; set; }

        /// <summary>
        /// The patient's lastname
        /// </summary>
        public string PatientLastName { get; set; }

        /// <summary>
        /// The patient's Id
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// The Patient notes
        /// </summary>
        public List<PatientNoteDto> PatientNotes { get; set; }
    }
}