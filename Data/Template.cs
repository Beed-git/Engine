using System.Collections.Immutable;

namespace Engine.Data;

public class Template
{
    public Template(string name, string description, IEnumerable<object> components)
    {
        Name = name ?? string.Empty;
        Description = description ?? string.Empty;
        Components = components.ToImmutableList();
    }

    public string Name { get; private init; }
    public string Description { get; private init; } 
    public IReadOnlyList<object> Components { get; private init; }
}
