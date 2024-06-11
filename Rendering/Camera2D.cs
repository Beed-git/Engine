using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Rendering;

public class Camera2D
{
    private Matrix _transform;
    private bool _isDirty;

    private Vector2 _position;
    private float _zoom;

    public Camera2D()
    {
        _transform = Matrix.Identity;
        _isDirty = true;

        _position = Vector2.Zero;
        _zoom = 1.0f;
    }

    public static Camera2D None => new ();

    public Matrix Transform => _transform;
    public Vector2 Position
    {
        get => _position;
        set
        {
            if (_position != value)
            {
                _position = value;
                _isDirty = true;
            }
        }
    }

    public float Zoom
    {
        get => _zoom;
        set
        {
            if (_zoom != value)
            {
                _zoom = Math.Max(1f, value);
                _isDirty = true;
            }
        }
    }

    internal void Update(GraphicsDevice graphics)
    {
        if (_isDirty)
        {
            var offsetX = -_position.X - 0.5f;
            var offsetY = -_position.Y - 0.5f;
            _isDirty = false;
            _transform = Matrix.CreateTranslation(offsetX, offsetY, 0)
                * Matrix.CreateScale(_zoom)
                * Matrix.CreateTranslation(graphics.Viewport.Width * 0.5f, graphics.Viewport.Height * 0.5f, 0.0f);
        }
    }

    public Vector2 ScreenPositionToWorld(Vector2 position)
    {
        var inverse = Matrix.Invert(_transform);
        var transformed = Vector2.Transform(position, inverse);
        return transformed;
    }

    public Vector2 WorldPositionToScreen(Vector2 position)
    {
        var transformed = Vector2.Transform(position, _transform);
        return transformed;
    }
}
