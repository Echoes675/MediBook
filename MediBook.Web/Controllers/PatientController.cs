namespace MediBook.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
    using MediBook.Services.Enums;
    using MediBook.Services.PatientAdministration;
    using MediBook.Services.PatientRecord;
    using MediBook.Web.Enums;
    using MediBook.Web.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The <see cref="PatientController"/>
    /// </summary>
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _log;
        private readonly IPatientAdministrationService _patientSvc;

        /// <summary>
        /// Initializes an instance of the <see cref="PatientController"/>
        /// </summary>
        public PatientController(ILogger<PatientController> log, IPatientAdministrationService patientSvc)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _patientSvc = patientSvc ?? throw new ArgumentNullException(nameof(patientSvc));
        }

        [HttpGet]
        //[Authorize(Roles = "Reception, PracticeAdmin, MedicalPractitioner")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Search")]
        //[Authorize(Roles = "Reception, PracticeAdmin, MedicalPractitioner")]
        public IActionResult Search()
        {
            return View(new PatientSearchViewModel());
        }

        [HttpPost("Search")]
        //[Authorize(Roles = "Reception, PracticeAdmin, MedicalPractitioner")]
        public async Task<IActionResult> Search([FromForm] PatientSearchViewModel searchViewModel)
        {
            if (searchViewModel == null)
            {
                throw new ArgumentNullException(nameof(searchViewModel));
            }

            var searchCriteria = searchViewModel.SearchCriteria ?? throw new ArgumentNullException(nameof(searchViewModel.SearchCriteria));
            
            var searchResults = await _patientSvc.PerformPatientSearchAsync(searchCriteria);

            if (searchResults == null)
            {
                var message = "Patient search encountered an error.";
                _log.LogError(message);
                Alert(message, AlertType.danger);
                return RedirectToAction(nameof(Index), "Patient");
            }

            searchViewModel.SearchResults = searchResults;

            return View(searchViewModel);
        }

        [HttpGet("Details")]
        //[Authorize(Roles = "Reception, PracticeAdmin, MedicalPractitioner")]
        public async Task<IActionResult> Details(int id)
        {
            if (id < 1)
            {
                _log.LogWarning($"Invalid Id passed to Patient/Details view. \"id\"{id}");
                Alert("Failed to load Patient Details due to invalid Id", AlertType.danger);
                return RedirectToAction(nameof(Index), "Patient");
            }

            var userId = GetLoggedInUserId();
            if (userId < 1)
            {
                _log.LogError($"Authentication failure. Could not extract User's Id from Claims Principal.");
                Alert("Failed to load logged in User for authorization", AlertType.danger);
                return RedirectToAction(nameof(Index), "Patient");
            }

            var patientDetails = await _patientSvc.GetPatientDetailsAsync(id, userId);
            if (patientDetails != null)
            {
                return View(patientDetails);
            }

            _log.LogInformation($"Not able to locate Patient Details. \"id\"{id}");
            Alert("Failed to load Patient Details", AlertType.warning);
            return RedirectToAction(nameof(Index), "Patient");
        }

        [HttpGet("Register")]
        //[Authorize(Roles = "Reception, PracticeAdmin")]
        public IActionResult Register()
        {
            return View(new PatientForRegistration());
        }

        [HttpPost("Register")]
        //[Authorize(Roles = "Reception, PracticeAdmin")]
        public async Task<IActionResult> Register([FromForm] PatientForRegistration newPatientDetails)
        {
            if (newPatientDetails == null)
            {
                Alert("Failed to receive new patient details for registration");
                throw new ArgumentNullException(nameof(newPatientDetails));
            }

            // Send the newPatientDetails to the Patient service to be processed
            var result = await _patientSvc.RegisterPatientAsync(newPatientDetails);

            if (result != null)
            {
                Alert(
                    "New patient registered successfully. " +
                    $"Name={newPatientDetails.Firstname + " " + newPatientDetails.Lastname}. PatientId={result.Id}",
                    AlertType.success);
                return View("Details", result);
            }

            Alert(
                $"Failed to register new patient. Name={newPatientDetails.Firstname + " " + newPatientDetails.Lastname}", 
                AlertType.danger);
            return View("Register", newPatientDetails);
        }

        [HttpGet("Edit")]
        //[Authorize(Roles = "Reception, PracticeAdmin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id < 1)
            {
                _log.LogWarning($"Invalid Id passed to Patient/Edit view. \"id\"{id}");
                Alert("Failed to load Patient Details due to invalid Id", AlertType.danger);
                return RedirectToAction(nameof(Index), "Patient");
            }

            var userId = GetLoggedInUserId();
            if (userId < 1)
            {
                _log.LogError($"Authentication failure. Could not extract User's Id from Claims Principal.");
                Alert("Failed to identify current User for authorization", AlertType.danger);
                return RedirectToAction(nameof(Index), "Patient");
            }

            var patientDetails = await _patientSvc.GetPatientDetailsAsync(id, userId);

            if (patientDetails != null)
            {
                return View(patientDetails);
            }

            _log.LogInformation($"Not able to locate Patient Details. \"id\"{id}");
            Alert("Failed to load Patient Details", AlertType.warning);
            return RedirectToAction(nameof(Index), "Patient");
        }

        [HttpPost("Edit")]
        //[Authorize(Roles = "Reception, PracticeAdmin")]
        public async Task<IActionResult> Edit([FromForm] PatientDetailsDto updatedPatientDetails)
        {
            if (updatedPatientDetails == null)
            {
                Alert("Failed to receive new patient details for patient update");
                throw new ArgumentNullException(nameof(updatedPatientDetails));
            }

            // Send the newPatientDetails to the Patient service to be processed
            var result = await _patientSvc.UpdatePatientDetailsAsync(updatedPatientDetails);

            if (result == ServiceResultStatusCode.Success)
            {
                Alert(
                    "Patient details updated successfully. " +
                    $"Name={updatedPatientDetails.Firstname + " " + updatedPatientDetails.Lastname}. PatientId={updatedPatientDetails.Id}",
                    AlertType.success);
                return View("Details", updatedPatientDetails);
            }

            Alert(
                $"Failed to save updated patient details. Name={updatedPatientDetails.Firstname + " " + updatedPatientDetails.Lastname}",
                AlertType.danger);
            return View("Edit", updatedPatientDetails);
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