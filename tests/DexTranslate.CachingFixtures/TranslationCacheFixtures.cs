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
    public class TranslationCacheFixtures
    {
        [Fact]
        public async Task It_Can_Get_Result_From_Cache()
        {
            // Arrange
            var repository = new Mock<ITranslationRepository>();
            repository.Setup(m => m.GetAll(It.Is<string>(l => l == "en-US"), It.Is<string>(p => p == "webshop"))).ReturnsAsync(TranslationList);
            var cache = SetUp(repository);

            // Act
            var result = await Enumerable.Range(0, 10).Select(async m => await cache.Get("en-US", "webshop")).ToList().First();

            // Assert
            Assert.NotNull(result);
            repository.Verify(m => m.GetAll("en-US", "webshop"), Times.Once);
        }

        [Fact]
        public async Task It_Can_Clear_Cache()
        {
            // Arrange
            var repository = new Mock<ITranslationRepository>();
            repository.Setup(m => m.GetAll(It.Is<string>(l => l == "en-US"), It.Is<string>(p => p == "webshop"))).ReturnsAsync(TranslationList);
            var cache = SetUp(repository);

            // Act
            await cache.Get("en-US", "webshop");
            cache.Clear("en-US", "webshop");
            await cache.Get("en-US", "webshop");
            await cache.Get("en-US", "webshop");

            // Assert
            repository.Verify(m => m.GetAll("en-US", "webshop"), Times.Exactly(2));
        }

        private static ITranslationCache SetUp(Mock<ITranslationRepository> repository)
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cache = new TranslationCache(repository.Object, memoryCache);
            return cache;
        }

        private static Translation TestTranslation => new Translation { Id = 1, LanguageKey = "en-US", ProjectKey = "webshop", Key = "page_title", Text = "my page title" };

        private static IList<Translation> TranslationList => new List<Translation> { TestTranslation }.AsReadOnly();
    }
}