using System;
using System.Text.RegularExpressions;

namespace TillWhen.Domain.Common;

public record Duration
{
    public static Duration Empty() => new();
    public static Duration Create(string value) => new(value);

    internal Duration() { }
    private Duration(string value)
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

        Days = timespan.Days;
        Hours = timespan.Hours;
        Minutes = timespan.Minutes;
        TotalHours = (int)timespan.TotalHours;
        Tomatoes = (int)(timespan.TotalMinutes / 25);
    }
    
    public static Duration operator +(Duration left, Duration right)
    {
        var minutes = left.Minutes + right.Minutes;
        var hours = left.Hours + right.Hours;
        var days = left.Days + right.Days;

        var timespan = TimeSpan.FromMinutes(minutes)
            .Add(TimeSpan.FromHours(hours))
            .Add(TimeSpan.FromDays(days));
        
        return new()
        {
            Minutes = timespan.Minutes,
            Hours = timespan.Hours,
            Days = timespan.Days,
            TotalHours = (int)timespan.TotalHours,
            Tomatoes = (int)(timespan.TotalMinutes / 25)
        };
    }     

    public int Minutes { get; internal set; }
    public int Hours { get; internal set; }
    public int Days { get; internal set; }
    public int TotalHours { get; private set; }
    public int Tomatoes { get; private set; }
}