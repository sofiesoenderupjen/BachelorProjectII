using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using BachelorProjectII.DomainLayer.Models; // Adjust namespace if necessary
using BachelorProjectII.DomainLayer.Services;
using BachelorProjectII.DataLayer.Repositories;

namespace NUnitTests.ServiceTests
{
    [TestFixture]
    public class ApotekServiceTests
    {
        private IApotekRepository _mockRepository;
        private ApotekService _sut;

        [SetUp]
        public void Setup()
        {
            _mockRepository = Substitute.For<IApotekRepository>();
            _sut = new ApotekService(_mockRepository);
        }

        [Test]
        public async Task GetApotekerAsync_ShouldReturnListOfApotekModel()
        {
            // Arrange
            var expectedApoteker = new List<ApotekModel>
            {
                new ApotekModel { Id = 1, ApotekNavn = "Apotek 1", Adresse = "Address 1", TelefonNummer = 12345678 },
                new ApotekModel { Id = 2, ApotekNavn = "Apotek 2", Adresse = "Address 2", TelefonNummer = 87654321 }
            };

            _mockRepository.GetApotekerAsync().Returns(expectedApoteker);

            // Act
            var result = await _sut.GetApotekerAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].ApotekNavn, Is.EqualTo("Apotek 1"));
        }

        [TestCase(1, true)]
        [TestCase(0, false)]
        [TestCase(-1, false)]
        public async Task GetApotekByIdAsync_ShouldReturnApotekModel_OrNullBasedOnId(int apotekId, bool isValid)
        {
            // Arrange
            var expectedApotek = new ApotekModel
            {
                Id = 1,
                ApotekNavn = "Apotek 1",
                Adresse = "Address 1",
                TelefonNummer = 12345678
            };

            _mockRepository.GetApotekByIdAsync(1).Returns(expectedApotek);

            // Act
            var result = await _sut.GetApotekByIdAsync(apotekId);

            // Assert
            if (isValid)
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(expectedApotek.Id));
                Assert.That(result.ApotekNavn, Is.EqualTo(expectedApotek.ApotekNavn));
                Assert.That(result.Adresse, Is.EqualTo(expectedApotek.Adresse));
                Assert.That(result.TelefonNummer, Is.EqualTo(expectedApotek.TelefonNummer));
            }
            else
            {
                Assert.That(result, Is.Null);
            }
        }

        [TestCase(1)]
        [TestCase(0)]
        [TestCase(-1)]
        public async Task GetApotekerWithIndlaegsseddelPaaLagerAsync_ShouldReturnListOfApotekModel(int indlaegsseddelId)
        {
            // Arrange
            var expectedApoteker = new List<ApotekModel>
            {
                new ApotekModel { Id = 1, ApotekNavn = "Apotek 1" },
                new ApotekModel { Id = 2, ApotekNavn = "Apotek 2" }
            };

            _mockRepository.GetApotekerWithIndlaegsseddelPaaLagerAsync(indlaegsseddelId).Returns(expectedApoteker);

            // Act
            var result = await _sut.GetApotekerWithIndlaegsseddelPaaLagerAsync(indlaegsseddelId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].ApotekNavn, Is.EqualTo("Apotek 1"));
        }

        [TestCase(1, 1)]
        [TestCase(1, 0)]
        [TestCase(1, -1)]
        public async Task IsIndlaegsseddelPaaLagerAsync_ShouldReturnTrue_WhenIndlaegsseddelIsOnStock(int apotekId, int indlaegsseddelId)
        {
            // Arrange
            _mockRepository.IsIndlaegsseddelPaaLagerAsync(apotekId, indlaegsseddelId).Returns(indlaegsseddelId > 0);

            // Act
            var result = await _sut.IsIndlaegsseddelPaaLagerAsync(apotekId, indlaegsseddelId);

            // Assert
            Assert.That(result, Is.EqualTo(indlaegsseddelId > 0));
        }

        [TestCase(1, 999)]
        [TestCase(1, 0)]
        [TestCase(1, -999)]
        public async Task IsIndlaegsseddelPaaLagerAsync_ShouldReturnFalse_WhenIndlaegsseddelIsNotOnStock(int apotekId, int indlaegsseddelId)
        {
            // Arrange
            _mockRepository.IsIndlaegsseddelPaaLagerAsync(apotekId, indlaegsseddelId).Returns(false);

            // Act
            var result = await _sut.IsIndlaegsseddelPaaLagerAsync(apotekId, indlaegsseddelId);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}

