using Enginus.Animation;
using Enginus.SceneObject;
using Enginus.Screen;

namespace Enginus.StateMachine
{
	public class ActionFunctions
    {
        public void LoadAnimation(NpcObject npc, string animationName)
        {
            Animator anim = npc.Animations.Find(x => x.Name.Equals(animationName));
            npc.LoadAnimation(anim);
        }
        public void ChangeRender(SceneObject.SceneObject obj, bool render)
        {
            obj.Render = render;
        }
        public bool CheckInventory(GameScene scene, int objID)
        {
            return scene.ScreenManager.InventoryManager.HasItem(objID);
        }
        public void ChangeScene(ExitObject exitObj, GameScene scene)
        {
            Loading.Load(scene.ScreenManager, false, null, GameSceneManager.InitNextScene(exitObj.Name, scene.ScreenManager, exitObj.PlayerPosition, exitObj.PlayerDirection, exitObj.PlayerLayerDepth));
        }
        public void LoadAnimation(Player player)
        {
            player.IdleAnimation();
        }
    }
}