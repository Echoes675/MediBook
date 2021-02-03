namespace MediBook.Web.Controllers
{
    using MediBook.Web.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using Microsoft.Extensions.Logging;
    public class HomeController : ControllerBase
    {
        private readonly ILogger _log;

        public HomeController(ILogger<HomeController> logger)
        {
            _log = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
