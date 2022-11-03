using System.Collections.Generic;
using FluentAssertions;
using TillWhen.Domain.Common;
using Xunit;

namespace TillWhen.Domain.Tests.Unit.Aggregates.Common;

public class DurationTests
{
    [Fact]
    public void ShouldDetectIncorrectFormat()
    {
        var duration = Duration.Create("invalid");

        duration.Minutes.Should().Be(0);
        duration.Hours.Should().Be(0);
        duration.Days.Should().Be(0);
        duration.Tomatoes.Should().Be(0);
    }
    
    [Theory]
    [MemberData(nameof(Values))]
    public void ShouldConvertMinutes(int minutes)
    {
        var duration = Duration.Create($"{minutes}m");

        duration.Minutes.Should().Be(minutes);
    }

    [Theory]
    [MemberData(nameof(Values))]
    public void ShouldConvertHours(int hours)
    {
        var duration = Duration.Create($"{hours}h");

        duration.Hours.Should().Be(hours);
    }
    
    [Theory]
    [InlineData(1, 24)]
    public void ShouldConvertDays(int days, int totalHours)
    {
        var duration = Duration.Create($"{days}d");

        duration.Days.Should().Be(days);
        duration.TotalHours.Should().Be(totalHours);
    }

    [Fact]
    public void ShouldConvertFullFormat()
    {
        var duration = Duration.Create("1d 2h 3m");

        duration.Days.Should().Be(1);
        duration.Hours.Should().Be(2);
        duration.Minutes.Should().Be(3);
        duration.TotalHours.Should().Be(26);
    }

    [Theory]
    [InlineData("61m", 0, 1, 1)]
    [InlineData("25h", 1, 1, 0)]
    public void ShouldDetectOverflow(string value, int days, int hours, int minutes)
    {
        var duration = Duration.Create(value);

        duration.Days.Should().Be(days);
        duration.Hours.Should().Be(hours);
        duration.Minutes.Should().Be(minutes);
    }
    
    [Fact]
    public void ShouldSumDurations()
    {
        var oneHour = Duration.Create("1h");
        var oneDay = Duration.Create("1d");
        
        var total = oneHour + oneDay;

        total.Hours.Should().Be(1);
        total.Days.Should().Be(1);
        total.TotalHours.Should().Be(25);
        total.Tomatoes.Should().Be(60);
    }
    
    [Fact]
    public void ShouldDetectOverflowWhenSumming()
    {
        var first = new Duration
        {
            Hours = 23,
            Minutes = 59
        };

        var second = new Duration
        {
            Minutes = 1
        };

        var total = first + second;

        total.Days.Should().Be(1);
        total.Hours.Should().Be(0);
        total.Minutes.Should().Be(0);
    }
    
    [Theory]
    [InlineData("25m", 1)]
    [InlineData("1h", 2)]
    public void ShouldConvertToTomatoes(string format, int tomatoes)
    {
        var duration = Duration.Create(format);

        duration.Tomatoes.Should().Be(tomatoes);
    }

    public static IEnumerable<object[]> Values()
    {
        return new List<object[]>
        {
            new object[] { 1 },
            new object[] { 10 }
        };
    }
}