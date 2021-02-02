namespace MediBook.Services.Test.Cryptography.Processors
{
    using System;
    using MediBook.Services.Cryptography.Processors;
    using NUnit.Framework;

    [TestFixture]
    public class AesProcessorTests
    {
        private static string _mockCryptoKey = "N1jrLsHDE0prsWzq2KnXUPaDJi12wmFi";
        private static string _validEncryptedString = "YsNgr8Sch4pUvVir++7UCQ==";
        private static string _validDecryptedString = "TestData";


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

            Assert.That(result, Is.EqualTo(_validEncryptedString));
        }

        [Test]
        public void Decrypt_DataValid_Succeeds()
        {
            var processor = new AesProcessor(_mockCryptoKey);
            var testArray = new byte[8];
            var result = processor.Decrypt(testArray);

            Assert.That(result, Is.EqualTo(testArray));
        }
    }
}
