using Enginus.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Enginus.Animation
{
    public struct FrameRange
    {
        public int StartNumber { get; set; }
        public int EndNumber { get; set; }
    }

    public class Animator
    {
        #region Properties

        public string Name { get; private set; }
        public float LayerDepth;
        /// <summary>
        /// Number of frames in a second.
        /// </summary>
        public float Fps
        {
            get { return fps; }
            set { fps = value; }
        }
        float fps;
        /// <summary>
        /// Number of loops that the animation should play. -1 is infinit.
        /// </summary>
        public int LoopCount
        {
            get { return loopCount; }
        }
        int loopCount;
        /// <summary>
        /// When the end of the animation is reached, should it
        /// continue playing from the beginning?
        /// </summary>
        public bool IsLooping
        {
            get { return isLooping; }
        }
        bool isLooping;
        /// <summary>
        /// Gets the width of a frame in the animation.
        /// </summary>
        public int FrameWidth
        {
            get { return spriteFiles[0].Width; }
        }
        /// <summary>
        /// Gets the height of a frame in the animation.
        /// </summary>
        public int FrameHeight
        {
            get { return spriteFiles[0].Height; }
        }
        /// <summary>
        /// Duration of time to wait for next random sprite row.
        /// </summary>
        public float Delay
        {
            get { return delay; }
        }
        float delay;
        /// <summary>
        /// Gets the number of frames in each row of the animation.
        /// </summary>
        public int[] RowsFrameCount
        {
            get { return rowsFrameCount; }
        }
        int[] rowsFrameCount;
        /// <summary>
        /// Gets the number of frames in each row of the animation.
        /// </summary>
        public Rectangle AnimRectangle
        {
            get
            {
                return animRectangle;
            }
        }
        Rectangle animRectangle;
        public AnimationFileType AnimationFileType
        {
            get { return animationFileType; }
        }
        AnimationFileType animationFileType;
        public AnimationType AnimationType
        {
            get { return animationType; }
        }
        AnimationType animationType;
        public TextureSpriteFile[] SpriteFiles
        {
            get { return spriteFiles; }
        }
        TextureSpriteFile[] spriteFiles;
        public SpriteFile SpriteFile
        {
            get { return spriteFile; }
        }
        SpriteFile spriteFile;
        public FrameRange FrameRange
        {
            get { return frameRange; }
        }
        FrameRange frameRange;
        public bool IsMoving { get; set; }
        public float MoveSpeed { get; set; }
        public Vector2 Destination { get; set; }
        
        #endregion

        #region Methods

        public Animator(string name, ContentManager content, SpriteFile[] spriteFiles, float fps, int loopCount, float delay, int[] rowsFrameCount, AnimationFileType animationFileType, AnimationType animType, float layerDepth)
        {
            Name = name;
            LayerDepth = layerDepth;
            this.fps = fps;
            this.loopCount = loopCount;
            if (loopCount.Equals(0))
                this.isLooping = false;
            else
                this.isLooping = true;
            this.delay = delay;
            this.rowsFrameCount = rowsFrameCount;
            this.animationFileType = animationFileType;
            this.animationType = animType;
            List<TextureSpriteFile> tempSpriteFiles = new List<TextureSpriteFile>();
            foreach (SpriteFile sf in spriteFiles)
            {
                tempSpriteFiles.Add(new TextureSpriteFile
                {
                    Texture = content.Load<Texture2D>(sf.Texture),
                    Width = sf.Width,
                    Height = sf.Height
                });
            }
            this.spriteFiles = tempSpriteFiles.ToArray();
            tempSpriteFiles.Clear();
        }
        public Animator(string name, Rectangle rectangle, ContentManager content, SpriteFile spriteFile, FrameRange frameRange, float fps, int loopCount, float delay, int[] rowsFrameCount, AnimationFileType animationFileType, AnimationType animType, bool isMoving, float moveSpeed, Vector2 destination, float layerDepth)
        {
            Name = name;
            LayerDepth = layerDepth;
            IsMoving = isMoving;
            if (IsMoving)
            {
                MoveSpeed = moveSpeed;
                Destination = destination;
            }

            animRectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            this.fps = fps;
            this.loopCount = loopCount;
            if (loopCount.Equals(0))
                this.isLooping = false;
            else
                this.isLooping = true;
            this.delay = delay;
            this.rowsFrameCount = rowsFrameCount;
            this.animationFileType = animationFileType;
            this.animationType = animType;
            this.frameRange = frameRange;
            this.spriteFile = spriteFile;
            List<TextureSpriteFile> tempSpriteFiles = new List<TextureSpriteFile>();
            for (int i = frameRange.StartNumber; i <= frameRange.EndNumber; i++)
            {
                tempSpriteFiles.Add(new TextureSpriteFile
                {
                    Texture = content.Load<Texture2D>(spriteFile.Texture + i.ToString("000")),
                    Width = spriteFile.Width,
                    Height = spriteFile.Height
                });
            }
            this.spriteFiles = tempSpriteFiles.ToArray();
            tempSpriteFiles.Clear();
        }
        
        #endregion
    }
}