namespace Engine.Resources.Models;

public class TileSetModel
{
    public IEnumerable<AutoModel> Auto { get; set; }
    public string Texture { get; set; }

    // Helper models.

    public class AutoModel
    {
        public RectangleModel Bounds { get; set; }
        public TileInformationModel Tiles { get; set; }
    }

    public class TileInformationModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

}