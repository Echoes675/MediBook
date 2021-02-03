namespace MediBook.Services.Test.Cryptography.Processors
{
    using System;
    using System.Linq;
    using MediBook.Services.Cryptography.Processors;
    using NUnit.Framework;

    [TestFixture]
    public class TripleDesProcessorTests
    {
        private static string _mockCryptoKey = "N1jrLsHDE0prsWzq2KnXUPaDJi12wmFi";
        private static string _validEncryptedString = "195,166,195,125,198,26,64,31,75,105,110,216,38,69,253,50";
        private static byte[] _validEncrypteBytes;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validEncrypteBytes = StringToBytes(_validEncryptedString);
        }

        [Test]
        public void Ctor_CryptographyKeyNull_ThrowsArgumentNullException()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new TripleDesProcessor(null));
            Assert.That(e.Message, Does.Contain("cryptographyKey"));
        }

        [Test]
        public void Ctor_CryptographyKeyEmpty_ThrowsArgumentNullException()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new TripleDesProcessor(string.Empty));
            Assert.That(e.Message, Does.Contain("cryptographyKey"));
        }

        [Test]
        public void Encrypt_DataNull_ThrowsArgumentNullException()
        {
            var processor = new TripleDesProcessor(_mockCryptoKey);
            var e = Assert.Throws<ArgumentNullException>(() => processor.Encrypt(null));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void Encrypt_DataEmpty_ThrowsArgumentNullException()
        {
            var processor = new TripleDesProcessor(_mockCryptoKey);
            var e = Assert.Throws<ArgumentNullException>(() => processor.Encrypt(string.Empty));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void Decrypt_DataNull_ThrowsArgumentNullException()
        {
            var processor = new TripleDesProcessor(_mockCryptoKey);
            var e = Assert.Throws<ArgumentNullException>(() => processor.Decrypt(null));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void Encrypt_DataValid_Succeeds()
        {
            var processor = new TripleDesProcessor(_mockCryptoKey);
            var result = processor.Encrypt("TestData");

            var resultAsString = string.Join(",", result);

            Assert.That(resultAsString, Is.EqualTo(_validEncryptedString));
        }

        [Test]
        public void Decrypt_DataValid_Succeeds()
        {
            var processor = new TripleDesProcessor(_mockCryptoKey);
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
