using DexTranslate.Core.Validation;
using DexTranslate.Model;
using System.Collections.Generic;
using Xunit;

namespace DexTranslate.CoreFixtures.Validation
{
    public class TranslationValidationFixtures
    {
        [Theory]
        [MemberData(nameof(GetTranslationTestData))]
        public void It_Can_Validate_Language(bool expectedValid, Translation translation)
        {
            Assert.Equal(expectedValid, TranslationValidation.IsValidTranslation(translation));
        }

        public static IEnumerable<object[]> GetTranslationTestData()
        {
            return new List<object[]>
            {
                new object[] { true, new Translation { LanguageKey = "en-US", ProjectKey = "websop", Key = "page_title" } },
                new object[] { false, new Translation { LanguageKey = "", ProjectKey = "websop", Key = "page_title" } },
                new object[] { false, new Translation { LanguageKey = "en-US", ProjectKey = "", Key = "page_title" } },
                new object[] { false, new Translation { LanguageKey = "en-US", ProjectKey = "websop", Key = "" } }
            };
        }
    }
}