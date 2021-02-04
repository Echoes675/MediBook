namespace MediBook.Web.TagHelpers
{
    using MediBook.Core.Enums;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    /// <summary>
    /// The AppointmentSlot text tag helper
    /// </summary>
    [HtmlTargetElement("asp-appointment-slot-icon")]
    public class AppointmentSlotIconTagHelper : TagHelper
    {
        /// <summary>
        /// The Appointment Slot
        /// </summary>
        public SlotState State { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "i";
            string colour;
            string icon;
            string title;
            if (State == SlotState.Available)
            {
                colour = "rgba-blue-grey-strong";
                icon = "fa-calendar-plus";
                title = "Book Appointment";
            }
            else
            {
                colour = "rgba-red-strong";
                icon = "fa-calendar-times";
                title = "Cancel Appointment";
            }

            output.Attributes.SetAttribute("class", $"far {icon} {colour} fa-lg");
            output.Attributes.SetAttribute("title", title);
            output.Content.SetContent($"");

        }
    }
}