using FluentAssertions;
using LightBDD.XUnit2;
using NSubstitute;
using TillWhen.Application.Projects;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Tests.Acceptance.Projects;

public partial class CreateProjectTests : FeatureFixture
{
    private readonly CreateProject.Handler _sut;
    private readonly IProjectRepository _projectRepository;
    private string _title = null!;
    private Duration _duration = null!;
    private CreateProject.Response _response = null!;

    public CreateProjectTests()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _sut = new(_projectRepository);
    }
    
    private void GivenValidProjectSpecs()
    {
        _title = "Project 1";
        _duration = "1d 2h 3m";
    }

    private async Task WhenActionIsExecuted()
    {
        _response = await _sut.Handle(new(_title, _duration)
        {
            Title = _title,
            Duration = _duration
        }, CancellationToken.None);
    }

    private void ThenProjectIsCreated()
    {
        _response.Duration.Should().BeEquivalentTo(new
        {
            Days = 1,
            Hours = 2,
            Minutes = 3
        });
        
        _projectRepository
            .Received(1)
            .Add(Arg.Is<Project>(p => 
                p.Title == _title && 
                p.Duration == _duration
            ));
    }
}