using Enginus.Animation;
using Enginus.Control;
using Enginus.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Enginus.SceneObject;

public class NpcObject : SceneObject
{
    public AnimationPlayer AnimationPlayer;
    public List<AnimatoionManager> Animations;
    int currentAnimationIndex;
    public AnimatoionManager CurrentAnimation 
    { 
        get {
            return currentAnimation; 
        } 
        set { 
            rectangle = value.AnimRectangle; 
            currentAnimation = value; 
        } 
    }
    AnimatoionManager currentAnimation;

    public NpcObject(string name, ContentManager content, float layerDepth, string idleGroup)
        : base(name, null, Constants.Image_PlaceHolder, content, CursorTexturType.Talk, layerDepth, idleGroup)
    {
        Animations = new List<AnimatoionManager>();
    }
    public void LoadAnimation(AnimatoionManager animation)
    {
        Animations.Add(animation);
        CurrentAnimation = animation;
        AnimationPlayer.LoadPlayer(CurrentAnimation);
    }
    public void AddAnimation(AnimatoionManager animation)
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
        AnimatoionManager currentAnimation = Animations[index];
        rectangle = currentAnimation.AnimRectangle;
        AnimationPlayer.LoadPlayer(currentAnimation);
    }
    public override void HandleInput(InputManager input, MouseCursor mouseCursor)
    {
        
        base.HandleInput(input, mouseCursor);
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