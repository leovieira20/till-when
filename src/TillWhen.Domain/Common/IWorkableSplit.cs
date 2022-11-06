namespace TillWhen.Domain.Common;

public interface IWorkableSplit
{
    string Title { get; }
    Duration Estimation { get; }
}