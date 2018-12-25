using DexTranslate.Abstractions.Service;
using DexTranslate.Api.Controllers;
using DexTranslate.ApiContract.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DexTranslate.ApiFixtures
{
    public class TranslationControllerFixtures
    {
        [Fact]
        public async Task It_Can_Get_All()
        {
            // Arrange
            var service = new Mock<ITranslationService>();
            service.Setup(m => m.GetAll(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<Model.Translation> {
                new Model.Translation{ Id = 1},
                new Model.Translation { Id = 2}
            }.AsEnumerable());

            var controller = SetUp(service);

            // Act
            var actual = await controller.Get("nl-NL", "crm");

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count());
        }

        [Fact]
        public async Task It_Can_Get_Single()
        {
            // Arrange
            var service = new Mock<ITranslationService>();
            service.Setup(m => m.GetByKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Model.Translation { Id = 1, Text = "name1" });

            var controller = SetUp(service);

            // Act
            var actual = await controller.Get("nl-NL", "crm", "name1");

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("name1", actual.Text);
        }

        [Fact]
        public async Task It_Can_Post_Entry()
        {
            // Arrange
            var service = new Mock<ITranslationService>();
            var controller = SetUp(service);

            // Act
            var result = await controller.Post(Translation) as OkResult;

            // Assert
            Assert.NotNull(result);
            service.Verify(m => m.Add(It.IsAny<Model.Translation>()), Times.Once);
        }

        [Fact]
        public async Task It_Returns_ErrorResult_On_Duplicate_Key_Post()
        {
            // Arrange
            var service = GetTranslationServiceMock(true);
            var controller = SetUp(service);

            // Act
            var result = await controller.Post(Translation) as ConflictObjectResult;
            var errorResponse = result?.Value as ApiResponse;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("A translation with this key already exists", errorResponse.Message);
        }

        [Fact]
        public async Task It_Returns_ErrorResult_On_Faulty_Post()
        {
            // Arrange
            var service = new Mock<ITranslationService>();
            service.Setup(m => m.Add(It.IsAny<Model.Translation>())).Throws(new ApplicationException("Test error"));
            var controller = SetUp(service);

            // Act
            var result = await controller.Post(Translation) as BadRequestObjectResult;
            var errorResponse = result.Value as ApiResponse;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test error", errorResponse.Message);
        }

        [Fact]
        public async Task It_Can_Put_Entry()
        {
            // Arrange
            var service = GetTranslationServiceMock(true);
            var controller = SetUp(service);

            // Act
            var result = await controller.Put(Translation) as OkResult;

            // Assert
            Assert.NotNull(result);
            service.Verify(m => m.Update(It.IsAny<Model.Translation>()), Times.Once);
        }

        [Fact]
        public async Task It_Returns_ErrorResult_On_Faulty_Put()
        {
            // Arrange
            var service = GetTranslationServiceMock(true);
            service.Setup(m => m.Update(It.IsAny<Model.Translation>())).Throws(new ApplicationException("Test error"));
            var controller = SetUp(service);

            // Act
            var result = await controller.Put(Translation) as BadRequestObjectResult;
            var errorResponse = result.Value as ApiResponse;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test error", errorResponse.Message);
        }

        [Fact]
        public async Task It_Returns_NotFound_On_Missing_Key_Put()
        {
            // Arrange
            var controller = SetUp();

            // Act
            var result = await controller.Put(Translation) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task It_Can_Delete_Entry()
        {
            // Arrange
            var service = GetTranslationServiceMock(true);
            var controller = SetUp(service);

            // Act
            var result = await controller.Delete("nl-NL", "dexgrid", "name1") as OkResult;

            // Assert
            Assert.NotNull(result);
            service.Verify(m => m.DeleteAsync(It.Is<string>(l => l == "nl-NL"), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task It_Returns_ErrorResult_On_Faulty_Delete()
        {
            // Arrange
            var service = GetTranslationServiceMock(true);
            service.Setup(m => m.DeleteAsync(It.Is<string>(l => l == "nl-NL"), It.IsAny<string>(), It.IsAny<string>())).Throws(new ApplicationException("Test error"));

            var controller = SetUp(service);

            // Act
            var result = await controller.Delete("nl-NL", "dexgrid", "name1") as BadRequestObjectResult;
            var errorResponse = result.Value as ApiResponse;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test error", errorResponse.Message);
        }

        [Fact]
        public async Task It_Returns_NotFound_On_Missing_Key_Delete()
        {
            // Arrange
            var controller = SetUp();

            // Act
            var result = await controller.Delete("nl-NL", "dexgrid", "name1") as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        private static TranslationController SetUp(
          Mock<ITranslationService> service = null,
          Mock<ILogger<TranslationController>> logger = null)
        {
            service = service ?? new Mock<ITranslationService>();
            logger = logger ?? new Mock<ILogger<TranslationController>>();

            var controller = new TranslationController(logger.Object, service.Object);
            return controller;
        }

        private static Mock<ITranslationService> GetTranslationServiceMock(bool exists)
        {
            var service = new Mock<ITranslationService>();
            service.Setup(m => m.ExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(exists);
            return service;
        }

        private static Translation Translation => new Translation { Key = "nl-NL", LanguageKey = "nl-NL", ProjectKey = "dexgrid", Text = "Dutch" };
    }
}