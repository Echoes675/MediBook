namespace MediBook.Services.Test.UserAdministration
{
    using System;
    using System.Collections.Generic;
    using MediBook.Core.DTOs;
    using MediBook.Core.Enums;
    using MediBook.Core.Models;
    using MediBook.Data.Repositories;
    using MediBook.Services.Cryptography;
    using MediBook.Services.Enums;
    using MediBook.Services.UserAdministration;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UserAdministrationServiceTests
    {
        [Test]
        public void Ctor_NullUserDal_ThrowsArgumentNullException()
        {
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();
            var e = Assert.Throws<ArgumentNullException>(() => new UserAdministrationService(mockLogger, null, mockCryptoSvc));
            Assert.That(e.Message, Does.Contain("userDal"));
        }

        [Test]
        public void Ctor_NullLogger_ThrowsArgumentNullException()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();
            var e = Assert.Throws<ArgumentNullException>(() => new UserAdministrationService(null, mockDal, mockCryptoSvc));
            Assert.That(e.Message, Does.Contain("logger"));
        }

        [Test]
        public void Ctor_NullCryptographyService_ThrowsArgumentNullException()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var e = Assert.Throws<ArgumentNullException>(() => new UserAdministrationService(mockLogger, mockDal, null));
            Assert.That(e.Message, Does.Contain("cryptographyService"));
        }

        [Test]
        public void LoadUserAccounts_UserDalReturnsTwoUsers_ReturnsTwoUserAccountDetailsDto()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();
            var mockJobDescription = new JobDescription()
            {
                Description = "Head Honcho",
                Role = UserRole.MedicalPractitioner
            };

            var mockEmployeeDetails = new Employee()
            {
                Firstname = "Aoife",
                Lastname = "McGonagle",
                Title = Title.Miss,
            };

            var mockUser = new User()
            {
                EmployeeDetails = mockEmployeeDetails,
                Username = "amcg",
                JobDescription = mockJobDescription,
                State = AccountState.Active
            };

            var users = new List<User>
            {
                mockUser,
                mockUser
            };

            mockDal.GetAllAsync().Returns(users);

            var adminSvc = new UserAdministrationService(mockLogger, mockDal, mockCryptoSvc);

            var result = adminSvc.LoadUserAccounts().GetAwaiter().GetResult();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void CreateUserAsync_UserForRegistrationDtoNull_ThrowsArgumentNullException()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var adminSvc = new UserAdministrationService(mockLogger, mockDal, mockCryptoSvc);

            var e = Assert.Throws<ArgumentNullException>(() => adminSvc.CreateUserAsync(null).GetAwaiter().GetResult());
            Assert.That(e.Message, Does.Contain("newUserDetails"));
        }

        [Test]
        public void CreateUserAsync_UserForRegistrationDtoValid_ReturnsUserFullDetailsDto()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var adminSvc = new UserAdministrationService(mockLogger, mockDal, mockCryptoSvc);

            var mockRegistration = new UserForRegistrationDto()
            {
                Title = Title.Miss,
                FirstName = "Aoife",
                LastName = "McGonagle",
                Username = "amcg",
                Password = "password123",
                ConfirmPassword = "password123",
                JobDescription = "Head Honcho",
                Role = UserRole.MedicalPractitioner,
                State = AccountState.Active
            };

            var mockJobDescription = new JobDescription()
            {
                Description = "Head Honcho",
                Role = UserRole.MedicalPractitioner
            };

            var mockEmployeeDetails = new Employee()
            {
                Firstname = "Aoife",
                Lastname = "McGonagle",
                Title = Title.Miss,
            };

            var mockUser = new User()
            {
                EmployeeDetails = mockEmployeeDetails,
                Username = "amcg",
                JobDescription = mockJobDescription,
                State = AccountState.Active
            };

            mockDal.AddAsync(Arg.Any<User>()).Returns(mockUser);

            var result =  adminSvc.CreateUserAsync(mockRegistration).GetAwaiter().GetResult();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo(Title.Miss));
            Assert.That(result.Firstname, Is.EqualTo("Aoife"));
            Assert.That(result.Lastname, Is.EqualTo("McGonagle"));
            Assert.That(result.Username, Is.EqualTo("amcg"));
            Assert.That(result.JobDescription, Is.EqualTo("Head Honcho"));
            Assert.That(result.Role, Is.EqualTo(UserRole.MedicalPractitioner));
            Assert.That(result.State, Is.EqualTo(AccountState.Active));
        }

        [Test]
        public void GetUserFullDetailsAsync_IdLessThanOne_ThrowsArgumentOutOfRangeException()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var adminSvc = new UserAdministrationService(mockLogger, mockDal, mockCryptoSvc);

            var e = Assert.Throws<ArgumentOutOfRangeException>(() => adminSvc.GetUserFullDetailsAsync(0).GetAwaiter().GetResult());
            Assert.That(e.Message, Does.Contain("Id cannot be less than 1."));
        }

        [Test]
        public void GetUserFullDetailsAsync_IdValid_ReturnsUserFullDetailsDto()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var adminSvc = new UserAdministrationService(mockLogger, mockDal, mockCryptoSvc);

            var mockJobDescription = new JobDescription()
            {
                Description = "Head Honcho",
                Role = UserRole.MedicalPractitioner
            };

            var mockEmployeeDetails = new Employee()
            {
                Firstname = "Aoife",
                Lastname = "McGonagle",
                Title = Title.Miss,
            };

            var mockUser = new User()
            {
                EmployeeDetails = mockEmployeeDetails,
                Username = "amcg",
                JobDescription = mockJobDescription,
                State = AccountState.Active
            };

            mockDal.GetEntityAsync(1).Returns(mockUser);

            var result = adminSvc.GetUserFullDetailsAsync(1).GetAwaiter().GetResult();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo(Title.Miss));
            Assert.That(result.Firstname, Is.EqualTo("Aoife"));
            Assert.That(result.Lastname, Is.EqualTo("McGonagle"));
            Assert.That(result.Username, Is.EqualTo("amcg"));
            Assert.That(result.JobDescription, Is.EqualTo("Head Honcho"));
            Assert.That(result.Role, Is.EqualTo(UserRole.MedicalPractitioner));
            Assert.That(result.State, Is.EqualTo(AccountState.Active));
        }

        [Test]
        public void UpdateUserDetailsAsync_UserDetailsNull_ThrowsArgumentNullException()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var adminSvc = new UserAdministrationService(mockLogger, mockDal, mockCryptoSvc);

            var e = Assert.Throws<ArgumentNullException>(() => adminSvc.UpdateUserDetailsAsync(null).GetAwaiter().GetResult());
            Assert.That(e.Message, Does.Contain("userDetails"));
        }

        [Test]
        public void UpdateUserDetailsAsync_UserNotFound_ReturnsNotFound()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var adminSvc = new UserAdministrationService(mockLogger, mockDal, mockCryptoSvc);

            var result = adminSvc.UpdateUserDetailsAsync(new UserFullDetailsDto()).GetAwaiter().GetResult();
            Assert.That(result, Is.EqualTo(ServiceResultStatusCode.NotFound));
        }

        [Test]
        public void UpdateUserDetailsAsync_DalFailsToUpdate_ReturnsServiceResultStatusCodeFailed()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var adminSvc = new UserAdministrationService(mockLogger, mockDal, mockCryptoSvc);

            var mockJobDescription = new JobDescription()
            {
                Description = "Head Honcho",
                Role = UserRole.MedicalPractitioner
            };

            var mockEmployeeDetails = new Employee()
            {
                Firstname = "Aoife",
                Lastname = "McGonagle",
                Title = Title.Miss,
            };

            var mockUser = new User()
            {
                EmployeeDetails = mockEmployeeDetails,
                Username = "amcg",
                JobDescription = mockJobDescription,
                State = AccountState.Active
            };

            mockDal.GetEntityAsync(1).Returns(mockUser);
            mockDal.UpdateAsync(Arg.Any<User>()).Returns((User)null);

            var result = adminSvc.UpdateUserDetailsAsync(new UserFullDetailsDto() { UserId = 1}).GetAwaiter().GetResult();
            Assert.That(result, Is.EqualTo(ServiceResultStatusCode.Failed));
        }

        [Test]
        public void UpdateUserDetailsAsync_UpdateSuccess_ReturnsServiceResultStatusCodeSuccess()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var adminSvc = new UserAdministrationService(mockLogger, mockDal, mockCryptoSvc);

            var mockJobDescription = new JobDescription()
            {
                Description = "Head Honcho",
                Role = UserRole.MedicalPractitioner
            };

            var mockEmployeeDetails = new Employee()
            {
                Firstname = "Aoife",
                Lastname = "McGonagle",
                Title = Title.Miss,
            };

            var mockUser = new User()
            {
                EmployeeDetails = mockEmployeeDetails,
                Username = "amcg",
                JobDescription = mockJobDescription,
                State = AccountState.Active
            };

            mockDal.GetEntityAsync(1).Returns(mockUser);
            mockDal.UpdateAsync(Arg.Any<User>()).Returns(mockUser);

            var result = adminSvc.UpdateUserDetailsAsync(new UserFullDetailsDto() { UserId = 1 }).GetAwaiter().GetResult();
            Assert.That(result, Is.EqualTo(ServiceResultStatusCode.Success));
        }

        [Test]
        public void DeleteUserAsync_IdLessThanOne_ThrowsArgumentNullException()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var adminSvc = new UserAdministrationService(mockLogger, mockDal, mockCryptoSvc);

            var e = Assert.Throws<ArgumentOutOfRangeException>(() => adminSvc.DeleteUserAsync(0).GetAwaiter().GetResult());
            Assert.That(e.Message, Does.Contain("Id provided cannot be less than 1"));
        }

        [Test]
        public void DeleteUserAsync_DeleteSuccess_ReturnsServiceResultStatusCodeSuccess()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            mockDal.DeleteAsync(1).Returns((true));

            var adminSvc = new UserAdministrationService(mockLogger, mockDal, mockCryptoSvc);

            var result = adminSvc.DeleteUserAsync(1).GetAwaiter().GetResult();
            Assert.That(result, Is.EqualTo(ServiceResultStatusCode.Success));
        }

        [Test]
        public void DeleteUserAsync_DeleteSuccess_ReturnsServiceResultStatusCodeFailed()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            mockDal.DeleteAsync(1).Returns((false));

            var adminSvc = new UserAdministrationService(mockLogger, mockDal, mockCryptoSvc);

            var result = adminSvc.DeleteUserAsync(1).GetAwaiter().GetResult();
            Assert.That(result, Is.EqualTo(ServiceResultStatusCode.Failed));
        }

        [Test]
        public void ResetPasswordAsync_UserPasswordResetDtoNull_ThrowsArgumentNullException()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var adminSvc = new UserAdministrationService(mockLogger, mockDal, mockCryptoSvc);

            var e = Assert.Throws<ArgumentNullException>(() => adminSvc.ResetPasswordAsync(null).GetAwaiter().GetResult());
            Assert.That(e.Message, Does.Contain("userPasswordReset"));
        }

        [Test]
        public void ResetPasswordAsync_UserNotFound_ReturnsServiceResultStatusCodeNotFound()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            mockDal.GetEntityAsync(1).Returns((User) null);

            var adminSvc = new UserAdministrationService(mockLogger, mockDal, mockCryptoSvc);
            
            var passwordRequest = new UserPasswordResetDto()
            {
                Id = 1
            };

            var result = adminSvc.ResetPasswordAsync(passwordRequest).GetAwaiter().GetResult();
            Assert.That(result, Is.EqualTo(ServiceResultStatusCode.NotFound));
        }

        [Test]
        public void ResetPasswordAsync_PasswordUpdateFails_ReturnsServiceResultStatusCodeFAiled()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var mockJobDescription = new JobDescription()
            {
                Description = "Head Honcho",
                Role = UserRole.MedicalPractitioner
            };

            var mockEmployeeDetails = new Employee()
            {
                Firstname = "Aoife",
                Lastname = "McGonagle",
                Title = Title.Miss,
            };

            var mockUser = new User()
            {
                EmployeeDetails = mockEmployeeDetails,
                Username = "amcg",
                JobDescription = mockJobDescription,
                State = AccountState.Active
            };

            mockDal.GetEntityAsync(1).Returns(mockUser);
            mockDal.UpdateAsync(Arg.Any<User>()).Returns((User)null);
            var adminSvc = new UserAdministrationService(mockLogger, mockDal, mockCryptoSvc);
            
            var passwordRequest = new UserPasswordResetDto()
            {
                Id = 1
            };

            var result = adminSvc.ResetPasswordAsync(passwordRequest).GetAwaiter().GetResult();
            Assert.That(result, Is.EqualTo(ServiceResultStatusCode.Failed));
        }

        [Test]
        public void ResetPasswordAsync_PasswordUpdateSuccess_ReturnsServiceResultStatusCodeSuccess()
        {
            var mockDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<UserAdministrationService>>();
            var mockCryptoSvc = Substitute.For<ICryptographyService>();

            var mockJobDescription = new JobDescription()
            {
                Description = "Head Honcho",
                Role = UserRole.MedicalPractitioner
            };

            var mockEmployeeDetails = new Employee()
            {
                Firstname = "Aoife",
                Lastname = "McGonagle",
                Title = Title.Miss,
            };

            var mockUser = new User()
            {
                EmployeeDetails = mockEmployeeDetails,
                Username = "amcg",
                JobDescription = mockJobDescription,
                State = AccountState.Active
            };

            mockDal.GetEntityAsync(1).Returns(mockUser);
            mockDal.UpdateAsync(Arg.Any<User>()).Returns(mockUser);
            var adminSvc = new UserAdministrationService(mockLogger, mockDal, mockCryptoSvc);

            var passwordRequest = new UserPasswordResetDto()
            {
                Id = 1
            };

            var result = adminSvc.ResetPasswordAsync(passwordRequest).GetAwaiter().GetResult();
            Assert.That(result, Is.EqualTo(ServiceResultStatusCode.Success));
        }
    }
}