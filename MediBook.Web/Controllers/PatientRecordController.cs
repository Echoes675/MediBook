namespace MediBook.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
    using MediBook.Services.PatientAdministration;
    using MediBook.Services.PatientRecord;
    using MediBook.Services.UserAdministration;
    using MediBook.Web.Enums;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The PatientRecord Controller
    /// </summary>
    public class PatientRecordController : ControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<PatientRecordController> _log;

        /// <summary>
        /// The Patient Record Service
        /// </summary>
        private readonly IPatientRecordService _patientRecordSvc;

        /// <summary>
        /// The User Admin Service
        /// </summary>
        private readonly IUserAdministrationService _userAdminSvc;

        /// <summary>
        /// The Patient Admin Service
        /// </summary>
        private readonly IPatientAdministrationService _patientAdminSvc;

        /// <summary>
        /// Initialise an instance of the <see cref="PatientRecordController"/>
        /// </summary>
        /// <param name="log"></param>
        /// <param name="patientRecordSvc"></param>
        /// <param name="userAdminSvc"></param>
        /// <param name="patientAdminSvc"></param>
        public PatientRecordController(
            ILogger<PatientRecordController> log,
            IPatientRecordService patientRecordSvc,
            IUserAdministrationService userAdminSvc,
            IPatientAdministrationService patientAdminSvc)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _patientRecordSvc = patientRecordSvc ?? throw new ArgumentNullException(nameof(patientRecordSvc));
            _userAdminSvc = userAdminSvc ?? throw new ArgumentNullException(nameof(userAdminSvc));
            _patientAdminSvc = patientAdminSvc ?? throw new ArgumentNullException(nameof(patientAdminSvc));
        }

        [HttpGet]
        //[Authorize(Roles = "MedicalPractitioner")]
        public async Task<IActionResult> Index(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var userId = GetLoggedInUserId();
            if (userId < 1)
            {
                _log.LogError($"Authentication failure. Could not extract User's Id from Claims Principal.");
                Alert("Failed to authorize current User access to the Patient Record", AlertType.danger);
                return RedirectToAction("Details", "Patient", id);
            }

            var patientDetails = await _patientAdminSvc.GetPatientDetailsAsync(id, userId);
            if (patientDetails == null)
            {
                _log.LogError($"Failed to load the Patient's details");
                Alert("Failed to load the Patient", AlertType.danger);
                return RedirectToAction("Details", "Patient", id);
            }

            var patientNotes = _patientRecordSvc.RetrievePatientRecords<PatientNoteDto>(id, userId);

            if (patientNotes == null)
            {
                _log.LogError($"Failed to load the Patient Notes from the Patients Record");
                Alert("Failed to load the Patient Notes from the Patients Record", AlertType.danger);
                return RedirectToAction("Details", "Patient", id);
            }
            var patientRecord = new PatientRecord()
            {
                PatientId = patientDetails.Id,
                PatientFirstName = patientDetails.Firstname,
                PatientLastName = patientDetails.Lastname,
                PatientNotes = patientNotes.OrderByDescending(t => t.Timestamp).ToList()
            };

            return View(patientRecord);
        }

        [HttpGet]
        //[Authorize(Roles = "MedicalPractitioner")]
        public async Task<IActionResult> AddPatientNote(int id)
        {
            var userId = GetLoggedInUserId();
            if (userId < 1)
            {
                _log.LogError($"Authentication failure. Could not extract User's Id from Claims Principal.");
                Alert("Failed to authorize current User access to the Patient Record", AlertType.danger);
                return RedirectToAction("Details", "Patient", id);
            }

            var medicalPractitioner = await _userAdminSvc.GetUserFullDetailsAsync(userId);
            if (medicalPractitioner == null)
            {
                _log.LogError($"Failed to load Medical Practitioner's account. \"Id\"={userId}");
                Alert("Failed to authorize current User access to the Patient Record", AlertType.danger);
                return RedirectToAction("Details", "Patient", id);
            }

            var newPatientNote = new PatientNoteDto()
            {
                PatientId = id,
                MedicalPractitionerId = userId,
                MedicalPractitionerFirstname = medicalPractitioner.Firstname,
                MedicalPractitionerLastname = medicalPractitioner.Lastname,
                MedicalPractitionerTitle = medicalPractitioner.Title.ToString()
            };

            return View(newPatientNote);
        }

        [HttpPost]
        //[Authorize(Roles = "MedicalPractitioner")]
        public async Task<IActionResult> AddPatientNote([FromForm] PatientNoteDto newPatientNote)
        {
            if (newPatientNote == null)
            {
                throw new ArgumentNullException(nameof(newPatientNote));
            }
            
            // Add the timestamp just prior to saving the patient note
            newPatientNote.Timestamp = DateTime.UtcNow;

            var result = await _patientRecordSvc.AddPatientRecordEntryAsync(newPatientNote);

            if (result != null)
            {
                Alert(
                    $"Patient note successfully saved to the patient's record. \"PatientId\"={newPatientNote.PatientId}", AlertType.success);
                return RedirectToAction("Index", newPatientNote.PatientId);
            }

            _log.LogError($"Failed to save new Patient note. \"PatientId\"={newPatientNote.PatientId}, \"MedicalPractitionerId\"={newPatientNote.MedicalPractitionerId}");
            Alert("Failed to save new Patient note.", AlertType.danger);
            return RedirectToAction("Index", newPatientNote.PatientId);
        }

        /// <summary>
        /// Extract the logged in user's Id from the Claims Principal
        /// </summary>
        /// <returns></returns>
        private int GetLoggedInUserId()
        {
            // extracting the custom user claim here
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity?.Claims.FirstOrDefault(x => x.Type == "Id");

            // extract user id from claim
            var idString = claim != null ? claim.Value : string.Empty;

            // Try to parse idString to an int
            if (int.TryParse(idString, out var id))
            {
                return id;
            }

            // Failed to parse the idString. Return -1 as a number that could never be an Id
            return -1;
        }
    }
}
