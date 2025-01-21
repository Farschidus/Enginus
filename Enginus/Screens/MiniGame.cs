using Enginus.Control;
using Enginus.Global;
using Enginus.SceneObject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Enginus.Screen
{
	public class PuzzleScene : GameScreen
    {
        const int closeDimension = 70;
        const int closeMargin = 20;
        const int itemWidth = 150;
        string name;
        string bgTexture;
        Texture2D backgroundTexture;
        Rectangle backgroundRectangle;
        Texture2D closeTexture;
        Rectangle closeRectangle;

        public List<LoopAnimationObject> MiniAutoSprites;
        public List<ForgroundObject> MiniForgrounds;
        public List<InteractiveObject> MiniInteractives;

        public PuzzleScene(string name, string backgroundTexture)
        {
            this.name = name;
            bgTexture = backgroundTexture;
            IsPopup = true;
            MiniAutoSprites = new List<LoopAnimationObject>();
            MiniForgrounds = new List<ForgroundObject>();
            MiniInteractives = new List<InteractiveObject>();

            TransitionOnTime = TimeSpan.FromSeconds(0.3);
            TransitionOffTime = TimeSpan.FromSeconds(0.3);
        }
        public override void LoadContent()
        {
            base.LoadContent();
            ContentManager content = ScreenManager.Game.Content;
            backgroundTexture = content.Load<Texture2D>(bgTexture);
            backgroundRectangle = new Rectangle(0, 0, Constants.GameOriginalWidth, Constants.GameOriginalHeight);
            closeTexture = content.Load<Texture2D>(Constants.Image_DialogueBg);
            closeRectangle = new Rectangle(Constants.GameOriginalWidth - closeMargin - closeDimension, closeMargin, closeDimension, closeDimension);
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
        }
        public override void HandleInput(InputState input)
        {
            if (input.IsPauseGame(null) || (input.MouseClicked && closeRectangle.Contains(input.MouseClickedPoint)))
                ExitScreen();
            foreach (InteractiveObject InteractiveObj in MiniInteractives)
            {
                InteractiveObj.HandleInput(input, ScreenManager.Cursor);
            }
        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen, InputState input)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen, input);
            if (IsActive)
            {
                foreach (InteractiveObject InteractiveObj in MiniInteractives)
                {
                    //InteractiveObj.Update(gameTime, this.);
                }
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);
            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Resolution.getScaleMatrix());

            spriteBatch.Draw(backgroundTexture, backgroundRectangle, null, color, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(closeTexture, closeRectangle, null, color, 0, Vector2.Zero, SpriteEffects.None, 1);

            foreach (InteractiveObject InteractiveObj in MiniInteractives)
            {
                InteractiveObj.Draw(gameTime, spriteBatch);
            }
            
            spriteBatch.End();
        }
    }
}