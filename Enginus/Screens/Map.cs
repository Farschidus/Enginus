using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Enginus.Global;
using Enginus.Control;

namespace Enginus.Screen
{
    class Map : GameScreen
    {
        const int mapWidth = 775;
        const int mapHeight = 582;
        Texture2D mapTexture;
        Rectangle mapRectangle;
        Texture2D lake;
        Rectangle LakeRectangle;

        public Map()
        {
            IsPopup = true;
            TransitionOnTime = TimeSpan.FromSeconds(0.3);
            TransitionOffTime = TimeSpan.FromSeconds(0.3);
        }
        public override void LoadContent()
        {
            base.LoadContent();
            ContentManager content = ScreenManager.Game.Content;
            mapTexture = content.Load<Texture2D>(Constants.Image_MapBg);
            mapRectangle = new Rectangle((Constants.GameOriginalWidth / 2) - (mapWidth / 2),
                                         (Constants.GameOriginalHeight / 2) - (mapHeight / 2),
                                         mapWidth, mapHeight);
            lake = content.Load<Texture2D>(Constants.Image_PlaceHolder);
            LakeRectangle = new Rectangle(mapRectangle.X + 341, mapRectangle.Y + 230, 124, 84);
        }
        public override void HandleInput(InputState input)
        {
            if (input.IsPauseGame(null) || (input.MouseClicked && !mapRectangle.Contains(input.MouseClickedPoint)))
                ExitScreen();
            if (LakeRectangle.Contains(input.CurrentMousePoint))
            {
                ScreenManager.Cursor.CursorType = CursorTexturType.Exit;
                if (input.MouseClicked)
                {
                    Loading.Load(ScreenManager, false, null, GameSceneManager.InitNextScene("Beach", ScreenManager, new Vector2(1800, 900), Enums.Direction.SouthWest, 0.9f));
                    TransitionOnTime = TimeSpan.FromSeconds(0.1);
                    TransitionOffTime = TimeSpan.FromSeconds(0.1);
                }
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            UpdateMapLocation();
            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Resolution.getScaleMatrix());

            spriteBatch.Draw(mapTexture, mapRectangle, color);
            spriteBatch.Draw(lake, LakeRectangle, color);

            spriteBatch.End();
        }

        void UpdateMapLocation()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // update location of map
            // each entry is to be centered horizontally
            mapRectangle.X = Constants.GameOriginalWidth / 2 - mapRectangle.Width / 2;

            if (ScreenState == ScreenState.TransitionOn)
                mapRectangle.X -= (int)(transitionOffset * 256);
            else
                mapRectangle.X += (int)(transitionOffset * 512);
        }
    }
}