namespace MediBook.Web.TagHelpers
{
    using System.Text;
    using MediBook.Core.Enums;
    using MediBook.Core.Models;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    /// <summary>
    /// The AppointmentSlot text tag helper
    /// </summary>
    [HtmlTargetElement("asp-appointment-slot-text")]
    public class AppointmentSlotTextTagHelper : TagHelper
    {
        /// <summary>
        /// The Appointment Slot
        /// </summary>
        public AppointmentSlot Slot { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var time = Slot.AppointmentDateTime.TimeOfDay.ToString("HH:mm");
            var slotTextSB = new StringBuilder();
            slotTextSB.Append(time + " - ");

            if (Slot.State == SlotState.Available)
            {
                var patientFirstName = Slot.Appointment.Patient.Firstname;
                var patientLastName = Slot.Appointment.Patient.Lastname;

                slotTextSB.Append(patientLastName + ", ");
                slotTextSB.Append(patientFirstName);

                output.TagName = "p";
                output.Content.SetContent(slotTextSB.ToString());
            }
            else
            {
                slotTextSB.Append("Available");
            }

            output.TagName = "p";
            output.Content.SetContent(slotTextSB.ToString());
        }
    }
}