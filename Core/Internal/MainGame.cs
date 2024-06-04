using Engine.Core.Config;
using Engine.DebugGUI.Internal;
using Engine.Util;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Internal;

public class MainGame
    : Game
{
    private readonly ILogger _logger;
    private readonly GraphicsDeviceManager _graphicsDeviceManager;
    private readonly EngineConfig _config;

    private EngineCore _core;
    private ImGuiRenderer _imGuiRenderer;
    private SystemManager _systems;

    public MainGame(EngineConfig config)
    {
        _logger = config.LoggerFactory.CreateLogger<MainGame>();
        _config = config;

        FNALoggerEXT.LogInfo = (msg) => _logger.LogInformation("{}", msg);
        FNALoggerEXT.LogWarn = (msg) => _logger.LogWarning("{}", msg);
        FNALoggerEXT.LogError = (msg) => _logger.LogError("{}", msg);

        _graphicsDeviceManager = new GraphicsDeviceManager(this);
        _graphicsDeviceManager.PreferredBackBufferHeight = 1080;
        _graphicsDeviceManager.PreferredBackBufferWidth = 1920;
        _graphicsDeviceManager.IsFullScreen = false;
        _graphicsDeviceManager.SynchronizeWithVerticalRetrace = true;
    }

    protected override void Initialize()
    {
        _imGuiRenderer = new ImGuiRenderer(this);
        _imGuiRenderer.RebuildFontAtlas();

        _core = new EngineCore(_config, _graphicsDeviceManager);
        _core.Dependencies.Screen.Resize(1920, 1080);
        
        _systems = new SystemManager(_core);

        base.Initialize();
    }

    protected override void Update(GameTime gameTime)
    {
        IsMouseVisible = _core.Dependencies.Screen.IsMouseVisible;

        _systems.StageChange();
        _systems.SceneChange();

        DirectInput.Update();
        _systems.Update(gameTime);
        _core.Dependencies.Stages.CurrentStage.Events.RunEvents(_core.Dependencies.Stages.CurrentStage.SceneManager.Current);
        _core.Dependencies.Stages.CurrentStage.SceneManager.Current.Camera.Update(GraphicsDevice);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(_core.Dependencies.Screen.BackgroundColor);

        _systems.Render(gameTime);

        _imGuiRenderer.BeforeLayout(gameTime);
        _systems.ImGuiRender(gameTime);
        _imGuiRenderer.AfterLayout();

        base.Draw(gameTime);
    }
}