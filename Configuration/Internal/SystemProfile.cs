using System.Diagnostics;

namespace Engine.Configuration.Internal;

public class SystemProfile
{
    private readonly Stopwatch _stopwatch;
    private readonly List<KeyValuePair<string, TimeSpan>> _results;

    private TimeSpan _total;

    public SystemProfile()
    {
        _stopwatch = new Stopwatch();
        _results = [];
    }

    public TimeSpan Total => _total;
    public IEnumerable<KeyValuePair<string, TimeSpan>> Results => _results;

    public void Start()
    {
        _results.Clear();
        _stopwatch.Start();
        _total = TimeSpan.Zero;
    }

    public void Record(string name)
    {
        var time = _stopwatch.Elapsed;
        _total += time;

        _results.Add(new (name, time));

        _stopwatch.Restart();
    }

    public void Stop()
    {
        _stopwatch.Stop();
    }
}
