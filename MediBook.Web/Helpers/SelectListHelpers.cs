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
    }
}