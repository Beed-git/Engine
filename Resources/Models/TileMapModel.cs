namespace Engine.Resources.Models;

public class TileMapModel
{
    public SizeModel Size { get; set; }
    public IEnumerable<TileLayerModel> Layers { get; set; }

    // Helper models.
    public class TileLayerModel
    {
        public string TileSet { get; set; }
        public IEnumerable<string> Tiles { get; set; } 
    }
}
