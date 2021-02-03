namespace MediBook.Services.Test.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MediBook.Services.Cryptography;
    using MediBook.Services.Cryptography.Processors;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CryptographyServiceTests
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
        public void Ctor_CryptographyProcessorFactoryNull_ThrowsArgumentNullException()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new CryptographyService(null));
            Assert.That(e.Message, Does.Contain("processorFactory"));
        }

        [Test]
        public void Encrypt_DataNull_ThrowsArgumentNullException()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var svc = new CryptographyService(factory);

            var e = Assert.Throws<ArgumentNullException>(() => svc.Encrypt((string)null));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void Encrypt_DataEmpty_ThrowsArgumentNullException()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var svc = new CryptographyService(factory);

            var e = Assert.Throws<ArgumentNullException>(() => svc.Encrypt(string.Empty));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void Encrypt_DataListNull_ThrowsArgumentNullException()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var svc = new CryptographyService(factory);

            var e = Assert.Throws<ArgumentNullException>(() => svc.Encrypt((List<string>)null));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void Encrypt_DataValid_EncryptionProcessorReceivedCall()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var encryptionProcessor = Substitute.For<IEncryptionProcessor>();
            factory.GetEncryptionProcessor().Returns(encryptionProcessor);

            var svc = new CryptographyService(factory);

            svc.Encrypt("Test Data");

            encryptionProcessor.Received().Encrypt("Test Data");
        }

        [Test]
        public void Encrypt_DataListContainsThreeValidItems_EncryptionProcessorReceivedThreeCalls()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var encryptionProcessor = Substitute.For<IEncryptionProcessor>();
            factory.GetEncryptionProcessor().Returns(encryptionProcessor);

            var svc = new CryptographyService(factory);

            var strings = new List<string>()
            {
                "one",
                "two",
                "three"
            };

            svc.Encrypt(strings);

            encryptionProcessor.Received(3).Encrypt(Arg.Any<string>());
        }

        [Test]
        public void Decrypt_DataNull_ThrowsArgumentNullException()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var svc = new CryptographyService(factory);

            var e = Assert.Throws<ArgumentNullException>(() => svc.Decrypt((byte[])null));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void Decrypt_DataListNull_ThrowsArgumentNullException()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var svc = new CryptographyService(factory);

            var e = Assert.Throws<ArgumentNullException>(() => svc.Decrypt((List<byte[]>)null));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void Decrypt_DataValid_EncryptionProcessorReceivedCall()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var encryptionProcessor = Substitute.For<IEncryptionProcessor>();
            factory.GetEncryptionProcessor().Returns(encryptionProcessor);

            var svc = new CryptographyService(factory);
            var testArray = new byte[8];
            svc.Decrypt(testArray);

            encryptionProcessor.Received().Decrypt(testArray);
        }

        [Test]
        public void Decrypt_DataListContainsThreeValidItems_EncryptionProcessorReceivedThreeCalls()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var encryptionProcessor = Substitute.For<IEncryptionProcessor>();
            factory.GetEncryptionProcessor().Returns(encryptionProcessor);

            var svc = new CryptographyService(factory);

            var testList = new List<byte[]>()
            {
                new byte[8],
                new byte[8],
                new byte[8],
            };

            svc.Decrypt(testList);

            encryptionProcessor.Received(3).Decrypt(Arg.Any<byte[]>());
        }

        [Test]
        public void CreateHash_NullData_ThrowsArgumentNullException()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var svc = new CryptographyService(factory);

            var e = Assert.Throws<ArgumentNullException>(() => svc.CreateHash(null, out var x, out var y));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void CreateHash_EmptyData_ThrowsArgumentNullException()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var svc = new CryptographyService(factory);

            var e = Assert.Throws<ArgumentNullException>(() => svc.CreateHash(string.Empty, out var x, out var y));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void CreateHash_ValidData_HashingProcessorReceivedCall()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var hashProcessor = Substitute.For<IHashingProcessor>();
            factory.GetHashingProcessor().Returns(hashProcessor);

            var svc = new CryptographyService(factory);

            svc.CreateHash("Test Data", out var hashResult, out var saltResult);

            hashProcessor.Received().CreateHash("Test Data", out Arg.Any<byte[]>(), out Arg.Any<byte[]>());
        }

        [Test]
        public void VerifyPasswordHash_PasswordHashNull_ThrowsArgumentNullException()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var hashProcessor = Substitute.For<IHashingProcessor>();
            factory.GetHashingProcessor().Returns(hashProcessor);

            var svc = new CryptographyService(factory);

            var e = Assert.Throws<ArgumentNullException>(() => svc.VerifyPasswordHash(null, _saltBytes, "Test Data"));
            Assert.That(e.Message, Does.Contain("passwordHash"));
        }

        [Test]
        public void VerifyPasswordHash_PasswordSaltNull_ThrowsArgumentNullException()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var hashProcessor = Substitute.For<IHashingProcessor>();
            factory.GetHashingProcessor().Returns(hashProcessor);

            var svc = new CryptographyService(factory);

            var e = Assert.Throws<ArgumentNullException>(() => svc.VerifyPasswordHash(_hashBytes, null, "Test Data"));
            Assert.That(e.Message, Does.Contain("passwordSalt"));
        }

        [Test]
        public void VerifyPasswordHash_PlainTextPasswordNull_ThrowsArgumentNullException()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var hashProcessor = Substitute.For<IHashingProcessor>();
            factory.GetHashingProcessor().Returns(hashProcessor);

            var svc = new CryptographyService(factory);

            var e = Assert.Throws<ArgumentNullException>(() => svc.VerifyPasswordHash(_hashBytes, _saltBytes, null));
            Assert.That(e.Message, Does.Contain("plainTextPassword"));
        }

        [Test]
        public void VerifyPasswordHash_PlainTextPasswordEmpty_ThrowsArgumentNullException()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var hashProcessor = Substitute.For<IHashingProcessor>();
            factory.GetHashingProcessor().Returns(hashProcessor);

            var svc = new CryptographyService(factory);

            var e = Assert.Throws<ArgumentNullException>(() => svc.VerifyPasswordHash(_hashBytes, _saltBytes, string.Empty));
            Assert.That(e.Message, Does.Contain("plainTextPassword"));
        }

        [Test]
        public void VerifyPasswordHash_PlainTextMatchesHashedPassword_ReturnsTrue()
        {
            var factory = Substitute.For<ICryptographyProcessorFactory>();
            var hashProcessor = Substitute.For<IHashingProcessor>();
            factory.GetHashingProcessor().Returns(hashProcessor);

            var svc = new CryptographyService(factory);

            svc.VerifyPasswordHash(_hashBytes, _saltBytes, "Test Data");

            hashProcessor.Received().VerifyPasswordHash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), "Test Data");
        }

        private byte[] StringToBytes(string input)
        {
            string[] strings = input.Split(',');
            byte[] bytes = strings.Select(s => byte.Parse(s)).ToArray();

            return bytes;
        }
    }
}