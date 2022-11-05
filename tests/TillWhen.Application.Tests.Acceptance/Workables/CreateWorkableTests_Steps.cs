using FluentAssertions;
using LightBDD.XUnit2;
using NSubstitute;
using TillWhen.Application.Workables;
using TillWhen.Domain.Aggregates.WorkableAggregate;
using TillWhen.Domain.Common;

namespace TillWhen.Application.Tests.Acceptance.Workables;

public partial class CreateWorkableTests : FeatureFixture
{
    private readonly CreateWorkable.Handler _sut;
    private readonly IWorkableRepository _workableRepository;
    private string _title = null!;
    private Duration _duration = null!;
    private CreateWorkable.Response _response = null!;

    public CreateWorkableTests()
    {
        _workableRepository = Substitute.For<IWorkableRepository>();
        _sut = new(_workableRepository);
    }
    
    private void GivenValidWorkableSpecs()
    {
        _title = "Workable 1";
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

    private void ThenWorkableIsCreated()
    {
        _response.Duration.Should().BeEquivalentTo(new
        {
            Days = 1,
            Hours = 2,
            Minutes = 3
        });
        
        _workableRepository
            .Received(1)
            .Add(Arg.Is<Workable>(p => 
                p.Title == _title && 
                p.Duration == _duration
            ));
    }
}