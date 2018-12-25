using DexTranslate.Abstractions.Caching;
using DexTranslate.Abstractions.Repository;
using DexTranslate.Core;
using DexTranslate.Model;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DexTranslate.CoreFixtures
{
    public class ProjectServiceFixtures
    {
        [Fact]
        public async Task It_Can_Add_ProjectAsync()
        {
            // Arrange
            var repository = new Mock<IProjectRepository>();
            var service = SetUp(repository: repository);

            // Act
            await service.Add(ExpectedProject);

            // Assert
            repository.Verify(m => m.Add(It.Is<Project>(l => l.Key == ExpectedProject.Key)), Times.Once);
        }

        [Fact]
        public async Task It_Clears_Cache_After_Add()
        {
            // Arrange
            var cache = new Mock<IProjectCache>();
            var service = SetUp(cache);

            // Act
            await service.Add(ExpectedProject);

            // Assert
            cache.Verify(m => m.Clear(), Times.Once);
        }

        [Fact]
        public async Task It_Can_Update_ProjecteAsync()
        {
            // Arrange
            var repository = new Mock<IProjectRepository>();
            var service = SetUp(repository: repository);

            // Act
            await service.Update(ExpectedProject);

            // Assert
            repository.Verify(m => m.Update(It.Is<Project>(i => i.Key == ExpectedProject.Key)));
        }

        [Fact]
        public async Task It_Clears_Cache_After_Update()
        {
            // Arrange
            var cache = new Mock<IProjectCache>();
            var service = SetUp(cache);

            // Act
            await service.Update(ExpectedProject);

            // Assert
            cache.Verify(m => m.Clear(), Times.Once);
        }

        [Fact]
        public async Task It_Can_Delete_Project()
        {
            // Arrange
            var repository = new Mock<IProjectRepository>();
            var service = SetUp(repository: repository);

            // Act
            await service.Delete("nl-NL");

            // Assert
            repository.Verify(m => m.Delete(It.Is<string>(i => i == "nl-NL")));
        }

        [Fact]
        public async Task It_Clears_Cache_After_Delete()
        {
            // Arrange
            var cache = new Mock<IProjectCache>();
            var service = SetUp(cache);

            // Act
            await service.Delete(ExpectedProject.Key);

            // Assert
            cache.Verify(m => m.Clear(), Times.Once);
        }

        [Fact]
        public async Task It_Can_Check_If_Project_Exists()
        {
            // Arrange
            var cache = new Mock<IProjectCache>();
            cache.Setup(m => m.Get()).ReturnsAsync(() => ExpectedProjects);

            var service = SetUp(cache);

            // Act
            var actual = await service.ExistsAsync("bookshop");

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task It_Can_Get_All_ProjectAsync()
        {
            // Arrange
            var cache = new Mock<IProjectCache>();
            cache.Setup(m => m.Get()).ReturnsAsync(() => ExpectedProjects);

            var service = SetUp(cache);

            // Act
            var actual = await service.GetAll();

            // Assert

            Assert.Equal(ExpectedProjects.Count(), actual.Count());
            actual.Should().BeEquivalentTo(ExpectedProjects);
        }

        private static ProjectService SetUp(
            Mock<IProjectCache> cache = null,
            Mock<IProjectRepository> repository = null)
        {
            cache = cache ?? new Mock<IProjectCache>();
            repository = repository ?? new Mock<IProjectRepository>();

            return new ProjectService(cache.Object, repository.Object);
        }

        private static Project ExpectedProject => new Project
        {
            Id = 1,
            Key = "bookshop",
            Title = "Bookshop webshop frontend"
        };

        private static List<Project> ExpectedProjects => new List<Project>
        {
            new Project { Id = 1, Key = "bookshop", Title = "Bookshop webshop frontend"},
            new Project { Id = 2, Key = "backoffice", Title = "Backoffice"}
        };
    }
}