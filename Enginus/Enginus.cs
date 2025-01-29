using Enginus.Core;
using Enginus.Core.Utilities;
using Enginus.MenuScreens;
using Enginus.Screen;
using Enginus.Sound;
using Microsoft.Xna.Framework;

namespace Enginus
{
    public class Enginus : Game
	{
        #region Fields

        readonly GraphicsDeviceManager graphics;
        readonly ScreenManager screenManager;
        readonly AudioManager audio;

        // By preloading any assets used by UI rendering, we avoid framerate glitches
        // when they suddenly need to be loaded in the middle of a menu transition.
        static readonly string[] preloadAssets =
        {
            Constants.ASSET_BG_MESSAGE_BOX,
            Constants.ASSET_BG_MAIN_MENU 
        };

        #endregion

        #region Initialization

        public Enginus()
        {
            IsMouseVisible = false;
            audio = new AudioManager(this, "Content");

            graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = true
            };
            this.IsFixedTimeStep = true;
            Resolution.Init(ref graphics);
            Content.RootDirectory = "Content";

            Resolution.SetVirtualResolution(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT);
            Resolution.SetResolution(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, Constants.FULL_SCREEN);
            screenManager = new ScreenManager(this, audio);

            Components.Add(screenManager);
            Components.Add(audio);

            // Activate the first screens.
            screenManager.AddScreen(new Background(), null);
            screenManager.AddScreen(new MainMenu(), null);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            foreach (string asset in preloadAssets)
            {
                Content.Load<object>(asset);
            }
        }

        #endregion

        #region Draw

        protected override void Draw(GameTime gameTime)
        {
            //TODO: Remove Later because main menu will have it's own bg eventualy but maybe its good to have black screen always anyway!
            graphics.GraphicsDevice.Clear(Color.Black);
            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }

        #endregion
    }
}
