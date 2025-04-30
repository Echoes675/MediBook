namespace MediBook.Services.Test.Cryptography
{
    using System;
    using MediBook.Services.Cryptography;
    using Microsoft.Extensions.Configuration;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class DefaultCryptographyProcessorOptionsTests
    {
        [Test]
        public void Ctor_ConfigNull_ThrowsArgumentNullException()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new DefaultCryptographyProcessorOptions(null));
            Assert.That(e.Message, Does.Contain("config"));
        }

        [Test]
        public void Ctor_ConfigNotPopulated_PropertiesAreNull()
        {
            var mockConfig = Substitute.For<IConfiguration>();
            var options = new DefaultCryptographyProcessorOptions(mockConfig);

            Assert.That(options.EncryptionAlgorithm, Is.Empty);
            Assert.That(options.HashingAlgorithm, Is.Empty);
            Assert.That(options.CryptographyKey, Is.Empty);
        }

        [Test]
        public void Ctor_ConfigPopulated_PropertiesAreNull()
        {
            var mockConfig = Substitute.For<IConfiguration>();
            mockConfig.GetSection("DefaultCryptographyOptions:CryptographyKey").Value.Returns("cryptographyKey");
            mockConfig.GetSection("DefaultCryptographyOptions:EncryptionAlgorithm").Value.Returns("encryptionAlgorithm");
            mockConfig.GetSection("DefaultCryptographyOptions:HashingAlgorithm").Value.Returns("hashingAlgorithm");
            var options = new DefaultCryptographyProcessorOptions(mockConfig);

            Assert.That(options.EncryptionAlgorithm, Is.EqualTo("encryptionAlgorithm"));
            Assert.That(options.HashingAlgorithm, Is.EqualTo("hashingAlgorithm"));
            Assert.That(options.CryptographyKey, Is.EqualTo("cryptographyKey"));
        }
    }
}