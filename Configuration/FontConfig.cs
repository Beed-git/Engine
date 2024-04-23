namespace Engine.Configuration;

public class FontConfig
{
    public FontConfig()
    {
        OverrideFallbackFont = null;
    }

    public string? OverrideFallbackFont { get; set; }
}