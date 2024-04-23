namespace Engine.Level;

public struct Tile
{
    public Tile(ushort value)
    {
        Value = value;
    }

    public ushort Value { get; set; }

    public static Tile None => new(0);
}