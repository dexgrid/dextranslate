using DexTranslate.Abstractions.Caching;
using DexTranslate.Abstractions.Repository;
using DexTranslate.Caching;
using DexTranslate.Model;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DexTranslate.CachingFixtures
{
    public class LanguageCacheFixtures
    {
        [Fact]
        public async Task It_Can_Get_Result_From_Cache()
        {
            // Arrange
            var repository = new Mock<ILanguageRepository>();
            repository.Setup(m => m.GetAll()).ReturnsAsync(LanguageList);
            var cache = SetUp(repository);

            // Act
            var result = await Enumerable.Range(0, 10).Select(async m => await cache.Get()).ToList().First();

            // Assert
            Assert.NotNull(result);
            repository.Verify(m => m.GetAll(), Times.Once);
        }

        [Fact]
        public async Task It_Can_Clear_Cache()
        {
            // Arrange
            var repository = new Mock<ILanguageRepository>();
            repository.Setup(m => m.GetAll()).ReturnsAsync(LanguageList);
            var cache = SetUp(repository);

            // Act
            await cache.Get();
            cache.Clear();
            await cache.Get();
            await cache.Get();

            // Assert
            repository.Verify(m => m.GetAll(), Times.Exactly(2));
        }

        private static ILanguageCache SetUp(Mock<ILanguageRepository> repository)
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cache = new LanguageCache(repository.Object, memoryCache);
            return cache;
        }

        private List<Language> LanguageList => new List<Language> { TestLanguage };
        private Language TestLanguage => new Language { Id = 1, Key = "en-US", Name = "English" };
    }
}