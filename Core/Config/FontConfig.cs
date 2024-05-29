namespace Engine.Core.Config;

public class FontConfig
{
    public const string FontExtension = ".ttf";

    public FontConfig()
    {
        OverrideFallbackFont = null;
    }

    public string? OverrideFallbackFont { get; set; }
}