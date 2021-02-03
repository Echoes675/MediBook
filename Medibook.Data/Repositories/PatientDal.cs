namespace MediBook.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
    using MediBook.Core.Enums;
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The PatientDal
    /// </summary>
    public class PatientDal : RepositoryBase<Patient>, IPatientDal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        public PatientDal(IDatabaseContext databaseContext, ILogger<PatientDal> logger) : base(databaseContext, logger)
        {
        }

        /// <summary>
        /// Accepts the PatientForRegistration dto and converts it to a Patient and sends to the db to be persisted
        /// </summary>
        /// <param name="newPatientDetails"></param>
        /// <returns></returns>
        public Task<Patient> AddAsync(PatientForRegistration newPatientDetails)
        {
            var patient = new Patient
            {
                Title = newPatientDetails.Title,
                Firstname = newPatientDetails.Firstname,
                Lastname = newPatientDetails.Lastname,
                DateOfBirth = newPatientDetails.DateOfBirth,
                HealthAndCare = newPatientDetails.HealthAndCare,
                Address1 = newPatientDetails.Address1,
                Address2 = newPatientDetails.Address2,
                City = newPatientDetails.City,
                County = newPatientDetails.County,
                PostCode = newPatientDetails.PostCode,
                PhoneNumber = newPatientDetails.PhoneNumber,
                MobilePhone = newPatientDetails.MobilePhone,
                Email = newPatientDetails.Email,
                Status = newPatientDetails.Status
            };

            return base.AddAsync(patient);
        }

        /// <summary>
        /// Return the Patient including all of their associated MedicalPractitioners with
        /// the Appointments filtered to the calling user's Id
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="callingUserId"></param>
        /// <returns></returns>
        public async Task<Patient> GetEntityAsync(int patientId, int callingUserId)
        {
            return await Db.Set<Patient>()
                .Include(a => a.Appointments.Where(o => o.MedicalPractitionerId == callingUserId))
                .Include(m => m.PatientsMedicalPractitioners)
                .FirstOrDefaultAsync(p => p.Id == patientId);
        }

        /// <summary>
        /// Return the Patient including all of their registered MedicalPractitioners and
        /// all of their Appointments
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<Patient> GetEntityAsync(int id)
        {
            return await Db.Set<Patient>()
                .Include(a => a.Appointments)
                .Include(m => m.PatientsMedicalPractitioners).ThenInclude(m => m.MedicalPractitioner)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Performs a search of patients
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public async Task<PatientSearchResults> PatientSearchAsync(PatientSearchCriteria searchCriteria)
        {
            if (searchCriteria == null)
            {
                throw new ArgumentNullException(nameof(searchCriteria));
            }

            if (string.IsNullOrEmpty(searchCriteria.SearchTerm))
            {
                throw new ArgumentNullException(nameof(searchCriteria.SearchTerm));
            }

            var searchTerm = searchCriteria.SearchTerm.Trim();

            var patients = new List<Patient>();

            // Find patients where the search term matches a string based property
            patients.AddRange(await SearchPatientStringBasedProperty(searchTerm));

            // Convert the search term and search for patients with matching Health and Care numbers
            patients.AddRange(await SearchPatientHealthAndCareNumber(searchTerm));

            // Convert the search term and search for patients with matching Id
            patients.AddRange(await SearchPatientIdNumber(searchTerm));

            // Remove any patients fromthe results if the search criteria does
            // not indicate patients who have left, or are deceased or who's status
            // is unknown.
            if (!searchCriteria.IncludeLeftDeceasedAndUnknown)
            {
                patients.RemoveAll(s => s.Status != PatientStatus.Active);
            }

            return new PatientSearchResults(patients);
        }

        private async Task<List<Patient>> SearchPatientIdNumber(string searchTerm)
        {
            // attempt to parse the searchTerm into an int value,
            // if successful search against Patients' Id numbers
            if (int.TryParse(searchTerm, out var criteriaInt))
            {
                return await Db.Set<Patient>().Where(p =>
                    p.HealthAndCare == criteriaInt).ToListAsync();
            }

            return new List<Patient>();
        }

        private async Task<List<Patient>> SearchPatientHealthAndCareNumber(string searchTerm)
        {
            // attempt to parse the searchTerm into a long value,
            // if successful search against Patients' Health and Care number
            if (!long.TryParse(searchTerm, out var criteriaLong))
            {
                return new List<Patient>();
            }

            return await Db.Set<Patient>().Where(p =>
                p.HealthAndCare == criteriaLong).ToListAsync();
        }
        private async Task<List<Patient>> SearchPatientStringBasedProperty(string searchTerm)
        {
            return await Db.Set<Patient>().Where(p =>
                EF.Functions.Like(p.Address1, searchTerm) ||
                EF.Functions.Like(p.Address2, searchTerm) ||
                EF.Functions.Like(p.Email, searchTerm) ||
                EF.Functions.Like(p.Firstname, searchTerm) ||
                EF.Functions.Like(p.Lastname, searchTerm) ||
                EF.Functions.Like(p.MobilePhone, searchTerm) ||
                EF.Functions.Like(p.PhoneNumber, searchTerm) ||
                EF.Functions.Like(p.PostCode, searchTerm)).ToListAsync();
        }
    }
}