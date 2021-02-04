namespace MediBook.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
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

        [HttpPost("GetSessionsForDay")]
        //[Authorize(Roles = "Reception, PracticeAdmin, MedicalPractitioner")]
        public async Task<IActionResult> GetSessionsForDay(AppointmentBookViewModel model)
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

        [HttpGet("CancelAppointment")]
        //[Authorize(Roles = "Reception,PracticeAdmin")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var result = await _apptSvc.CancelAppointmentAsync(id);

            if (result.ResultCode != ServiceResultStatusCode.Success)
            {
                _log.LogError($"Failed to cancel Appointment. \"SlotId\"={id}");
                Alert("Failed to cancel Appointment.", AlertType.danger);
                return RedirectToAction(nameof(Index));
            }

            Alert("Successfully cancelled Appointment.", AlertType.success);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("CreateSession")]
        //[Authorize(Roles = "PracticeAdmin")]
        public IActionResult CreateSession()
        {
            return View(new CreateAppointmentSessionViewModel());
        }

        [HttpPost("CreateSession")]
        //[Authorize(Roles = "PracticeAdmin")]
        public async Task<IActionResult> CreateSession([FromForm] CreateAppointmentSessionViewModel createSessionDetails)
        {
            if (createSessionDetails == null)
            {
                _log.LogError($"Failed to create Appointment Session. No Create Session Details received.");
                Alert("Failed to create Appointment Session due to an internal error.", AlertType.danger);
                return RedirectToAction(nameof(Index));
            }

            // Round up the session duration to the next 5 mins
            var validSessionDuration = createSessionDetails.DurationInMins;
            if (validSessionDuration % 5 != 0)
            {
                var remainder = validSessionDuration % 5;

                validSessionDuration = remainder >= 3 ? 
                    validSessionDuration + (5 - remainder) : 
                    validSessionDuration - remainder;
            }

            var newSessionConfig = new AppointmentSessionConfiguration()
            {
                DurationInMins = validSessionDuration,
                StartDateTime = createSessionDetails.StartTime,
                NumberOfAppointmentSlots = createSessionDetails.NumberOfAppointmentSlots,
                MedicalPractitionerId = createSessionDetails.MedicalPractitionerId
            };

            var result = await _apptSvc.CreateNewAppointmentSession(newSessionConfig);
            if (result.ResultCode != ServiceResultStatusCode.Success)
            {
                _log.LogError($"Failed to create Appointment Session.");
                Alert("Failed to create Appointment Session.", AlertType.danger);
                return RedirectToAction(nameof(Index));
            }

            Alert("Successfully created Appointment Session.", AlertType.success);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("DeleteSession")]
        //[Authorize(Roles = "PracticeAdmin")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            if (id <= 0)
            {
                _log.LogError($"Failed to delete Appointment Session. \"SessionId\"={id}");
                Alert("Failed to delete Appointment Session.", AlertType.danger);
                return RedirectToAction(nameof(Index));
            }

            var result = await _apptSvc.CancelAppointmentBookSession(id);
            if (result.ResultCode != ServiceResultStatusCode.Success)
            {
                _log.LogError($"Failed to delete Appointment Session. \"SessionId\"={id}");
                Alert("Failed to delete Appointment Session.", AlertType.danger);
                return RedirectToAction(nameof(Index));
            }

            Alert("Successfully deleted Appointment Session.", AlertType.success);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("BookAppointment")]
        //[Authorize(Roles = "Reception, PracticeAdmin")]
        public IActionResult BookAppointment(int slotId)
        {
            return View();
        }
    }
}
