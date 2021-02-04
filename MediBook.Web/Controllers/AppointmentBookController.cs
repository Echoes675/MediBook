namespace MediBook.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using MediBook.Data.Repositories;
    using MediBook.Services.AppointmentBook;
    using MediBook.Services.Enums;
    using MediBook.Web.Enums;
    using MediBook.Web.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AppointmentBookController : ControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly Logger<AppointmentBookController> _log;

        /// <summary>
        /// The AppointmentBook Service
        /// </summary>
        private readonly IAppointmentBookService _apptSvc;

        public AppointmentBookController(Logger<AppointmentBookController> log, IAppointmentBookService apptSvc, IUserDal userDal)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _apptSvc = apptSvc ?? throw new ArgumentNullException(nameof(apptSvc));
        }

        [HttpGet]
        //[Authorize(Roles = "Reception, PracticeAdmin, MedicalPractitioner")]
        public async Task<IActionResult> Index()
        {
            var currentLoggedInUser = GetLoggedInUserId();
            if (currentLoggedInUser < 1)
            {
                _log.LogError($"Authentication failure. Could not extract User's Id from Claims Principal.");
                Alert("Failed to identify current User for authorization", AlertType.danger);
                return RedirectToAction(nameof(Index), "Home");
            }

            var date = DateTime.UtcNow.Date;
            var appointmentSessions = await _apptSvc.GetAppointmentBookSessions(currentLoggedInUser, date);
            if (appointmentSessions.ResultCode != ServiceResultStatusCode.Success)
            {
                _log.LogError($"Failed to load Appointment Session(s). \"ResultCode\"={appointmentSessions.ResultCode}");
                Alert("Failed to load Appointment Session(s).", AlertType.danger);
                return RedirectToAction(nameof(Index), "Home");
            }

            var appointmentBookViewModel = new AppointmentBookViewModel()
            {
                Date = date,
                AppointmentSessions = appointmentSessions.AppointmentSessionDetails
            };

            return View(appointmentBookViewModel);
        }

        [HttpPost("GetNextDay")]
        //[Authorize(Roles = "Reception, PracticeAdmin, MedicalPractitioner")]
        public async Task<IActionResult> GetNextDay(AppointmentBookViewModel model)
        {
            var nextDate = model.Date.AddDays(1);

            var currentLoggedInUser = GetLoggedInUserId();
            if (currentLoggedInUser < 1)
            {
                _log.LogError($"Authentication failure. Could not extract User's Id from Claims Principal.");
                Alert("Failed to identify current User for authorization", AlertType.danger);
                return RedirectToAction(nameof(Index), "Home");
            }

            var appointmentSessions = await _apptSvc.GetAppointmentBookSessions(currentLoggedInUser, nextDate);
            if (appointmentSessions.ResultCode != ServiceResultStatusCode.Success)
            {
                _log.LogError($"Failed to load Appointment Session(s). \"ResultCode\"={appointmentSessions.ResultCode}");
                Alert("Failed to load Appointment Session(s).", AlertType.danger);
                return RedirectToAction(nameof(Index), "Home");
            }

            var appointmentBookViewModel = new AppointmentBookViewModel()
            {
                Date = nextDate,
                AppointmentSessions = appointmentSessions.AppointmentSessionDetails
            };

            return View(nameof(Index), appointmentBookViewModel);
        }

        [HttpPost("GetPreviousDay")]
        //[Authorize(Roles = "Reception, PracticeAdmin, MedicalPractitioner")]
        public async Task<IActionResult> GetPreviousDay(AppointmentBookViewModel model)
        {
            var previousDate = model.Date.AddDays(-1);

            var currentLoggedInUser = GetLoggedInUserId();
            if (currentLoggedInUser < 1)
            {
                _log.LogError($"Authentication failure. Could not extract User's Id from Claims Principal.");
                Alert("Failed to identify current User for authorization", AlertType.danger);
                return RedirectToAction(nameof(Index), "Home");
            }

            var appointmentSessions = await _apptSvc.GetAppointmentBookSessions(currentLoggedInUser, previousDate);
            if (appointmentSessions.ResultCode != ServiceResultStatusCode.Success)
            {
                _log.LogError($"Failed to load Appointment Session(s). \"ResultCode\"={appointmentSessions.ResultCode}");
                Alert("Failed to load Appointment Session(s).", AlertType.danger);
                return RedirectToAction(nameof(Index), "Home");
            }

            var appointmentBookViewModel = new AppointmentBookViewModel()
            {
                Date = previousDate,
                AppointmentSessions = appointmentSessions.AppointmentSessionDetails
            };

            return View(nameof(Index), appointmentBookViewModel);
        }

        [HttpPost("GetDay")]
        //[Authorize(Roles = "Reception, PracticeAdmin, MedicalPractitioner")]
        public async Task<IActionResult> GetDay(AppointmentBookViewModel model)
        {
            var currentLoggedInUser = GetLoggedInUserId();
            if (currentLoggedInUser < 1)
            {
                _log.LogError($"Authentication failure. Could not extract User's Id from Claims Principal.");
                Alert("Failed to identify current User for authorization", AlertType.danger);
                return RedirectToAction(nameof(Index), "Home");
            }

            var appointmentSessions = await _apptSvc.GetAppointmentBookSessions(currentLoggedInUser, model.Date);
            if (appointmentSessions.ResultCode != ServiceResultStatusCode.Success)
            {
                _log.LogError($"Failed to load Appointment Session(s). \"ResultCode\"={appointmentSessions.ResultCode}");
                Alert("Failed to load Appointment Session(s).", AlertType.danger);
                return RedirectToAction(nameof(Index), "Home");
            }

            var appointmentBookViewModel = new AppointmentBookViewModel()
            {
                Date = model.Date,
                AppointmentSessions = appointmentSessions.AppointmentSessionDetails
            };

            return View(nameof(Index), appointmentBookViewModel);
        }

        [HttpGet("BookAppointment")]
        //[Authorize(Roles = "Reception, PracticeAdmin")]
        public IActionResult BookAppointment(int slotId)
        {
            return View();
        }

        [HttpGet("CancelAppointment")]
        //[Authorize(Roles = "MedicalPractitioner")]
        public IActionResult CancelAppointment(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("CreateSession")]
        //[Authorize(Roles = "PracticeAdmin")]
        public IActionResult CreateSession()
        {

            return View();
        }

        [HttpGet("DeleteSession")]
        //[Authorize(Roles = "PracticeAdmin")]
        public IActionResult DeleteSession()
        {
            return View("Index");
        }
    }
}
