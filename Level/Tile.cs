
namespace Engine.Level;

public struct Tile
{
    public Tile(ushort value)
    {
        Value = value;
    }

    public ushort Value { get; set; }

    public static Tile None => new(0);

    public readonly override bool Equals(object? obj)
    {
        return obj is Tile tile &&
               Value == tile.Value;
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }

    public readonly override string ToString()
    {
        return Value.ToString();
    }

    public static bool operator ==(Tile left, Tile right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Tile left, Tile right)
    {
        return !(left == right);
    }
}