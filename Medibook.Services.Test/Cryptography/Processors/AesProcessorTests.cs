namespace MediBook.Services.Test.Cryptography.Processors
{
    using System;
    using System.Linq;
    using MediBook.Services.Cryptography.Processors;
    using NUnit.Framework;

    [TestFixture]
    public class AesProcessorTests
    {
        private static string _mockCryptoKey = "N1jrLsHDE0prsWzq2KnXUPaDJi12wmFi";
        private static string _validEncryptedString = "98,195,96,175,196,156,135,138,84,189,88,171,251,238,212,9";
        private static byte[] _validEncrypteBytes;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validEncrypteBytes = StringToBytes(_validEncryptedString);
        }

        [Test]
        public void Ctor_CryptographyKeyNull_ThrowsArgumentNullException()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new AesProcessor(null));
            Assert.That(e.Message, Does.Contain("cryptographyKey"));
        }

        [Test]
        public void Ctor_CryptographyKeyEmpty_ThrowsArgumentNullException()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new AesProcessor(string.Empty));
            Assert.That(e.Message, Does.Contain("cryptographyKey"));
        }

        [Test]
        public void Encrypt_DataNull_ThrowsArgumentNullException()
        {
            var processor = new AesProcessor(_mockCryptoKey);
            var e = Assert.Throws<ArgumentNullException>(() => processor.Encrypt(null));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void Encrypt_DataEmpty_ThrowsArgumentNullException()
        {
            var processor = new AesProcessor(_mockCryptoKey);
            var e = Assert.Throws<ArgumentNullException>(() => processor.Encrypt(string.Empty));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void Decrypt_DataNull_ThrowsArgumentNullException()
        {
            var processor = new AesProcessor(_mockCryptoKey);
            var e = Assert.Throws<ArgumentNullException>(() => processor.Decrypt(null));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void Encrypt_DataValid_Succeeds()
        {
            var processor = new AesProcessor(_mockCryptoKey);
            var result = processor.Encrypt("TestData");
            var resultAsString = string.Join(",", result);

            Assert.That(resultAsString, Is.EqualTo(_validEncryptedString));
        }

        [Test]
        public void Decrypt_DataValid_Succeeds()
        {
            var processor = new AesProcessor(_mockCryptoKey);
            var result = processor.Decrypt(_validEncrypteBytes);

            Assert.That(result, Is.EqualTo("TestData"));
        }

        private byte[] StringToBytes(string input)
        {
            string[] strings = input.Split(',');
            byte[] bytes = strings.Select(s => byte.Parse(s)).ToArray();

            return bytes;
        }
    }
}
