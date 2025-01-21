using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Enginus.Global;

namespace Enginus.Particle
{
    /// <summary>
    /// SmokePlumeParticleSystem is a specialization of ParticleSystem which sends up a
    /// plume of smoke. The smoke is blown to the right by the wind.
    /// </summary>
    public class LighthouseFog : ParticleEngine
    {
        // keep a timer that will tell us when it's time to add more particles to the scene.
        float TimeBetweenParticlesPuff;
        float timeTillPuff;

        public LighthouseFog() { }
        public override void Initialize()
        {
            textureFilename = "Particles/test";
            Name = "BeachFogParticle";
            howManyEffects = 40;
            LayerDepth = 0.999f;

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
            minInitialSpeed = 3.0f;
            maxInitialSpeed = 10.0f;
            // we don't want the particles to accelerate at all, aside from what we
            // do in our overriden InitializeParticle.
            minAcceleration = 0.0f;
            maxAcceleration = 0.0f;
            // long lifetime, this can be changed to create thinner or thicker smoke.
            // tweak minNumParticles and maxNumParticles to complement the effect.
            minLifetime = 10.0f;
            maxLifetime = 15.0f;
            // Scaling
            minScale = 15.0f;
            maxScale = 20.0f;
            //Numbers of Particles
            minNumParticles = 5;
            maxNumParticles = 10;
            // rotate slowly, we want a fairly relaxed effect
            minRotationSpeed = -0.01f; // -MathHelper.Pi / 100;
            maxRotationSpeed = 0.01f; // MathHelper.Pi / 100;

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
            //p.Acceleration.X += Global.Helper.RandomBetween(10, 200);
            p.Acceleration.X += 3f;
        }

        public override void Update(GameTime gameTime)
        {
            timeTillPuff -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeTillPuff < 0)
            {
                Vector2 where = Vector2.Zero;
                where.X = Global.Helper.RandomBetween(0, 1920);
                where.Y = Global.Helper.RandomBetween(860, 1080);
                base.AddParticles(where);
                // and then reset the timer.
                timeTillPuff = TimeBetweenParticlesPuff;
            }
            base.Update(gameTime);
        }
    }
}
