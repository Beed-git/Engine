﻿namespace Engine.Resources;

public static class ResourceNameHelpers
{
    public static string Normalize(string name)
    {
        var normalized = name
            .Trim()
            .ToLower();

        if (normalized.StartsWith(Resource.Separator))
        {
            normalized = normalized[1..];
        }

        ThrowIfInvalid(name);

        return normalized;
    }

    private static void ThrowIfInvalid(string path)
    {
        if (path.StartsWith(Resource.Separator))
        {
            throw new Exception($"Can't start with more than one separator ('{Resource.Separator}')");
        }

        char lastChar = '\0';
        for (int i = 0; i < path.Length; i++)
        {
            var ch = path[i];

            if (ch == '\\')
            {
                throw new Exception($"Resource name must not contain any '\\'. Use '{Resource.Separator}' instead. Invalid resource is: '{path}'");
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