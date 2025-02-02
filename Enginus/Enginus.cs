using Enginus.Control;
using Enginus.Core;
using Enginus.Core.Utilities;
using Enginus.MenuScreens;
using Enginus.Screen;
using Enginus.Sound;
using Microsoft.Xna.Framework;

namespace Enginus;

public class Enginus : Game
	{
    readonly GraphicsDeviceManager graphics;
    ScreenManager screenManager;
    AudioManager audio;
    InputManager input;

    // By preloading any assets used by UI rendering, we avoid framerate glitches
    // when they suddenly need to be loaded in the middle of a menu transition.
    static readonly string[] preloadAssets =
    {
        Constants.ASSET_BG_MESSAGE_BOX,
        Constants.ASSET_BG_MAIN_MENU 
    };

    public Enginus()
    {
        IsMouseVisible = false;

        graphics = new GraphicsDeviceManager(this)
        {
            SynchronizeWithVerticalRetrace = true
        };
        this.IsFixedTimeStep = true;
        Resolution.Init(ref graphics);
        Content.RootDirectory = "Content";

        Resolution.SetVirtualResolution(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT);
        Resolution.SetResolution(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, Constants.FULL_SCREEN);

        audio = new AudioManager(this, "Content");
        input = new InputManager(this);
        screenManager = new ScreenManager(this, audio, input);
    }

    protected override void Initialize()
    {
        Components.Add(screenManager);
        Components.Add(audio);
        Components.Add(input);

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
        //TODO: Remove Later because main menu will have it's own bg eventualy but maybe its good to have black screen always anyway!
        graphics.GraphicsDevice.Clear(Color.Black);
        // The real drawing happens inside the screen manager component.
        base.Draw(gameTime);
    }
}
