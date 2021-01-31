namespace MediBook.Services.UserAuth
{
    using System;
    using Microsoft.Extensions.Logging;

    public class UserAuthService
    {
        private ILogger _log;

        public UserAuthService(ILogger logger)
        {
            _log = logger ?? throw new ArgumentNullException(nameof(logger));
        }

    }
}