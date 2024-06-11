namespace Engine.Resources;

public readonly partial struct ResourceName
{
    private static string Normalize(string name)
    {
        var normalized = name
            .Trim()
            .ToLower();

        if (normalized.StartsWith(ResourceName.Separator))
        {
            normalized = normalized[1..];
        }

        ThrowIfInvalid(name);

        return normalized;
    }

    private static void ThrowIfInvalid(string path)
    {
        if (path.StartsWith(ResourceName.Separator))
        {
            throw new Exception($"Can't start with more than one separator ('{ResourceName.Separator}')");
        }

        char lastChar = '\0';
        for (int i = 0; i < path.Length; i++)
        {
            var ch = path[i];

            if (ch == '\\')
            {
                throw new Exception($"Resource name must not contain any '\\'. Use '{ResourceName.Separator}' instead. Invalid resource is: '{path}'");
            }
            if (ch == '.')
            {
                throw new Exception($"Resource name must not contain any '.'s. Invalid resource is: '{path}'");
            }
            if (ch == '/' && lastChar == '/')
            {
                throw new Exception($"Resource name must not contain consecutive '/'s. Invalid resource is: '{path}'");
            }

            lastChar = ch;
        }
    }
}
