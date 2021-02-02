namespace MediBook.Services.UserAuthentication
{
    using System.Security.Claims;
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
        /// <param name="claimsPrincipal"></param>
        public UserLoginResult(ServiceResultStatusCode resultStatus, string message, ClaimsPrincipal claimsPrincipal)
        {
            ResultStatus = resultStatus;
            Message = message;
            ClaimsPrincipal = claimsPrincipal;
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
        /// The ClaimsPrincipal
        /// </summary>
        public ClaimsPrincipal ClaimsPrincipal { get; }
    }
}