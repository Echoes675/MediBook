namespace MediBook.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using MediBook.Core.Enums;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class SelectListHelpers
    {
        /// <summary>
        /// Gets the list of SelectListItems based on the Roles enum for the drop down
        /// </summary>
        /// <param name="selectedRole"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetRolesSelectList(UserRole selectedRole = UserRole.Unknown)
        {
            return Enum.GetValues(typeof(UserRole))
                .Cast<UserRole>()
                .Select(v => new SelectListItem
                {
                    Selected = (v == selectedRole),
                    Text = v.ToString(),
                    Value = ((int) v).ToString(CultureInfo.InvariantCulture)
                }).ToList();
        }

        /// <summary>
        /// Gets the list of SelectListItems based on the AccountState enum for the drop down
        /// </summary>
        /// <param name="selectedState"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetAccountStatesSelectList(AccountState selectedState)
        {
            return Enum.GetValues(typeof(AccountState))
                .Cast<AccountState>()
                .Select(v => new SelectListItem
                {
                    Selected = (v == selectedState),
                    Text = v.ToString(),
                    Value = ((int)v).ToString(CultureInfo.InvariantCulture)
                }).ToList();
        }

        /// <summary>
        /// Gets the list of SelectListItems based on the Title enum for the drop down
        /// </summary>
        /// <param name="selectedTitle"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetTitlesSelectList(Title selectedTitle)
        {
            return Enum.GetValues(typeof(Title))
                .Cast<Title>()
                .Select(v => new SelectListItem
                {
                    Selected = (v == selectedTitle),
                    Text = v.ToString(),
                    Value = ((int)v).ToString(CultureInfo.InvariantCulture)
                }).ToList();
        }
    }
}