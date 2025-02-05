using Enginus.Core;
using Enginus.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Enginus.Control;

public class MouseCursor(ContentManager Content)
{
    private Vector2 cursorPosition;
    private CursorTexturType cursorType = CursorTexturType.Default;
    private Texture2D cursorTexture = Content.Load<Texture2D>(Constants.Cursor_Pointer);

    public Texture2D CursorTexture { get => cursorTexture; set => cursorTexture = value; }
    public CursorTexturType CursorType { get => cursorType;  set => cursorType = value; }
    public object ActiveInventoryItem { get; set; }

    public void Update(InputManager input)
    {
        cursorPosition.Y = input.CurrentMousePoint.Y;
        cursorPosition.X = input.CurrentMousePoint.X;

        SetCursorTexture();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Resolution.GetScaleMatrix());
        spriteBatch.Draw(cursorTexture, cursorPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        spriteBatch.End();
    }

    private void SetCursorTexture()
    {
        cursorTexture = cursorType switch
        {
            CursorTexturType.Default => Content.Load<Texture2D>(Constants.Cursor_Pointer),
            CursorTexturType.Intract => Content.Load<Texture2D>(Constants.Cursor_Intract),
            CursorTexturType.Talk => Content.Load<Texture2D>(Constants.Cursor_Talk),
            CursorTexturType.Walk => Content.Load<Texture2D>(Constants.Cursor_Walk),
            CursorTexturType.Exit => Content.Load<Texture2D>(Constants.Cursor_Exit),
            CursorTexturType.Texture => CursorTexture,
            _ => Content.Load<Texture2D>(Constants.Cursor_Pointer),
        };
        cursorType = CursorTexturType.Default;
    }
}
