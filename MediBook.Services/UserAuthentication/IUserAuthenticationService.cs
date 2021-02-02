namespace MediBook.Services.UserAuthentication
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IUserAuthenticationService
    {
        /// <summary>
        /// Method to process login functionality
        /// </summary>
        /// <param name = "username" > String of the username</param>
        /// <param name = "password" > String of the submitted password</param>
        /// <returns>User object if authentication successful</returns>
        Task<UserLoginResult> Login(string username, string password);
    }
}