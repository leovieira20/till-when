namespace TillWhen.Domain.Common;

public interface IWorkable
{
    Duration Duration { get; }
    void ReduceEffortBy(Duration capacity);
}