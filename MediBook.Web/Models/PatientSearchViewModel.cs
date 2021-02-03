namespace MediBook.Web.Models
{
    using System.Collections.Generic;
    using MediBook.Core.DTOs;
    /// <summary>
    /// The Patient Search View Model
    /// </summary>
    public class PatientSearchViewModel
    {
        /// <summary>
        /// The search criteria
        /// </summary>
        public PatientSearchCriteria SearchCriteria { get; set; }

        /// <summary>
        /// The list of patients found
        /// </summary>
        public PatientSearchResults SearchResults { get; set; }
    }
}
