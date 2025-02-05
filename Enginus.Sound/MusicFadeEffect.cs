using Microsoft.Xna.Framework;
using System;

namespace Enginus.Sound;

public class Music
{
    public string Name { get; set; }
    public string File { get; set; }
}
struct MusicFadeEffect
{
    public float SourceVolume { get; private set; }
    public float TargetVolume { get; private set; }

    private TimeSpan _time;
    private TimeSpan _duration;

    public MusicFadeEffect(float sourceVolume, float targetVolume, TimeSpan duration)
    {
        SourceVolume = sourceVolume;
        TargetVolume = targetVolume;
        _time = TimeSpan.Zero;
        _duration = duration;
    }
    public bool Update(TimeSpan time)
    {
        _time += time;

        if (_time >= _duration)
        {
            _time = _duration;
            return true;
        }

        return false;
    }
    public float GetVolume()
    {
        return MathHelper.Lerp(SourceVolume, TargetVolume, (float)_time.Ticks / _duration.Ticks);
    }
}

/// <summary>
/// Options for AudioManager.CancelFade
/// </summary>
public enum FadeCancelOptions
{
    /// <summary>
    /// Return to pre-fade volume
    /// </summary>
    Source,
    /// <summary>
    /// Snap to fade target volume
    /// </summary>
    Target,
    /// <summary>
    /// Keep current volume
    /// </summary>
    Current
}
