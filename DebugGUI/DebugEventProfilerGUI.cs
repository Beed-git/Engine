using Engine.Level;
using ImGuiNET;
using Microsoft.CSharp.RuntimeBinder;

namespace Engine.DebugGUI;

public class DebugEventProfilerGUI
{
    private readonly StageManager _stages;

    public DebugEventProfilerGUI(StageManager stages)
    {
        _stages = stages;
    }

    public void Render()
    {
        ImGui.Begin("Event Profiler");

        var total = TimeSpan.Zero;
        foreach (var (type, runner) in _stages.CurrentStage.EventRegistry.Runners) 
        {
            ImGui.SeparatorText(type.Name);
            foreach (var (name, profiler) in runner.HandlerProfilers)
            {
                ImGui.Text($"{name}: {profiler.Average.TotalMilliseconds}ms");
                total += profiler.Average;
            }
            ImGui.NewLine();
            ImGui.Text($"Total: {total.TotalMilliseconds}ms");
            total = TimeSpan.Zero;
        }

        ImGui.End();
    }
}
