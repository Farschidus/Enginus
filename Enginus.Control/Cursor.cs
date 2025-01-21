using Enginus.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Enginus.Control
{
	public class Cursor
    {
        Texture2D cursorTexture;
        ContentManager content;
        CursorTexturType cursorType;
        Vector2 cursorPosition;
        bool isVisible;
        object activeInventoryItemName;

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

        public Cursor(ContentManager Content)
        {
            content = Content;
            cursorType = CursorTexturType.Pointer;
            isVisible = false;

            cursorTexture = Content.Load<Texture2D>(Constants.Cursor_Pointer);
        }
        public void Update(InputState input)
        {
            if (!isVisible) 
            { 
                isVisible = true;
            }
            cursorPosition.Y = input.CurrentMousePoint.Y;
            cursorPosition.X = input.CurrentMousePoint.X;

            mSetCursorTexture();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Resolution.getScaleMatrix());
                spriteBatch.Draw(cursorTexture, cursorPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                spriteBatch.End();
            }
        }

        private void mSetCursorTexture()
        {
            switch (cursorType)
            {
                case CursorTexturType.Pointer:                    
                        cursorTexture = content.Load<Texture2D>(Constants.Cursor_Pointer);
                        break;
                case CursorTexturType.Intract:
                        cursorTexture = content.Load<Texture2D>(Constants.Cursor_Intract);
                        break;
                case CursorTexturType.Talk:
                        cursorTexture = content.Load<Texture2D>(Constants.Cursor_Talk);
                        break;
                case CursorTexturType.Walk:
                        cursorTexture = content.Load<Texture2D>(Constants.Cursor_Walk);
                        break;
                case CursorTexturType.Exit:
                        cursorTexture = content.Load<Texture2D>(Constants.Cursor_Exit);
                        break;
                case CursorTexturType.Texture:
                        cursorTexture = CursorTexture;
                        break;
                default:
                        cursorTexture = content.Load<Texture2D>(Constants.Cursor_Pointer);
                        break;
            }
            cursorType = CursorTexturType.Pointer;
        }
    }

    public enum CursorTexturType
    {
        Pointer,
        Intract,
        Talk,
        Walk,
        Exit,
        Texture
    }
}