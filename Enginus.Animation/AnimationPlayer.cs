using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Enginus.Animation
{
	/// <summary>
	/// Controls playback of an Animation.
	/// </summary>
	public struct AnimationPlayer
    {
        #region Properties

        /// <summary>
        /// Gets the animation which is currently playing.
        /// </summary>
        public Animator Animation
        {
            get { return animation; }
        }
        Animator animation;
        /// <summary>
        /// Gets the index of the current frame in the animation.
        /// </summary>
        public int FrameIndex
        {
            get { return frameIndex; }
        }
        int frameIndex;
        /// <summary>
        /// Gets the texture origin.
        /// </summary>
        private Vector2 origin;
        public Vector2 Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
            }
        }
        /// <summary>
        /// Get whether animation is ended or not.
        /// </summary>
        private bool animationEnded;
        public bool AnimationEnded
        {
            get { return animationEnded; }
            set { animationEnded = value; }
        }

        /// <summary>
        /// The current TextureSpriteFile.
        /// </summary>
        TextureSpriteFile spriteFile;
        /// <summary>
        /// The amount of time in seconds that the current frame has been shown for.
        /// </summary>
        float timePerFrame;
        /// <summary>
        /// The amount of time in seconds that the current frame has been shown for.
        /// </summary>
        float totalElapsed;
        /// <summary>
        /// The amount of time in seconds that the player must wait for showing the next random animation.
        /// </summary>
        float delayTime;
        /// <summary>
        /// The zero base random number of the rows in the current spritesheet.
        /// </summary>
        int randomRow;
        /// <summary>
        /// The zero base number of the current rows in the spritesheet.
        /// </summary>
        int rowIndex;
        /// <summary>
        /// Says that it times to move to the next row in spritesheet.
        /// </summary>
        bool isNewRow;
        /// <summary>
        /// Says that the new frame position set in animation.
        /// </summary>
        bool isFrameSet;
        /// <summary>
        /// counting the number of loops that animation is played
        /// </summary>
        int loopCounter;

        #endregion

        #region Methods

        /// <summary>
        /// Begins or continues playback of an animation.
        /// </summary>
        public void LoadPlayer(Animator animation)
        {
            // If this animation is already running, do not restart it.
            if (Animation == animation)
                return;
            // Start the new animation.
            this.animation = animation;
            timePerFrame = (float)1 / animation.Fps;
            totalElapsed = 0;
            delayTime = 0;
            animationEnded = false;
            delayTime = animation.Delay;
            randomRow = 0;
            rowIndex = 0;
            frameIndex = 0;
            spriteFile = Animation.SpriteFiles[0];
            isFrameSet = false;
            loopCounter = 0;
        }
        public void SetFrame(int frameIndex)
        {
            isFrameSet = true;
            this.frameIndex = frameIndex;
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle? destRectangle, SpriteEffects spriteEffects)
        {
            if (Animation == null || Animation.RowsFrameCount == null)
                throw new NotSupportedException("No animation is currently playing or animation is not initialized properly");

            Rectangle destinationRectangle = (destRectangle.HasValue) ? destRectangle.Value : animation.AnimRectangle;

            switch (animation.AnimationFileType)
            {
                case AnimationFileType.Single:
                    {
                        if (animation.AnimationType.Equals(AnimationType.Linear))
                            this.DrawSingleLinear(gameTime, spriteBatch, destinationRectangle, spriteEffects);
                        else
                            this.DrawSingleRandom(gameTime, spriteBatch, destinationRectangle, spriteEffects);
                        return;
                    }
                case AnimationFileType.Seprate:
                    {
                        if (animation.AnimationType.Equals(AnimationType.Linear))
                            this.DrawSeprateLinear(gameTime, spriteBatch, destinationRectangle, spriteEffects);
                        else
                            this.DrawSeperateRandom(gameTime, spriteBatch, destinationRectangle, spriteEffects);
                        return;
                    }
            }
        }

        /// <summary>
        /// Advances the time position and draws the correct sprite file of the animation.
        /// </summary>
        private void DrawSingleLinear(GameTime gameTime, SpriteBatch spriteBatch, Rectangle destRectangle, SpriteEffects spriteEffects)
        {
            if (!animationEnded)
            {
                delayTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (delayTime > Animation.Delay)
                {
                    totalElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (totalElapsed > timePerFrame)
                    {
                        totalElapsed -= timePerFrame;

                        if (Animation.RowsFrameCount[rowIndex].Equals(frameIndex + 1))
                            isNewRow = true;
                        else
                            isNewRow = false;

                        if (Animation.IsLooping)
                        {                            
                            if (Animation.LoopCount.Equals(-1))
                            {
                                frameIndex = (frameIndex + 1) % Animation.RowsFrameCount[rowIndex];
                            }
                            else
                            {
                                if (loopCounter.Equals(animation.LoopCount))
                                {
                                    animationEnded = true;
                                }
                                else
                                {
                                    frameIndex = (frameIndex + 1) % Animation.RowsFrameCount[rowIndex];
                                    if (Animation.RowsFrameCount[rowIndex].Equals(frameIndex + 1) && Animation.RowsFrameCount.Length.Equals(rowIndex + 1))
                                        loopCounter++;
                                }
                            }
                        }
                        else
                        {
                            if (!isFrameSet)
                            {
                                frameIndex = Math.Min(frameIndex + 1, Animation.RowsFrameCount[rowIndex] - 1);
                                if (frameIndex == Animation.RowsFrameCount[rowIndex] - 1)
                                    animationEnded = true;
                            }
                        }
                        if (isNewRow && !animationEnded)
                        {
                            if (Animation.RowsFrameCount.Length.Equals(rowIndex + 1))
                                rowIndex = 0;
                            else
                                rowIndex++;
                        }
                        if (Animation.RowsFrameCount[rowIndex].Equals(frameIndex + 1) && Animation.RowsFrameCount.Length.Equals(rowIndex + 1))
                            delayTime = 0;
                    }
                }
            }
            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(FrameIndex * spriteFile.Width, rowIndex * spriteFile.Height, spriteFile.Width, spriteFile.Height);
            spriteBatch.Draw(spriteFile.Texture, destRectangle, source, Color.White, 0, origin, spriteEffects, Animation.LayerDepth);
        }
        private void DrawSeprateLinear(GameTime gameTime, SpriteBatch spriteBatch, Rectangle destRectangle, SpriteEffects spriteEffects)
        {
            if (!animationEnded)
            {
                delayTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (delayTime > Animation.Delay)
                {
                    totalElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (totalElapsed > timePerFrame)
                    {
                        totalElapsed -= timePerFrame;

                        if (Animation.IsLooping)
                        {
                            if (Animation.LoopCount.Equals(-1))
                            {
                                frameIndex = (frameIndex + 1) % Animation.SpriteFiles.Length;
                            }
                            else
                            {
                                if (loopCounter.Equals(animation.LoopCount))
                                {
                                    animationEnded = true;
                                }
                                else
                                {
                                    frameIndex = (frameIndex + 1) % Animation.SpriteFiles.Length;
                                    if (Animation.SpriteFiles.Length.Equals(frameIndex + 1))
                                        loopCounter++;
                                }
                            }
                        }
                        else
                        {
                            frameIndex = Math.Min(frameIndex + 1, Animation.SpriteFiles.Length - 1);
                            if (frameIndex == Animation.SpriteFiles.Length - 1)
                                animationEnded = true;
                        }
                        spriteFile = Animation.SpriteFiles[frameIndex];
                        if (Animation.SpriteFiles.Length.Equals(frameIndex + 1))
                            delayTime = 0;
                    }
                }
            }
            spriteBatch.Draw(spriteFile.Texture, destRectangle, null, Color.White, 0, origin, spriteEffects, Animation.LayerDepth);
        }
        /// <summary>
        /// Advances the time position and draws the correct sprite file of the animation.
        /// </summary>
        private void DrawSingleRandom(GameTime gameTime, SpriteBatch spriteBatch, Rectangle destRectangle, SpriteEffects spriteEffects)
        {
            delayTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (delayTime > Animation.Delay)
            {
                totalElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (totalElapsed > timePerFrame)
                {
                    totalElapsed -= timePerFrame;

                    frameIndex = Math.Min(frameIndex + 1, Animation.RowsFrameCount[randomRow] - 1);
                    if (frameIndex == Animation.RowsFrameCount[randomRow] - 1)
                    {
                        delayTime = 0;
                        animationEnded = true;
                        frameIndex = 0;
                        if (randomRow < animation.RowsFrameCount.Length - 1)
                            randomRow += 1;
                        else
                            randomRow = 0;
                    }
                }
            }
            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(FrameIndex * spriteFile.Width, randomRow * spriteFile.Height, spriteFile.Width, spriteFile.Height);
            spriteBatch.Draw(spriteFile.Texture, destRectangle, source, Color.White, 0, Origin, spriteEffects, Animation.LayerDepth);
        }
        private void DrawSeperateRandom(GameTime gameTime, SpriteBatch spriteBatch, Rectangle destRectangle, SpriteEffects spriteEffects)
        {
            delayTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (delayTime > Animation.Delay)
            {
                totalElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (totalElapsed > timePerFrame)
                {
                    totalElapsed -= timePerFrame;

                    frameIndex = Math.Min(frameIndex + 1, Animation.RowsFrameCount[randomRow] - 1);
                    if (frameIndex == Animation.RowsFrameCount[randomRow] - 1)
                    {
                        delayTime = 0;
                        animationEnded = true;
                        frameIndex = 0;
                        if (randomRow < animation.RowsFrameCount.Length - 1)
                            randomRow += 1;
                        else
                            randomRow = 0;
                    }
                    spriteFile = Animation.SpriteFiles[frameIndex];
                }
            }
            spriteBatch.Draw(spriteFile.Texture, destRectangle, null, Color.White, 0, Origin, spriteEffects, Animation.LayerDepth);
        }

        #endregion
    }
}