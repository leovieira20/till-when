using System;
using FluentAssertions;
using TillWhen.Domain.Aggregates.WorkableAggregate;
using Xunit;

namespace TillWhen.Domain.Tests.Unit.Aggregates.WorkableAggregate;

public class WorkableTests
{
    [Fact]
    public void SplitWorkableIntoTwo()
    {
        var workable = Workable.Create(Guid.Empty.ToString(), "2h");

        var splits = workable.GetSplitsFor("1h");

        splits[0].ScheduledEffort.Hours.Should().Be(1);
        splits[0].RemainingEffort.Hours.Should().Be(1);
        
        splits[1].ScheduledEffort.Hours.Should().Be(1);
        splits[1].RemainingEffort.Hours.Should().Be(0);
    }
}