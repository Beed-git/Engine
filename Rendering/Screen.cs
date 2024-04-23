using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Rendering;

public class Screen
{
    private readonly GraphicsDeviceManager _graphicsDeviceManager;
    
    private int _width;
    private int _height;

    internal Screen(GraphicsDeviceManager graphicsDeviceManager)
    {
        _graphicsDeviceManager = graphicsDeviceManager;
        _width = 0;
        _height = 0;

        IsMouseVisible = true;
        BackgroundColor = Color.LightSlateGray;
    }

    public bool IsMouseVisible { get; set; }
    public Color BackgroundColor { get; set; }

    public int Width => _width;
    public int Height => _height;
    public Point Size => new(_width, _height);

    public Rectangle Bounds => new(0, 0, Width, Height);

    public void Resize(Point size)
    {
        Resize(size.X, size.Y);
    }

    public void Resize(int width, int height)
    {
        if (_width != width || _height != height)
        {
            _width = width;
            _height = height;
            _graphicsDeviceManager.PreferredBackBufferWidth = width;
            _graphicsDeviceManager.PreferredBackBufferHeight = height;
            _graphicsDeviceManager.ApplyChanges();
        }
    }
}
