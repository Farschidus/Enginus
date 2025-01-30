using Enginus.Control;
using Enginus.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Enginus.Inventory
{
	public class Icon
    {
        private bool isActivated;
        public bool IsHover;
        private Texture2D icon;
        private Rectangle iconRectangle;
        private ContentManager content;

        public Icon(ContentManager content)
        {
            this.content = content;
            IsHover = false;
            isActivated = false;
            icon = content.Load<Texture2D>(Constants.Inventory_IconNorm);
            iconRectangle = new Rectangle(1820, 980, 100, 100);
        }
        public void HandleInput(InputState input)
        {
            if (iconRectangle.Contains(input.CurrentMousePoint))
            {
                IsHover = true;
                icon = content.Load<Texture2D>(Constants.Inventory_IconHover);
            }
            else
            {
                IsHover = false;
                icon = content.Load<Texture2D>(Constants.Inventory_IconNorm);
            }  
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(icon, iconRectangle, null, Color.White,0, Vector2.Zero, SpriteEffects.None, 1);
        }
    }
}