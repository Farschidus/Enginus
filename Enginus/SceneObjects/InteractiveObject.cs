using Enginus.Control;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Enginus.SceneObject
{
	public class InteractiveObject : SceneObject
    {
        public InteractiveObject(string name, Rectangle recSprite, string texture, ContentManager content, float layerDepth)
            : base(name, recSprite, texture, content, CursorTexturType.Intract, layerDepth, string.Empty)
        {
        }
        public override void HandleInput(InputState input, Cursor cursor)
        {
            base.HandleInput(input, cursor);
            if (input.MouseClicked && IsHover)
            {
                Activated = true;
            }
            if (input.MouseClicked && !IsHover)
            {
                Activated = false;
            }
        }
        public override void Update(GameTime gameTime, Screen.GameScene scene)
        {
            if (Render)
            {
                base.Update(gameTime, scene);
                if (Activated)
                {
                    scene.ScreenManager.State.ChangeState("BakeryFirstVisit", true);
                    Activated = false;
                }
            }
        }
    }
}