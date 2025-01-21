using Enginus.Control;
using Enginus.Global;
using Enginus.Inventory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Enginus.Screen
{
	class Inventory : GameScreen
    {
        const int inventoryWidth = 1000;
        const int inventoryHeight = 1000;
        const int itemWidth = 140;
        const int margin = 50;
        Texture2D backgroundTexture;
        Rectangle backgroundRectangle;

        public Inventory()
        {
            IsPopup = true;
            TransitionOnTime = TimeSpan.FromSeconds(0.3);
            TransitionOffTime = TimeSpan.FromSeconds(0.3);
        }
        public override void LoadContent()
        {
            base.LoadContent();
            ContentManager content = ScreenManager.Game.Content;
            backgroundTexture = content.Load<Texture2D>(Constants.Inventory_Bg);
            backgroundRectangle = new Rectangle((Constants.GameOriginalWidth / 2) - (inventoryWidth / 2),
                                                (Constants.GameOriginalHeight / 2) - (inventoryHeight / 2),
                                                inventoryWidth, 
                                                inventoryHeight);
        }
        public override void HandleInput(InputState input)
        {
            if (input.IsPauseGame(null) || (input.MouseClicked && !backgroundRectangle.Contains(input.MouseClickedPoint)))
                ExitScreen();

            foreach (Item item in ScreenManager.InventoryManager.ItemsCollection.Values)
            {                
                if (input.MouseClicked && item.ItemRectangle.Contains(input.MouseClickedPoint))
                {
                    ScreenManager.Cursor.CursorType = CursorTexturType.Texture;
                    ScreenManager.Cursor.CursorTexture = item.ItemTexture;
                    ScreenManager.Cursor.ActiveInventoryItemName = this;
                }
                item.Update(input, ScreenManager.Cursor, ScreenManager.InventoryManager);
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

            int xCounter, yCounter, recX, recY;
            xCounter = yCounter = recX = recY = 0;
 
            foreach (Item item in ScreenManager.InventoryManager.ItemsCollection.Values)
            {
                recY = backgroundRectangle.Y + (margin * (yCounter + 1) + yCounter * itemWidth);
                recX = backgroundRectangle.X + (margin * (xCounter + 1) + xCounter * itemWidth);

                spriteBatch.Draw(item.ItemTexture, new Rectangle(recX, recY, itemWidth, itemWidth), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
                xCounter++;
                if (xCounter.Equals(5))
                {
                    yCounter++;
                    xCounter = 0;
                }
            }
            spriteBatch.End();
        }
    }
}