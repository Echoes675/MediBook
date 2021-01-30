namespace MediBook.Core.Enums
{
    /// <summary>
    /// The Appointment State
    /// </summary>
    public enum AppointmentState
    {
        PendingPatientArrival = 0,
        PatientArrived = 1,
        PatientBeingSeen = 2,
        Completed = 3,
        DidNotArrive = 4,
        Cancelled = 5
    }
}