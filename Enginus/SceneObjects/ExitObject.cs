using Enginus.Control;
using Enginus.Core;
using Enginus.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Enginus.SceneObject
{
	public class ExitObject : SceneObject
    {
        public Vector2 PlayerPosition;
        public Direction PlayerDirection;
        public float PlayerLayerDepth;
        bool quickExit;

        public ExitObject(Rectangle recSprite, ContentManager content, string texture, string sceneName, Vector2 playerPosition, Direction playerDirection, float playerLayerDepth, string initGroup)
            : base(sceneName, recSprite, texture, content, CursorTexturType.Exit, 0.99f, initGroup)
        {
            PlayerPosition = playerPosition;
            PlayerDirection = playerDirection;
            PlayerLayerDepth = playerLayerDepth;
        }

        public bool Intersects(Rectangle rectPlayer)
        {
            return (rectPlayer.Right > rectangle.Left && rectPlayer.Left < rectangle.Right &&
                    rectPlayer.Bottom > rectangle.Top && rectPlayer.Top < rectangle.Bottom);
        }
        public override void HandleInput(InputState input, Cursor cursor)
        {
            if (Render)
            {
                base.HandleInput(input, cursor);

                if (input.DoubleClick && IsHover)
                    quickExit = true;
            }
        }
        public override void Update(GameTime gameTime, Screen.GameScene scene)
        {
            base.Update(gameTime, scene);
            if (Render)
            {
                if (quickExit || Activated && this.rectangle.Contains((int)scene.Player.Position.X, (int)scene.Player.Position.Y))
                {
                    Loading.Load(scene.ScreenManager, false, null, GameSceneManager.InitNextScene(Name, scene.ScreenManager, PlayerPosition, PlayerDirection, PlayerLayerDepth));
                    //Scene sceneObj = (Scene)(Activator.CreateInstance(System.Type.GetType(sceneExit.ExitScene)));
                }
            }
        }
    }
}