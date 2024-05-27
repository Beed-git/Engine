namespace Engine.Core.Utility;

public class Profiler
{
    private readonly TimeSpan[] _results;

    private int _index;
    private int _count;

    internal const int ResultCount = 8;

    public Profiler()
    {
        _results = new TimeSpan[ResultCount];

        LastResult = TimeSpan.Zero;
        Average = TimeSpan.Zero;
    }

    public int NumberOfResults => _count;
    public TimeSpan LastResult { get; private set; }
    public TimeSpan Average { get; private set; }

    public void Record(TimeSpan time)
    {
        _results[_index] = time;
        _index = (_index + 1) % ResultCount;
        _count = Math.Min(_count + 1, ResultCount);

        LastResult = time;
        CalculateAverage();
    }

    private void CalculateAverage()
    {
        var total = TimeSpan.Zero;
        for (int i = 0; i < _count; i++)
        {
            total += _results[i];
        }

        Average = total / _count;
    }
}