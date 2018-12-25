using AutoFixture;
using DexTranslate.Api.Mapping;
using DexTranslate.ApiContract.v1;
using FluentAssertions;
using Xunit;

namespace DexTranslate.ApiFixtures.Mapping
{
    public class ProjectMappingFixtures
    {
        [Fact]
        public void It_Can_Map_Project_To_ContractList()
        {
            var fixture = new Fixture();
            var projects = fixture.CreateMany<Model.Project>(5);
            var contract = projects.ToContract();
            contract.Should().BeEquivalentTo(projects, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void It_Can_Map_Project_To_Contract()
        {
            var fixture = new Fixture();
            var project = fixture.Create<Model.Project>();
            var contract = project.ToContract();
            contract.Should().BeEquivalentTo(project, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void It_Can_Map_Project_From_ContractList()
        {
            var fixture = new Fixture();
            var projects = fixture.CreateMany<Project>(5);
            var contract = projects.FromContract();
            contract.Should().BeEquivalentTo(projects, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void It_Can_Map_Project_From_Contract()
        {
            var fixture = new Fixture();
            var project = fixture.Create<Project>();
            var contract = project.FromContract();
            contract.Should().BeEquivalentTo(project, options => options.ExcludingMissingMembers());
        }
    }
}