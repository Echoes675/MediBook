namespace MediBook.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using MediBook.Services.PatientAdministration;
    using MediBook.Services.PatientRecord;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The <see cref="PatientController"/>
    /// </summary>
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _log;
        private readonly IPatientAdministrationService _patientSvc;
        private readonly IPatientRecordService _patientRecordSvc;

        /// <summary>
        /// Initializes an instance of the <see cref="PatientController"/>
        /// </summary>
        public PatientController(ILogger<PatientController> log, IPatientAdministrationService patientSvc, IPatientRecordService patientRecordSvc)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _patientSvc = patientSvc ?? throw new ArgumentNullException(nameof(patientSvc));
            _patientRecordSvc = patientRecordSvc ?? throw new ArgumentNullException(nameof(patientRecordSvc));
        }

        [HttpGet]
        //[Authorize(Roles = "Reception, PracticeAdmin, MedicalPractitioner")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}