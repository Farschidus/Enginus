using Enginus.Control;
using Enginus.Core;
using Enginus.StateMachine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
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

        protected Rectangle rectangle;
        public Rectangle Rectangle { get => rectangle; }

        protected CursorTexturType HoverCursor;
        readonly float LayerDepth;
        readonly FiniteStateManager fsm = new FiniteStateManager();
        public Dictionary<StateID, string> GroupActions = new Dictionary<StateID, string>();

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
            var stateMappings = new Dictionary<StateID, Func<FiniteState>>
        {
            { StateID.InitGroup, () => new InitGroupState(this) },
            { StateID.IdleGroup, () => new IdleGroupState(this) },
            { StateID.LookGroup, () => new LookGroupState() },
            { StateID.TalkGroup, () => new TalkGroupState() },
            { StateID.UseGroup, () => new UseGroupState() },
            { StateID.TakeGroup, () => new TakeGroupState() }
        };

            foreach (var groupAction in GroupActions)
            {
                if (stateMappings.TryGetValue(groupAction.Key, out var stateFactory))
                {
                    var state = stateFactory();
                    state.AddTransition((Transition)Enum.Parse(typeof(Transition), groupAction.Key.ToString()[..^5]), groupAction.Key);
                    fsm.AddState(state);
                }
            }

            fsm.CurrentState?.Act(scene.ScreenManager);
        }
        /// <summary>
        /// This method will be called within the ActionState classes in Reason methods for firing needed transition
        /// </summary>
        /// <param name="t">Staring Transition</param>
        public void SetTransition(Transition tran)
        {
            fsm.PerformTransition(tran);
        }
        public virtual void HandleInput(InputManager input, MouseCursor mouseCursor)
        {
            if (!Render)
                return;

            if (rectangle.Contains(input.CurrentMousePoint))
            {
                mouseCursor.CursorType = HoverCursor;
                IsHover = true;
            }
            else
                IsHover = false;

            if (IsHover && input.MouseClicked)
                Activated = true;
            if (!IsHover && input.MouseClicked)
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