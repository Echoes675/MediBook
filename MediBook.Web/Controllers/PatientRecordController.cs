namespace MediBook.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
    using MediBook.Services.AppointmentBook;
    using MediBook.Services.Enums;
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
        /// The AppointmentBook service
        /// </summary>
        private readonly IAppointmentBookService _appointmentBookService;

        /// <summary>
        /// Initialise an instance of the <see cref="PatientRecordController"/>
        /// </summary>
        /// <param name="log"></param>
        /// <param name="patientRecordSvc"></param>
        /// <param name="userAdminSvc"></param>
        /// <param name="patientAdminSvc"></param>
        /// <param name="appointmentBookService"></param>
        public PatientRecordController(
            ILogger<PatientRecordController> log,
            IPatientRecordService patientRecordSvc,
            IUserAdministrationService userAdminSvc,
            IPatientAdministrationService patientAdminSvc,
            IAppointmentBookService appointmentBookService)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _patientRecordSvc = patientRecordSvc ?? throw new ArgumentNullException(nameof(patientRecordSvc));
            _userAdminSvc = userAdminSvc ?? throw new ArgumentNullException(nameof(userAdminSvc));
            _patientAdminSvc = patientAdminSvc ?? throw new ArgumentNullException(nameof(patientAdminSvc));
            _appointmentBookService = appointmentBookService;
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
                return RedirectToAction("Details", "Patient", new { id = id });
            }

            var patientDetails = await _patientAdminSvc.GetPatientDetailsAsync(id, userId);
            if (patientDetails == null)
            {
                _log.LogError($"Failed to load the Patient's details");
                Alert("Failed to load the Patient", AlertType.danger);
                return RedirectToAction("Details", "Patient", new { id = id });
            }

            // Retrieve the patient notes authored by the calling MedicalPractitioner
            var patientNotes = _patientRecordSvc.RetrievePatientRecords<PatientNoteDto>(id, userId);

            if (patientNotes == null)
            {
                _log.LogError($"Failed to load the Patient Notes from the Patients Record");
                Alert("Failed to load the Patient Notes from the Patients Record", AlertType.danger);
                return RedirectToAction("Details", "Patient", new { id = id });
            }

            // Set up the PatientRecord
            var patientRecord = new PatientRecord()
            {
                PatientId = patientDetails.Id,
                PatientFirstName = patientDetails.Firstname,
                PatientLastName = patientDetails.Lastname,
                PatientNotes = patientNotes.OrderByDescending(t => t.Timestamp).ToList()
            };

            // Retrieve the patient appointments associated by the calling MedicalPractitioner
            var patientAppointmentsResult = await _appointmentBookService.GetPatientAppointmentHistory(id, userId);

            if (patientAppointmentsResult.ResultCode != ServiceResultStatusCode.Success)
            {
                _log.LogError($"Failed to load the Patient Appointment History from the Patient's Record");
                Alert("Failed to load the Patient Appointment History from the Patient's Record", AlertType.danger);
                return RedirectToAction("Details", "Patient", new { id = id });
            }

            // Add the Patient's Appointments to the PatientRecord
            patientRecord.PatientAppointments = patientAppointmentsResult.AppointmentsDetails;

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
                return RedirectToAction("Details", "Patient", new { id = id });
            }

            var medicalPractitioner = await _userAdminSvc.GetUserFullDetailsAsync(userId);
            if (medicalPractitioner == null)
            {
                _log.LogError($"Failed to load Medical Practitioner's account. \"Id\"={userId}");
                Alert("Failed to authorize current User access to the Patient Record", AlertType.danger);
                return RedirectToAction("Details", "Patient", new { id = id });
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
                return RedirectToAction("Index",new { id = newPatientNote.PatientId});
            }

            _log.LogError($"Failed to save new Patient note. \"PatientId\"={newPatientNote.PatientId}, \"MedicalPractitionerId\"={newPatientNote.MedicalPractitionerId}");
            Alert("Failed to save new Patient note.", AlertType.danger);
            return RedirectToAction("Index", new { id = newPatientNote.PatientId });
        }
    }
}
