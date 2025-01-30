using Enginus.Control;
using Enginus.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Enginus.SceneObject
{
    public class InventoryObject : SceneObject
    {
        public int itemId;
        public InventoryObject(int id, string name, Rectangle recSprite, string texture, ContentManager content, float layerDepth)
            : base(name, recSprite, texture, content, CursorTexturType.Intract, layerDepth, string.Empty)
        {
            itemId = id;
        }
        public override void HandleInput(InputState input, Cursor cursor)
        {
            if (Render)
            {
                base.HandleInput(input, cursor);
            }
        }
        public void Update(Scene gameScene)
        {
            if (Render)
            {
                if (Activated)
                {
                    Render = Activated = false;
                }
            }
        }
    }
}