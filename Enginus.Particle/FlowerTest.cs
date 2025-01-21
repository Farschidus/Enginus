using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Enginus.Global;

namespace Enginus.Particle
{
    public class FlowerTest : ParticleEngine
    {
        int particleAlpha;
        // keep a timer that will tell us when it's time to add more particles to the scene.
        float TimeBetweenParticlePuff;
        float timeTillPuff;

        public FlowerTest() { }
        public override void Initialize()
        {
            textureFilename = "Particles/Flower";
            Name = "AlleyParticle";
            howManyEffects = 30;
            LayerDepth = 1;

            particleAlpha = 15;
            TimeBetweenParticlePuff = 0f;
            timeTillPuff = 0f;

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
            minAcceleration = 2.0f;
            maxAcceleration = 10.0f;
            // long lifetime, this can be changed to create thinner or thicker smoke.
            // tweak minNumParticles and maxNumParticles to complement the effect.
            minLifetime = 2f;
            maxLifetime = 2f;
            // Scaling
            minScale = 1f;
            maxScale = 1f;
            //Numbers of Particles
            minNumParticles = 100;
            maxNumParticles = 100;
            // rotate slowly, we want a fairly relaxed effect
            minRotationSpeed = 0;
            maxRotationSpeed = 0;

            blendState = BlendState.Additive;
            ParticleColor = Color.FromNonPremultiplied(255, 255, 255, particleAlpha);
        }
        /// <summary>
        /// PickRandomDirection is overriden so that we can make the particles always 
        /// move have an initial velocity pointing up.
        /// </summary>
        /// <returns>a random direction which points basically up.</returns>
        protected override Vector2 PickRandomDirection()
        {
            // tweak this to make the smoke have more or less spread.
            float radians = Global.Helper.RandomBetween(MathHelper.ToRadians(80), MathHelper.ToRadians(100));

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
            // the base is mostly good, but we want to simulate a little bit of wind.
            p.Acceleration.X += Global.Helper.RandomBetween(-50, -150);
            p.Acceleration.Y += Global.Helper.RandomBetween(50, 150);
        }

        public override void Update(GameTime gameTime)
        {
            timeTillPuff += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeTillPuff > TimeBetweenParticlePuff)
            {
                Vector2 where = Vector2.Zero;
                where.X = 1342;
                where.Y = 229;
                base.AddParticles(where);
                // and then reset the timer.
                timeTillPuff = 0;
            }
            base.Update(gameTime);
        }
    }
}