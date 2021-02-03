namespace MediBook.Services.PatientAdministration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
    using MediBook.Core.Models;
    using MediBook.Data.Repositories;
    using MediBook.Services.Enums;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The Patient Administration class
    /// </summary>
    public class PatientAdministrationService : IPatientAdministrationService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<PatientAdministrationService> _log;
        private readonly IPatientDal _patientDal;
        private readonly IUserDal _userDal;

        /// <summary>
        /// Initializes an instance of the <see cref="PatientAdministrationService"/> class
        /// </summary>
        /// <param name="log"></param>
        /// <param name="patientDal"></param>
        /// <param name="userDal"></param>
        public PatientAdministrationService(ILogger<PatientAdministrationService> log, IPatientDal patientDal, IUserDal userDal)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _patientDal = patientDal ?? throw new ArgumentNullException(nameof(patientDal));
            _userDal = userDal ?? throw new ArgumentNullException(nameof(userDal));
        }

        /// <summary>
        /// Return a collection of summaries for all patients
        /// </summary>
        /// <returns></returns>
        public async Task<List<PatientDetailsDto>> LoadPatientsDetails()
        {
            var patients = await _patientDal.GetAllAsync();

            return patients.Select(r => new PatientDetailsDto(r)).ToList();
        }

        /// <summary>
        /// Return a collection of summaries for patients matching the search criteria
        /// </summary>
        /// <returns></returns>
        public async Task<PatientSearchResults> PerformPatientSearchAsync(PatientSearchCriteria searchCriteria)
        {
            if (searchCriteria == null)
            {
                throw new ArgumentNullException(nameof(searchCriteria));
            }

            return await _patientDal.PatientSearchAsync(searchCriteria);
        }

        /// <summary>
        /// Creates a new Patient from the details provided
        /// </summary>
        /// <param name="newPatientDetails"></param>
        /// <returns></returns>
        public async Task<PatientDetailsDto> RegisterPatientAsync(PatientForRegistration newPatientDetails)
        {
            if (newPatientDetails == null)
            {
                throw new ArgumentNullException(nameof(newPatientDetails));
            }

            var patient = await _patientDal.AddAsync(newPatientDetails);

            return patient != null ? new PatientDetailsDto(patient) : null;
        }

        /// <summary>
        /// Return the Patient including all of their associated MedicalPractitioners with
        /// the Appointments and PatientNotes filtered to the calling user's Id
        /// </summary>
        /// <returns></returns>
        public async Task<PatientDetailsDto> GetPatientDetailsAsync(int patientId, int callingUserId)
        {
            if (patientId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(patientId));
            }

            if (callingUserId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(callingUserId));
            }

            var patient = await _patientDal.GetEntityAsync(patientId, callingUserId);

            var patientDetails = new PatientDetailsDto(patient);

            if (patient.PatientsMedicalPractitioners != null)
            {
                var medicalPractitionersIds = patient.PatientsMedicalPractitioners.Select(m => m.MedicalPractitioner.Id).ToList();
                var medicalPractitioners = await _userDal.GetEntitiesAsync(medicalPractitionersIds);
                if (medicalPractitioners.Any())
                {
                    patientDetails.MedicalPractitioners =
                        medicalPractitioners.Select(userAccount => 
                            new MedicalPractitionerDetails(userAccount.Id, userAccount.JobDescription.Description, userAccount.EmployeeDetails)).ToList();
                }
            }

            return patientDetails;

        }

        /// <summary>
        /// Saves the updated user details
        /// </summary>
        /// <param name="patientDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResultStatusCode> UpdatePatientDetailsAsync(PatientDetailsDto patientDetails)
        {
            if (patientDetails == null)
            {
                throw new ArgumentNullException(nameof(patientDetails));
            }

            var patient = await _patientDal.GetEntityAsync(patientDetails.Id);
            if (patient == null)
            {
                return ServiceResultStatusCode.NotFound;
            }

            UpdatePatientProperties(ref patient, patientDetails);

            return await _patientDal.UpdateAsync(patient) != null ? ServiceResultStatusCode.Success : ServiceResultStatusCode.Failed;
        }

        private void UpdatePatientProperties(ref Patient patient, PatientDetailsDto updatedDetails)
        {
            patient.Id = updatedDetails.Id;
            patient.Title = updatedDetails.Title;
            patient.Firstname = updatedDetails.Firstname;
            patient.Lastname = updatedDetails.Lastname;
            patient.DateOfBirth = updatedDetails.DateOfBirth;
            patient.HealthAndCare = updatedDetails.HealthAndCare;
            patient.Address1 = updatedDetails.Address1;
            patient.Address2 = updatedDetails.Address2;
            patient.City = updatedDetails.City;
            patient.County = updatedDetails.County;
            patient.PostCode = updatedDetails.PostCode.ToUpper();
            patient.PhoneNumber = updatedDetails.PhoneNumber;
            patient.MobilePhone = updatedDetails.MobilePhone;
            patient.Email = updatedDetails.Email.ToLower();
            patient.Status = updatedDetails.PatientStatus;
        }
    }
}