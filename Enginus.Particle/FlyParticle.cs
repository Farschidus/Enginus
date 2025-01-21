using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Enginus.Global;

namespace Enginus.Particle
{
    public class FlyParticle : ParticleEngine
    {
        // keep a timer that will tell us when it's time to add more particles to the scene.
        float TimeBetweenMusicNotesPuff;
        float timeTillPuff;

        public FlyParticle() { }
        public override void Initialize()
        {
            textureFilename = "Particles/Fly";
            Name = "FlyParticle";
            howManyEffects = 3;
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
            minAcceleration = 0.0f;
            maxAcceleration = 0.0f;
            // long lifetime, this can be changed to create thinner or thicker smoke.
            // tweak minNumParticles and maxNumParticles to complement the effect.
            minLifetime = 5.0f;
            maxLifetime = 6.0f;
            // Scaling
            minScale = 1.0f;
            maxScale = 1.0f;
            //Numbers of Particles
            minNumParticles = 1;
            maxNumParticles = 1;
            // rotate slowly, we want a fairly relaxed effect
            minRotationSpeed = -MathHelper.Pi / 10;
            maxRotationSpeed = MathHelper.Pi / 10;

            blendState = BlendState.AlphaBlend;
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

            // the base is mostly good, but we want to simulate a little bit of wind heading to the up.
            p.Acceleration.X += Global.Helper.RandomBetween(10, 15);
            p.Acceleration.Y -= Global.Helper.RandomBetween(10, 15);
        }

        public override void Update(GameTime gameTime)
        {
            timeTillPuff -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeTillPuff < 0)
            {
                Vector2 where = Vector2.Zero;
                where.X = Global.Helper.RandomBetween(1152, 1401);
                where.Y = Global.Helper.RandomBetween(853, 918);
                base.AddParticles(where);
                // and then reset the timer.
                timeTillPuff = TimeBetweenMusicNotesPuff;
            }
            base.Update(gameTime);
        }
    }
}
