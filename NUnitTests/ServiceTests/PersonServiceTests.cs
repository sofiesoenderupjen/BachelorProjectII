using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.DomainLayer.Services;
using BachelorProjectII.DataLayer.Repositories;

namespace NUnitTests.ServiceTests
{
    [TestFixture]
    public class PersonServiceTests
    {
        private IPersonRepository _mockRepository;
        private PersonService _sut;

        [SetUp]
        public void Setup()
        {
            _mockRepository = Substitute.For<IPersonRepository>();
            _sut = new PersonService(_mockRepository);
        }

        [TestCase(1, "Person One", 30, "1993-01-01", "12345-abcde", true)]
        [TestCase(2, "Person Two", 25, "1998-01-01", "54321-edcba", false)]
        [TestCase(3, "Invalid User", -1, "2000-01-01", "", false)]
        public async Task CreatePersonAsync_ShouldReturnExpectedResult(
            int id, 
            string navn, 
            int alder, 
            string fodselsdato, 
            string uuid,
            bool expectedResult)
        {
            // Arrange
            var person = new PersonModel
            {
                Id = id,
                Navn = navn,
                Alder = alder,
                Fodselsdato = fodselsdato,
                UUID = uuid
            };

            _mockRepository.CreatePersonAsync(person).Returns(expectedResult);

            // Act
            var result = await _sut.CreatePersonAsync(person);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetPersonerAsync_ShouldReturnListOfPersonModel()
        {
            // Arrange
            var expectedPersons = new List<PersonModel>
        {
            new PersonModel { Id = 1, Navn = "Person 1" },
            new PersonModel { Id = 2, Navn = "Person 2" }
        };

            _mockRepository.GetPersonerAsync().Returns(expectedPersons);

            // Act
            var result = await _sut.GetPersonerAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Navn, Is.EqualTo("Person 1"));
        }

        [TestCase("valid-uuid")]
        [TestCase("non-existent-uuid")]
        [TestCase("")]
        public async Task GetPersonByUUIDAsync_ShouldReturnPersonModel_OrNullBasedOnUUID(string uuid)
        {
            // Arrange
            var expectedPerson = new PersonModel { Id = 1, Navn = "Person", UUID = "valid-uuid" };
            var gemteIndlaegssedler = new List<IndlaegsseddelModel> { new IndlaegsseddelModel { Id = 1, Navn = "Indlægsseddel 1" } };
            var receptIndlaegssedler = new List<IndlaegsseddelModel> { new IndlaegsseddelModel { Id = 2, Navn = "Recept Indlægsseddel" } };

            _mockRepository.GetPersonByUUIDAsync("valid-uuid").Returns(expectedPerson);
            _mockRepository.GetGemteIndlaegssedlerAsync(expectedPerson.Id).Returns(gemteIndlaegssedler);
            _mockRepository.GetReceptIndlaegssedlerAsync(expectedPerson.Id).Returns(receptIndlaegssedler);

            // Act
            var result = await _sut.GetPersonByUUIDAsync(uuid);

            // Assert
            if (uuid == "valid-uuid")
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(1));
                Assert.That(result.GemteIndlaegssedler, Is.EqualTo(gemteIndlaegssedler));
                Assert.That(result.ReceptIndlaegssedler, Is.EqualTo(receptIndlaegssedler));
            }
            else
            {
                Assert.That(result, Is.Null);
            }
        }

        [TestCase(1, 1234)]
        [TestCase(0, 5678)]
        [TestCase(-1, 4321)]
        public async Task UpdatePinkodeAsync_ShouldReturnTrue_WhenPinKodeUpdated(int personId, int newPinKode)
        {
            // Arrange
            _mockRepository.UpdatePinkodeAsync(personId, newPinKode).Returns(true);

            // Act
            var result = await _sut.UpdatePinkodeAsync(personId, newPinKode);

            // Assert
            Assert.That(result, Is.True);
        }

        [TestCase(1, 10)]
        [TestCase(0, 10)]
        [TestCase(-1, 10)]
        public async Task SetFortrukneApotekAsync_ShouldReturnTrue_WhenFortrukneApotekSet(int personId, int apotekId)
        {
            // Arrange
            _mockRepository.SetFortrukneApotekAsync(personId, apotekId).Returns(true);

            // Act
            var result = await _sut.SetFortrukneApotekAsync(personId, apotekId);

            // Assert
            Assert.That(result, Is.True);
        }

        [TestCase(1, 5)]
        [TestCase(0, 5)]
        [TestCase(-1, 5)]
        public async Task AddGemteIndlaegsseddelAsync_ShouldReturnTrue_WhenIndlaegsseddelAdded(int personId, int indlaegsseddelId)
        {
            // Arrange
            _mockRepository.AddGemteIndlaegsseddelAsync(personId, indlaegsseddelId).Returns(true);

            // Act
            var result = await _sut.AddGemteIndlaegsseddelAsync(personId, indlaegsseddelId);

            // Assert
            Assert.That(result, Is.True);
        }

        [TestCase(1, 5)]
        [TestCase(0, 5)]
        [TestCase(-1, 5)]
        public async Task RemoveGemteIndlaegsseddelAsync_ShouldReturnTrue_WhenIndlaegsseddelRemoved(int personId, int indlaegsseddelId)
        {
            // Arrange
            _mockRepository.RemoveGemteIndlaegsseddelAsync(personId, indlaegsseddelId).Returns(true);

            // Act
            var result = await _sut.RemoveGemteIndlaegsseddelAsync(personId, indlaegsseddelId);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
