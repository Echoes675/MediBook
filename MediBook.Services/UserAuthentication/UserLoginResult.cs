namespace MediBook.Services.UserAuthentication
{
    using MediBook.Core.DTOs;
    using MediBook.Services.Enums;

    /// <summary>
    /// Result of the login attempt
    /// </summary>
    public class UserLoginResult
    {
        /// <summary>
        /// Initializes an instance of the <see cref="UserLoginResult"/> class
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <param name="message"></param>
        /// <param name="userAccountDetails"></param>
        public UserLoginResult(ServiceResultStatusCode resultStatus, string message, UserAccountDetailsDto userAccountDetails)
        {
            ResultStatus = resultStatus;
            Message = message;
            UserAccountDetails = userAccountDetails;
        }

        /// <summary>
        /// The Service Result StatusCode
        /// </summary>
        public ServiceResultStatusCode ResultStatus { get; }

        /// <summary>
        /// The Message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// The UserAccountDetails
        /// </summary>
        public UserAccountDetailsDto UserAccountDetails { get; }

        /// <summary>
        /// The PatientId if the user is a PatientUser
        /// </summary>
        public int PatientAccountPatientId { get; set; }
    }
}