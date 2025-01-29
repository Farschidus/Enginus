using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Enginus.Core.Utilities;

public class Utils
{
    // a handy little function that gives a random float between two
    // values. This will be used in several places in the sample, in particilar in
    // ParticleSystem.InitializeParticle.
    private static readonly Random random = new();
    public static float RandomBetween(float min, float max)
    {
        return min + (float)random.NextDouble() * (max - min);
    }
}

public static class PSBlendState
{
    public static readonly BlendState Multiply = new()
    {
        ColorSourceBlend = Blend.DestinationColor,
        ColorDestinationBlend = Blend.InverseSourceAlpha,
        ColorBlendFunction = BlendFunction.Add,
        AlphaSourceBlend = Blend.SourceAlpha,
        AlphaDestinationBlend = Blend.InverseSourceAlpha
    };
    public static readonly BlendState Screen = new()
    {
        ColorSourceBlend = Blend.InverseDestinationColor,
        ColorDestinationBlend = Blend.InverseSourceAlpha,
        ColorBlendFunction = BlendFunction.Add,
        AlphaSourceBlend = Blend.SourceAlpha,
        AlphaDestinationBlend = Blend.InverseSourceAlpha
    };
    public static readonly BlendState Darken = new()
    {
        ColorSourceBlend = Blend.One,
        ColorDestinationBlend = Blend.InverseSourceAlpha,
        ColorBlendFunction = BlendFunction.Min,
        AlphaSourceBlend = Blend.SourceAlpha,
        AlphaDestinationBlend = Blend.InverseSourceAlpha
    };
    public static readonly BlendState Lighten = new()
    {
        ColorSourceBlend = Blend.One,
        ColorDestinationBlend = Blend.InverseSourceAlpha,
        ColorBlendFunction = BlendFunction.Max,
        AlphaSourceBlend = Blend.SourceAlpha,
        AlphaDestinationBlend = Blend.InverseSourceAlpha
    };
    public static readonly BlendState LinearDodge = new()
    {
        ColorSourceBlend = Blend.One,
        ColorDestinationBlend = Blend.InverseSourceAlpha,
        ColorBlendFunction = BlendFunction.Add,
        AlphaSourceBlend = Blend.SourceAlpha,
        AlphaDestinationBlend = Blend.InverseSourceAlpha
    };
    public static readonly BlendState LinearBurn = new()
    {
        ColorSourceBlend = Blend.One,
        ColorDestinationBlend = Blend.InverseSourceAlpha,
        ColorBlendFunction = BlendFunction.ReverseSubtract,
        AlphaSourceBlend = Blend.SourceAlpha,
        AlphaDestinationBlend = Blend.InverseSourceAlpha
    };
    public static readonly BlendState ColorDodge = new()
    {
        AlphaSourceBlend = Blend.SourceAlpha,
        ColorSourceBlend = Blend.One,
        ColorBlendFunction = BlendFunction.Add,
        ColorDestinationBlend = Blend.InverseSourceAlpha,
        AlphaDestinationBlend = Blend.InverseSourceAlpha
    };
}

/// <summary>
/// Custom event argument which includes the index of the player who
/// triggered the event. This is used by the MenuEntry.Selected event.
/// </summary>
/// <remarks>
/// Constructor.
/// </remarks>
public class PlayerIndexEventArgs(PlayerIndex playerIndex) : EventArgs
{
    public PlayerIndex PlayerIndex { get; } = playerIndex;
}
