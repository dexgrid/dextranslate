using DexTranslate.Abstractions.Repository;
using DexTranslate.Abstractions.Service;
using DexTranslate.Core;
using DexTranslate.Model;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DexTranslate.CoreFixtures
{
    public class ExportServiceFixtures
    {
        private string CsvContent => @"sep=,
LanguageKey,ProjectKey,Key,Text
en-US,webshop,page_title,my page title
";

        [Fact]
        public async Task It_Can_Export_FileAsync()
        {
            // Arrange
            var repository = new Mock<ITranslationRepository>();
            repository.Setup(m => m.GetAll("en-US", "webshop")).ReturnsAsync(new List<Translation>
            {
                new Translation { Id = 1,  LanguageKey = "en-US", ProjectKey = "webshop", Key = "page_title", Text = "my page title" }
            }.AsEnumerable());
            var service = SetUp(repository);

            // Act

            var result = await service.GetFileExportAsync("en-US", "webshop");

            // Assert

            Assert.NotNull(result);
            Assert.Equal(CsvContent, Encoding.UTF8.GetString(result));
        }

        public IExportService SetUp(Mock<ITranslationRepository> translationRepository)
        {
            translationRepository = translationRepository ?? new Mock<ITranslationRepository>();
            return new ExportService(translationRepository.Object);
        }
    }
}