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
    public class ProjectControllerFixtures
    {
        [Fact]
        public async Task It_Can_Get_ProjectsAsync()
        {
            // Arrange
            var service = new Mock<IProjectService>();
            service.Setup(m => m.GetAll()).ReturnsAsync(new List<Model.Project> {
                new Model.Project{ Id = 1},
                new Model.Project { Id = 2}
            }.AsEnumerable());
            var controller = SetUp(service);

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
            var service = new Mock<IProjectService>();
            var controller = SetUp(service);

            // Act
            var result = await controller.Post(Project) as OkResult;

            // Assert
            Assert.NotNull(result);
            service.Verify(m => m.Add(It.IsAny<Model.Project>()), Times.Once);
        }

        [Fact]
        public async Task It_Returns_ErrorResult_On_Duplicate_Key_Post()
        {
            // Arrange
            var service = GetProjectServiceMock(true);
            var controller = SetUp(service);

            // Act
            var result = await controller.Post(Project) as ConflictObjectResult;
            var errorResponse = result?.Value as ApiResponse;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("A project with this key already exists", errorResponse.Message);
        }

        [Fact]
        public async Task It_Returns_ErrorResult_On_Faulty_Post()
        {
            // Arrange
            var service = new Mock<IProjectService>();
            service.Setup(m => m.Add(It.IsAny<Model.Project>())).Throws(new ApplicationException("Test error"));
            var controller = SetUp(service);

            // Act
            var result = await controller.Post(Project) as BadRequestObjectResult;
            var errorResponse = result.Value as ApiResponse;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test error", errorResponse.Message);
        }

        [Fact]
        public async Task It_Can_Put_Entry()
        {
            // Arrange
            Mock<IProjectService> service = GetProjectServiceMock(true);
            var controller = SetUp(service);

            // Act
            var result = await controller.Put(Project) as OkResult;

            // Assert
            Assert.NotNull(result);
            service.Verify(m => m.Update(It.IsAny<Model.Project>()), Times.Once);
        }

        [Fact]
        public async Task It_Returns_ErrorResult_On_Faulty_Put()
        {
            // Arrange
            var service = GetProjectServiceMock(true);
            service.Setup(m => m.Update(It.IsAny<Model.Project>())).Throws(new ApplicationException("Test error"));
            var controller = SetUp(service);

            // Act
            var result = await controller.Put(Project) as BadRequestObjectResult;
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
            var result = await controller.Put(Project) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task It_Can_Delete_Entry()
        {
            // Arrange
            var service = GetProjectServiceMock(true);
            var controller = SetUp(service);

            // Act
            var result = await controller.Delete("nl-NL") as OkResult;

            // Assert
            Assert.NotNull(result);
            service.Verify(m => m.Delete(It.Is<string>(l => l == "nl-NL")), Times.Once);
        }

        [Fact]
        public async Task It_Returns_ErrorResult_On_Faulty_Delete()
        {
            // Arrange
            var service = GetProjectServiceMock(true);
            service.Setup(m => m.Delete(It.Is<string>(l => l == "nl-NL"))).Throws(new ApplicationException("Test error"));

            var controller = SetUp(service);

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

        private static ProjectController SetUp(
          Mock<IProjectService> service = null,
          Mock<ILogger<ProjectController>> logger = null)
        {
            service = service ?? new Mock<IProjectService>();
            logger = logger ?? new Mock<ILogger<ProjectController>>();

            var controller = new ProjectController(logger.Object, service.Object);
            return controller;
        }

        private static Mock<IProjectService> GetProjectServiceMock(bool exists)
        {
            var projectService = new Mock<IProjectService>();
            projectService.Setup(m => m.ExistsAsync(It.IsAny<string>())).ReturnsAsync(exists);
            return projectService;
        }

        private static Project Project => new Project { Key = "nl-NL", Title = "Dutch" };
    }
}