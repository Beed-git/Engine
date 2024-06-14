using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Rendering;

public class TextureSystem
    : IDisposable
{
    internal TextureSystem(GraphicsDevice graphics)
    {
        Graphics = graphics;

        WhiteSquare = new Texture2D(graphics, 1, 1);
        WhiteSquare.SetData([Color.White]);

        MissingTexture = new Texture2D(graphics, 2, 2);
        MissingTexture.SetData([Color.DarkViolet, Color.Black, Color.Black, Color.DarkViolet]);
    }

    public GraphicsDevice Graphics { get; private init; }
    public Texture2D WhiteSquare { get; private init; }
    public Texture2D MissingTexture { get; private init; }

    public Texture2D Create(int width, int height)
    {
        var texture = new Texture2D(Graphics, width, height);
        return texture;
    }

    public void Dispose()
    {
        WhiteSquare.Dispose();
        MissingTexture.Dispose();
    }
}