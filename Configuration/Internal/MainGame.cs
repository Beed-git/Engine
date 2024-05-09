using Engine.DebugGUI;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Configuration.Internal;

public class MainGame
    : Game
{
    private readonly ILogger _logger;
    private readonly EngineCore _core;
    private readonly GraphicsDeviceManager _graphicsDeviceManager;

    private ImGuiRenderer _imGuiRenderer;
    private Dependencies _dependencies;
    private SystemManager _systems;

    internal MainGame(ILoggerFactory loggerFactory, EngineCore core)
    {
        _logger = loggerFactory.CreateLogger<MainGame>();
        _core = core;

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

        _dependencies = _core.BuildDependencies(_graphicsDeviceManager);
        _dependencies.Screen.Resize(1920, 1080);

        var stageRepo = _core.BuildStages(_dependencies);
        _systems = new SystemManager(_dependencies.LoggerFactory, stageRepo, _dependencies.Stages);

        base.Initialize();
    }

    protected override void Update(GameTime gameTime)
    {
        IsMouseVisible = _dependencies.Screen.IsMouseVisible;

        _systems.StageChange();
        _systems.SceneChange();

        _systems.Update(gameTime);
        _dependencies.Stages.CurrentStage.SceneManager.Current.Camera.Update(GraphicsDevice);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(_dependencies.Screen.BackgroundColor);

        _systems.Render(gameTime);

        _imGuiRenderer.BeforeLayout(gameTime);
        _systems.ImGuiRender(gameTime);
        _imGuiRenderer.AfterLayout();

        base.Draw(gameTime);
    }
}