using Enginus.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Enginus.SceneObject
{
    public class ForgroundObject : SceneObject
    {
        public ForgroundObject(string name, Rectangle recSprite, string texture, ContentManager content, float layerDepth)
            : base(name, recSprite, texture, content, CursorTexturType.Pointer, layerDepth, string.Empty)
        {
        }
    }
}







