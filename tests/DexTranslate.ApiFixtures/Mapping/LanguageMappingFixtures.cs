using AutoFixture;
using DexTranslate.Api.Mapping;
using DexTranslate.ApiContract.v1;
using FluentAssertions;
using Xunit;

namespace DexTranslate.ApiFixtures.Mapping
{
    public class LanguageMappingFixtures
    {
        [Fact]
        public void It_Can_Map_Language_To_ContractList()
        {
            var fixture = new Fixture();
            var language = fixture.CreateMany<Model.Language>(5);
            var contract = language.ToContract();
            contract.Should().BeEquivalentTo(language, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void It_Can_Map_Language_To_Contract()
        {
            var fixture = new Fixture();
            var language = fixture.Create<Model.Language>();
            var contract = language.ToContract();
            contract.Should().BeEquivalentTo(language, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void It_Can_Map_Language_From_ContractList()
        {
            var fixture = new Fixture();
            var language = fixture.CreateMany<Language>(5);
            var contract = language.FromContract();
            contract.Should().BeEquivalentTo(language, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void It_Can_Map_Language_From_Contract()
        {
            var fixture = new Fixture();
            var language = fixture.Create<Language>();
            var contract = language.FromContract();
            contract.Should().BeEquivalentTo(language, options => options.ExcludingMissingMembers());
        }
    }
}