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
    public class ProjectCacheFixtures
    {
        [Fact]
        public async Task It_Can_Get_Result_From_Cache()
        {
            var repository = new Mock<IProjectRepository>();
            repository.Setup(m => m.GetAll()).ReturnsAsync(new List<Project>
            {
                new Project { Id = 1, Key = "en-US", Title = "webshop" }
            });

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cache = new ProjectCache(repository.Object, memoryCache);

            var result = await Enumerable.Range(0, 10).Select(async m => await cache.Get()).ToList().First();
            Assert.NotNull(result);
            repository.Verify(m => m.GetAll(), Times.Once);
        }
    }
}