namespace MediBook.Services.PatientRecord.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
    using MediBook.Core.Models;
    using MediBook.Data.Repositories;
    using MediBook.Services.Cryptography;
    using Microsoft.Extensions.Logging;

    public class PatientNoteProcessor : IPatientNoteProcessor
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<PatientNoteProcessor> _log;

        /// <summary>
        /// The cryptography service
        /// </summary>
        private readonly ICryptographyService _cryptoSvc;

        /// <summary>
        /// The Patient note dal
        /// </summary>
        private readonly IPatientNoteDal _noteDal;

        /// <summary>
        /// Creates an instance of the <see cref="PatientNoteProcessor"/> class
        /// </summary>
        /// <param name="log"></param>
        /// <param name="cryptoSvc"></param>
        /// <param name="noteDal"></param>
        public PatientNoteProcessor(ILogger<PatientNoteProcessor> log, ICryptographyService cryptoSvc, IPatientNoteDal noteDal)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _cryptoSvc = cryptoSvc ?? throw new ArgumentNullException(nameof(cryptoSvc));
            _noteDal = noteDal ?? throw new ArgumentNullException(nameof(noteDal));
        }

        /// <summary>
        /// Adds a new PatientRecordEntry
        /// </summary>
        /// <param name="newPatientRecordEntry"></param>
        /// <returns></returns>
        public async Task<PatientNoteDto> AddPatientRecordEntryAsync(PatientNoteDto newPatientRecordEntry)
        {
            if (newPatientRecordEntry == null)
            {
                throw new ArgumentNullException(nameof(newPatientRecordEntry));
            }

            var newPatientRecordEntryString = JsonSerializer.Serialize(newPatientRecordEntry);
            var encryptedContent = _cryptoSvc.Encrypt(newPatientRecordEntryString);

            var newPatientNote = new PatientNote()
            {
                PatientId = newPatientRecordEntry.PatientId,
                MedicalPractitionerId = newPatientRecordEntry.MedicalPractitionerId,
                Timestamp = newPatientRecordEntry.Timestamp,
                Content = encryptedContent
            };

            var result = await _noteDal.AddAsync(newPatientNote);

            if (result == null)
            {
                return null;
            }

            newPatientRecordEntry.Id = result.Id;
            return newPatientRecordEntry;
        }

        /// <summary>
        /// Retrieves the patient records for a medical practitioner was the author
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="medicalPractitionerId"></param>
        /// <returns></returns>
        public List<PatientNoteDto> RetrievePatientRecords(int patientId, int medicalPractitionerId)
        {
            if (patientId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(patientId));
            }

            if (medicalPractitionerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(medicalPractitionerId));
            }

            // Get only the patient notes authored by the incoming medicalPractitionerId
            var result = _noteDal.Filter(x => x.PatientId == patientId &&
                                              x.MedicalPractitionerId == medicalPractitionerId);

            // Decrypt and deserialize the content of the patient notes
            var decryptedNoteStrings = result.Select(x => _cryptoSvc.Decrypt(x.Content));

            var deserializedNotes = decryptedNoteStrings.Select(y => JsonSerializer.Deserialize<PatientNoteDto>(y));

            return deserializedNotes.OrderByDescending(x => x?.Timestamp).ToList();
        }

        /// <summary>
        /// Retrieves an individual patient record for a medical practitioner where they were the author
        /// </summary>
        /// <param name="patientRecordId"></param>
        /// <param name="medicalPractitionerId"></param>
        /// <returns></returns>
        public PatientNoteDto RetrievePatientRecord(int patientRecordId, int medicalPractitionerId)
        {
            if (patientRecordId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(patientRecordId));
            }

            if (medicalPractitionerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(medicalPractitionerId));
            }

            // Get only the patient notes authored by the incoming medicalPractitionerId
            var matchingResults = _noteDal.Filter(x => x.Id == patientRecordId &&
                                              x.MedicalPractitionerId == medicalPractitionerId);
            var result = matchingResults.FirstOrDefault();
            if (result == null)
            {
                _log.LogInformation($"No Patient Note located with matching Id and an author of MedicalPractitionerId. \"Id\"={patientRecordId}, \"MedicalPractitionerId\"={medicalPractitionerId}");
                return null;
            }

            // Decrypt and deserialize the content of the patient notes
            var decryptedNoteStrings = _cryptoSvc.Decrypt(result.Content);
            var deserializedNote = JsonSerializer.Deserialize<PatientNoteDto>(decryptedNoteStrings);

            return deserializedNote;
        }
    }
}
