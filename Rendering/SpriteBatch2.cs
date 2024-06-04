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

    public void DrawFilledRect(RectangleF bounds, Color color)
    {
        throw new NotImplementedException();
    }

    public void DrawHollowRect(RectangleF bounds, float thickness, Color color)
    {
        _spriteBatch.Draw(_texture, new Vector2(bounds.X, bounds.Y), _texture.Bounds, color, 0.0f, Vector2.Zero, new Vector2(bounds.Width, thickness), SpriteEffects.None, 0.0f);
        _spriteBatch.Draw(_texture, new Vector2(bounds.X, bounds.Y + bounds.Height - thickness), _texture.Bounds, color, 0.0f, Vector2.Zero, new Vector2(bounds.Width, thickness), SpriteEffects.None, 0.0f);

        _spriteBatch.Draw(_texture, new Vector2(bounds.X, bounds.Y), _texture.Bounds, color, 0.0f, Vector2.Zero, new Vector2(thickness, bounds.Height), SpriteEffects.None, 0.0f);
        _spriteBatch.Draw(_texture, new Vector2(bounds.X + bounds.Width - thickness, bounds.Y), _texture.Bounds, color, 0.0f, Vector2.Zero, new Vector2(thickness, bounds.Height), SpriteEffects.None, 0.0f);
    }

    public void Dispose()
    {
        _spriteBatch.Dispose();
        _texture.Dispose();
    }
}
