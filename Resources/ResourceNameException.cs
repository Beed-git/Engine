namespace Engine.Resources;

public class ResourceNameException
    : Exception
{
    public ResourceNameException(string name, string message)
        : base($"Invalid resource name '{name}'\n{message}")
    {
    }
}