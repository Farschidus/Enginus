using Enginus.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Enginus.Particle
{
    public class MonkeyParticle : ParticleEngine
    {
        // keep a timer that will tell us when it's time to add more particles to the scene.
        float TimeBetweenMusicNotesPuff;
        float timeTillPuff;

        public MonkeyParticle() { }
        public override void Initialize()
        {
            textureFilename = "Particles/smoke3";
            Name = "MonkeyParticle";
            howManyEffects = 50;
            LayerDepth = 1f;

            TimeBetweenMusicNotesPuff = 0.2f;
            timeTillPuff = 0.0f;

            InitializeConstants();
            base.Initialize();
        }
        /// <summary>
        /// Set up the constants that will give this particle system its behavior and properties.
        /// </summary>
        protected override void InitializeConstants()
        {
            minInitialSpeed = 0f;
            maxInitialSpeed = 0f;
            // we don't want the particles to accelerate at all, aside from what we
            // do in our overriden InitializeParticle.
            minAcceleration = 1.0f;
            maxAcceleration = 3.0f;
            // long lifetime, this can be changed to create thinner or thicker smoke.
            // tweak minNumParticles and maxNumParticles to complement the effect.
            minLifetime = 2.0f;
            maxLifetime = 5.5f;
            // Scaling
            minScale = 3.0f;
            maxScale = 5.0f;
            //Numbers of Particles
            minNumParticles = 2;
            maxNumParticles = 2;
            // rotate slowly, we want a fairly relaxed effect
            minRotationSpeed = -MathHelper.Pi / 20;
            maxRotationSpeed = MathHelper.Pi / 20;

            blendState = BlendState.AlphaBlend;
            ParticleColor = Color.FromNonPremultiplied(200, 200, 200, 50);
        }
        /// <summary>
        /// PickRandomDirection is overriden so that we can make the particles always 
        /// move have an initial velocity pointing up.
        /// </summary>
        /// <returns>a random direction which points basically up.</returns>
        protected override Vector2 PickRandomDirection()
        {
            // tweak this to make the smoke have more or less spread.
            float radians = Utils.RandomBetween(MathHelper.ToRadians(80), MathHelper.ToRadians(100));

            Vector2 direction = Vector2.Zero;
            // from the unit circle, cosine is the x coordinate and sine is the
            // y coordinate. We're negating y because on the screen increasing y moves
            // down the monitor.
            direction.X = (float)Math.Cos(radians);
            direction.Y = -(float)Math.Sin(radians);
            return direction;
        }
        /// <summary>
        /// InitializeParticle is overridden to add the appearance of wind.
        /// </summary>
        /// <param name="p">the particle to set up</param>
        /// <param name="where">where the particle should be placed</param>
        protected override void InitializeParticle(Particle p, Vector2 where)
        {
            base.InitializeParticle(p, where);

            // the base is mostly good, but we want to simulate a little bit of wind heading to the up.
            p.Acceleration.Y -= Utils.RandomBetween(10, 15);
        }

        public override void Update(GameTime gameTime)
        {
            timeTillPuff -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeTillPuff < 0)
            {
                Vector2 where = Vector2.Zero;
                where.X = Utils.RandomBetween(518, 787);
                where.Y = Utils.RandomBetween(853, 875);
                base.AddParticles(where);
                // and then reset the timer.
                timeTillPuff = TimeBetweenMusicNotesPuff;
            }
            base.Update(gameTime);
        }
    }
}
