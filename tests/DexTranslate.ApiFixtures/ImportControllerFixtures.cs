using DexTranslate.Abstractions.Service;
using DexTranslate.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace DexTranslate.ApiFixtures
{
    public class ImportControllerFixtures
    {
        [Fact]
        public async Task It_Can_Import_Csv_File()
        {
            // Arrange
            var service = new Mock<IImportService>();
            var controller = SetUp(service);

            // Act
            var file = new Mock<IFormFile>();
            var result = await controller.Post(file.Object, "nl-NL", true) as OkResult;

            // Assert
            Assert.NotNull(result);
            service.Verify(s => s.ImportTranslations(It.Is<string>(l => l == "nl-NL"), It.Is<bool>(m => m), It.IsAny<Stream>()), Times.Once);
        }

        private static ImportController SetUp(
            Mock<IImportService> service = null,
            Mock<ILogger<ImportController>> logger = null)
        {
            service = service ?? new Mock<IImportService>();
            logger = logger ?? new Mock<ILogger<ImportController>>();

            var controller = new ImportController(logger.Object, service.Object);
            return controller;
        }
    }
}