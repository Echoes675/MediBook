namespace MediBook.Web.Controllers
{
    using MediBook.Web.Enums;
    using Microsoft.AspNetCore.Mvc;

    // Enumeration of the different alert types

    /// <summary>
    /// ControllerBase provides alert functionality to the derived Controller classes
    /// </summary>
    public class ControllerBase : Controller
    {
        /// <summary>
        /// Set the details of the alert into TempData
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public void Alert(string message, AlertType type = AlertType.info)
        {
            TempData["Alert.Message"] = message;
            TempData["Alert.Type"] = type.ToString();
        }
    }
}
