namespace Engine.Level;

public class TileMap
{
    private readonly List<TileLayer> _layers;

    public TileMap(int width, int height)
    {
        Width = Math.Max(1, width);
        Height = Math.Max(1, height);
        _layers = [];
    }

    public int Width { get; private init; }
    public int Height { get; private init; }
    public IReadOnlyList<TileLayer> Layers => _layers;

    public void AddLayer(TileLayer layer)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(layer.Tiles.Length, Width * Height);

        _layers.Add(layer);
    }

    public void RemoveLayer(int index)
    {
        _layers.RemoveAt(index); 
    }
}