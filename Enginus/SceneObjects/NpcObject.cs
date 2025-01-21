using Enginus.Animation;
using Enginus.Control;
using Enginus.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Enginus.SceneObject
{
	public class NpcObject : SceneObject
    {
        public AnimationPlayer AnimationPlayer;
        public List<Animator> Animations;
        int currentAnimationIndex;
        public Animator CurrentAnimation 
        { 
            get {
                return currentAnimation; 
            } 
            set { 
                rectangle = value.AnimRectangle; 
                currentAnimation = value; 
            } 
        }
        Animator currentAnimation;

        public NpcObject(string name, ContentManager content, float layerDepth, string idleGroup)
            : base(name, null, Constants.Image_PlaceHolder, content, CursorTexturType.Talk, layerDepth, idleGroup)
        {
            Animations = new List<Animator>();
        }
        public void LoadAnimation(Animator animation)
        {
            Animations.Add(animation);
            CurrentAnimation = animation;
            AnimationPlayer.LoadPlayer(CurrentAnimation);
        }
        public void AddAnimation(Animator animation)
        {
            Animations.Add(animation);
            if (CurrentAnimation == null)
            {
                CurrentAnimation = animation;
                AnimationPlayer.LoadPlayer(CurrentAnimation);
            }
        }
        public void mSetCurrentAnimation(int index)
        {
            currentAnimationIndex = index;
            Animator currentAnimation = Animations[index];
            rectangle = currentAnimation.AnimRectangle;
            AnimationPlayer.LoadPlayer(currentAnimation);
        }
        public override void HandleInput(InputState input, Cursor cursor)
        {
            
            base.HandleInput(input, cursor);
            if (Render)
            {
                if (AnimationPlayer.AnimationEnded)
                {
                    if (Animations.Count == currentAnimationIndex + 1)
                    {
                        mSetCurrentAnimation(0);

                    }
                    else
                    {
                        currentAnimationIndex++;
                        mSetCurrentAnimation(currentAnimationIndex);
                    }
                }
            }
        }
        public override void Update(GameTime gameTime, Screen.GameScene scene)
        {
            base.Update(gameTime, scene);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(Render)
                AnimationPlayer.Draw(gameTime, spriteBatch, null, SpriteEffects.None);
        }
    }
}