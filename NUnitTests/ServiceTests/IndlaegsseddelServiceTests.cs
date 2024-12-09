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
    public class IndlaegsseddelServiceTests
    {
        private IIndlaegsseddelRepository _mockRepository;
        private IndlaegsseddelService _sut;

        [SetUp]
        public void Setup()
        {
            _mockRepository = Substitute.For<IIndlaegsseddelRepository>();
            _sut = new IndlaegsseddelService(_mockRepository);
        }

        [Test]
        public async Task GetIndlaegssedler_ShouldReturnListOfIndlaegsseddelModel()
        {
            // Arrange
            var expectedIndlaegssedler = new List<IndlaegsseddelModel>
            {
                new IndlaegsseddelModel { Id = 1, Navn = "Test 1", FormOgStyrke = "Form 1", Virksomhed = "Company 1", Indlaegsseddel = "Indlaegsseddel 1" },
                new IndlaegsseddelModel { Id = 2, Navn = "Test 2", FormOgStyrke = "Form 2", Virksomhed = "Company 2", Indlaegsseddel = "Indlaegsseddel 2" }
            };

            _mockRepository.GetIndlaegssedler().Returns(expectedIndlaegssedler);

            // Act
            var result = await _sut.GetIndlaegssedler();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Navn, Is.EqualTo("Test 1"));
        }

        [TestCase(1, true)]
        [TestCase(0, false)]
        [TestCase(-1, false)]
        public async Task GetIndlaegsseddelById_ShouldReturnIndlaegsseddelModel_OrNullBasedOnId(int id, bool isValid)
        {
            // Arrange
            var expectedIndlaegsseddel = new IndlaegsseddelModel
            {
                Id = 1,
                Navn = "Test",
                FormOgStyrke = "Form",
                Virksomhed = "Company",
                Indlaegsseddel = "Indlaegsseddel"
            };

            _mockRepository.GetIndlaegsseddelById(1).Returns(expectedIndlaegsseddel);

            // Act
            var result = await _sut.GetIndlaegsseddelById(id);

            // Assert
            if (isValid)
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(expectedIndlaegsseddel.Id));
                Assert.That(result.Navn, Is.EqualTo(expectedIndlaegsseddel.Navn));
                Assert.That(result.FormOgStyrke, Is.EqualTo(expectedIndlaegsseddel.FormOgStyrke));
                Assert.That(result.Virksomhed, Is.EqualTo(expectedIndlaegsseddel.Virksomhed));
                Assert.That(result.Indlaegsseddel, Is.EqualTo(expectedIndlaegsseddel.Indlaegsseddel));
            }
            else
            {
                Assert.That(result, Is.Null);
            }
        }
    }
}
