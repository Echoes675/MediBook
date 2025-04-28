namespace MediBook.Services.Test.PatientAdministration
{
    using System;
    using System.Collections.Generic;
    using MediBook.Core.DTOs;
    using MediBook.Core.Models;
    using MediBook.Data.Repositories;
    using MediBook.Services.PatientAdministration;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class PatientAdministrationServiceTests
    {
        [Test]
        public void Ctor_NullUserDal_ThrowsArgumentNullException()
        {
            var mockLogger = Substitute.For<ILogger<PatientAdministrationService>>();
            var mockPatientDal = Substitute.For<IPatientDal>();
            var e = Assert.Throws<ArgumentNullException>(() => new PatientAdministrationService(mockLogger, mockPatientDal, null));
            Assert.That(e.Message, Does.Contain("userDal"));
        }

        [Test]
        public void Ctor_NullLogger_ThrowsArgumentNullException()
        {
            var mockUserDal = Substitute.For<IUserDal>();
            var mockPatientDal = Substitute.For<IPatientDal>();
            var e = Assert.Throws<ArgumentNullException>(() => new PatientAdministrationService(null, mockPatientDal, mockUserDal));
            Assert.That(e.Message, Does.Contain("log"));
        }

        [Test]
        public void Ctor_NullPatientDal_ThrowsArgumentNullException()
        {
            var mockUserDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<PatientAdministrationService>>();
            var e = Assert.Throws<ArgumentNullException>(() => new PatientAdministrationService(mockLogger, null, mockUserDal));
            Assert.That(e.Message, Does.Contain("patientDal"));
        }

        [Test]
        public void LoadPatientDetails_PatientDalReturnsTwoPatientss_ReturnsTwoPatientDetailsDto()
        {
            var mockPatientDal = Substitute.For<IPatientDal>();
            var mockUserDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<PatientAdministrationService>>();

            var mockPatient = new Patient()
            {
                Firstname = "Jane",
                Lastname = "Black"
            };

            var patients = new List<Patient>
            {
                mockPatient,
                mockPatient
            };

            mockPatientDal.GetAllAsync().Returns(patients);
            var adminSvc = new PatientAdministrationService(mockLogger, mockPatientDal, mockUserDal);

            var result = adminSvc.LoadPatientsDetails().GetAwaiter().GetResult();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Is.TypeOf<List<PatientDetailsDto>>());
        }

        [Test]
        public void PerformPatientSearchAsync_NullSearchCriteria_ThrowsArgumentNullException()
        {
            var mockPatientDal = Substitute.For<IPatientDal>();
            var mockUserDal = Substitute.For<IUserDal>();
            var mockLogger = Substitute.For<ILogger<PatientAdministrationService>>();
            var svc= new PatientAdministrationService(mockLogger, mockPatientDal, mockUserDal);
            var e = Assert.Throws<ArgumentNullException>(() =>
                svc.PerformPatientSearchAsync(null).GetAwaiter().GetResult());
            Assert.That(e.Message, Does.Contain("searchCriteria"));
        }
    }
}
