using Enginus.Animation;
using Enginus.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Enginus.SceneObject
{
    public class LoopAnimationObject : SceneObject
    {
        #region Properties
        
        /// <summary>
        /// The "close enough" limit, if the object is inside this many pixel 
        /// to it's destination it's considered at it's destination
        /// </summary>
        private const float atDestinationLimit = 1;
        /// <summary>
        /// Linear distance to the object's destination
        /// </summary>
        private float DistanceToDestination
        {
            get { return Vector2.Distance(position, destination); }
        }
        private AnimationPlayer AnimationPlayer;
        public List<Animator> Animations;
        public bool IsLooping;
        int currentAnimationIndex;
        bool isMoving;
        Point startPoint;
        Vector2 position;
        Vector2 destination;
        Vector2 direction;
        float moveSpeed;

        #endregion

        public LoopAnimationObject(string name, ContentManager content, bool isLooping, float layerDepth)
            : base(name, null, Constants.Image_PlaceHolder, content, CursorTexturType.Talk, layerDepth, string.Empty)
        {
            IsLooping = isLooping;
            Animations = new List<Animator>();
        }
        public void SetCurrentAnimation(int index)
        {
            currentAnimationIndex = index;
            Animator currentAnimation = Animations[index];
            rectangle = currentAnimation.AnimRectangle;
            startPoint = new Point(currentAnimation.AnimRectangle.X, currentAnimation.AnimRectangle.Y);
            position = new Vector2(startPoint.X, startPoint.Y);
            this.isMoving = currentAnimation.IsMoving;
            if (this.isMoving)
            {
                this.moveSpeed = currentAnimation.MoveSpeed;
                this.destination = currentAnimation.Destination;
            }
            AnimationPlayer.LoadPlayer(currentAnimation);
        }
        public void AddAnimation(Animator animation)
        {
            Animations.Add(animation);
        }
        public override void Update(GameTime gameTime, Screen.GameScene scene)
        {
            // TODO: best place for adding state machine
            if (isMoving)
            {
                direction = -(position - destination);
                if (direction != Vector2.Zero)
                    direction.Normalize();
                position += direction * moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //TODO: do it without rounding by website help on spriteBatch properties in Scene No. Draw Methods
                position = new Vector2((float)Math.Floor(position.X), (float)Math.Floor(position.Y));
                //TODO: fix the issue about the tolerance with speed and DestinationLimit
                //TODO: There is an error in calculating the vector of start and end poind. (baker reach the 0y of screen in 1 1 destination??)
                if (DistanceToDestination > atDestinationLimit)
                {
                    rectangle.X = (int)position.X;
                    rectangle.Y = (int)position.Y;
                }
                else
                {
                    if (Animations[currentAnimationIndex].IsLooping)
                    {
                        AnimationPlayer.AnimationEnded = true;
                    }
                    else
                    {
                        isMoving = false;
                    }
                }
            }
            if (AnimationPlayer.AnimationEnded)
            {
                position = new Vector2(startPoint.X, startPoint.Y);
                rectangle.X = startPoint.X;
                rectangle.Y = startPoint.Y;

                if (Animations.Count != currentAnimationIndex + 1)
                {
                    currentAnimationIndex++;
                    SetCurrentAnimation(currentAnimationIndex);
                }
                else if (IsLooping)
                {
                    SetCurrentAnimation(0);
                }
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationPlayer.Draw(gameTime, spriteBatch, rectangle, SpriteEffects.None);
        }
    }
}