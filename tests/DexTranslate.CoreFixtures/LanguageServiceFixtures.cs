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
    public class LanguageServiceFixtures
    {
        [Fact]
        public async Task It_Can_Add_LanguageAsync()
        {
            // Arrange
            var repository = new Mock<ILanguageRepository>();
            var service = SetUp(repository: repository);

            // Act
            await service.Add(ExpectedLanguage);

            // Assert
            repository.Verify(m => m.Add(It.Is<Language>(l => l.Key == ExpectedLanguage.Key)), Times.Once);
        }

        [Fact]
        public async Task It_Clears_Cache_After_Add()
        {
            // Arrange
            var cache = new Mock<ILanguageCache>();
            var service = SetUp(cache);

            // Act
            await service.Add(ExpectedLanguage);

            // Assert
            cache.Verify(m => m.Clear(), Times.Once);
        }

        [Fact]
        public async Task It_Can_Update_LanguageAsync()
        {
            // Arrange
            var repository = new Mock<ILanguageRepository>();
            var service = SetUp(repository: repository);

            // Act
            await service.Update(ExpectedLanguage);

            // Assert
            repository.Verify(m => m.Update(It.Is<Language>(i => i.Key == "nl-NL")));
        }

        [Fact]
        public async Task It_Clears_Cache_After_Update()
        {
            // Arrange
            var cache = new Mock<ILanguageCache>();
            var service = SetUp(cache);

            // Act
            await service.Update(ExpectedLanguage);

            // Assert
            cache.Verify(m => m.Clear(), Times.Once);
        }

        [Fact]
        public async Task It_Can_Delete_Language()
        {
            // Arrange
            var repository = new Mock<ILanguageRepository>();
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
            var cache = new Mock<ILanguageCache>();
            var service = SetUp(cache);

            // Act
            await service.Delete(ExpectedLanguage.Key);

            // Assert
            cache.Verify(m => m.Clear(), Times.Once);
        }

        [Fact]
        public async Task It_Can_Check_If_Language_Exists()
        {
            // Arrange
            var cache = new Mock<ILanguageCache>();
            cache.Setup(m => m.Get()).ReturnsAsync(() => ExpectedLanguages);

            var service = SetUp(cache);

            // Act
            var actual = await service.ExistsAsync("ru-RU");

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task It_Can_Get_All_LanguagesAsync()
        {
            // Arrange
            var cache = new Mock<ILanguageCache>();
            cache.Setup(m => m.Get()).ReturnsAsync(() => ExpectedLanguages);

            var service = SetUp(cache);

            // Act
            var actual = await service.GetAll();

            // Assert

            Assert.Equal(ExpectedLanguages.Count(), actual.Count());
            actual.Should().BeEquivalentTo(ExpectedLanguages);
        }

        private static LanguageService SetUp(
            Mock<ILanguageCache> cache = null,
            Mock<ILanguageRepository> repository = null)
        {
            cache = cache ?? new Mock<ILanguageCache>();
            repository = repository ?? new Mock<ILanguageRepository>();

            return new LanguageService(cache.Object, repository.Object);
        }

        private static Language ExpectedLanguage => new Language
        {
            Key = "nl-NL",
            Name = "Dutch"
        };

        private static List<Language> ExpectedLanguages => new List<Language>
        {
            new Language { Id = 1, Key = "nl-NL", Name = "Dutch"},
            new Language { Id = 2, Key = "ru-RU", Name = "Russian"}
        };
    }
}