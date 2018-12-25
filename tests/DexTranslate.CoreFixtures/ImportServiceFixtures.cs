using DexTranslate.Abstractions.Service;
using DexTranslate.Core;
using DexTranslate.Model;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace DexTranslate.CoreFixtures
{
    public class ImportServiceFixtures
    {
        private string CsvContent => @"LanguageKey,ProjectKey,Key,Text
en-US,webshop,DexTranslateExampleClient.Controllers.HomeController_Your application description page.,Your application description page.
en-US,webshop,DexTranslateExampleClient.Models.SharedResource_Your application description page.,Your application description page.";

        [Fact]
        public void It_Can_Import_File()
        {
            var translationService = new Mock<ITranslationService>();
            translationService.Setup(m => m.RecordsAreValid(It.IsAny<IEnumerable<Translation>>())).Returns(true);

            var service = Setup(translationService);

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(CsvContent)))
            {
                service.ImportTranslations("en-US", true, stream);
            }

            translationService.Verify(v => v.Add(It.IsAny<Translation>()), Times.Exactly(2));
        }

        public IImportService Setup(Mock<ITranslationService> translationService = null)
        {
            translationService = translationService ?? new Mock<ITranslationService>();

            return new ImportService(translationService.Object);
        }
    }
}