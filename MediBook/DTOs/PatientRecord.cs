namespace MediBook.Core.DTOs
{
    using System.Collections.Generic;

    /// <summary>
    /// The Patient Record
    /// </summary>
    public class PatientRecord
    {
        /// <summary>
        /// The Patient notes
        /// </summary>
        public List<PatientNoteDto> PatientNotes { get; set; }
    }
}