using Enginus.Control;
using Enginus.Global;
using Enginus.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Enginus.SceneObject
{
	public class PopupTriggerObject : SceneObject
    {
        public int Id;
        public PopupTriggerObject(int id, string name, Rectangle recSprite, string texture, ContentManager content, float layerDepth)
            : base(name, recSprite, texture, content, CursorTexturType.Intract, layerDepth, string.Empty)
        {
            this.Id = id;
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
        public void Update(Screen.GameScene gameScene)
        {
            if (Activated)
            {
                gameScene.ScreenManager.AddScreen(InitMiniScene(Id, gameScene.ScreenManager), null);
                Activated = false;
            }
        }


        public static PuzzleScene InitMiniScene(int Id, Screen.ScreenManager screenManager)
        {
            ContentManager content = new ContentManager(screenManager.Game.Services, "Content");
            PuzzlePopups puzzlePopupList = content.Load<PuzzlePopups>(Constants.Scenes_PuzzlePopups_File);
            PuzzlePopup puzzlePopup = puzzlePopupList.Popups.Find(asset => asset.Id.Equals(Id));
            PuzzleScene miniScene = new PuzzleScene(puzzlePopup.SceneName, puzzlePopup.Background);

            foreach (Interactive interactive in puzzlePopup.Interactives)
                miniScene.MiniInteractives.Add(new InteractiveObject(interactive.Name, interactive.Rectangle, interactive.Texture, content, interactive.LayerDepth));
            //foreach (AutoSprite auto in miniAssets.AutoSprites)
            //{
            //    AutoSpriteObject autoSprite = new AutoSpriteObject(auto.Name, content, true, auto.LayerDepth);
            //    foreach (Animation animation in auto.Animations)
            //    {
            //        autoSprite.mAddAnimation(new Animation.Animation(animation.Name, animation.Rectangle, content, animation.SpriteFile, animation.FramesRange, animation.Fps, animation.LoopCount, animation.Delay, animation.RowFrameCount, animation.FileType, animation.AnimType, animation.IsMoving, animation.MoveSpeed, animation.Destination, auto.LayerDepth));
            //    }
            //    autoSprite.mSetCurrentAnimation(0);
            //    miniScene.SceneAutoSprites.Add(autoSprite);
            //}
            return miniScene;
        }
    }
}