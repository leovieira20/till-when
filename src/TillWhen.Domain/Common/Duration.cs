using System;
using System.Text.RegularExpressions;

namespace TillWhen.Domain.Common;

public record Duration
{
    private TimeSpan _timespan;
    private string _originalDuration;

    public static Duration Zero() => new();
    public static Duration Create(string value) => new(value);

    private Duration()
    {
    }

    private Duration(TimeSpan timespan) => _timespan = timespan;

    private Duration(string value)
    {
        _timespan = ConvertStringDurationToTimestamp(value);
        OriginalDuration = value;
    }

    private TimeSpan ConvertStringDurationToTimestamp(string value)
    {
        var timespan = TimeSpan.Zero;

        var minutesMatch = Regex.Match(value, @"\d+m");
        if (minutesMatch.Success && !string.IsNullOrWhiteSpace(minutesMatch.Value))
        {
            var minutes = minutesMatch.Value.Replace("m", string.Empty);
            timespan = timespan.Add(TimeSpan.FromMinutes(int.Parse(minutes)));
        }

        var hoursMatch = Regex.Match(value, @"\d+h");
        if (hoursMatch.Success && !string.IsNullOrWhiteSpace(hoursMatch.Value))
        {
            var hours = hoursMatch.Value.Replace("h", string.Empty);
            timespan = timespan.Add(TimeSpan.FromHours(int.Parse(hours)));
        }

        var daysMatch = Regex.Match(value, @"\d+d");
        if (daysMatch.Success && !string.IsNullOrWhiteSpace(daysMatch.Value))
        {
            var days = daysMatch.Value.Replace("d", string.Empty);
            timespan = timespan.Add(TimeSpan.FromDays(int.Parse(days)));
        }

        return timespan;
    }

    public (Duration adjusted, Duration remaining) Reduce(Duration reduceBy)
    {
        var left = this - reduceBy;
        return left > Zero()
            ? (reduceBy, left)
            : (this, Zero());
    }

    public static implicit operator Duration(string d) => Create(d);

    public static Duration operator +(Duration left, Duration right) => new(left._timespan.Add(right._timespan));

    public static Duration operator -(Duration left, Duration right)
    {
        var result = left._timespan.Subtract(right._timespan);
        return result < TimeSpan.Zero ? Zero() : new(result);
    }

    public static bool operator >(Duration left, Duration right) => left._timespan > right._timespan;

    public static bool operator >=(Duration left, Duration right) => left._timespan >= right._timespan;

    public static bool operator <(Duration left, Duration right) => left._timespan < right._timespan;

    public static bool operator <=(Duration left, Duration right) => left._timespan <= right._timespan;

    public string OriginalDuration
    {
        get => _originalDuration;
        set
        {
            _originalDuration = value;
            _timespan = ConvertStringDurationToTimestamp(value);
        }
    }

    public int Days => _timespan.Days;
    public int Hours => _timespan.Hours;
    public int TotalHours => (int)_timespan.TotalHours;
    public int Minutes => _timespan.Minutes;
    public int Tomatoes => (int)(_timespan.TotalMinutes / 25);
}