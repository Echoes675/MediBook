namespace MediBook.Services.Test.Cryptography.Processors
{
    using System;
    using System.Linq;
    using MediBook.Services.Cryptography.Processors;
    using NUnit.Framework;

    [TestFixture]
    public class Sha256ProcessorTests
    {
        private string _hashString = "177,249,13,202,41,126,110,12,213,169,162,164,95,38,194,133,196,138,94,90,10,1,56,224,222,55,150,33,129,190,240,254";
        private string _saltString = "208,72,190,57,43,5,192,81,202,237,7,90,67,195,114,42,208,45,45,50,61,50,228,6,231,90,24,41,94,232,122,76,4,14,168,225,216,30,65,202,246,72,106,149,111,47,74,241,211,156,204,8,71,51,143,14,73,198,172,83,148,121,236,144";
        private byte[] _hashBytes;
        private byte[] _saltBytes;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _hashBytes = StringToBytes(_hashString);
            _saltBytes = StringToBytes(_saltString);
        }

        [Test]
        public void CreateHash_NullData_ThrowsArgumentNullException()
        {
            var processor = new Sha256Processor();

            var e = Assert.Throws<ArgumentNullException>(() => processor.CreateHash(null, out var x, out var y));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void CreateHash_EmptyData_ThrowsArgumentNullException()
        {
            var processor = new Sha256Processor();

            var e = Assert.Throws<ArgumentNullException>(() => processor.CreateHash(string.Empty, out var x, out var y));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void CreateHash_ValidData_ReturnsHashAndSalt()
        {
            var processor = new Sha256Processor();

            processor.CreateHash("Test Data", out var hashResult, out var saltResult);

            Assert.That(hashResult, Is.Not.Null);
            Assert.That(saltResult, Is.Not.Null);
            Assert.That(hashResult, Is.Not.Empty);
            Assert.That(saltResult, Is.Not.Empty);
        }

        [Test]
        public void VerifyPasswordHash_PasswordHashNull_ThrowsArgumentNullException()
        {
            var processor = new Sha256Processor();

            var e = Assert.Throws<ArgumentNullException>(() => processor.VerifyPasswordHash(null, _saltBytes, "Test Data"));
            Assert.That(e.Message, Does.Contain("passwordHash"));
        }

        [Test]
        public void VerifyPasswordHash_PasswordSaltNull_ThrowsArgumentNullException()
        {
            var processor = new Sha256Processor();

            var e = Assert.Throws<ArgumentNullException>(() => processor.VerifyPasswordHash(_hashBytes, null, "Test Data"));
            Assert.That(e.Message, Does.Contain("passwordSalt"));
        }

        [Test]
        public void VerifyPasswordHash_PlainTextPasswordNull_ReturnsTrue()
        {
            var processor = new Sha256Processor();

            var e = Assert.Throws<ArgumentNullException>(() => processor.VerifyPasswordHash(_hashBytes, _saltBytes, null));
            Assert.That(e.Message, Does.Contain("plainTextPassword"));
        }

        [Test]
        public void VerifyPasswordHash_PlainTextPasswordEmpty_ReturnsTrue()
        {
            var processor = new Sha256Processor();

            var e = Assert.Throws<ArgumentNullException>(() => processor.VerifyPasswordHash(_hashBytes, _saltBytes, string.Empty));
            Assert.That(e.Message, Does.Contain("plainTextPassword"));
        }

        [Test]
        public void VerifyPasswordHash_PlainTextMatchesHashedPassword_ReturnsTrue()
        {
            var processor = new Sha256Processor();

            var result = processor.VerifyPasswordHash(_hashBytes, _saltBytes, "Test Data");
            Assert.That(result, Is.True);
        }

        [Test]
        public void VerifyPasswordHash_PlainTextDoesNotMatchHashedPassword_ReturnsFalse()
        {
            var processor = new Sha256Processor();

            var result = processor.VerifyPasswordHash(_hashBytes, _saltBytes, "No Match");
            Assert.That(result, Is.False);
        }

        private byte[] StringToBytes(string input)
        {
            string[] strings = input.Split(',');
            byte[] bytes = strings.Select(s => byte.Parse(s)).ToArray();

            return bytes;
        }
    }
}