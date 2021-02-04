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
            var status = Slot.State;
            var patientFirstName = Slot.Appointment.Patient.Firstname;
            var patientLastName = Slot.Appointment.Patient.Lastname;

            var slotTextSB = new StringBuilder();
            slotTextSB.Append(time + " - ");

            if (status == SlotState.Available)
            {
                slotTextSB.Append("Available");
            }
            else
            {
                slotTextSB.Append(patientLastName + ", ");
                slotTextSB.Append(patientFirstName);
            }

            output.TagName = "p";
            output.Content.SetContent(slotTextSB.ToString());
        }
    }
}