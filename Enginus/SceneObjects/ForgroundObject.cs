using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Enginus.Global;
using Enginus.Control;

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







