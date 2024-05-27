using Engine.Core.Internal;
using Engine.Level;
using ImGuiNET;

namespace Engine.DebugGUI;

public class DebugSystemProfilerGUI
{
    private readonly StageManager _stages;

    public DebugSystemProfilerGUI(StageManager stages)
    {
        _stages = stages;
    }

    public void Render()
    {
        var stage = _stages.CurrentStage;
        ImGui.Begin("Systems");

        RenderGroup("Update", stage.UpdateSystems);
        RenderGroup("Render Systems", stage.RenderSystems);
        RenderGroup("Debug UI", stage.DebugUIs);

        ImGui.End();
    }

    private void RenderGroup(string name, IEnumerable<FrameUpdateSystem> systems)
    {
        var total = TimeSpan.Zero;
        ImGui.SeparatorText(name);
        foreach (var system in systems)
        {
            ImGui.Text($"{system.Name}: {system.Profile.Average.TotalMilliseconds}ms");
            total += system.Profile.Average;
        }

        ImGui.NewLine();
        ImGui.Text($"Total: {total.TotalMilliseconds}ms");
    }
}