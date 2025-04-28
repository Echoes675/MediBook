namespace MediBook.Core.DTOs
{
    using System.Collections.Generic;
    using System.Linq;
    using MediBook.Core.Models;

    /// <summary>
    /// The Patient Search Results
    /// </summary>
    public class PatientSearchResults
    {
        /// <summary>
        /// Initialize an instance of the <see cref="PatientSearchResults"/> class
        /// </summary>
        public PatientSearchResults()
        {
        }

        /// <summary>
        /// Initialize an instance of the <see cref="PatientSearchResults"/> class
        /// </summary>
        public PatientSearchResults(IEnumerable<Patient> patients)
        {
            Patients = patients.Select(x => new PatientDetailsDto(x)).ToList();
        }

        /// <summary>
        /// The list of patients found matching the search criteria
        /// </summary>
        public List<PatientDetailsDto> Patients { get; set; }
    }
}
