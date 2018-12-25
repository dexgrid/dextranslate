using DexTranslate.Abstractions.Service;
using DexTranslate.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DexTranslate.ApiFixtures
{
    public class ExportControllerFixtures
    {
        [Fact]
        public async Task It_Can_Export_Csv_File()
        {
            // Arrange
            var service = new Mock<IExportService>();
            service.Setup(m => m.GetFileExportAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Encoding.UTF8.GetBytes("this is a csv file"));
            var controller = SetUp(service);

            // Act
            var file = new Mock<IFormFile>();
            var result = await controller.Get("nl-NL", "webshop") as FileContentResult;

            // Assert
            Assert.NotNull(result);
            service.Verify(s => s.GetFileExportAsync(It.Is<string>(l => l == "nl-NL"), It.Is<string>(l => l == "webshop")), Times.Once);
        }

        private static ExportController SetUp(
            Mock<IExportService> service = null,
            Mock<ILogger<ExportController>> logger = null)
        {
            service = service ?? new Mock<IExportService>();
            logger = logger ?? new Mock<ILogger<ExportController>>();

            var controller = new ExportController(logger.Object, service.Object);
            return controller;
        }
    }
}