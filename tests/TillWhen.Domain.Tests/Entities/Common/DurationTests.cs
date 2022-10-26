using System.Collections.Generic;
using FluentAssertions;
using TillWhen.Domain.Common;
using Xunit;

namespace TillWhen.Domain.Tests.Entities.Common;

public class DurationTests
{
    [Fact]
    public void ShouldDetectIncorrectFormat()
    {
        var duration = new Duration("invalid");

        duration.Minutes.Should().Be(0);
        duration.Hours.Should().Be(0);
        duration.Days.Should().Be(0);
    }
    
    [Theory]
    [MemberData(nameof(Values))]
    public void ShouldConvertMinutes(int minutes)
    {
        var duration = new Duration($"{minutes}m");

        duration.Minutes.Should().Be(minutes);
    }

    [Theory]
    [MemberData(nameof(Values))]
    public void ShouldConvertHours(int hours)
    {
        var duration = new Duration($"{hours}h");

        duration.Hours.Should().Be(hours);
    }
    
    [Theory]
    [MemberData(nameof(Values))]
    public void ShouldConvertDays(int days)
    {
        var duration = new Duration($"{days}d");

        duration.Days.Should().Be(days);
    }

    [Fact]
    public void ShouldConvertFullFormat()
    {
        var duration = new Duration("1d 2h 3m");

        duration.Days.Should().Be(1);
        duration.Hours.Should().Be(2);
        duration.Minutes.Should().Be(3);
    }

    public static IEnumerable<object[]> Values()
    {
        return new List<object[]>
        {
            new object[] { 1 },
            new object[] { 10 },
            new object[] { 100 }
        };
    }
}