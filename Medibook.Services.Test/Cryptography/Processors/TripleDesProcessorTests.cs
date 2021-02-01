namespace MediBook.Services.Test.Cryptography.Processors
{
    using System;
    using MediBook.Services.Cryptography.Processors;
    using NUnit.Framework;

    [TestFixture]
    public class TripleDesProcessorTests
    {
        private static string _mockCryptoKey = "N1jrLsHDE0prsWzq2KnXUPaDJi12wmFi";
        private static string _validEncryptedString = "w6bDfcYaQB9LaW7YJkX9Mg==";
        private static string _validDecryptedString = "TestData";


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
        public void Decrypt_DataEmpty_ThrowsArgumentNullException()
        {
            var processor = new TripleDesProcessor(_mockCryptoKey);
            var e = Assert.Throws<ArgumentNullException>(() => processor.Decrypt(string.Empty));
            Assert.That(e.Message, Does.Contain("data"));
        }

        [Test]
        public void Encrypt_DataValid_Succeeds()
        {
            var processor = new TripleDesProcessor(_mockCryptoKey);
            var result = processor.Encrypt("TestData");

            Assert.That(result, Is.EqualTo(_validEncryptedString));
        }

        [Test]
        public void Decrypt_DataValid_Succeeds()
        {
            var processor = new TripleDesProcessor(_mockCryptoKey);
            var result = processor.Decrypt(_validEncryptedString);

            Assert.That(result, Is.EqualTo(_validDecryptedString));
        }
    }
}
