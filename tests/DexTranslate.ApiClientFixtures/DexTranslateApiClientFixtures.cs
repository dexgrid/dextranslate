using DexTranslate.ApiClient;
using RichardSzalay.MockHttp;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DexTranslate.ApiClientFixtures
{
    public class DexTranslateApiClientFixtures
    {
        private const string BaseUrl = "http://dextranslate.example.com/api/";

        [Fact]
        public async Task It_Can_Get_Languages()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseUrl}Language").Respond("application/json", "[{'key': 'nl-NL','name': 'Dutch'}]");

            var client = mockHttp.ToHttpClient();
            var apiClient = new DexTranslateApiClient(client, BaseUrl, string.Empty, string.Empty);

            var languages = await apiClient.GetLanguages();
            Assert.NotNull(languages);
            Assert.Single(languages);
            Assert.Equal("nl-NL", languages.First().Key);
            Assert.Equal("Dutch", languages.First().Name);
        }
    }
}