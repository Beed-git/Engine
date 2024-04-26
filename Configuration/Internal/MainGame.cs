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
    private StageRepository _stageRepository;
    private Dependencies _dependencies;

    internal MainGame(ILoggerFactory loggerFactory, EngineCore core)
    {
        _logger = loggerFactory.CreateLogger<MainGame>();
        _core = core;

        FNALoggerEXT.LogInfo = (msg) => _logger.LogInformation(msg);
        FNALoggerEXT.LogWarn = (msg) => _logger.LogWarning(msg);
        FNALoggerEXT.LogError = (msg) => _logger.LogError(msg);

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
        _stageRepository = _core.BuildStages(_dependencies);

        base.Initialize();
    }

    protected override void Update(GameTime gameTime)
    {
        IsMouseVisible = _dependencies.Screen.IsMouseVisible;

        _dependencies.Screen.Resize(1920, 1080);

        var stages = _dependencies.Stages;
        var stage = stages.CurrentStage;
        if (stages.Next is not null)
        {
            _logger.LogInformation("Destroying stage '{}'", stage.Name);
            foreach (var destroy in stage.DestroySystems)
            {
                destroy.Invoke();
            }

            stage = _stageRepository.Create(stages.Next);
            stages.CurrentStage = stage;
            stages.Next = null;

            _logger.LogInformation("Initializing stage '{}'", stage.Name);
            foreach (var init in stage.InitSystems)
            {
                init.Invoke();
            }
        }

        foreach (var update in stage.UpdateSystems)
        {
            update.Invoke(gameTime);
        }
        //stage.SceneManager.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(_dependencies.Screen.BackgroundColor);

        var stage = _dependencies.Stages.CurrentStage;
        foreach (var renderer in stage.RenderSystems)
        {
            renderer.Invoke(gameTime);
        }

        _imGuiRenderer.BeforeLayout(gameTime);
        foreach (var gui in stage.DebugUIs)
        {
            gui.Invoke(gameTime);
        }
        _imGuiRenderer.AfterLayout();

        base.Draw(gameTime);
    }
}