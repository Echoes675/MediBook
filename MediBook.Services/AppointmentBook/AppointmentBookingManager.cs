namespace MediBook.Services.AppointmentBook
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
    using MediBook.Core.Enums;
    using MediBook.Core.Models;
    using MediBook.Data.Repositories;
    using MediBook.Services.Enums;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The AppointmentBooking Manager
    /// </summary>
    public class AppointmentBookingManager : IAppointmentBookingManager
    {
        /// <summary>
        /// The Logger
        /// </summary>
        private readonly ILogger<AppointmentBookingManager> _log;

        /// <summary>
        /// The User Dal
        /// </summary>
        private readonly IUserDal _userDal;

        /// <summary>
        /// The AppointmentSlot Dal
        /// </summary>
        private readonly IAppointmentSlotDal _apptSlotDal;

        /// <summary>
        /// The Appointment Dal
        /// </summary>
        private readonly IAppointmentDal _apptDal;

        /// <summary>
        /// The Patient Dal
        /// </summary>
        private readonly IPatientDal _patientDal;

        /// <summary>
        /// The PatientsMedicalPractitioner Dal
        /// </summary>
        private readonly IPatientsMedicalPractitionerDal _patientsMedicalPractitionerDal;

        /// <summary>
        /// The AppointmentSession Dal
        /// </summary>
        private readonly IAppointmentSessionDal _sessionDal;

        /// <summary>
        /// Initializes an instance of the <see cref="AppointmentBookingManager"/>
        /// </summary>
        public AppointmentBookingManager(
            ILogger<AppointmentBookingManager> log, 
            IUserDal userDal, 
            IAppointmentSlotDal apptSlotDal, 
            IAppointmentDal apptDal, 
            IPatientDal patientDal, 
            IPatientsMedicalPractitionerDal patientsMedicalPractitionerDal,
            IAppointmentSessionDal sessionDal)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _userDal = userDal ?? throw new ArgumentNullException(nameof(userDal));
            _apptSlotDal = apptSlotDal ?? throw new ArgumentNullException(nameof(apptSlotDal));
            _apptDal = apptDal ?? throw new ArgumentNullException(nameof(apptDal));
            _patientDal = patientDal ?? throw new ArgumentNullException(nameof(patientDal));
            _patientsMedicalPractitionerDal = patientsMedicalPractitionerDal ?? throw new ArgumentNullException(nameof(patientsMedicalPractitionerDal));
            _sessionDal = sessionDal ?? throw new ArgumentNullException(nameof(sessionDal));
        }

        /// <summary>
        /// Gets the Appointment history for a given patient where the calling user is a MedicalPractitioner and was
        /// also associated with the appointment
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<AppointmentBookResults> GetPatientAppointmentHistory(int patientId, int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            // Get the account of the calling user so we can determine their role
            var callingUser = await _userDal.GetEntityAsync(userId);

            if (callingUser.JobDescription == null)
            {
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Failed
                };
            }

            if (callingUser.JobDescription.Role == UserRole.MedicalPractitioner)
            {
                var appointmentSlots = await
                    _sessionDal.GetPatientAppointmentSlotsAssociatedWithMedicalPractitionerSessions(userId, patientId);
                var appointmentDetails = appointmentSlots.Select(x => new AppointmentDetails(callingUser, x)).OrderByDescending(x => x.AppointmentDateTime).ToList();
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Success,
                    AppointmentsDetails = appointmentDetails
                };
            }

            return new AppointmentBookResults()
            {
                ResultCode = ServiceResultStatusCode.Failed
            };
        }

        /// <summary>
        /// Book a new appointment for a patient
        /// </summary>
        /// <returns></returns>
        public async Task<AppointmentBookResults> BookAppointmentAsync(AddOrUpdateAppointmentData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            // Get Appointment Slot
            var appointmentSlotTask = _apptSlotDal.GetEntityAsync(data.SlotId);

            // Get Medical Practitioner user account
            var medicalPractitionerTask = _userDal.GetEntityAsync(data.MedicalPractitionerId);

            // Get Patient details
            var patientTask = _patientDal.GetEntityAsync(data.PatientId);

            //Check PatientsMedicalPractitioners to see if this user is registered with this Medical Practitioner
            var patientMedicalPractitionerTask =
                _patientsMedicalPractitionerDal.CheckEntityExistsAsync(data.PatientId, data.MedicalPractitionerId);
            Task.WaitAll(appointmentSlotTask, medicalPractitionerTask, patientTask, patientMedicalPractitionerTask);

            // Get the results of the completed tasks
            var appointmentSlot = await appointmentSlotTask;
            if (appointmentSlot == null)
            {
                _log.LogError($"Unable to load AppointmentSlot for Appointment to be updated. \"AppointmentId\"={data.AppointmentId} \"PatientId\"={data.PatientId}, \"MedicalPractitioner\"={data.MedicalPractitionerId}");
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Failed
                };
            }

            var medicalPractitioner = await medicalPractitionerTask;
            if (medicalPractitioner == null)
            {
                _log.LogError($"Unable to load MedicalPractitioner for Appointment to be updated. \"AppointmentId\"={data.AppointmentId} \"PatientId\"={data.PatientId}, \"MedicalPractitioner\"={data.MedicalPractitionerId}");
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Failed
                };
            }

            var patient = await patientTask;
            if (patient == null)
            {
                _log.LogError($"Unable to load Patient for Appointment to be updated. \"AppointmentId\"={data.AppointmentId} \"PatientId\"={data.PatientId}, \"MedicalPractitioner\"={data.MedicalPractitionerId}");
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Failed
                };
            }

            var patientRegisteredWithMedicalPractitioner = await patientMedicalPractitionerTask;

            // Create a new Appointment and add it to the AppointmentSlot to make the booking
            appointmentSlot.Appointment = new Appointment()
            {
                Patient = patient
            };

            // update the status of the slot to pending patient arrival to indicate it is booked
            appointmentSlot.State = SlotState.Booked;

            var slotUpdateResultTask = _apptSlotDal.UpdateAsync(appointmentSlot);

            if (!patientRegisteredWithMedicalPractitioner)
            {
                await RegisterPatientIfNotRegisteredToMedicalPractitioner(data, patient, medicalPractitioner);
            }


            // Collect the result of the slot update task
            var slotUpdateResult = await slotUpdateResultTask;

            if (slotUpdateResult != null)
            {
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Success,
                    AppointmentsDetails = new List<AppointmentDetails>()
                    {
                        new AppointmentDetails(medicalPractitioner, slotUpdateResult)
                    }
                };
            }

            return new AppointmentBookResults()
            {
                ResultCode = ServiceResultStatusCode.Failed
            };
        }

        /// <summary>
        /// Update an existing appointment for a patient
        /// </summary>
        /// <returns></returns>
        public async Task<AppointmentBookResults> UpdateAppointmentAsync(AddOrUpdateAppointmentData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            // Get current Appointment Slot
            var currentAppointmentSlotTask = _apptSlotDal.FilterAsync(x => x.Id == data.SlotId);

            // Get the proposed new Appointment Slot
            var newAppointmentSlotTask = _apptSlotDal.GetEntityAsync(data.SlotId);

            // Get Medical Practitioner Employee Id
            var medicalPractitionerTask = _userDal.GetEntityAsync(data.MedicalPractitionerId);

            // Get Patient details
            var patientTask = _patientDal.GetEntityAsync(data.PatientId);

            //Check PatientsMedicalPractitioners to see if this user is registered with this Medical Practitioner
            var patientMedicalPractitionerTask =
                _patientsMedicalPractitionerDal.CheckEntityExistsAsync(data.PatientId, data.MedicalPractitionerId);
            Task.WaitAll(currentAppointmentSlotTask, newAppointmentSlotTask, medicalPractitionerTask, patientTask, patientMedicalPractitionerTask);

            // Get the results of the completed tasks
            var currentAppointmentSlotFiltered = await currentAppointmentSlotTask;
            var currentAppointmentSlot = currentAppointmentSlotFiltered.FirstOrDefault();
            if (currentAppointmentSlot == null)
            {
                _log.LogError($"Unable to load current AppointmentSlot for Appointment to be updated. \"AppointmentId\"={data.AppointmentId} \"PatientId\"={data.PatientId}, \"MedicalPractitioner\"={data.MedicalPractitionerId}");
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Failed
                };
            }

            var newAppointmentSlot = await newAppointmentSlotTask;
            if (newAppointmentSlot == null)
            {
                _log.LogError($"Unable to load proposed new AppointmentSlot for Appointment to be updated. \"AppointmentId\"={data.AppointmentId} \"PatientId\"={data.PatientId}, \"MedicalPractitioner\"={data.MedicalPractitionerId}");
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Failed
                };
            }

            var medicalPractitioner = await medicalPractitionerTask;
            if (medicalPractitioner == null)
            {
                _log.LogError($"Unable to load MedicalPractitioner for Appointment to be updated. \"AppointmentId\"={data.AppointmentId} \"PatientId\"={data.PatientId}, \"MedicalPractitioner\"={data.MedicalPractitionerId}");
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Failed
                };
            }

            var patient = await patientTask;
            if (patient == null)
            {
                _log.LogError($"Unable to load Patient for Appointment to be updated. \"AppointmentId\"={data.AppointmentId} \"PatientId\"={data.PatientId}, \"MedicalPractitioner\"={data.MedicalPractitionerId}");
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Failed
                };
            }

            var patientRegisteredWithMedicalPractitioner = await patientMedicalPractitionerTask;

            newAppointmentSlot.Appointment = currentAppointmentSlot.Appointment;
            newAppointmentSlot.State = SlotState.Booked;
            currentAppointmentSlot.Appointment = null;
            currentAppointmentSlot.State = SlotState.Available;

            var appointmentSlots = new List<AppointmentSlot>
            {
                newAppointmentSlot,
                currentAppointmentSlot,
            };

            // Save the list of updated appointment slots
            await _apptSlotDal.Update(appointmentSlots);

            // If patient not already Registered with MedicalPractitioner do so now
            if (!patientRegisteredWithMedicalPractitioner)
            {
                await RegisterPatientIfNotRegisteredToMedicalPractitioner(data, patient, medicalPractitioner);
            }

            return new AppointmentBookResults()
            {
                ResultCode = ServiceResultStatusCode.Success,
                AppointmentsDetails = new List<AppointmentDetails>()
                {
                    new AppointmentDetails(medicalPractitioner, newAppointmentSlot)
                }
            };
        }

        /// <summary>
        /// Cancel a booked appointment for a patient
        /// </summary>
        /// <returns></returns>
        public async Task<AppointmentBookResults> CancelAppointmentAsync(int slotId)
        {
            if (slotId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(slotId));
            }

            var slot = await _apptSlotDal.GetEntityAsync(slotId);

            if (slot != null)
            {
                return await CancelAppointmentAsync(slot);
            }

            return new AppointmentBookResults()
            {
                ResultCode = ServiceResultStatusCode.Failed
            };
        }

        /// <summary>
        /// Cancel a booked appointment for a patient
        /// </summary>
        /// <returns></returns>
        public async Task<AppointmentBookResults> CancelAppointmentAsync(AppointmentSlot slot)
        {
            if (slot == null)
            {
                throw new ArgumentNullException(nameof(slot));
            }

            if (await _apptDal.DeleteAsync(slot.AppointmentId))
            {
                // If the Appointment was marked as Cancelled/Deleted remove the association with the slot
                slot.Appointment = null;

                // Reset the slot status to Available
                slot.State = SlotState.Available;

                // Update the slot in the Db
                await _apptSlotDal.UpdateAsync(slot);

                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Success
                };
            }

            return new AppointmentBookResults()
            {
                ResultCode = ServiceResultStatusCode.Failed
            };
        }

        /// <summary>
        /// Returns a list of SelectListItems representing the appointment slots that
        /// are available to book for a given active Medical Practitioner from today (now) onwards
        /// </summary>
        /// <returns></returns>
        public async Task<AppointmentBookResults> GetMedicalPractitionerFreeSlotsSelectList(int medicalPractitionerUserId)
        {
            if (medicalPractitionerUserId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(medicalPractitionerUserId));
            }

            // Load the associated user account and ensure it is active and is for a Medical Practitioner
            var medicalPractitioner = await _userDal.GetEntityAsync(medicalPractitionerUserId);
            if (medicalPractitioner == null)
            {
                _log.LogWarning($"Failed to find MedicalPractitioner with matching Id. Can not build MedicalPractitioner free slots SelectList. \"UserId\"={medicalPractitionerUserId}");
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Failed
                };
            }

            if (medicalPractitioner.State != AccountState.Active)
            {
                _log.LogWarning($"MedicalPractitioner account is not Active. Can not build MedicalPractitioner free slots SelectList. \"UserId\"={medicalPractitionerUserId}, \"AccountState\"={medicalPractitioner.State}");
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Failed
                };
            }

            if (medicalPractitioner.JobDescription == null)
            {
                _log.LogWarning($"Unable to verify User account role. Can not build MedicalPractitioner free slots SelectList. \"UserId\"={medicalPractitionerUserId}");
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Failed
                };
            }

            if (medicalPractitioner.JobDescription.Role != UserRole.MedicalPractitioner)
            {
                _log.LogWarning($"User account role is not MedicalPractitioner. Can not build MedicalPractitioner free slots SelectList. \"UserId\"={medicalPractitionerUserId}, \"Role\"={medicalPractitioner.JobDescription.Role}");
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Failed
                };
            }

            // Get the medical practitioner's upcoming sessions
            var medicalPractitionerSessions = _sessionDal.Filter(x => x.StartDateTime.Date >= DateTime.UtcNow.Date);

            var freeSlots =
                medicalPractitionerSessions.SelectMany(x => x.AppointmentSlots
                    .Where(y => y.State == SlotState.Available))
                    .OrderByDescending(z => z.AppointmentDateTime);
            
            var selectListItems = freeSlots.Select(x =>
                new SelectListItem(x.AppointmentDateTime.ToString("dd/MM/yyyy HH:mm"), x.Id.ToString())).ToList();

            return new AppointmentBookResults()
            {
                ResultCode = ServiceResultStatusCode.Success,
                SelectList = selectListItems
            };
        }

        /// <summary>
        /// If the patient is not currently registered with this Medical Practitioner, create the new PatientsMedicalPractitioner link
        /// </summary>
        /// <param name="data"></param>
        /// <param name="patient"></param>
        /// <param name="medicalPractitioner"></param>
        /// <returns></returns>
        private async Task RegisterPatientIfNotRegisteredToMedicalPractitioner(
            AddOrUpdateAppointmentData data, Patient patient, User medicalPractitioner)
        {
            var patientMedicalPractitionerLink = new PatientsMedicalPractitioner()
            {
                Patient = patient,
                MedicalPractitioner = medicalPractitioner.EmployeeDetails
            };

            var newPatientMedicalPractitionerLink = await
                _patientsMedicalPractitionerDal.AddAsync(patientMedicalPractitionerLink);

            if (newPatientMedicalPractitionerLink == null)
            {
                _log.LogError(
                    $"Failed to register patient with Medical Practitioner. \"PatientId\"={data.PatientId}, \"MedicalPractitionerId\"={data.MedicalPractitionerId}");
            }
        }
    }
}