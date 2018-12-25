using DexTranslate.Abstractions.Caching;
using DexTranslate.Abstractions.Repository;
using DexTranslate.Abstractions.Service;
using DexTranslate.Core;
using DexTranslate.Model;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DexTranslate.CoreFixtures
{
    public class TranslationServiceFixtures
    {
        [Fact]
        public async Task It_Can_Add_TranslationAsync()
        {
            // Arrange
            var repository = new Mock<ITranslationRepository>();
            var service = SetUp(repository: repository);

            // Act
            await service.Add(ExpectedTranslation);

            // Assert
            repository.Verify(m => m.Add(It.Is<Translation>(l => l.Key == ExpectedTranslation.Key)), Times.Once);
        }

        [Fact]
        public async Task It_Clears_Cache_After_Add()
        {
            // Arrange
            var cache = new Mock<ITranslationCache>();
            var service = SetUp(cache);

            // Act
            await service.Add(ExpectedTranslation);

            // Assert
            cache.Verify(m => m.Clear(It.Is<string>(i => i == ExpectedTranslation.LanguageKey), It.Is<string>(i => i == ExpectedTranslation.ProjectKey)), Times.Once);
        }

        [Fact]
        public async Task It_Can_Update_TranslationAsync()
        {
            // Arrange
            var repository = new Mock<ITranslationRepository>();
            var service = SetUp(repository: repository);

            // Act
            await service.Update(ExpectedTranslation);

            // Assert
            repository.Verify(m => m.Update(It.Is<Translation>(i => i.Key == ExpectedTranslation.Key)));
        }

        [Fact]
        public async Task It_Throws_When_Updating_Invalid_TranslationAsync()
        {
            // Arrange
            var service = SetUp();
            // Act / Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.Update(new Translation { }));
        }

        [Fact]
        public async Task It_Throws_When_Updating_Translation_Project_Does_Not_Exist_Async()
        {
            // Arrange
            var projectService = new Mock<IProjectService>();
            projectService.Setup(m => m.ExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            var service = SetUp(projectService: projectService);

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => service.Update(new Translation { ProjectKey = "shop", LanguageKey = "en-US", Key = "a1" }));
        }

        [Fact]
        public async Task It_Throws_When_Updating_Translation_Language_Does_Not_Exist_Async()
        {
            // Arrange
            var languageService = new Mock<ILanguageService>();
            languageService.Setup(m => m.ExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            var service = SetUp(languageService: languageService);

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => service.Update(new Translation { ProjectKey = "shop", LanguageKey = "en-DE", Key = "a1" }));
        }

        [Fact]
        public async Task It_Clears_Cache_After_Update()
        {
            // Arrange
            var cache = new Mock<ITranslationCache>();
            var service = SetUp(cache);

            // Act
            await service.Update(ExpectedTranslation);

            // Assert
            cache.Verify(m => m.Clear(It.Is<string>(i => i == ExpectedTranslation.LanguageKey), It.Is<string>(i => i == ExpectedTranslation.ProjectKey)), Times.Once);
        }

        [Fact]
        public async Task It_Can_Delete_Translation()
        {
            // Arrange
            var repository = new Mock<ITranslationRepository>();
            var service = SetUp(repository: repository);

            // Act
            await service.DeleteAsync(ExpectedTranslation.LanguageKey, ExpectedTranslation.ProjectKey, ExpectedTranslation.Key);

            // Assert
            repository.Verify(m => m.Delete(It.Is<string>(i => i == "nl-NL"), It.Is<string>(i => i == "bookshop"), It.Is<string>(i => i == "page_title")));
        }

        [Fact]
        public async Task It_Clears_Cache_After_Delete()
        {
            // Arrange
            var cache = new Mock<ITranslationCache>();
            var service = SetUp(cache);

            // Act
            await service.DeleteAsync(ExpectedTranslation.LanguageKey, ExpectedTranslation.ProjectKey, ExpectedTranslation.Key);

            // Assert
            cache.Verify(m => m.Clear(ExpectedTranslation.LanguageKey, ExpectedTranslation.ProjectKey), Times.Once);
        }

        [Fact]
        public async Task It_Can_Check_If_Translation_Exists()
        {
            // Arrange
            var cache = new Mock<ITranslationCache>();
            cache.Setup(m => m.Get(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => ExpectedTranslations);

            var service = SetUp(cache);

            // Act
            var actual = await service.ExistsAsync("en-US", "bookshop", "page_title");

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task It_Can_Get_All_TranslationAsync()
        {
            // Arrange
            var cache = new Mock<ITranslationCache>();
            cache.Setup(m => m.Get(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => ExpectedTranslations);

            var service = SetUp(cache);

            // Act
            var actual = await service.GetAll("en-US", "bookshop");

            // Assert

            Assert.Equal(ExpectedTranslations.Count(), actual.Count());
            actual.Should().BeEquivalentTo(ExpectedTranslations);
        }

        [Fact]
        public async Task It_Can_Get_Translation_By_Key_Async()
        {
            // Arrange
            var cache = new Mock<ITranslationCache>();
            cache.Setup(m => m.Get(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => ExpectedTranslations.Where(m => m.LanguageKey == "nl-NL"));

            var service = SetUp(cache);

            // Act
            var actual = await service.GetByKey("nl-NL", "bookshop", "page_title");

            // Assert

            Assert.NotNull(actual);
            actual.Should().BeEquivalentTo(ExpectedTranslation);
        }

        [Fact]
        public async Task It_Can_Purge_Records()
        {
            // Arrange
            var repo = new Mock<ITranslationRepository>();
            var service = SetUp(repository: repo);

            // Act
            await service.PurgeRecordsAsync("nl-NL", "webshop");

            // Assert

            repo.Verify(m => m.DeleteAll(It.Is<string>(f => f == "nl-NL"), It.Is<string>(f => f == "webshop")));
        }

        private static TranslationService SetUp(
            Mock<ITranslationCache> cache = null,
            Mock<ITranslationRepository> repository = null,
            Mock<ILanguageService> languageService = null,
            Mock<IProjectService> projectService = null)
        {
            cache = cache ?? new Mock<ITranslationCache>();
            repository = repository ?? new Mock<ITranslationRepository>();

            if (languageService == null)
            {
                languageService = new Mock<ILanguageService>();
                languageService.Setup(m => m.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            }

            if (projectService == null)
            {
                projectService = new Mock<IProjectService>();
                projectService.Setup(m => m.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            }

            return new TranslationService(languageService.Object, projectService.Object, cache.Object, repository.Object);
        }

        private static Translation ExpectedTranslation => new Translation
        {
            Id = 1,
            ProjectKey = "bookshop",
            Text = "Bookshop webshop frontend",
            Key = "page_title",
            LanguageKey = "nl-NL"
        };

        private static List<Translation> ExpectedTranslations => new List<Translation>
        {
            new Translation { Id = 1, ProjectKey = "bookshop", Text = "Bookshop webshop frontend", Key = "page_title", LanguageKey = "nl-NL"},
            new Translation { Id = 2, ProjectKey = "bookshop", Text = "Bookshop webshop frontend", Key = "page_title", LanguageKey = "en-US"},
        };
    }
}