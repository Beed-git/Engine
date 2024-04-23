
namespace Engine.ECS.Components;

[Component]
public partial struct NameComponent
{
    public string Name;

    public NameComponent(string name)
    {
        Name = name;
    }

    public override readonly bool Equals(object? obj)
    {
        return obj is NameComponent component &&
               Name == component.Name;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(Name);
    }

    public static bool operator ==(NameComponent left, NameComponent right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NameComponent left, NameComponent right)
    {
        return !(left == right);
    }
}
