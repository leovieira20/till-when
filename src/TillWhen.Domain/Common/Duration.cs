using System;
using System.Text.RegularExpressions;

namespace TillWhen.Domain.Common;

public record Duration
{
    private TimeSpan _timespan;

    public static Duration Empty() => new();
    public static Duration Create(string value) => new(value);

    internal Duration()
        : this(TimeSpan.Zero)
    {
    }

    private Duration(TimeSpan timespan)
    {
        _timespan = timespan;
    }

    private Duration(string value)
    {
        _timespan = TimeSpan.Zero;

        var minutesMatch = Regex.Match(value, @"\d+m");
        if (minutesMatch.Success && !string.IsNullOrWhiteSpace(minutesMatch.Value))
        {
            var minutes = minutesMatch.Value.Replace("m", string.Empty);
            _timespan = _timespan.Add(TimeSpan.FromMinutes(int.Parse(minutes)));
        }

        var hoursMatch = Regex.Match(value, @"\d+h");
        if (hoursMatch.Success && !string.IsNullOrWhiteSpace(hoursMatch.Value))
        {
            var hours = hoursMatch.Value.Replace("h", string.Empty);
            _timespan = _timespan.Add(TimeSpan.FromHours(int.Parse(hours)));
        }

        var daysMatch = Regex.Match(value, @"\d+d");
        if (daysMatch.Success && !string.IsNullOrWhiteSpace(daysMatch.Value))
        {
            var days = daysMatch.Value.Replace("d", string.Empty);
            _timespan = _timespan.Add(TimeSpan.FromDays(int.Parse(days)));
        }

        Days = _timespan.Days;
        Hours = _timespan.Hours;
        Minutes = _timespan.Minutes;
        TotalHours = (int)_timespan.TotalHours;
        Tomatoes = (int)(_timespan.TotalMinutes / 25);
    }

    public static Duration operator +(Duration left, Duration right)
    {
        var minutes = left.Minutes + right.Minutes;
        var hours = left.Hours + right.Hours;
        var days = left.Days + right.Days;

        var timespan = TimeSpan.FromMinutes(minutes)
            .Add(TimeSpan.FromHours(hours))
            .Add(TimeSpan.FromDays(days));

        return new(timespan)
        {
            Minutes = timespan.Minutes,
            Hours = timespan.Hours,
            Days = timespan.Days,
            TotalHours = (int)timespan.TotalHours,
            Tomatoes = (int)(timespan.TotalMinutes / 25)
        };
    }
    
    public static Duration operator -(Duration left, Duration right)
    {
        var minutes = left.Minutes - right.Minutes;
        var hours = left.Hours - right.Hours;
        var days = left.Days - right.Days;

        var timespan = TimeSpan.FromMinutes(minutes)
            .Add(TimeSpan.FromHours(hours))
            .Add(TimeSpan.FromDays(days));

        return new(timespan)
        {
            Minutes = timespan.Minutes,
            Hours = timespan.Hours,
            Days = timespan.Days,
            TotalHours = (int)timespan.TotalHours,
            Tomatoes = (int)(timespan.TotalMinutes / 25)
        };
    }

    public static bool operator >(Duration left, Duration right) => left._timespan > right._timespan;
    public static bool operator >=(Duration left, Duration right) => left._timespan >= right._timespan;
    public static bool operator <(Duration left, Duration right) => left._timespan < right._timespan;
    public static bool operator <=(Duration left, Duration right) => left._timespan <= right._timespan;

    public int Minutes { get; internal set; }
    public int Hours { get; internal set; }
    public int Days { get; internal set; }
    public int TotalHours { get; private set; }
    public int Tomatoes { get; private set; }
}