using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Enginus.Particle
{
	/// <summary>
	/// ParticleSystem is an abstract class that provides the basic functionality to
	/// create a particle effect. Different subclasses will have different effects,
	/// such as fire, explosions, and plumes of smoke. To use these subclasses, 
	/// simply call AddParticles, and pass in where the particles should exist
	/// </summary>
	public abstract class ParticleEngine
    {
        #region Fildes and Properties

        // these two values control the order that particle systems are drawn in.
        // typically, particles that use additive blending should be drawn on top of
        // particles that use regular alpha blending. ParticleSystems should therefore
        // set their DrawOrder to the appropriate value in InitializeConstants, though
        // it is possible to use other values for more advanced effects.
        public const int AlphaBlendDrawOrder = 100;
        public const int AdditiveDrawOrder = 200;
        /// <summary>
        /// returns the number of particles that are available for a new effect.
        /// </summary>
        public int FreeParticleCount
        {
            get { return freeParticles.Count; }
        }
        public string Name;
        public float LayerDepth;

        public ContentManager  content;
        private Texture2D texture;
        // the origin when we're drawing textures. this will be the middle of the
        // texture.
        private Vector2 origin;
        // this number represents the maximum number of effects this particle system
        // will be expected to draw at one time. this is set in the constructor and is
        // used to calculate how many particles we will need.
        public int howManyEffects;        
        // the array of particles used by this system. these are reused, so that calling
        // AddParticles will not cause any allocations.
        Particle[] particles;
        // the queue of free particles keeps track of particles that are not curently
        // being used by an effect. when a new effect is requested, particles are taken
        // from this queue. when particles are finished they are put onto this queue.
        Queue<Particle> freeParticles;


        # endregion

        // This region of values control the "look" of the particle system, and should 
        // be set by deriving particle systems in the InitializeConstants method. The
        // values are then used by the virtual function InitializeParticle. Subclasses
        // can override InitializeParticle for further
        // customization.
        #region constants to be set by subclasses

        /// <summary>
        /// minNumParticles and maxNumParticles control the number of particles that are
        /// added when AddParticles is called. The number of particles will be a random
        /// number between minNumParticles and maxNumParticles.
        /// </summary>
        protected int minNumParticles;
        protected int maxNumParticles;
       
        /// <summary>
        /// this controls the texture that the particle system uses. It will be used as
        /// an argument to ContentManager.Load.
        /// </summary>
        public string textureFilename;

        /// <summary>
        /// minInitialSpeed and maxInitialSpeed are used to control the initial velocity
        /// of the particles. The particle's initial speed will be a random number 
        /// between these two. The direction is determined by the function 
        /// PickRandomDirection, which can be overriden.
        /// </summary>
        protected float minInitialSpeed;
        protected float maxInitialSpeed;

        /// <summary>
        /// minAcceleration and maxAcceleration are used to control the acceleration of
        /// the particles. The particle's acceleration will be a random number between
        /// these two. By default, the direction of acceleration is the same as the
        /// direction of the initial velocity.
        /// </summary>
        protected float minAcceleration;
        protected float maxAcceleration;

        /// <summary>
        /// minRotationSpeed and maxRotationSpeed control the particles' angular
        /// velocity: the speed at which particles will rotate. Each particle's rotation
        /// speed will be a random number between minRotationSpeed and maxRotationSpeed.
        /// Use smaller numbers to make particle systems look calm and wispy, and large 
        /// numbers for more violent effects.
        /// </summary>
        protected float minRotationSpeed;
        protected float maxRotationSpeed;

        /// <summary>
        /// minLifetime and maxLifetime are used to control the lifetime. Each
        /// particle's lifetime will be a random number between these two. Lifetime
        /// is used to determine how long a particle "lasts." Also, in the base
        /// implementation of Draw, lifetime is also used to calculate alpha and scale
        /// values to avoid particles suddenly "popping" into view
        /// </summary>
        protected float minLifetime;
        protected float maxLifetime;

        /// <summary>
        /// to get some additional variance in the appearance of the particles, we give
        /// them all random scales. the scale is a value between minScale and maxScale,
        /// and is additionally affected by the particle's lifetime to avoid particles
        /// "popping" into view.
        /// </summary>
        protected float minScale;
        protected float maxScale;

        /// <summary>
        /// different effects can use different blend states. fire and explosions work
        /// well with additive blending, for example.
        /// </summary>
		protected BlendState blendState;

        /// <summary>
        /// Set the Color of Particles in drawing method
        /// </summary>
        protected Color ParticleColor;

        #endregion

        #region Methods

        public ParticleEngine() { }

        /// <summary>
        /// override the base class's Initialize to do some additional work; we want to
        /// call InitializeConstants to let subclasses set the constants that we'll use.
        /// 
        /// also, the particle array and freeParticles queue are set up here.
        /// </summary>
        public virtual void Initialize()
        {
            if (content == null || howManyEffects == 0)
                throw new Exception("Content or HowManyEffects did not set");
            // calculate the total number of particles we will ever need, using the
            // max number of effects and the max number of particles per effect.
            // once these particles are allocated, they will be reused, so that
            // we don't put any pressure on the garbage collector.
            particles = new Particle[howManyEffects * maxNumParticles];
            freeParticles = new Queue<Particle>(howManyEffects * maxNumParticles);
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new Particle();
                freeParticles.Enqueue(particles[i]);
            }
            if (string.IsNullOrEmpty(textureFilename))
                throw new InvalidOperationException("textureFilename wasn't set properly");
            // load the texture....
            texture = content.Load<Texture2D>(textureFilename);
            // calculate the center. this'll be used in the draw call,
            // we always want to rotate and scale around this point.
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;

            if (ParticleColor == Color.FromNonPremultiplied(0, 0, 0, 0))
                ParticleColor = Color.White;
        }
        /// <summary>
        /// this abstract function must be overriden by subclasses of ParticleSystem.
        /// It's here that they should set all the constants marked in the region
        /// "constants to be set by subclasses", which give each ParticleSystem its
        /// specific flavor.
        /// </summary>
        protected abstract void InitializeConstants();
        /// <summary>
        /// AddParticles's job is to add an effect somewhere on the screen. If there 
        /// aren't enough particles in the freeParticles queue, it will use as many as 
        /// it can. This means that if there not enough particles available, calling
        /// AddParticles will have no effect.
        /// </summary>
        /// <param name="where">where the particle effect should be created</param>
        public void AddParticles(Vector2 where)
        {
            // the number of particles we want for this effect is a random number
            // somewhere between the two constants specified by the subclasses.
            int numParticles = (int)Global.Helper.RandomBetween(minNumParticles, maxNumParticles);

            // create that many particles, if you can.
            for (int i = 0; i < numParticles && freeParticles.Count > 0; i++)
            {
                // grab a particle from the freeParticles queue, and Initialize it.
                Particle p = freeParticles.Dequeue();
                InitializeParticle(p, where);
            }
        }
        /// <summary>
        /// InitializeParticle randomizes some properties for a particle, then
        /// calls initialize on it. It can be overriden by subclasses if they 
        /// want to modify the way particles are created. For example, 
        /// SmokePlumeParticleSystem overrides this function make all particles
        /// accelerate to the right, simulating wind.
        /// </summary>
        /// <param name="p">the particle to initialize</param>
        /// <param name="where">the position on the screen that the particle should be
        /// </param>
        protected virtual void InitializeParticle(Particle p, Vector2 where)
        {
            // first, call PickRandomDirection to figure out which way the particle
            // will be moving. velocity and acceleration's values will come from this.
            Vector2 direction = PickRandomDirection();
            // pick some random values for our particle
            float velocity = Global.Helper.RandomBetween(minInitialSpeed, maxInitialSpeed);
            float acceleration = Global.Helper.RandomBetween(minAcceleration, maxAcceleration);
            float lifetime = Global.Helper.RandomBetween(minLifetime, maxLifetime);
            float scale = Global.Helper.RandomBetween(minScale, maxScale);
            float rotationSpeed = Global.Helper.RandomBetween(minRotationSpeed, maxRotationSpeed);
            // then initialize it with those random values. initialize will save those,
            // and make sure it is marked as active.
            p.Initialize(where, velocity * direction, acceleration * direction, lifetime, scale, rotationSpeed);
        }
        /// <summary>
        /// PickRandomDirection is used by InitializeParticles to decide which direction
        /// particles will move. The default implementation is a random vector in a
        /// circular pattern.
        /// </summary>
        protected virtual Vector2 PickRandomDirection()
        {
            float angle = Global.Helper.RandomBetween(0, MathHelper.TwoPi);
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
        /// <summary>
        /// Update will update all of the active particles.
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            // calculate dt, the change in the since the last frame. the particle
            // updates will use this value.
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // go through all of the particles...
            foreach (Particle p in particles)
            {
                
                if (p.Active)
                {
                    // ... and if they're active, update them.
                    p.Update(dt);
                    // if that update finishes them, put them onto the free particles
                    // queue.
                    if (!p.Active)
                    {
                        freeParticles.Enqueue(p);
                    }
                }   
            }
        }
        /// <summary>
        /// Draw will use ParticleSampleGame's sprite batch to render all of the active particles.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Particle p in particles)
            {
                // skip inactive particles
                if (!p.Active)
                    continue;

                // normalized lifetime is a value from 0 to 1 and represents how far
                // a particle is through its life. 0 means it just started, .5 is half
                // way through, and 1.0 means it's just about to be finished.
                // this value will be used to calculate alpha and scale, to avoid 
                // having particles suddenly appear or disappear.
                float normalizedLifetime = p.TimeSinceStart / p.Lifetime;

                // we want particles to fade in and fade out, so we'll calculate alpha
                // to be (normalizedLifetime) * (1-normalizedLifetime). this way, when
                // normalizedLifetime is 0 or 1, alpha is 0. the maximum value is at
                // normalizedLifetime = .5, and is
                // (normalizedLifetime) * (1-normalizedLifetime)
                // (.5)                 * (1-.5)
                // .25
                // since we want the maximum alpha to be 1, not .25, we'll scale the 
                // entire equation by 4.
                float alpha = 4 * normalizedLifetime * (1 - normalizedLifetime);
                Color color = ParticleColor * alpha;
                
                // make particles grow as they age. they'll start at 75% of their size,
                // and increase to 100% once they're finished.
                float scale = p.Scale * (.75f + .25f * normalizedLifetime);

                spriteBatch.Draw(texture, p.Position, null, color,
                    p.Rotation, origin, scale, SpriteEffects.None, LayerDepth);
            }
        }

        #endregion
    }
}
