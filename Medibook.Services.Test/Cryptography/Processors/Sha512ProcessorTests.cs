namespace MediBook.Services.Test.Cryptography.Processors
{
    using System;
    using System.Linq;
    using MediBook.Services.Cryptography.Processors;
    using NUnit.Framework;

    [TestFixture]
    public class Sha512ProcessorTests
    {
        private string _hashString = "5,137,107,79,13,114,1,215,172,168,157,135,80,163,45,216,125,72,231,22,220,157,74,173,100,241,224,64,8,59,52,182,223,25,19,37,57,201,82,160,93,34,179,254,82,126,22,158,114,227,63,203,28,142,5,50,177,41,174,185,88,112,35,57";
        private string _saltString = "85,133,180,122,29,22,14,133,207,42,203,23,204,46,238,24,171,53,254,133,188,166,174,182,127,190,176,18,205,175,24,165,34,217,225,200,22,194,12,82,128,36,238,64,231,153,51,108,148,210,205,190,152,146,191,157,103,140,203,34,75,101,195,12,46,155,56,19,185,212,206,10,160,28,67,214,75,95,28,26,124,100,137,25,247,41,160,245,154,84,255,116,144,160,42,103,52,153,94,14,0,134,50,5,18,215,212,242,229,207,53,227,61,43,19,175,142,32,79,182,44,142,228,29,0,104,178,63";
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
            var processor = new Sha512Processor();

            var e = Assert.Throws<ArgumentNullException>(() => processor.CreateHash(null, out var x, out var y));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void CreateHash_EmptyData_ThrowsArgumentNullException()
        {
            var processor = new Sha512Processor();

            var e = Assert.Throws<ArgumentNullException>(() => processor.CreateHash(string.Empty, out var x, out var y));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void CreateHash_ValidData_ReturnsHashAndSalt()
        {
            var processor = new Sha512Processor();

            processor.CreateHash("Test Data", out var hashResult, out var saltResult);

            Assert.That(hashResult, Is.Not.Null);
            Assert.That(saltResult, Is.Not.Null);
            Assert.That(hashResult, Is.Not.Empty);
            Assert.That(saltResult, Is.Not.Empty);
        }

        [Test]
        public void VerifyPasswordHash_PasswordHashNull_ThrowsArgumentNullException()
        {
            var processor = new Sha512Processor();

            var e = Assert.Throws<ArgumentNullException>(() => processor.VerifyPasswordHash(null, _saltBytes, "Test Data"));
            Assert.That(e.Message, Does.Contain("passwordHash"));
        }

        [Test]
        public void VerifyPasswordHash_PasswordSaltNull_ThrowsArgumentNullException()
        {
            var processor = new Sha512Processor();

            var e = Assert.Throws<ArgumentNullException>(() => processor.VerifyPasswordHash(_hashBytes, null, "Test Data"));
            Assert.That(e.Message, Does.Contain("passwordSalt"));
        }

        [Test]
        public void VerifyPasswordHash_PlainTextPasswordNull_ReturnsTrue()
        {
            var processor = new Sha512Processor();

            var e = Assert.Throws<ArgumentNullException>(() => processor.VerifyPasswordHash(_hashBytes, _saltBytes, null));
            Assert.That(e.Message, Does.Contain("plainTextPassword"));
        }

        [Test]
        public void VerifyPasswordHash_PlainTextPasswordEmpty_ReturnsTrue()
        {
            var processor = new Sha512Processor();

            var e = Assert.Throws<ArgumentNullException>(() => processor.VerifyPasswordHash(_hashBytes, _saltBytes, string.Empty));
            Assert.That(e.Message, Does.Contain("plainTextPassword"));
        }

        [Test]
        public void VerifyPasswordHash_PlainTextMatchesHashedPassword_ReturnsTrue()
        {
            var processor = new Sha512Processor();
            
            var result = processor.VerifyPasswordHash(_hashBytes, _saltBytes, "Test Data");
            Assert.That(result, Is.True);
        }

        [Test]
        public void VerifyPasswordHash_PlainTextDoesNotMatchHashedPassword_ReturnsFalse()
        {
            var processor = new Sha512Processor();

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