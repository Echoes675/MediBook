namespace MediBook.Core.DTOs
{
    /// <summary>
    /// The patient search criteria
    /// </summary>
    public class PatientSearchCriteria
    {
        /// <summary>
        /// Search criteria
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Flag whether to include patients who have left or are deceased
        /// </summary>
        public bool IncludeLeftDeceasedAndUnknown { get; set; }
    }
}
