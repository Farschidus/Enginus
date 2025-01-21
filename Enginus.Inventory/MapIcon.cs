using Enginus.Control;
using Enginus.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Enginus.InventorySystem
{
	public class MapIcon
    {
        private bool isActivated;
        public bool IsHover;
        private Texture2D icon;
        private Rectangle iconRectangle;
        private ContentManager content;

        public MapIcon(ContentManager content)
        {
            this.content = content;
            IsHover = false;
            isActivated = false;
            icon = content.Load<Texture2D>(Constants.Image_MapIcon);
            iconRectangle = new Rectangle(1710, 980, 110, 110);
        }
        public void HandleInput(InputState input)
        {
            if (iconRectangle.Contains(input.CurrentMousePoint))
            {
                IsHover = true;
                icon = content.Load<Texture2D>(Constants.Image_MapIconHover);
            }
            else
            {
                IsHover = false;
                icon = content.Load<Texture2D>(Constants.Image_MapIcon);
            }  
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(icon, iconRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
        }
    }
}