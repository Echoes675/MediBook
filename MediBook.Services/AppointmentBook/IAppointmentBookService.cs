﻿namespace MediBook.Services.AppointmentBook
{
    using System;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;

    public interface IAppointmentBookService
    {
        /// <summary>
        /// Creates and saves a new Appointment Session and Appointment Slots per the config specification
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        Task<AppointmentBookResults> CreateNewAppointmentSession(AppointmentSessionConfiguration config);

        /// <summary>
        /// Gets all the AppointmentBookSessions on a given day. If the calling user is a MedicalPractitioner they
        /// will only see their own sessions only. If they are Reception or Practice Admin they will see the
        /// appointment books for all staff
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<AppointmentBookResults> GetAppointmentBookSessions(int userId, DateTime date);

        /// <summary>
        /// Gets the AppointmentBookSessions showing only free slots on a given day.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<AppointmentBookResults> GetAppointmentBookSessionsFreeSlots(DateTime date);

        /// <summary>
        /// Cancels a given AppointmentBook Session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        Task<AppointmentBookResults> CancelAppointmentBookSession(int sessionId);

        /// <summary>
        /// Gets the Appointment history for a given patient where the calling user is a MedicalPractitioner and was
        /// also associated with the appointment
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<AppointmentBookResults> GetPatientAppointmentHistory(int patientId, int userId);

        /// <summary>
        /// Book a new appointment for a patient
        /// </summary>
        /// <returns></returns>
        Task<AppointmentBookResults> BookAppointmentAsync(AddOrUpdateAppointmentData data);

        /// <summary>
        /// Update an existing appointment for a patient
        /// </summary>
        /// <returns></returns>
        Task<AppointmentBookResults> UpdateAppointmentAsync(AddOrUpdateAppointmentData data);

        /// <summary>
        /// Cancel a booked appointment for a patient
        /// </summary>
        /// <returns></returns>
        Task<AppointmentBookResults> CancelAppointmentAsync(int slotId);

        /// <summary>
        /// Returns a list of SelectListItems representing the active Medical Practitioners
        /// </summary>
        /// <returns></returns>
        Task<AppointmentBookResults> GetMedicalPractitionerSelectList();

        /// <summary>
        /// Returns a list of SelectListItems representing the appointment slots that
        /// are available to book for a given active Medical Practitioner from today (now) onwards
        /// </summary>
        /// <returns></returns>
        Task<AppointmentBookResults> GetMedicalPractitionerFreeSlotsSelectList(int slotId);
    }
}