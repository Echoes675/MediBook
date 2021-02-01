namespace MediBook.Services.Test.UserAuth
{
    using System;
    using MediBook.Data.Repositories;
    using MediBook.Services.UserAuth;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UserAuthServiceTests
    {
        [Test]
        public void Ctor_NullDatabaseContext_ThrowsArgumentNullException()
        {
            var mockLogger = Substitute.For<ILogger<JobDescriptionDal>>();
            var e = Assert.Throws<ArgumentNullException>(() => new UserAuthService(null, mockLogger));
            Assert.That(e.Message, Does.Contain("userDal"));
        }

        [Test]
        public void Ctor_NullLogger_ThrowsArgumentNullException()
        {
            var mockDal = Substitute.For<IUserDal>();
            var e = Assert.Throws<ArgumentNullException>(() => new UserAuthService(mockDal, null));
            Assert.That(e.Message, Does.Contain("logger"));
        }
    }
}