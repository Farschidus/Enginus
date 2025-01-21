using Enginus.Control;
using Enginus.Screen;

namespace Enginus.StateMachine
{
	public class InitGroupState : FiniteState
    {
        public InitGroupState(SceneObject.SceneObject obj)
        {
            stateID = StateID.InitGroup;
            sceneObj = obj;
        }
        public override void Reason(InputState input, SceneObject.SceneObject sceneObj)
        {
            //if (input.MouseClicked && sceneObj.ObjectRectangle.Contains(input.MouseClickedPoint))
            //    sceneObj.mSetTransition(Transition.Idle);
        }
        public override void Act(ScreenManager screenManager)
        {
            string Init = string.Empty;
            if (sceneObj.GroupActions.TryGetValue(StateMachine.StateID.InitGroup, out Init))
                screenManager.JSEngine.Execute(Init);
            //sceneObj.CurrentAnimation = sceneObj.Animations.Find(x => x.Name.Equals(Transition.Idle.ToString()));
            //if (sceneObj.CurrentAnimation != null)
            //    sceneObj.AnimationPlayer.LoadPlayer(sceneObj.CurrentAnimation);
        }
    }
    public class IdleGroupState : FiniteState
    {
        public IdleGroupState(SceneObject.SceneObject obj)
        {
            stateID = StateID.IdleGroup;
            sceneObj = obj;
        }
        public override void Reason(InputState input, SceneObject.SceneObject sceneObj)
        {
            if (input.MouseClicked && sceneObj.Rectangle.Contains(input.MouseClickedPoint))
                sceneObj.SetTransition(Transition.Talk);
        }
        public override void Act(ScreenManager screenManager)
        {
            //var a = sceneObj as NpcObject;
            //if (a != null)
            //    a.AnimationPlayer.LoadPlayer(a.CurrentAnimation);
            string Idle = string.Empty;
            if (sceneObj.GroupActions.TryGetValue(StateMachine.StateID.IdleGroup, out Idle))
                screenManager.JSEngine.Execute(Idle);
        }
    }
    public class LookGroupState : FiniteState
    {
        public LookGroupState()
        {
            stateID = StateID.LookGroup;
        }
        public override void Reason(InputState input, SceneObject.SceneObject sceneObj)
        {
            if (input.MouseClicked && sceneObj.Rectangle.Contains(input.MouseClickedPoint))
                sceneObj.SetTransition(Transition.Talk);
        }
        public override void Act(ScreenManager screenManager)
        {
            //sceneObj.CurrentAnimation = sceneObj.Animations.Find(x => x.Name.Equals(Transition.Idle.ToString()));
            //if (sceneObj.CurrentAnimation != null)
            //    sceneObj.AnimationPlayer.LoadPlayer(sceneObj.CurrentAnimation);
        }
    }
    public class TalkGroupState : FiniteState
    {
        public TalkGroupState()
        {
            stateID = StateID.TalkGroup;
        }
        public override void Reason(InputState input, SceneObject.SceneObject sceneObj)
        {
            if (input.MouseClicked && sceneObj.Rectangle.Contains(input.MouseClickedPoint))
                sceneObj.SetTransition(Transition.Use);
        }
        public override void Act(ScreenManager screenManager)
        {
            //sceneObj.CurrentAnimation = sceneObj.Animations.Find(x => x.Name.Equals(Transition.Talk.ToString()));
            //if (sceneObj.CurrentAnimation != null)
            //    sceneObj.AnimationPlayer.LoadPlayer(sceneObj.CurrentAnimation);
        }
    }
    public class UseGroupState : FiniteState
    {
        public UseGroupState()
        {
            stateID = StateID.UseGroup;
        }
        public override void Reason(InputState input, SceneObject.SceneObject sceneObj)
        {
            if (input.MouseClicked && sceneObj.Rectangle.Contains(input.MouseClickedPoint))
                sceneObj.SetTransition(Transition.Idle);
        }
        public override void Act(ScreenManager screenManager)
        {
            //sceneObj.CurrentAnimation = sceneObj.Animations.Find(x => x.Name.Equals(Transition.Use.ToString()));
            //if (sceneObj.CurrentAnimation != null)
            //    sceneObj.AnimationPlayer.LoadPlayer(sceneObj.CurrentAnimation);
        }
    }
    public class TakeGroupState : FiniteState
    {
        public TakeGroupState()
        {
            stateID = StateID.TakeGroup;
        }
        public override void Reason(InputState input, SceneObject.SceneObject sceneObj)
        {
            if (input.MouseClicked && sceneObj.Rectangle.Contains(input.MouseClickedPoint))
                sceneObj.SetTransition(Transition.Idle);
        }
        public override void Act(ScreenManager screenManager)
        {
            //sceneObj.CurrentAnimation = sceneObj.Animations.Find(x => x.Name.Equals(Transition.Take.ToString()));
            //sceneObj.AnimationPlayer.LoadPlayer(sceneObj.CurrentAnimation);
        }
    }
}