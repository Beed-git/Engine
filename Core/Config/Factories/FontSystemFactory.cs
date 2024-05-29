using Engine.Files;
using FontStashSharp;

namespace Engine.Core.Config.Factories;

internal static class FontSystemFactory
{
    internal static FontSystem Create(FontConfig config, FileSystem files)
    {
        var fontPath = config.OverrideFallbackFont
            ?? throw new NotImplementedException("Default font has not been added.");

        var resource = $"{FileSystemSettings.FontsFolder}{fontPath}";
        if (!files.TryReadBinary(resource, FontConfig.FontExtension, out var font))
        {
            throw new Exception($"Failed to find font resource at path '{resource}'");
        }

        var fontSystem = new FontSystem();
        fontSystem.AddFont(font);

        return fontSystem;
    }
}
