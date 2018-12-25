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
    public class LanguageControllerFixtures
    {
        [Fact]
        public async Task It_Can_Get_Languages()
        {
            // Arrange
            var languageService = new Mock<ILanguageService>();
            languageService.Setup(m => m.GetAll()).ReturnsAsync(new List<Model.Language>
            {
                new Model.Language { Id = 1},
                new Model.Language { Id = 2 }
            }.AsEnumerable());

            var controller = SetUp(languageService);

            // Act
            var actual = await controller.Get();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count());
        }

        [Fact]
        public async Task It_Can_Post_Entry()
        {
            // Arrange
            var languageService = new Mock<ILanguageService>();
            var controller = SetUp(languageService);

            // Act
            var result = await controller.Post(Language) as OkResult;

            // Assert
            Assert.NotNull(result);
            languageService.Verify(m => m.Add(It.IsAny<Model.Language>()), Times.Once);
        }

        [Fact]
        public async Task It_Returns_ErrorResult_On_Faulty_Post()
        {
            // Arrange
            var languageService = new Mock<ILanguageService>();
            languageService.Setup(m => m.Add(It.IsAny<Model.Language>())).Throws(new ApplicationException("Test error"));
            var controller = SetUp(languageService);

            // Act
            var result = await controller.Post(Language) as BadRequestObjectResult;
            var errorResponse = result.Value as ApiResponse;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test error", errorResponse.Message);
        }

        [Fact]
        public async Task It_Can_Put_Entry()
        {
            // Arrange
            Mock<ILanguageService> languageService = GetLanguageServiceMock(true);
            var controller = SetUp(languageService);

            // Act
            var result = await controller.Put(Language) as OkResult;

            // Assert
            Assert.NotNull(result);
            languageService.Verify(m => m.Update(It.IsAny<Model.Language>()), Times.Once);
        }

        [Fact]
        public async Task It_Returns_ErrorResult_On_Faulty_Put()
        {
            // Arrange
            var languageService = GetLanguageServiceMock(true);
            languageService.Setup(m => m.Update(It.IsAny<Model.Language>())).Throws(new ApplicationException("Test error"));
            var controller = SetUp(languageService);

            // Act
            var result = await controller.Put(Language) as BadRequestObjectResult;
            var errorResponse = result.Value as ApiResponse;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test error", errorResponse.Message);
        }

        [Fact]
        public async Task It_Returns_NotFound_On_Missing_Key_Put()
        {
            // Arrange
            LanguageController controller = SetUp();

            // Act
            var result = await controller.Put(Language) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task It_Can_Delete_Entry()
        {
            // Arrange
            var languageService = GetLanguageServiceMock(true);
            var controller = SetUp(languageService);

            // Act
            var result = await controller.Delete("nl-NL") as OkResult;

            // Assert
            Assert.NotNull(result);
            languageService.Verify(m => m.Delete(It.Is<string>(l => l == "nl-NL")), Times.Once);
        }

        [Fact]
        public async Task It_Returns_ErrorResult_On_Faulty_Delete()
        {
            // Arrange
            var languageService = GetLanguageServiceMock(true);
            languageService.Setup(m => m.Delete(It.Is<string>(l => l == "nl-NL"))).Throws(new ApplicationException("Test error"));

            var controller = SetUp(languageService);

            // Act
            var result = await controller.Delete("nl-NL") as BadRequestObjectResult;
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
            var result = await controller.Delete("nl-NL") as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        private static LanguageController SetUp(
          Mock<ILanguageService> languageService = null,
          Mock<ILogger<LanguageController>> logger = null)
        {
            languageService = languageService ?? new Mock<ILanguageService>();
            logger = logger ?? new Mock<ILogger<LanguageController>>();

            var controller = new LanguageController(logger.Object, languageService.Object);
            return controller;
        }

        private static Mock<ILanguageService> GetLanguageServiceMock(bool exists)
        {
            var languageService = new Mock<ILanguageService>();
            languageService.Setup(m => m.ExistsAsync(It.IsAny<string>())).ReturnsAsync(exists);
            return languageService;
        }

        private static Language Language => new Language { Key = "nl-NL", Name = "Dutch" };
    }
}