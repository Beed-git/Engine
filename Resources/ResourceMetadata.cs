namespace Engine.Resources;

internal class ResourceMetadata
{
    public Version FileVersion { get; set; }

    public static Version CurrentVersion => new (1, 0);
}