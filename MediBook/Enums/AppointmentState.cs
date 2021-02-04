namespace MediBook.Core.Enums
{
    /// <summary>
    /// The appointment state
    /// </summary>
    public enum AppointmentState
    {
        Unknown = 0,
        PendingPatientArrival = 1,
        PatientArrived = 2,
        PatientBeingSeen = 3,
        Completed = 4,
        DidNotArrive = 5,
        Cancelled = 6
    }
}