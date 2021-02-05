namespace MediBook.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using MediBook.Core.Enums;
    using MediBook.Web.Enums;
    using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Extract the logged in user's Id from the Claims Principal
        /// </summary>
        /// <returns></returns>
        protected int GetLoggedInUserId()
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

        /// <summary>
        /// Extract the logged in user's Role from the Claims Principal
        /// </summary>
        /// <returns></returns>
        protected UserRole GetLoggedInUserRole()
        {
            // extracting the custom user claim here
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity?.Claims.FirstOrDefault(x => x.Type == "Role");

            // extract user id from claim
            var roleString = claim != null ? claim.Value : string.Empty;


            if (Enum.TryParse<UserRole>(roleString, out var userRole))
            {
                return userRole;
            }

            // Failed to parse the roleString. Return Unknown
            return UserRole.Unknown;
        }
    }
}
