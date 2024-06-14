using ImGuiNET;

namespace Engine.Resources;

public readonly partial struct ResourceName
{
    private static string Create(string name)
    {
        var normalized = ResourceNameHelpers.Normalize(name);
        ResourceNameHelpers.ValidateString(normalized);

        return normalized;
    }
}

file static class ResourceNameHelpers
{
    public static string Normalize(string name)
    {
        var trimmed = name
            .Trim()
            .ToLower();

        if (trimmed.StartsWith(ResourceName.SeparatorChar))
        {
            trimmed = trimmed[1..];
        }
        else if (trimmed.StartsWith(ResourceName.InternalResourceChar)
            && trimmed[1] == ResourceName.SeparatorChar) 
        {
            trimmed = trimmed.Remove(1, 1);
        }

        return trimmed;
    }

    public static void ValidateString(string name)
    {
        if (name.StartsWith(ResourceName.SeparatorChar))
        {
            ThrowConsecutiveSeparators(name);
        }

        var startIndex = name.StartsWith(ResourceName.InternalResourceChar)
            ? 1 : 0;

        bool periodFound = false;
        var lastChar = '\0';
        for (int i = startIndex; i < name.Length; i++)
        {
            var ch = name[i];

            if (ch == '\\')
            {
                ThrowInvalidSeparator(name, ch);
            }
            if (ch == '#')
            {
                throw new ResourceNameException(name, $"Resource name may only contain one '#' at the start of the name. Any others are not allowed.");
            }
            if (ch == '.')
            {
                if (lastChar == '.')
                {
                    throw new ResourceNameException(name, $"Resource name may not use .. to reach the parent directory.");
                }
                if (periodFound)
                {
                    throw new ResourceNameException(name, $"Resource name may only contain one '{ch}' for the extension. Invalid resource is: '{name}'");
                }
                periodFound = true;
            }
            if (ch == '/')
            {
                if (periodFound)
                {
                    throw new ResourceNameException(name, $"Resource name may not contain any '{ch}'s after period.");
                }
                if (lastChar == '/')
                {
                    ThrowConsecutiveSeparators(name);
                }
            }

            lastChar = ch;
        }
    }

    private static void ThrowInvalidSeparator(string name, char invalidSeparator)
    {
        throw new ResourceNameException(name, $"Resource name must not contain any '{invalidSeparator}'. Use '{ResourceName.SeparatorChar}' instead.");
    }

    private static void ThrowConsecutiveSeparators(string name)
    {
        throw new ResourceNameException(name, $"Resource name must not contain consecutive '{ResourceName.SeparatorChar}'s.");
    }
}
