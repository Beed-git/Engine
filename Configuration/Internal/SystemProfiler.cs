using ImGuiNET;

namespace Engine.Configuration.Internal;

public class SystemProfiler
{
    private readonly Dictionary<string, SystemProfile> _metrics;

    public SystemProfiler()
    {
        _metrics = [];
    }

    public SystemProfile StartProfiling(string group)
    {
        if (!_metrics.TryGetValue(group, out var profile))
        {
            profile = new SystemProfile();
            _metrics.Add(group, profile);
        }

        profile.Start();
        return profile;
    }

    public void Render()
    {
        ImGui.Begin("System Profiler");

        foreach (var (group, profile) in _metrics)
        {
            ImGui.SeparatorText(group);
            foreach (var (metric, time) in profile.Results)
            {
                ImGui.Text($"{metric}: {time.TotalMilliseconds}ms");
            }
            ImGui.Text($"Total Time: {profile.Total.TotalMilliseconds}ms");
            ImGui.NewLine();
        }

        ImGui.End();
    }
}
