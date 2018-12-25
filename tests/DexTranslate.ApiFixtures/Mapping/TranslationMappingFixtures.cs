using AutoFixture;
using DexTranslate.Api.Mapping;
using DexTranslate.ApiContract.v1;
using FluentAssertions;
using Xunit;

namespace DexTranslate.ApiFixtures.Mapping
{
    public class TranslationMappingFixtures
    {
        [Fact]
        public void It_Can_Map_Null_Pluralizations_To_Contract()
        {
            // Arrange
            var translation = new Model.Translation();

            // Act
            var translationContract = translation.ToContract();

            // Assert
            Assert.NotNull(translationContract);
        }

        [Fact]
        public void It_Can_Map_Translation_To_Contract()
        {
            var fixture = new Fixture();
            var tranlsation = fixture.Create<Model.Translation>();
            var contract = tranlsation.ToContract();
            contract.Should().BeEquivalentTo(tranlsation, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void It_Can_Map_Translation_From_Contract()
        {
            var fixture = new Fixture();
            var translation = fixture.Create<Translation>();
            var model = translation.FromContract();
            model.Should().BeEquivalentTo(translation, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void It_Can_Map_Null_Pluralizations_From_Contract()
        {
            // Arrange
            var translation = new Translation();

            // Act
            var translationModel = translation.FromContract();

            // Assert
            Assert.NotNull(translationModel);
        }
    }
}