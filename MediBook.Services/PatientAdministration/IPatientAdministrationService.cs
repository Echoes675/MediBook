namespace MediBook.Services.PatientAdministration
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
    using MediBook.Services.Enums;

    public interface IPatientAdministrationService
    {
        /// <summary>
        /// Return a collection of summaries for all patients
        /// </summary>
        /// <returns></returns>
        Task<List<PatientDetailsDto>> LoadPatientsDetails();

        /// <summary>
        /// Return a collection of summaries for patients matching the search criteria
        /// </summary>
        /// <returns></returns>
        Task<PatientSearchResults> PerformPatientSearchAsync(PatientSearchCriteria searchCriteria);

        /// <summary>
        /// Creates a new Patient from the details provided
        /// </summary>
        /// <param name="newPatientDetails"></param>
        /// <returns></returns>
        Task<PatientDetailsDto> RegisterPatientAsync(PatientForRegistration newPatientDetails);

        /// <summary>
        /// Return the Patient including all of their associated MedicalPractitioners with
        /// the Appointments and PatientNotes filtered to the calling user's Id
        /// </summary>
        /// <returns></returns>
        Task<PatientDetailsDto> GetPatientDetailsAsync(int patientId, int callingUserId);

        /// <summary>
        /// Saves the updated user details
        /// </summary>
        /// <param name="patientDetails"></param>
        /// <returns></returns>
        Task<ServiceResultStatusCode> UpdatePatientDetailsAsync(PatientDetailsDto patientDetails);
    }
}