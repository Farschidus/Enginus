using Enginus.Control;
using Enginus.Global;
using Enginus.StateMachine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Enginus.SceneObject
{
	public abstract class SceneObject : ISceneObject
    {
        /// <summary>
        /// When player clicked on scene object
        /// </summary>
        public bool Activated;
        public bool IsHover;
        public bool Render;
		public string Name { get; set; }
		public Texture2D Texture { get; set; }

		public Rectangle Rectangle
        {
            get
            {
                return rectangle;
            }
        }
        protected Rectangle rectangle;

        protected CursorTexturType HoverCursor;
        public Dictionary<StateID, string> GroupActions = new Dictionary<StateID, string>();
		readonly FiniteStateManager fsm = new FiniteStateManager();
		readonly float LayerDepth;

        public SceneObject(string name, Rectangle? recSprite, ContentManager content, CursorTexturType hoverCursor, float layerDepth, string initGroup) :
            this(name, recSprite, Constants.Image_PlaceHolder, content, hoverCursor, layerDepth, initGroup)
        { }

        public SceneObject(string name, Rectangle? recSprite, string texture, ContentManager content, CursorTexturType hoverCursor, float layerDepth, string initGroup)
        {
            Name = name;
            LayerDepth = layerDepth;
            HoverCursor = hoverCursor;
            IsHover = false;
            Activated = false;
            Render = true;
            Texture = content.Load<Texture2D>(texture);
            if (recSprite.HasValue)
                rectangle = new Rectangle(recSprite.Value.X, recSprite.Value.Y, recSprite.Value.Width, recSprite.Value.Height);

            if (!string.IsNullOrEmpty(initGroup))
            {
                GroupActions.Add(StateID.InitGroup, initGroup);
            }
            //if (!string.IsNullOrEmpty(idleGroup))
            //    GroupActions.Add(StateID.IdleGroup, idleGroup);
            //if (!string.IsNullOrEmpty(idleGroup))
            //    GroupActions.Add(StateID.LookGroup, a.Value);
            //if (!string.IsNullOrEmpty(idleGroup))
            //    GroupActions.Add(StateID.TalkGroup, a.Value);
            //if (!string.IsNullOrEmpty(idleGroup))
            //    GroupActions.Add(StateID.UseGroup, a.Value);
            //if (!string.IsNullOrEmpty(idleGroup))
            //    GroupActions.Add(StateID.TakeGroup, a.Value);

            // SHOULD BE THE LAST Thing to Init
            //mFSMInit(scene);
        }

        public void FSMInit(Screen.GameScene scene)
        {
            if (GroupActions.ContainsKey(StateID.InitGroup))
            {
                InitGroupState Init = new InitGroupState((SceneObject)this);
                Init.AddTransition(Transition.Init, StateID.InitGroup);
                fsm.AddState(Init);
            }
            if (GroupActions.ContainsKey(StateID.IdleGroup))
            {
                IdleGroupState Idle = new IdleGroupState((SceneObject)this);
                Idle.AddTransition(Transition.Idle, StateID.IdleGroup);
                fsm.AddState(Idle);
            }
            if (GroupActions.ContainsKey(StateID.LookGroup))
            {
                LookGroupState Look = new LookGroupState();
                Look.AddTransition(Transition.Look, StateID.LookGroup);
                fsm.AddState(Look);
            }
            if (GroupActions.ContainsKey(StateID.TalkGroup))
            {
                TalkGroupState Talk = new TalkGroupState();
                Talk.AddTransition(Transition.Talk, StateID.TalkGroup);
                fsm.AddState(Talk);
            }
            if (GroupActions.ContainsKey(StateID.UseGroup))
            {
                UseGroupState Use = new UseGroupState();
                Use.AddTransition(Transition.Use, StateID.UseGroup);
                fsm.AddState(Use);
            }
            if (GroupActions.ContainsKey(StateID.TakeGroup))
            {
                TakeGroupState Take = new TakeGroupState();
                Take.AddTransition(Transition.Take, StateID.TakeGroup);
                fsm.AddState(Take);
            }

            if (fsm.CurrentState != null)
            {
                fsm.CurrentState.Act(scene.ScreenManager);
            }
        }
        /// <summary>
        /// This method will be called within the ActionState classes in Reason methods for firing needed transition
        /// </summary>
        /// <param name="t">Staring Transition</param>
        public void SetTransition(Transition tran)
        {
            fsm.PerformTransition(tran);
        }
        public virtual void HandleInput(InputState input, Cursor cursor)
        {
            if (!Render)
                return;

            if (rectangle.Contains(input.CurrentMousePoint))
            {
                cursor.CursorType = HoverCursor;
                IsHover = true;
            }
            else
                IsHover = false;

            if (input.MouseClicked && IsHover)
                Activated = true;
            if (input.MouseClicked && !IsHover)
                Activated = false;

            if (fsm != null && fsm.CurrentState != null)
                fsm.CurrentState.Reason(input, this);
        }
        public virtual void Update(GameTime gameTime, Screen.GameScene scene)
        {
            if (!Render)
                return;

            if (fsm != null && fsm.CurrentState != null)
                fsm.CurrentState.Act(scene.ScreenManager);
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Render)
                spriteBatch.Draw(Texture, rectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, LayerDepth);
        }
    }
}