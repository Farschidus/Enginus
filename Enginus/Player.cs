using Enginus.Animation;
using Enginus.Control;
using Enginus.Global;
using Enginus.Navigation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Enginus
{
	/// <summary>
	/// Our Amnesiac adventurer!
	/// </summary>
	public class Player
    {
        float layerDepth;
        // Animations
        Animator currentAnimation;
        Animator runAnimation, idleAnimation;
        Animator northWalkAnimation, southWalkAnimation, westWalkAnimation, eastWalkAnimation, northWestWalkAnimation, southWestWalkAnimation, northEastWalkAnimation, southEastWalkAnimation;
        List<Point> path = new List<Point>();
        PathFinder pathFinder = new PathFinder();
        bool followingPath;
        bool canWalkMousePosition;
        int currentPathIndex = 1;
        float walkSpeed = 250; //200;
        Vector2 direction;
        float MizukiScale;

        #region Properties

        private const int playerWidth = 225;
        private const int playerHeight = 703;
        public Rectangle PlayerRectangle
        {
            get
            {
                return new Rectangle((int)(position.X - playerWidth), (int)(position.Y - playerHeight), playerWidth, playerHeight);
            }
        }

        public Screen.GameScene Scene
        {
            get { return scene; }
        }
        Screen.GameScene scene;
        // Physics state
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;
        private Point currentPosition
        {
            get { return new Point((int)position.X, (int)position.Y); }
        }
        public Vector2 Destination
        {
            get { return destination; }
            set { destination = value; }
        }
        Vector2 destination;
        
        /// <summary>
        /// The "close enough" limit, if the Player is inside this many pixel 
        /// to it's destination it's considered at it's destination
        /// </summary>
        private const float atDestinationLimit = 5;
        /// <summary>
        /// Linear distance to the Player's destination
        /// </summary>
        private float DistanceToDestination
        {
            get { return Vector2.Distance(position, destination); }
        }

        private SpriteFont font;
        private Enums.Direction normalizedDirection;

        #endregion

        #region Methods

        public Player(Screen.GameScene scene, Vector2 position, Enums.Direction playerDirection, float layerDepth)
        {
            MizukiScale = 0.65f;
            this.layerDepth = layerDepth;
            this.scene = scene;
            LoadContent();
            this.position = position;
            Destination = this.position;
            normalizedDirection = playerDirection;
            IdleAnimation();
        }
        public void LoadContent()
        {
            font = Scene.Content.Load<SpriteFont>("Fonts/DialoguesTahoma");
            idleAnimation = new Animator("MizukiIdle", Scene.Content, new SpriteFile[1] { new SpriteFile { Texture = "Sprites/Idles", Width = playerWidth, Height = playerHeight } }, 0.1f, 0, 0, new int[1] { 8 }, AnimationFileType.Single, AnimationType.Linear, layerDepth);

            northWalkAnimation = new Animator("MizukiNorthWalk", Scene.Content, new SpriteFile[1] { new SpriteFile { Texture = "Sprites/North", Width = 185, Height = 690 } }, 12f, -1, 0, new int[2] { 6, 6 }, AnimationFileType.Single, AnimationType.Linear, layerDepth);
            southWalkAnimation = new Animator("MizukiSouthWalk", Scene.Content, new SpriteFile[1] { new SpriteFile { Texture = "Sprites/South", Width = 214, Height = 700 } }, 12f, -1, 0, new int[2] { 6, 6 }, AnimationFileType.Single, AnimationType.Linear, layerDepth);
            westWalkAnimation = new Animator("MizukiWestWalk", Scene.Content, new SpriteFile[1] { new SpriteFile { Texture = "Sprites/West", Width = 262, Height = 704 } }, 12f, -1, 0, new int[2] { 6, 6 }, AnimationFileType.Single, AnimationType.Linear, layerDepth);
            northWestWalkAnimation = new Animator("MizukiNorthWestWalk", Scene.Content, new SpriteFile[1] { new SpriteFile { Texture = "Sprites/NorthWest", Width = 255, Height = 706 } }, 12f, -1, 0, new int[2] { 6, 6 }, AnimationFileType.Single, AnimationType.Linear, layerDepth);
            southWestWalkAnimation = new Animator("MizukiSouthWestWalk", Scene.Content, new SpriteFile[1] { new SpriteFile { Texture = "Sprites/SouthWest", Width = 258, Height = 699 } }, 12f, -1, 0, new int[2] { 6, 6 }, AnimationFileType.Single, AnimationType.Linear, layerDepth);
            eastWalkAnimation = new Animator("MizukiEastWalk", Scene.Content, new SpriteFile[1] { new SpriteFile { Texture = "Sprites/East", Width = 261, Height = 703 } }, 12f, -1, 0, new int[2] { 6, 6 }, AnimationFileType.Single, AnimationType.Linear, layerDepth);
            northEastWalkAnimation = new Animator("MizukiNorthEastWalk", Scene.Content, new SpriteFile[1] { new SpriteFile { Texture = "Sprites/NorthEast", Width = 254, Height = 709 } }, 12f, -1, 0, new int[2] { 6, 6 }, AnimationFileType.Single, AnimationType.Linear, layerDepth);
            southEastWalkAnimation = new Animator("MizukiSouthEastWalk", Scene.Content, new SpriteFile[1] { new SpriteFile { Texture = "Sprites/SouthEast", Width = 259, Height = 699 } }, 12f, -1, 0, new int[2] { 6, 6 }, AnimationFileType.Single, AnimationType.Linear, layerDepth);
        }
        public void IdleAnimation()
        {
            currentAnimation = idleAnimation;
            scene.AnimationPlayer.LoadPlayer(idleAnimation);
            scene.AnimationPlayer.Origin = new Vector2(scene.AnimationPlayer.Animation.FrameWidth / 2, scene.AnimationPlayer.Animation.FrameHeight - 10);
            switch (normalizedDirection)
            {
                case Enums.Direction.North:
                    scene.AnimationPlayer.SetFrame(2);
                    return;
                case Enums.Direction.South:
                    scene.AnimationPlayer.SetFrame(0);
                    return;
                case Enums.Direction.NorthEast:
                    scene.AnimationPlayer.SetFrame(3);
                    return;
                case Enums.Direction.NorthWest:
                    scene.AnimationPlayer.SetFrame(0);
                    return;
                case Enums.Direction.SouthEast:
                    scene.AnimationPlayer.SetFrame(0);
                    return;
                case Enums.Direction.SouthWest:
                    scene.AnimationPlayer.SetFrame(1);
                    return;
                case Enums.Direction.East:
                    scene.AnimationPlayer.SetFrame(5);
                    return;
                case Enums.Direction.West:
                    scene.AnimationPlayer.SetFrame(4);
                    return;
                default:
                    scene.AnimationPlayer.SetFrame(0);
                    return;
            }
        }
        public void RunAnimation()
        {
            scene.AnimationPlayer.Origin = new Vector2(scene.AnimationPlayer.Animation.FrameWidth / 2, scene.AnimationPlayer.Animation.FrameHeight - 10);
            scene.AnimationPlayer.LoadPlayer(runAnimation);
            currentAnimation = runAnimation;
        }
        public void Update(GameTime gameTime, float elapsedTime, InputState input, NavMesh sceneNavMesh)
        {
            if (input.MouseClicked)
            {
                canWalkMousePosition = sceneNavMesh.PolygonList.Any(x => x.Intersects(input.MouseClickedPoint));
                if (canWalkMousePosition)
                {
                    currentPathIndex = 1;
                    path.Clear();
                    path = pathFinder.GetPath(currentPosition, input.MouseClickedPoint, sceneNavMesh);
                    if (path.Count >= 2)
                    {
                        followingPath = true;
                    }
                }
            }
            if (followingPath)
                UpdatePath(elapsedTime);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            scene.AnimationPlayer.Draw(gameTime, spriteBatch, new Rectangle((int)(position.X), (int)(position.Y), (int)(Math.Ceiling(currentAnimation.FrameWidth * MizukiScale)), (int)(Math.Ceiling(currentAnimation.FrameHeight * MizukiScale))), SpriteEffects.None);
            //TODO: Delete This Line when ready to release or move it to diagnostic options to be controled manualy
            spriteBatch.DrawString(font, normalizedDirection.ToString(), new Vector2(10, 100), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.999f);
        }

        private Enums.Direction VectorToDirection(Vector2 direction)
        {
            Vector2 north = new Vector2(0, -1);
            Vector2 south = new Vector2(0, 1);
            Vector2 east = new Vector2(1, 0);
            Vector2 west = new Vector2(-1, 0);
            Vector2 northEast = new Vector2(1, -1);
            Vector2 southEast = new Vector2(1, 1);
            Vector2 northWest = new Vector2(-1, -1);
            Vector2 southWest = new Vector2(-1, 1);

            double northDiff = AcosDotProduct(direction, north);
            double southDiff = AcosDotProduct(direction, south);
            double eastDiff = AcosDotProduct(direction, east);
            double westDiff = AcosDotProduct(direction, west);
            double northEastDiff = AcosDotProduct(direction, northEast);
            double southEastDiff = AcosDotProduct(direction, southEast);
            double northWestDiff = AcosDotProduct(direction, northWest);
            double southWestDiff = AcosDotProduct(direction, southWest);

            double smallest1 = Math.Min(Math.Min(northDiff, southDiff), Math.Min(westDiff, eastDiff));
            double smallest2 = Math.Min(Math.Min(northEastDiff, southWestDiff), Math.Min(northWestDiff, southEastDiff));
            double smallest = Math.Min(smallest1, smallest2);

            // yes there's a precidence if they're the same value, it doesn't matter
            if (smallest == northEastDiff)
            {
                runAnimation = northEastWalkAnimation;
                return Enums.Direction.NorthEast;
            }
            else if (smallest == northWestDiff)
            {
                runAnimation = northWestWalkAnimation;
                return Enums.Direction.NorthWest;
            }
            else if (smallest == southEastDiff)
            {
                runAnimation = southEastWalkAnimation;
                return Enums.Direction.SouthEast;
            }
            else if (smallest == southWestDiff)
            {
                runAnimation = southWestWalkAnimation;
                return Enums.Direction.SouthWest;
            }
            else if (smallest == northDiff)
            {
                runAnimation = northWalkAnimation;
                return Enums.Direction.North;
            }
            else if (smallest == southDiff)
            {
                runAnimation = southWalkAnimation;
                return Enums.Direction.South;
            }
            else if (smallest == westDiff)
            {
                runAnimation = westWalkAnimation;
                return Enums.Direction.West;
            }
            else if (smallest == eastDiff)
            {
                runAnimation = eastWalkAnimation;
                return Enums.Direction.East;
            }
            else
                return Enums.Direction.Unknown;

        }
        private double AcosDotProduct(Vector2 direction, Vector2 unitVector)
        {
            double DotProduct = (direction.X * unitVector.X) + (direction.Y * unitVector.Y);
            double directionSize = Math.Sqrt(Math.Pow(direction.X, 2) + Math.Pow(direction.Y, 2));
            double unitSize = Math.Sqrt(Math.Pow(unitVector.X, 2) + Math.Pow(unitVector.Y, 2));
            return Math.Acos(DotProduct / (directionSize * unitSize));
        }
        private void UpdatePath(float elapsedTime)
        {
            destination = new Vector2(path[currentPathIndex].X, path[currentPathIndex].Y);
            direction = -(position - destination);
            normalizedDirection = VectorToDirection(direction);
            if (direction != Vector2.Zero)
                direction.Normalize();
            position += ((direction * walkSpeed) * elapsedTime);
            //TODO: do it without rounding by website help on spriteBatch properties in Scene No. Draw Methods
            position = new Vector2((float)Math.Floor(position.X), (float)Math.Floor(position.Y));
            RunAnimation();

            if (DistanceToDestination < atDestinationLimit)
            {
                currentPathIndex++;
                if (currentPathIndex.Equals(path.Count))
                {
                    followingPath = false;
                    currentPathIndex = 1;
                    path.Clear();
                    IdleAnimation();
                }
            }
        }

        #endregion
    }
}