namespace MediBook.Services.Test.UserAuth
{
    using System;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using MediBook.Core.Enums;
    using MediBook.Core.Models;
    using MediBook.Data.Repositories;
    using MediBook.Services.Cryptography;
    using MediBook.Services.Enums;
    using MediBook.Services.UserAuthentication;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UserAuthenticationServiceTests
    {
        [Test]
        public void Ctor_NullUserDal_ThrowsArgumentNullException()
        {
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();
            var e = Assert.Throws<ArgumentNullException>(() => new UserAuthenticationService(null, mockLogger, mockCryptoSvc));
            Assert.That(e.Message, Does.Contain("userDal"));
        }

        [Test]
        public void Ctor_NullLogger_ThrowsArgumentNullException()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();
            var e = Assert.Throws<ArgumentNullException>(() => new UserAuthenticationService(mockDal, null, mockCryptoSvc));
            Assert.That(e.Message, Does.Contain("logger"));
        }

        [Test]
        public void Ctor_NullCryptographyService_ThrowsArgumentNullException()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var e = Assert.Throws<ArgumentNullException>(() => new UserAuthenticationService(mockDal, mockLogger, null));
            Assert.That(e.Message, Does.Contain("cryptographyService"));
        }

        [Test]
        public void Login_UsernameIsNull_ThrowsArgumentNullException()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();
            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var e = Assert.ThrowsAsync<ArgumentNullException>(() => svc.Login(null, "password"));
            Assert.That(e.Message, Does.Contain("username"));
        }

        [Test]
        public void Login_UsernameIsEmpty_ThrowsArgumentNullException()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();
            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var e = Assert.ThrowsAsync<ArgumentNullException>(() => svc.Login(string.Empty, "password"));
            Assert.That(e.Message, Does.Contain("username"));
        }

        [Test]
        public void Login_PasswordIsNull_ThrowsArgumentNullException()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();
            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var e = Assert.ThrowsAsync<ArgumentNullException>(() => svc.Login("username", null));
            Assert.That(e.Message, Does.Contain("password"));
        }

        [Test]
        public void Login_PasswordIsEmpty_ThrowsArgumentNullException()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();
            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var e = Assert.ThrowsAsync<ArgumentNullException>(() => svc.Login("username", string.Empty));
            Assert.That(e.Message, Does.Contain("password"));
        }

        [Test]
        public void Login_UserNotFound_ReturnsFailedResult()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            mockDal.GetUserAsync("username").Returns((User)null);

            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var result = svc.Login("username", "password").GetAwaiter().GetResult();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ResultStatus, Is.EqualTo(ServiceResultStatusCode.NotFound));
            Assert.That(result.Message, Does.Contain("User not found. \"Username\"=username"));
        }

        [Test]
        public void Login_UserFoundButAccountNotActive_ReturnsFailedResult()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var mockUser = new User()
            {
                State = AccountState.Deleted
            };
            mockDal.GetUserAsync("username").Returns(mockUser);
            mockCryptoSvc.VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "wrongPassword").Returns(false);

            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var result = svc.Login("username", "wrongPassword").GetAwaiter().GetResult();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ResultStatus, Is.EqualTo(ServiceResultStatusCode.Failed));
            Assert.That(result.Message, Does.Contain("Account login failed. Account state is not active "));
        }

        [Test]
        public void Login_UserFoundButPasswordIncorrect_ReturnsFailedResult()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var mockUser = new User()
            {
                Username = "jsmith",
                State = AccountState.Active
            };
            mockDal.GetUserAsync("username").Returns(mockUser);
            mockCryptoSvc.VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "wrongPassword").Returns(false);

            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var result = svc.Login("username", "wrongPassword").GetAwaiter().GetResult();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ResultStatus, Is.EqualTo(ServiceResultStatusCode.Failed));
            Assert.That(result.Message, Does.Contain("Account login failed. Username or password not recognised."));
        }

        [Test]
        public void Login_UserFoundHasUnknownRole_ReturnsFailedResult()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var user = new User()
            {
                Id = 1,
                Username = "name",
                JobDescription = new JobDescription()
                {
                    Description = "General Practitioner",
                    Role = UserRole.Unknown
                },
                State = AccountState.Active,
                PasswordHash = new byte[8],
                PasswordSalt = new byte[8],
            };

            mockDal.GetUserAsync("username").Returns(user);
            mockCryptoSvc.VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "rightPassword").Returns(true);

            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var result = svc.Login("username", "rightPassword").GetAwaiter().GetResult();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ResultStatus, Is.EqualTo(ServiceResultStatusCode.Failed));
            Assert.That(result.Message, Does.Contain("Account login failed. Internal error."));
            mockCryptoSvc.Received().VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "rightPassword");
        }

        [Test]
        public void Login_UserFoundHasValidDetails_ReturnsClaimsPrincipal()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var user = new User()
            {
                Id = 1,
                Username = "username",
                JobDescription = new JobDescription()
                {
                    Description = "General Practitioner",
                    Role = UserRole.MedicalPractitioner
                },
                State = AccountState.Active,
                PasswordHash = new byte[8],
                PasswordSalt = new byte[8],
            };

            mockDal.GetUserAsync("username").Returns(user);
            mockCryptoSvc.VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "rightPassword").Returns(true);

            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var result = svc.Login("username", "rightPassword").GetAwaiter().GetResult();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ResultStatus, Is.EqualTo(ServiceResultStatusCode.Success));
            Assert.That(result.ClaimsPrincipal, Is.Not.Null);
        }
    }
}