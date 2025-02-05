using Enginus.Core;
using Enginus.Core.Utilities;
using Enginus.MenuScreens;
using Enginus.Screen;
using Enginus.Sound;
using Microsoft.Xna.Framework;

namespace Enginus;

internal class Enginus : Game
	{
    readonly AudioManager audio;
    readonly ScreenManager screenManager;
    readonly GraphicsDeviceManager graphics;
    // By preloading any assets used by UI rendering, we avoid framerate glitches
    // when they suddenly need to be loaded in the middle of a menu transition.
    static readonly string[] preloadAssets =
    {
        Constants.ASSET_BG_MESSAGE_BOX,
        Constants.ASSET_BG_MAIN_MENU
    };

    public Enginus()
    {
        IsFixedTimeStep = true;
        IsMouseVisible = false;

        graphics = new GraphicsDeviceManager(this)
        {
            SynchronizeWithVerticalRetrace = true,
            PreferredBackBufferWidth = Constants.SCREEN_WIDTH,
            PreferredBackBufferHeight = Constants.SCREEN_HEIGHT,
        };

        Content.RootDirectory = "Content";

        Resolution.Init(ref graphics);
        Resolution.SetVirtualResolution(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT);
        Resolution.SetResolution(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, Constants.FULL_SCREEN);

        audio = new AudioManager(this);
        screenManager = new ScreenManager(this, audio);
    }

    protected override void Initialize()
    {
        Components.Add(screenManager);
        Components.Add(audio);

        // Activate the first screens.
        screenManager.AddScreen(new Background());
        screenManager.AddScreen(new MainMenu());

        base.Initialize();
    }

    protected override void LoadContent()
    {
        foreach (string asset in preloadAssets)
        {
            Content.Load<object>(asset);
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        // The real drawing happens inside the screen manager component.
        base.Draw(gameTime);
    }
}
