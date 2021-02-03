namespace MediBook.Web.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AppointmentBookController : ControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("BookAppointment")]
        public IActionResult BookAppointment(int id)
        {
            return View();
        }

        [HttpGet("PatientAppointments")]
        public IActionResult PatientAppointments(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return View();
        }
    }
}
