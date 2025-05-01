namespace MediBook.Data.Test.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using MediBook.Data.Repositories;
    using Microsoft.Extensions.Logging;
    using MockQueryable.NSubstitute;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class PatientDalTests
    {
        [Test]
        public void Ctor_NullDatabaseContext_ThrowsArgumentNullException()
        {
            var mockLogger = Substitute.For<ILogger<PatientDal>>();
            var e = Assert.Throws<ArgumentNullException>(() => new PatientDal(null, mockLogger));
            Assert.That(e.Message, Does.Contain("databaseContext"));
        }

        [Test]
        public void Ctor_NullLogger_ThrowsArgumentNullException()
        {
            var mockDbContext = Substitute.For<IDatabaseContext>();
            var e = Assert.Throws<ArgumentNullException>(() => new PatientDal(mockDbContext,null));
            Assert.That(e.Message, Does.Contain("logger"));
        }


        [Test]
        public void AddAsync_NullEntity_ThrowsArgumentNullException()
        {
            var mockDbContext = Substitute.For<IDatabaseContext>();
            var mockLogger = Substitute.For<ILogger<PatientDal>>();
            var dal = new PatientDal(mockDbContext, mockLogger);
            var e = Assert.Throws<ArgumentNullException>(() => dal.AddAsync(null).GetAwaiter().GetResult());
            Assert.That(e.Message, Does.Contain("newPatientDetails"));
        }

        [Test]
        public void AddAsync_EntityAlreadyExists_ReturnsNull()
        {
            var entity = new Patient
            {
                Id = 1,
            };

            var data = new List<Patient>
            {
                entity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();

            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<Patient>().Returns(mockSet);
            var mockLogger = Substitute.For<ILogger<PatientDal>>();
            var dal = new PatientDal(mockDbContext, mockLogger);

            var result = Task.Run(async () => await dal.AddAsync(entity)).GetAwaiter().GetResult();
            Assert.That(result, Is.Null);
        }

        [Test]
        public void AddAsync_EntityDoesNotAlreadyExist_ReturnsEntity()
        {
            var firstEntity = new Patient
            {
                Id = 1,
            };

            var secondEntity = new Patient
            {
                Id = 2,
            };

            var data = new List<Patient>
            {
                firstEntity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<Patient>().Returns(mockSet);
            var mockLogger = Substitute.For<ILogger<PatientDal>>();
            var dal = new PatientDal(mockDbContext, mockLogger);

            var result = Task.Run(async () => await dal.AddAsync(secondEntity)).GetAwaiter().GetResult();
            Assert.That(result, Is.EqualTo(secondEntity));
        }

        [Test]
        public void DeleteAsync_IdNotRecognised_ReturnsFalse()
        {
            var firstEntity = new Patient
            {
                Id = 1,
            };

            var data = new List<Patient>
            {
                firstEntity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<Patient>().Returns(mockSet);
            var mockLogger = Substitute.For<ILogger<PatientDal>>();
            var dal = new PatientDal(mockDbContext, mockLogger);

            var result = Task.Run(async () => await dal.DeleteAsync(2)).GetAwaiter().GetResult();
            Assert.That(result, Is.False);
        }

        [Test]
        public void DeleteAsync_IdRecognised_ReturnsTrue()
        {
            var firstEntity = new Patient
            {
                Id = 1,
            };

            var data = new List<Patient>
            {
                firstEntity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<Patient>().Returns(mockSet);
            var mockLogger = Substitute.For<ILogger<PatientDal>>();
            var dal = new PatientDal(mockDbContext, mockLogger);

            var result = Task.Run(async () => await dal.DeleteAsync(1)).GetAwaiter().GetResult();
            Assert.That(result, Is.True);
        }

        [Test]
        public void UpdateAsync_NullEntity_ThrowsArgumentNullException()
        {
            var mockDbContext = Substitute.For<IDatabaseContext>();
            var mockLogger = Substitute.For<ILogger<PatientDal>>();
            var dal = new PatientDal(mockDbContext, mockLogger);
            var e = Assert.Throws<ArgumentNullException>(() => dal.UpdateAsync(null).GetAwaiter().GetResult());
            Assert.That(e.Message, Does.Contain("entity"));
        }

        [Test]
        public void UpdateAsync_EntityDoesNotExist_ReturnsNull()
        {
            var firstEntity = new Patient
            {
                Id = 1,
            };

            var updatedEntity = new Patient
            {
                Id = 2,
            };

            var data = new List<Patient>
            {
                firstEntity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<Patient>().Returns(mockSet);
            var mockLogger = Substitute.For<ILogger<PatientDal>>();
            var dal = new PatientDal(mockDbContext, mockLogger);

            var result = Task.Run(async () => await dal.UpdateAsync(updatedEntity)).GetAwaiter().GetResult();
            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdateAsync_EntityExists_ReturnsUpdatedEntity()
        {
            var firstEntity = new Patient
            {
                Id = 1,
            };

            var updatedEntity = new Patient
            {
                Id = 1,
            };

            var data = new List<Patient>
            {
                firstEntity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<Patient>().Returns(mockSet);
            var mockLogger = Substitute.For<ILogger<PatientDal>>();
            var dal = new PatientDal(mockDbContext, mockLogger);

            var result = Task.Run(async () => await dal.UpdateAsync(updatedEntity)).GetAwaiter().GetResult();
            Assert.That(result, Is.EqualTo(updatedEntity));
        }

        [Test]
        public void GetEntityAsync_EntityFound_ReturnsEntity()
        {
            var entity = new Patient
            {
                Id = 1,
            };

            var data = new List<Patient>
            {
                entity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<Patient>().Returns(mockSet);
            var mockLogger = Substitute.For<ILogger<PatientDal>>();
            var dal = new PatientDal(mockDbContext, mockLogger);

            var result = Task.Run(async () => await dal.GetEntityAsync(1)).GetAwaiter().GetResult();
            Assert.That(result, Is.EqualTo(entity));
        }

        [Test]
        public void GetEntityAsync_EntityNotFound_ReturnsNull()
        {
            var entity = new Patient
            {
                Id = 1,
            };

            var data = new List<Patient>
            {
                entity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<Patient>().Returns(mockSet);
            var mockLogger = Substitute.For<ILogger<PatientDal>>();
            var dal = new PatientDal(mockDbContext, mockLogger);

            var result = Task.Run(async () => await dal.GetEntityAsync(2)).GetAwaiter().GetResult();
            Assert.That(result, Is.Null);
        }

        [Test]
        public void CheckEntityExistsAsync_NullEntity_ThrowsArgumentNullException()
        {
            var mockDbContext = Substitute.For<IDatabaseContext>();
            var mockLogger = Substitute.For<ILogger<PatientDal>>();
            var dal = new PatientDal(mockDbContext, mockLogger);
            var e = Assert.Throws<ArgumentNullException>(
                () => dal.CheckEntityExistsAsync(null).GetAwaiter().GetResult());
            Assert.That(e.Message, Does.Contain("entity"));
        }

        [Test]
        public void CheckEntityExistsAsync_EntityNotFound_ReturnsFalse()
        {
            var entity = new Patient
            {
                Id = 1,
            };

            var data = new List<Patient>
            {
                entity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<Patient>().Returns(mockSet);
            var mockLogger = Substitute.For<ILogger<PatientDal>>();
            var dal = new PatientDal(mockDbContext, mockLogger);

            var result = Task.Run(async () => await dal.CheckEntityExistsAsync(2)).GetAwaiter().GetResult();
            Assert.That(result, Is.False);
        }

        [Test]
        public void CheckEntityExistsAsync_EntityFound_ReturnsTrue()
        {
            var entity = new Patient
            {
                Id = 1,
            };

            var data = new List<Patient>
            {
                entity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<Patient>().Returns(mockSet);
            var mockLogger = Substitute.For<ILogger<PatientDal>>();
            var dal = new PatientDal(mockDbContext, mockLogger);

            var result = Task.Run(async () => await dal.CheckEntityExistsAsync(1)).GetAwaiter().GetResult();
            Assert.That(result, Is.True);
        }

        [Test]
        public void Filter_PredicateIsNull_ThrowsArgumentNullException()
        {
            var mockDbContext = Substitute.For<IDatabaseContext>();
            var mockLogger = Substitute.For<ILogger<PatientDal>>();
            var dal = new PatientDal(mockDbContext, mockLogger);

            var e = Assert.Throws<ArgumentNullException>(() => dal.Filter(null));
            Assert.That(e.Message, Does.Contain("predicate"));
        }
    }
}