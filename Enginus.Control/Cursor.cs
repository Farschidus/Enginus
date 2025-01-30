using Enginus.Core;
using Enginus.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Enginus.Control
{
	public class Cursor(ContentManager Content)
    {
        private Texture2D cursorTexture = Content.Load<Texture2D>(Constants.Cursor_Pointer);
        private CursorTexturType cursorType = CursorTexturType.Pointer;
        private Vector2 cursorPosition;
        private bool isVisible = false;
        private object activeInventoryItemName;

        public Texture2D CursorTexture;
        public CursorTexturType CursorType
        {
            set
            {
                cursorType = value;
            }
        }        
        public bool IsVisible
        {
            set
            {
                isVisible = value;
            }
        }
        public object ActiveInventoryItemName
        {
            get
            {
                return activeInventoryItemName;
            }
            set
            {
                activeInventoryItemName = value;
            }
        }

        public void Update(InputState input)
        {
            if (!isVisible) 
            { 
                isVisible = true;
            }
            cursorPosition.Y = input.CurrentMousePoint.Y;
            cursorPosition.X = input.CurrentMousePoint.X;

            SetCursorTexture();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Resolution.GetScaleMatrix());
                spriteBatch.Draw(cursorTexture, cursorPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                spriteBatch.End();
            }
        }

        private void SetCursorTexture()
        {
            cursorTexture = cursorType switch
            {
                CursorTexturType.Pointer => Content.Load<Texture2D>(Constants.Cursor_Pointer),
                CursorTexturType.Intract => Content.Load<Texture2D>(Constants.Cursor_Intract),
                CursorTexturType.Talk => Content.Load<Texture2D>(Constants.Cursor_Talk),
                CursorTexturType.Walk => Content.Load<Texture2D>(Constants.Cursor_Walk),
                CursorTexturType.Exit => Content.Load<Texture2D>(Constants.Cursor_Exit),
                CursorTexturType.Texture => CursorTexture,
                _ => Content.Load<Texture2D>(Constants.Cursor_Pointer),
            };
            cursorType = CursorTexturType.Pointer;
        }
    }
}