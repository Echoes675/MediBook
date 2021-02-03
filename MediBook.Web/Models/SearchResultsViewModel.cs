namespace MediBook.Web.Models
{
    using System.Collections.Generic;
    using MediBook.Core.DTOs;

    /// <summary>
    /// The SearchResults ViewModel
    /// </summary>
    public class SearchResultsViewModel
    {
        /// <summary>
        /// The original search term
        /// </summary>
        public string OriginalSearchTerm { get; set; }

        /// <summary>
        /// Flag indicating whether to includes patients who have left or are deceased 
        /// </summary>
        public bool IncludeLeftAndDeceased { get; set; }

        /// <summary>
        /// The list of patients found
        /// </summary>
        public List<PatientDetailsDto> Patients { get; set; }
    }
}