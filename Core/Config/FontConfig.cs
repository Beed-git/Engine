namespace Engine.Core.Config;

public class FontConfig
{
    public FontConfig()
    {
        OverrideFallbackFont = null;
    }

    public string? OverrideFallbackFont { get; set; }
}