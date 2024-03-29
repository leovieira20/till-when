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
        Duration duration = "invalid";

        duration.Minutes.Should().Be(0);
        duration.Hours.Should().Be(0);
        duration.Days.Should().Be(0);
        duration.Tomatoes.Should().Be(0);
    }
    
    [Theory]
    [MemberData(nameof(Values))]
    public void ShouldConvertMinutes(int minutes)
    {
        Duration duration = $"{minutes}m";

        duration.Minutes.Should().Be(minutes);
    }

    [Theory]
    [MemberData(nameof(Values))]
    public void ShouldConvertHours(int hours)
    {
        Duration duration = $"{hours}h";

        duration.Hours.Should().Be(hours);
    }
    
    [Theory]
    [InlineData(1, 24)]
    public void ShouldConvertDays(int days, int totalHours)
    {
        Duration duration = $"{days}d";

        duration.Days.Should().Be(days);
        duration.TotalHours.Should().Be(totalHours);
    }

    [Fact]
    public void ShouldConvertFullFormat()
    {
        Duration duration = "1d 2h 3m";

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
        Duration duration = value;

        duration.Days.Should().Be(days);
        duration.Hours.Should().Be(hours);
        duration.Minutes.Should().Be(minutes);
    }
    
    [Fact]
    public void ShouldSumDurations()
    {
        Duration oneHour = "1h";
        Duration oneDay = "1d";
        
        var total = oneHour + oneDay;

        total.Hours.Should().Be(1);
        total.Days.Should().Be(1);
        total.TotalHours.Should().Be(25);
        total.Tomatoes.Should().Be(60);
    }
    
    [Fact]
    public void ShouldDetectOverflowWhenSumming()
    {
        Duration first = "23h 59m";
        Duration second = "1m";

        var total = first + second;

        total.Days.Should().Be(1);
        total.Hours.Should().Be(0);
        total.Minutes.Should().Be(0);
    }
    
    [Fact]
    public void ShouldNotAllowSubtractionToGoBelowZero()
    {
        Duration first = "1m";
        Duration second = "2m";

        var total = first - second;

        total.Days.Should().Be(0);
        total.Hours.Should().Be(0);
        total.Minutes.Should().Be(0);
    }

    [Fact]
    public void ShouldReduceDurationWithRemaining()
    {
        Duration original = "2m";
        Duration reduceBy = "1m";
        
        var (actual, remaining) = original.Reduce(reduceBy);

        actual.Minutes.Should().Be(1);
        remaining.Minutes.Should().Be(1);
    }
    
    [Fact]
    public void ShouldReduceDurationWithoutRemaining()
    {
        Duration original = "1m";
        Duration reduceBy = "1m";
        
        var (actual, remaining) = original.Reduce(reduceBy);

        actual.Minutes.Should().Be(1);
        remaining.Minutes.Should().Be(0);
    }
    
    [Theory]
    [InlineData("25m", 1)]
    [InlineData("1h", 2)]
    public void ShouldConvertToTomatoes(string format, int tomatoes)
    {
        Duration duration = format;

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