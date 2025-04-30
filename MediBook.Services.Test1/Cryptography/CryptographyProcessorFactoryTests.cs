namespace MediBook.Services.Test.Cryptography
{
    using System;
    using MediBook.Services.Cryptography;
    using MediBook.Services.Cryptography.Processors;
    using Microsoft.Extensions.Configuration;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CryptographyProcessorFactoryTests
    {
        [Test]
        public void Ctor_ConfigNull_ThrowsArgumentNullException()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new CryptographyProcessorFactory((IConfiguration)null));
            Assert.That(e.Message, Does.Contain("config"));
        }

        [Test]
        public void Ctor_OptionsNull_ThrowsArgumentNullException()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new CryptographyProcessorFactory((ICryptographyProcessorOptions)null));
            Assert.That(e.Message, Does.Contain("options"));
        }

        [Test]
        public void Ctor_OptionsHashingAlgorithmEmpty_ThrowsArgumentNullException()
        {
            var mockOptions = Substitute.For<ICryptographyProcessorOptions>();
            mockOptions.HashingAlgorithm.Returns(string.Empty);

            var e = Assert.Throws<ArgumentNullException>(() => new CryptographyProcessorFactory(mockOptions));
            Assert.That(e.Message, Does.Contain("options"));
        }

        [Test]
        public void Ctor_OptionsHashingAlgorithmNull_ThrowsArgumentNullException()
        {
            var mockOptions = Substitute.For<ICryptographyProcessorOptions>();
            mockOptions.HashingAlgorithm.Returns((string)null);

            var e = Assert.Throws<ArgumentNullException>(() => new CryptographyProcessorFactory(mockOptions));
            Assert.That(e.Message, Does.Contain("options"));
        }

        [Test]
        public void Ctor_OptionsHashingAlgorithmNotSupported_ThrowsArgumentNullException()
        {
            var mockOptions = Substitute.For<ICryptographyProcessorOptions>();
            mockOptions.HashingAlgorithm.Returns("SomethingWrong");
            mockOptions.CryptographyKey.Returns("key");
            mockOptions.EncryptionAlgorithm.Returns("AES");

            var e = Assert.Throws<NotSupportedException>(() => new CryptographyProcessorFactory(mockOptions));
            Assert.That(e.Message, Does.Contain("Unsupported Hashing algorithm found in options."));
        }

        [Test]
        public void Ctor_OptionsCryptographyKeyEmpty_ThrowsArgumentNullException()
        {
            var mockOptions = Substitute.For<ICryptographyProcessorOptions>();
            mockOptions.HashingAlgorithm.Returns("SHA512");
            mockOptions.CryptographyKey.Returns(string.Empty);

            var e = Assert.Throws<ArgumentNullException>(() => new CryptographyProcessorFactory(mockOptions));
            Assert.That(e.Message, Does.Contain("options"));
        }

        [Test]
        public void Ctor_OptionsCryptographyKeyNull_ThrowsArgumentNullException()
        {
            var mockOptions = Substitute.For<ICryptographyProcessorOptions>();
            mockOptions.HashingAlgorithm.Returns("SHA512");
            mockOptions.CryptographyKey.Returns((string) null);

            var e = Assert.Throws<ArgumentNullException>(() => new CryptographyProcessorFactory(mockOptions));
            Assert.That(e.Message, Does.Contain("options"));
        }

        [Test]
        public void Ctor_OptionsEncryptionAlgorithmNull_ThrowsArgumentNullException()
        {
            var mockOptions = Substitute.For<ICryptographyProcessorOptions>();
            mockOptions.HashingAlgorithm.Returns("SHA512");
            mockOptions.CryptographyKey.Returns("key");
            mockOptions.EncryptionAlgorithm.Returns((string)null);

            var e = Assert.Throws<ArgumentNullException>(() => new CryptographyProcessorFactory(mockOptions));
            Assert.That(e.Message, Does.Contain("options"));
        }

        [Test]
        public void Ctor_OptionsEncryptionAlgorithmEmpty_ThrowsArgumentNullException()
        {
            var mockOptions = Substitute.For<ICryptographyProcessorOptions>();
            mockOptions.HashingAlgorithm.Returns("SHA512");
            mockOptions.CryptographyKey.Returns("key");
            mockOptions.EncryptionAlgorithm.Returns(string.Empty);

            var e = Assert.Throws<ArgumentNullException>(() => new CryptographyProcessorFactory(mockOptions));
            Assert.That(e.Message, Does.Contain("options"));
        }

        [Test]
        public void Ctor_OptionsEncryptionAlgorithmNotSupported_ThrowsArgumentNullException()
        {
            var mockOptions = Substitute.For<ICryptographyProcessorOptions>();
            mockOptions.HashingAlgorithm.Returns("SHA512");
            mockOptions.CryptographyKey.Returns("key");
            mockOptions.EncryptionAlgorithm.Returns("SomethingWrong");

            var e = Assert.Throws<NotSupportedException>(() => new CryptographyProcessorFactory(mockOptions));
            Assert.That(e.Message, Does.Contain("Unsupported Encryption algorithm found in options."));
        }

        [Test]
        public void Ctor_OptionsEncryptionAlgorithmAES_AesProcessorReceived()
        {
            var mockOptions = Substitute.For<ICryptographyProcessorOptions>();
            mockOptions.HashingAlgorithm.Returns("SHA512");
            mockOptions.CryptographyKey.Returns("key");
            mockOptions.EncryptionAlgorithm.Returns("AES");

            var factory = new CryptographyProcessorFactory(mockOptions);

            var result = factory.GetEncryptionProcessor();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<AesProcessor>());
        }

        [Test]
        public void Ctor_OptionsEncryptionAlgorithmTripleDes_TripleDesProcessorReceived()
        {
            var mockOptions = Substitute.For<ICryptographyProcessorOptions>();
            mockOptions.HashingAlgorithm.Returns("SHA512");
            mockOptions.CryptographyKey.Returns("key");
            mockOptions.EncryptionAlgorithm.Returns("TripleDes");

            var factory = new CryptographyProcessorFactory(mockOptions);

            var result = factory.GetEncryptionProcessor();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<TripleDesProcessor>());
        }

        [Test]
        public void Ctor_OptionsHashingAlgorithmSHA256_Sha256ProcessorReceived()
        {
            var mockOptions = Substitute.For<ICryptographyProcessorOptions>();
            mockOptions.HashingAlgorithm.Returns("SHA256");
            mockOptions.CryptographyKey.Returns("key");
            mockOptions.EncryptionAlgorithm.Returns("TripleDes");

            var factory = new CryptographyProcessorFactory(mockOptions);

            var result = factory.GetHashingProcessor();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Sha256Processor>());
        }

        [Test]
        public void Ctor_OptionsHashingAlgorithmSHA512_Sha512ProcessorReceived()
        {
            var mockOptions = Substitute.For<ICryptographyProcessorOptions>();
            mockOptions.HashingAlgorithm.Returns("SHA512");
            mockOptions.CryptographyKey.Returns("key");
            mockOptions.EncryptionAlgorithm.Returns("TripleDes");

            var factory = new CryptographyProcessorFactory(mockOptions);

            var result = factory.GetHashingProcessor();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Sha512Processor>());
        }
    }
}