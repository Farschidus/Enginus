using Enginus.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Enginus.Particle
{
    /// <summary>
    /// SmokePlumeParticleSystem is a specialization of ParticleSystem which sends up a
    /// plume of smoke. The smoke is blown to the right by the wind.
    /// </summary>
    public class BeachFogParticle : ParticleEngine
    {
        // keep a timer that will tell us when it's time to add more particles to the scene.
        float TimeBetweenParticlesPuff;
        float timeTillPuff;

        public BeachFogParticle() { }
        public override void Initialize()
        {
            textureFilename = "Particles/test";
            Name = "BeachFogParticle";
            howManyEffects = 30;
            LayerDepth = 0.3f;

            TimeBetweenParticlesPuff = 0.1f;
            timeTillPuff = 0.0f; 

            InitializeConstants();
            base.Initialize();
        }
        /// <summary>
        /// Set up the constants that will give this particle system its behavior and
        /// properties.
        /// </summary>
        protected override void InitializeConstants()
        {
            minInitialSpeed = 0.0f;
            maxInitialSpeed = 0.0f;
            // we don't want the particles to accelerate at all, aside from what we
            // do in our overriden InitializeParticle.
            minAcceleration = 0.0f;
            maxAcceleration = 0.0f;
            // long lifetime, this can be changed to create thinner or thicker smoke.
            // tweak minNumParticles and maxNumParticles to complement the effect.
            minLifetime = 10.0f;
            maxLifetime = 30.0f;
            // Scaling
            minScale = 15.0f;
            maxScale = 15.0f;
            //Numbers of Particles
            minNumParticles = 4;
            maxNumParticles = 4;
            // rotate slowly, we want a fairly relaxed effect
            minRotationSpeed = -0.001f; // -MathHelper.Pi / 100;
            maxRotationSpeed = 0.001f; // MathHelper.Pi / 100;

            blendState = BlendState.AlphaBlend;
            ParticleColor = Color.FromNonPremultiplied(254, 255, 255, 200);
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
            p.Acceleration.X += Utils.RandomBetween(10, 100);
            p.Acceleration.X += 0.7f;
        }

        public override void Update(GameTime gameTime)
        {
            timeTillPuff -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeTillPuff < 0)
            {
                Vector2 where = Vector2.Zero;
                where.X = Utils.RandomBetween(0, 1920);
                where.Y = Utils.RandomBetween(378, 580);
                base.AddParticles(where);
                // and then reset the timer.
                timeTillPuff = TimeBetweenParticlesPuff;
            }
            base.Update(gameTime);
        }
    }
}
