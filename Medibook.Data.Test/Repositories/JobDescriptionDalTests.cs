namespace MediBook.Data.Test.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediBook.Core.Models;
    using Medibook.Data.DataAccess;
    using Medibook.Data.Repositories;
    using MockQueryable.NSubstitute;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class JobDescriptionDalTests
    {
        [Test]
        public void Ctor_NullDatabaseContext_ThrowsArgumentNullException()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new JobDescriptionDal(null));
            Assert.That(e.Message, Does.Contain("databaseContext"));
        }

        [Test]
        public void AddAsync_NullEntity_ThrowsArgumentNullException()
        {
            var mockDbContext = Substitute.For<IDatabaseContext>();
            var dal = new JobDescriptionDal(mockDbContext);
            var e = Assert.Throws<ArgumentNullException>(() => dal.AddAsync(null).GetAwaiter().GetResult());
            Assert.That(e.Message, Does.Contain("entity"));
        }

        [Test]
        public void AddAsync_EntityAlreadyExists_ReturnsNull()
        {
            var entity = new JobDescription
            {
                Id = 1,
                Description = "Receptionist"
            };

            var data = new List<JobDescription>
            {
                entity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();

            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<JobDescription>().Returns(mockSet);

            var dal = new JobDescriptionDal(mockDbContext);

            var result = Task.Run(async () => await dal.AddAsync(entity)).GetAwaiter().GetResult();
            Assert.That(result, Is.Null);
        }

        [Test]
        public void AddAsync_EntityDoesNotAlreadyExist_ReturnsEntity()
        {
            var firstEntity = new JobDescription
            {
                Id = 1,
                Description = "Receptionist"
            };

            var secondEntity = new JobDescription
            {
                Id = 2,
                Description = "General Practitioner"
            };

            var data = new List<JobDescription>
            {
                firstEntity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<JobDescription>().Returns(mockSet);
            var dal = new JobDescriptionDal(mockDbContext);

            var result = Task.Run(async () => await dal.AddAsync(secondEntity)).GetAwaiter().GetResult();
            Assert.That(result, Is.EqualTo(secondEntity));
        }

        [Test]
        public void DeleteAsync_IdNotRecognised_ReturnsFalse()
        {
            var firstEntity = new JobDescription
            {
                Id = 1,
                Description = "Dentist"
            };

            var data = new List<JobDescription>
            {
                firstEntity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<JobDescription>().Returns(mockSet);
            var dal = new JobDescriptionDal(mockDbContext);

            var result = Task.Run(async () => await dal.DeleteAsync(2)).GetAwaiter().GetResult();
            Assert.That(result, Is.False);
        }

        [Test]
        public void DeleteAsync_IdRecognised_ReturnsTrue()
        {
            var firstEntity = new JobDescription
            {
                Id = 1,
                Description = "Physiotherapist"
            };

            var data = new List<JobDescription>
            {
                firstEntity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<JobDescription>().Returns(mockSet);
            var dal = new JobDescriptionDal(mockDbContext);

            var result = Task.Run(async () => await dal.DeleteAsync(1)).GetAwaiter().GetResult();
            Assert.That(result, Is.True);
        }

        [Test]
        public void UpdateAsync_NullEntity_ThrowsArgumentNullException()
        {
            var mockDbContext = Substitute.For<IDatabaseContext>();
            var dal = new JobDescriptionDal(mockDbContext);
            var e = Assert.Throws<ArgumentNullException>(() => dal.UpdateAsync(null).GetAwaiter().GetResult());
            Assert.That(e.Message, Does.Contain("entity"));
        }

        [Test]
        public void UpdateAsync_EntityDoesNotExist_ReturnsNull()
        {
            var firstEntity = new JobDescription
            {
                Id = 1,
                Description = "Physiotherapist"
            };

            var updatedEntity = new JobDescription
            {
                Id = 2,
                Description = "General Practitioner"
            };

            var data = new List<JobDescription>
            {
                firstEntity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<JobDescription>().Returns(mockSet);
            var dal = new JobDescriptionDal(mockDbContext);

            var result = Task.Run(async () => await dal.UpdateAsync(updatedEntity)).GetAwaiter().GetResult();
            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdateAsync_EntityExists_ReturnsUpdatedEntity()
        {
            var firstEntity = new JobDescription
            {
                Id = 1,
                Description = "General Practitioner"
            };

            var updatedEntity = new JobDescription
            {
                Id = 1,
                Description = "Lead General Practitioner"
            };

            var data = new List<JobDescription>
            {
                firstEntity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<JobDescription>().Returns(mockSet);
            var dal = new JobDescriptionDal(mockDbContext);

            var result = Task.Run(async () => await dal.UpdateAsync(updatedEntity)).GetAwaiter().GetResult();
            Assert.That(result, Is.EqualTo(updatedEntity));
        }

        [Test]
        public void GetEntityAsync_EntityFound_ReturnsEntity()
        {
            var entity = new JobDescription
            {
                Id = 1,
                Description = "General Practitioner"
            };

            var data = new List<JobDescription>
            {
                entity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<JobDescription>().Returns(mockSet);
            var dal = new JobDescriptionDal(mockDbContext);

            var result = Task.Run(async () => await dal.GetEntityAsync(1)).GetAwaiter().GetResult();
            Assert.That(result, Is.EqualTo(entity));
        }

        [Test]
        public void GetEntityAsync_EntityNotFound_ReturnsNull()
        {
            var entity = new JobDescription
            {
                Id = 1,
                Description = "Dentist"
            };

            var data = new List<JobDescription>
            {
                entity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<JobDescription>().Returns(mockSet);
            var dal = new JobDescriptionDal(mockDbContext);

            var result = Task.Run(async () => await dal.GetEntityAsync(2)).GetAwaiter().GetResult();
            Assert.That(result, Is.Null);
        }

        [Test]
        public void CheckEntityExistsAsync_NullEntity_ThrowsArgumentNullException()
        {
            var mockDbContext = Substitute.For<IDatabaseContext>();
            var dal = new JobDescriptionDal(mockDbContext);
            var e = Assert.Throws<ArgumentNullException>(
                () => dal.CheckEntityExistsAsync(null).GetAwaiter().GetResult());
            Assert.That(e.Message, Does.Contain("entity"));
        }

        [Test]
        public void CheckEntityExistsAsync_EntityNotFound_ReturnsFalse()
        {
            var entity = new JobDescription
            {
                Id = 1,
                Description = "General Practitioner"
            };

            var data = new List<JobDescription>
            {
                entity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<JobDescription>().Returns(mockSet);
            var dal = new JobDescriptionDal(mockDbContext);

            var result = Task.Run(async () => await dal.CheckEntityExistsAsync(2)).GetAwaiter().GetResult();
            Assert.That(result, Is.False);
        }

        [Test]
        public void CheckEntityExistsAsync_EntityFound_ReturnsTrue()
        {
            var entity = new JobDescription
            {
                Id = 1,
                Description = "General Practitioner"
            };

            var data = new List<JobDescription>
            {
                entity
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockDbContext = Substitute.For<IDatabaseContext>();
            mockDbContext.Set<JobDescription>().Returns(mockSet);
            var dal = new JobDescriptionDal(mockDbContext);

            var result = Task.Run(async () => await dal.CheckEntityExistsAsync(1)).GetAwaiter().GetResult();
            Assert.That(result, Is.True);
        }

        [Test]
        public void Filter_PredicateIsNull_ThrowsArgumentNullException()
        {
            var mockDbContext = Substitute.For<IDatabaseContext>();
            var dal = new JobDescriptionDal(mockDbContext);
            var e = Assert.Throws<ArgumentNullException>(() => dal.Filter(null));
            Assert.That(e.Message, Does.Contain("predicate"));
        }
    }
}