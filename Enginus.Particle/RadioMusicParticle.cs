using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Enginus.Particle
{
    /// <summary>
    /// SmokePlumeParticleSystem is a specialization of ParticleSystem which sends up a
    /// plume of smoke. The smoke is blown to the right by the wind.
    /// </summary>
    public class RadioMusicParticle : ParticleEngine
    {
        public RadioMusicParticle(ContentManager content, int howManyEffects)
        {
            textureFilename = "Scenes/No1/RadioParticle";
            InitializeConstants();
            base.Initialize();
        }
        /// <summary>
        /// Set up the constants that will give this particle system its behavior and
        /// properties.
        /// </summary>
        protected override void InitializeConstants()
        {
            minInitialSpeed = 50;
            maxInitialSpeed = 80;
            // we don't want the particles to accelerate at all, aside from what we
            // do in our overriden InitializeParticle.
            minAcceleration = 20;
            maxAcceleration = 40;
            // long lifetime, this can be changed to create thinner or thicker smoke.
            // tweak minNumParticles and maxNumParticles to complement the effect.
            minLifetime = 1.0f;
            maxLifetime = 3.0f;
            // Scaling
            minScale = .4f;
            maxScale = .6f;
            //Numbers of Particles
            minNumParticles = 1;
            maxNumParticles = 2;
            // rotate slowly, we want a fairly relaxed effect
            minRotationSpeed = 0.1f; //-MathHelper.PiOver2 ;
            maxRotationSpeed = 0.2f; //MathHelper.PiOver2;

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
            float radians = Global.Helper.RandomBetween(MathHelper.ToRadians(70), MathHelper.ToRadians(110));

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

            // the base is mostly good, but we want to simulate a little bit of wind
            // heading to the right.
            p.Acceleration.X += Global.Helper.RandomBetween(-30, 30);
        }
    }
}
