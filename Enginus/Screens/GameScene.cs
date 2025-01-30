using Enginus.Animation;
using Enginus.Control;
using Enginus.Core;
using Enginus.Editor;
using Enginus.MenuScreens;
using Enginus.Navigation;
using Enginus.Particle;
using Enginus.SceneObject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Enginus.Screen
{
    public abstract class GameScene : GameScreen
    {
        #region Fields And Properties

        protected string SceneName;
        protected Rectangle backgroundRectangle;
        protected Texture2D backgroundTexture;
        public string BackgroundTexture;
        protected string SceneMusic;
        public Player Player
        {
            get { return player; }
        }
        public ContentManager Content
        {
            get { return content; }
        }
        protected ContentManager content;
        protected Player player;

        public AnimationPlayer AnimationPlayer;
        protected Random random = new Random();
        protected float pauseAlpha;
        protected bool isGamePause;
        protected float elapsedTime;
        protected List<ExitObject> SceneExits;
        protected List<NpcObject> SceneCharacters;
        protected List<LoopAnimationObject> SceneAutoSprites;
        protected List<ForgroundObject> SceneForgrounds;
        protected List<InventoryObject> SceneInventoryItems;
        protected List<InteractiveObject> SceneInteractives;
        protected List<PopupTriggerObject> SceneMiniGameTrigers;
        protected List<ParticleEngine> SceneParticles;
        public NavMesh SceneMesh;

        public Plotter plotter;
        Vector2 playerPosition;
        Direction playerDirection;
        float playerLayerDepth;

        #endregion

        #region Methods

        public GameScene(string sceneName, string background, string sceneMusic, Vector2 playerPosition, Direction playerDirection, float playerLayerDepth)
        { 
            SceneName = sceneName;
            BackgroundTexture = background;
            this.playerPosition = playerPosition;
            this.playerDirection = playerDirection;
            this.playerLayerDepth = playerLayerDepth;

            SceneMusic = sceneMusic;
            AnimationPlayer = new AnimationPlayer();
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            SceneExits = new List<ExitObject>();
            SceneCharacters = new List<NpcObject>();
            SceneAutoSprites = new List<LoopAnimationObject>();
            SceneForgrounds = new List<ForgroundObject>();
            SceneInventoryItems = new List<InventoryObject>();
            SceneInteractives = new List<InteractiveObject>();
            SceneMiniGameTrigers = new List<PopupTriggerObject>();
            SceneParticles = new List<ParticleEngine>();
            SceneMesh = new NavMesh();
        }
        public override void LoadContent()
        {
            base.LoadContent(); //TODO: no need since there is nothing happening in base method!
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            ScreenManager.Game.ResetElapsedTime();
            backgroundTexture = content.Load<Texture2D>(BackgroundTexture);
            backgroundRectangle = new Rectangle(0, 0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT);
            player = new Player(this, playerPosition, playerDirection, playerLayerDepth);
            ScreenManager.Audio.LoadSong(SceneMusic);
            ScreenManager.Audio.PlaySong(SceneMusic, true);
            plotter = new Plotter(ScreenManager, content);
            foreach (ConvexPolygon poly in SceneMesh.PolygonList)
                plotter.AddPolygon(poly);
            foreach (PolygonLink link in SceneMesh.Links)
                plotter.AddLink(link);
        }
        public override void UnloadContent()
        {
            content.Unload();
        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen, InputState input)
        {
            base.Update(gameTime, otherScreenHasFocus, false, input);
            elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
            {
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            }
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
        }
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("InputState Is Null");

            if (input.IsPauseGame(null))
            {
                ScreenManager.AddScreen(new PauseMenu());
                isGamePause = true;
            }
            else
            {
                isGamePause = false;
            }
            if (input.MouseClicked)
            {
                player.Destination = new Vector2(input.MouseClickedPoint.X, input.MouseClickedPoint.Y);
            }
            plotter.Update(input, this);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #endregion
    }
}