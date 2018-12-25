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
            var repository = new Mock<ITranslationRepository>();
            repository.Setup(m => m.GetAll(It.Is<string>(l => l == "en-US"), It.Is<string>(p => p == "webshop"))).ReturnsAsync(new List<Translation>
            {
                new Translation { Id = 1,  LanguageKey = "en-US", ProjectKey = "webshop", Key = "page_title", Text = "my page title" }
            });

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cache = new TranslationCache(repository.Object, memoryCache);

            var result = await Enumerable.Range(0, 10).Select(async m => await cache.Get("en-US", "webshop")).ToList().First();
            Assert.NotNull(result);
            repository.Verify(m => m.GetAll("en-US", "webshop"), Times.Once);
        }
    }
}