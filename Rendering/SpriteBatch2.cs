using Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Rendering;

public class SpriteBatch2
    : IDisposable
{
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;

    public SpriteBatch2(GraphicsDevice graphics)
    {
        _spriteBatch = new SpriteBatch(graphics);
        _texture = new Texture2D(graphics, 1, 1);
        _texture.SetData([Color.White]);
    }

    [Obsolete]
    public SpriteBatch TempBatch => _spriteBatch;

    public void DrawTile(TileSet tiles, int index, Rectangle destination)
    {
        DrawTile(tiles, index, destination, Color.White);
    }
    
    public void DrawTile(TileSet tiles, int index, Rectangle destination, Color color)
    {
        var source = tiles.GetSource(index);
        if (source == Rectangle.Empty)
        {
            // TODO: Replace with missing texture.
            _spriteBatch.Draw(_texture, destination, Color.Purple);
        }
        else
        {
            _spriteBatch.Draw(tiles.Texture, destination, source, color);
        }
    }

    public void DrawFilledRect(RectangleF bounds, Color color)
    {
        _spriteBatch.Draw(_texture, new Vector2(bounds.X, bounds.Y), null, color, 0.0f, Vector2.Zero, new Vector2(bounds.Width, bounds.Height), SpriteEffects.None, 0.0f);
    }

    public void DrawHollowRect(RectangleF bounds, float thickness, Color color)
    {
        _spriteBatch.Draw(_texture, new Vector2(bounds.X, bounds.Y), null, color, 0.0f, Vector2.Zero, new Vector2(bounds.Width, thickness), SpriteEffects.None, 0.0f);
        _spriteBatch.Draw(_texture, new Vector2(bounds.X, bounds.Y + bounds.Height - thickness), null, color, 0.0f, Vector2.Zero, new Vector2(bounds.Width, thickness), SpriteEffects.None, 0.0f);

        _spriteBatch.Draw(_texture, new Vector2(bounds.X, bounds.Y), null, color, 0.0f, Vector2.Zero, new Vector2(thickness, bounds.Height), SpriteEffects.None, 0.0f);
        _spriteBatch.Draw(_texture, new Vector2(bounds.X + bounds.Width - thickness, bounds.Y), null, color, 0.0f, Vector2.Zero, new Vector2(thickness, bounds.Height), SpriteEffects.None, 0.0f);
    }

    public void Begin()
    {
        _spriteBatch.Begin();
    }

    public void Begin(Camera2D camera,
                        SpriteSortMode spriteSortMode = SpriteSortMode.Deferred,
                        BlendState? blendState = null,
                        SamplerState? samplerState = null,
                        DepthStencilState? depthStencilState = null,
                        RasterizerState? rasterizerState = null,
                        Effect? effect = null)
    {
        _spriteBatch.Begin(spriteSortMode,
                   blendState ?? BlendState.AlphaBlend,
                   samplerState ?? SamplerState.LinearClamp,
                   depthStencilState ?? DepthStencilState.None,
                   rasterizerState ?? RasterizerState.CullCounterClockwise,
                   effect,
                   camera?.Transform ?? Matrix.Identity);
    }

    public void End()
    {
        _spriteBatch.End();
    }

    public void Dispose()
    {
        _spriteBatch.Dispose();
        _texture.Dispose();
    }
}
