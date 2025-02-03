using Enginus.Control;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Enginus.SceneObject;

interface ISceneObject
{
    void HandleInput(InputManager input, MouseCursor mouseCursor);
    void Update(GameTime gameTime, Screen.GameScene scene);
    void Draw(GameTime gameTime, SpriteBatch spriteBatch);
}