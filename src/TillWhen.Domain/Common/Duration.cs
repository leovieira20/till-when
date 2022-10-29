using System.Text.RegularExpressions;

namespace TillWhen.Domain.Common;

public record Duration
{
    private Duration() { }

    public static Duration Empty() => new();
    public static Duration Create(string value)
    {
        return new(value);
    }
    
    private Duration(string value)
    {
        var minutesMatch = Regex.Match(value, @"\d+m");
        if (minutesMatch.Success && !string.IsNullOrWhiteSpace(minutesMatch.Value))
        {
            var minutes = minutesMatch.Value.Replace("m", string.Empty);
            Minutes = int.Parse(minutes);
        }
        
        var hoursMatch = Regex.Match(value, @"\d+h");
        if (hoursMatch.Success && !string.IsNullOrWhiteSpace(hoursMatch.Value))
        {
            var hours = hoursMatch.Value.Replace("h", string.Empty);
            Hours = int.Parse(hours);
        }
        
        var daysMatch = Regex.Match(value, @"\d+d");
        if (daysMatch.Success && !string.IsNullOrWhiteSpace(daysMatch.Value))
        {
            var days = daysMatch.Value.Replace("d", string.Empty);
            Days = int.Parse(days);
        }
    }
    
    public static Duration operator +(Duration left, Duration right)
    {
        return new Duration
        {
            Minutes = left.Minutes + right.Minutes,
            Hours = left.Hours + right.Hours,
            Days = left.Days + right.Days
        };
    }     

    public int Minutes { get; private set; }
    public int Hours { get; private set; }
    public int Days { get; private set; }
}