namespace MediBook.Web.Controllers
{
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
    }
}
