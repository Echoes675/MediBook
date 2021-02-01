namespace MediBook.Services.Test.UserAuth
{
    using System;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using MediBook.Core.Enums;
    using MediBook.Core.Models;
    using MediBook.Data.Repositories;
    using MediBook.Services.Cryptography;
    using MediBook.Services.UserAuthentication;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UserAuthenticationServiceTests
    {
        [Test]
        public void Ctor_NullDatabaseContext_ThrowsArgumentNullException()
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
        public void Login_UserNotFound_ReturnsNull()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            mockDal.GetUserAsync("username").Returns((User)null);

            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var result = svc.Login("username", "password").GetAwaiter().GetResult();
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Login_UserFoundButPasswordIncorrect_ReturnsNull()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            mockDal.GetUserAsync("username").Returns(new User());
            mockCryptoSvc.VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "wrongPassword").Returns(false);

            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var result = svc.Login("username", "wrongPassword").GetAwaiter().GetResult();
            Assert.That(result, Is.Null);
            mockCryptoSvc.Received().VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "wrongPassword");
        }

        [Test]
        public void Login_UserFoundHasNullUsername_ReturnsNull()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            mockDal.GetUserAsync("username").Returns(new User());
            mockCryptoSvc.VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "rightPassword").Returns(true);

            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var result = svc.Login("username", "rightPassword").GetAwaiter().GetResult();

            Assert.That(result, Is.Null);
            mockCryptoSvc.Received().VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "rightPassword");
        }

        [Test]
        public void Login_UserFoundHasEmptyUsername_ReturnsNull()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            mockDal.GetUserAsync("username").Returns(new User(){Username = string.Empty});
            mockCryptoSvc.VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "rightPassword").Returns(true);

            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var result = svc.Login("username", "rightPassword").GetAwaiter().GetResult();

            Assert.That(result, Is.Null);
            mockCryptoSvc.Received().VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "rightPassword");
        }

        [Test]
        public void Login_UserFoundHasIdLessThan1_ReturnsNull()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var user = new User()
            {
                Id = 0,
                Username = "name"
            };

            mockDal.GetUserAsync("username").Returns(user);
            mockCryptoSvc.VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "rightPassword").Returns(true);

            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var result = svc.Login("username", "rightPassword").GetAwaiter().GetResult();

            Assert.That(result, Is.Null);
            mockCryptoSvc.Received().VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "rightPassword");
        }

        [Test]
        public void Login_UserFoundHasNullJobDescription_ReturnsNull()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAuthenticationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var user = new User()
            {
                Id = 1,
                Username = "name",
                JobDescription = null
            };

            mockDal.GetUserAsync("username").Returns(user);
            mockCryptoSvc.VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "rightPassword").Returns(true);

            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var result = svc.Login("username", "rightPassword").GetAwaiter().GetResult();

            Assert.That(result, Is.Null);
            mockCryptoSvc.Received().VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "rightPassword");
        }

        [Test]
        public void Login_UserFoundHasUnknownRole_ReturnsNull()
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
                }
            };

            mockDal.GetUserAsync("username").Returns(user);
            mockCryptoSvc.VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "rightPassword").Returns(true);

            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var result = svc.Login("username", "rightPassword").GetAwaiter().GetResult();

            Assert.That(result, Is.Null);
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
                Username = "name",
                JobDescription = new JobDescription()
                {
                    Description = "General Practitioner",
                    Role = UserRole.MedicalPractitioner
                }
            };

            mockDal.GetUserAsync("username").Returns(user);
            mockCryptoSvc.VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "rightPassword").Returns(true);

            var svc = new UserAuthenticationService(mockDal, mockLogger, mockCryptoSvc);

            var result = svc.Login("username", "rightPassword").GetAwaiter().GetResult();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ClaimsPrincipal>());
        }
    }
}