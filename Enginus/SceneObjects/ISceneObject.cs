using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Enginus.Control;

namespace Enginus.SceneObject
{
    interface ISceneObject
    {
        void HandleInput(InputState input, Cursor cursor);
        void Update(GameTime gameTime, Screen.GameScene scene);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}