using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileEditor.Controllers;
using ProfileEditor.Models;
using ProfileEditor.Services.ImageStore;
using ProfileEditor.Services.Persons;

namespace ProfileEditorTests.Controllers {
    public class PersonControllerTests {
        private readonly Mock<IPersonsService> _mockPersonsService;
        private readonly PersonController _controller;

        public PersonControllerTests() {
            _mockPersonsService = new Mock<IPersonsService>();
            _controller = new PersonController(_mockPersonsService.Object, Mock.Of<IImageStoreService>());
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfPersons() {
            // Arrange
            var persons = new List<PersonVm> {
                generatePerson("John", "Doe"),
                generatePerson("Jane", "Doe")
            };
            _mockPersonsService.Setup(service => service.GetPersonsAsync()).ReturnsAsync(persons);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<PersonVm>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull() {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithPerson() {
            // Arrange
            var person = generatePerson("John", "Doe");
            _mockPersonsService.Setup(service => service.GetPersonAsync(person.Id)).ReturnsAsync(person);

            // Act
            var result = await _controller.Details(person.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PersonVm>(viewResult.ViewData.Model);
            Assert.Equal(person.Id, model.Id);
        }


        private PersonVm generatePerson(string firstName, string lastName) {
            return new PersonVm {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = $"{firstName}.{lastName}@example.com"
            };
        }
    }
}