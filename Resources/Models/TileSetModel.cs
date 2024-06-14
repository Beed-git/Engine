namespace Engine.Resources.Models;

public class TileSetModel
{
    public IEnumerable<AutoModel> Auto { get; set; }
    public string Texture { get; set; }

    // Helper models.

    public class AutoModel
    {
        public RectangleModel Bounds { get; set; }
        public SizeModel Tiles { get; set; }
    }

}