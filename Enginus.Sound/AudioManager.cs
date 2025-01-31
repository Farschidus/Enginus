using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Enginus.Sound
{
    /// <summary>
    /// Manages playback of sounds and music.
    /// </summary>
    public class AudioManager : GameComponent
    {
        #region Private fields

        private ContentManager _content;

        private Dictionary<string, Song> _songs = new Dictionary<string, Song>();
        private Dictionary<string, SoundEffect> _sounds = new Dictionary<string, SoundEffect>();

        private Song _currentSong = null;
        private SoundEffectInstance[] _playingSounds = new SoundEffectInstance[MaxSounds];

        private bool _isMusicPaused = false;

        private bool _isFading = false;
        private MusicFadeEffect _fadeEffect;

        // Change MaxSounds to set the maximum number of simultaneous sounds that can be playing.
        private const int MaxSounds = 32;

        #endregion

        #region Public fields

        /// <summary>
        /// Gets the name of the currently playing song, or null if no song is playing.
        /// </summary>
        public string CurrentSong { get; private set; }
        /// <summary>
        /// Gets or sets the volume to play songs. 1.0f is max volume.
        /// </summary>
        public float MusicVolume
        {
            get { return MediaPlayer.Volume; }
            set { MediaPlayer.Volume = value; }
        }
        /// <summary>
        /// Gets or sets the master volume for all sounds. 1.0f is max volume.
        /// </summary>
        public float SoundVolume
        {
            get { return SoundEffect.MasterVolume; }
            set { SoundEffect.MasterVolume = value; }
        }
        /// <summary>
        /// Gets whether a song is playing or paused (i.e. not stopped).
        /// </summary>
        public bool IsSongActive { get { return _currentSong != null && MediaPlayer.State != MediaState.Stopped; } }
        /// <summary>
        /// Gets whether the current song is paused.
        /// </summary>
        public bool IsSongPaused { get { return _currentSong != null && _isMusicPaused; } }

        #endregion

        /// <summary>
        /// Creates a new Audio Manager. Add this to the Components collection of your Game.
        /// </summary>
        /// <param name="game">The Game</param>
        public AudioManager(Game game) : base(game)
        {
            _content = new ContentManager(game.Content.ServiceProvider, game.Content.RootDirectory);
        }
        /// <summary>
        /// Creates a new Audio Manager. Add this to the Components collection of your Game.
        /// </summary>
        /// <param name="game">The Game</param>
        /// <param name="contentFolder">Root folder to load audio content from</param>
        public AudioManager(Game game, string contentFolder) : base(game)
        {
            _content = new ContentManager(game.Content.ServiceProvider, contentFolder);
        }
        /// <summary>
        /// Loads a Song into the AudioManager.
        /// </summary>
        /// <param name="songName">Name of the song to load</param>
        public void LoadSong(string songName)
        {
            LoadSong(songName, songName);
        }
        /// <summary>
        /// Loads a Song into the AudioManager.
        /// </summary>
        /// <param name="songName">Name of the song to load</param>
        /// <param name="songPath">Path to the song asset file</param>
        public void LoadSong(string songName, string songPath)
        {
            if (!string.IsNullOrEmpty(songName))
            {
                if (!_songs.ContainsKey(songName) && !songName.Equals(_currentSong))
                {
                    _songs.Add(songName, _content.Load<Song>(songPath));
                }
            }
        }
        /// <summary>
        /// Loads a SoundEffect into the AudioManager.
        /// </summary>
        /// <param name="soundName">Name of the sound to load</param>
        public void LoadSound(string soundName)
        {
            LoadSound(soundName, soundName);
        }
        /// <summary>
        /// Loads a SoundEffect into the AudioManager.
        /// </summary>
        /// <param name="soundName">Name of the sound to load</param>
        /// <param name="soundPath">Path to the song asset file</param>
        public void LoadSound(string soundName, string soundPath)
        {
            _sounds.Clear();
            //if (_sounds.ContainsKey(soundName))
            //{
            //    throw new InvalidOperationException(string.Format("Sound '{0}' has already been loaded", soundName));
            //}

            _sounds.Add(soundName, _content.Load<SoundEffect>(soundPath));
        }
        /// <summary>
        /// Unloads all loaded songs and sounds.
        /// </summary>
        public void UnloadContent()
        {
            _songs.Clear();
            _sounds.Clear();
            _isFading = _isMusicPaused = false;
            _currentSong = null;
            _content.Unload();
        }
        /// <summary>
        /// Starts playing the song with the given name. If it is already playing, this method
        /// does nothing. If another song is currently playing, it is stopped first.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        public void PlaySong(string songName)
        {
            PlaySong(songName, false);
        }
        /// <summary>
        /// Starts playing the song with the given name. If it is already playing, this method
        /// does nothing. If another song is currently playing, it is stopped first.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        /// <param name="loop">True if song should loop, false otherwise</param>
        public void PlaySong(string songName, bool loop)
        {
            if (CurrentSong != songName)
            {
                if (_currentSong != null || string.IsNullOrEmpty(songName))
                {
                    MediaPlayer.Stop();
                    CurrentSong = null;
                    return;
                }

                if (!string.IsNullOrEmpty(songName) && !_songs.TryGetValue(songName, out _currentSong))
                {
                    throw new ArgumentException(string.Format("Song '{0}' not found", songName));
                }

                CurrentSong = songName;

                _isMusicPaused = false;
                MediaPlayer.IsRepeating = loop;
                MediaPlayer.Play(_currentSong);

                if (!Enabled)
                {
                    MediaPlayer.Pause();
                }
            }
        }
        /// <summary>
        /// Pauses the currently playing song. This is a no-op if the song is already paused,
        /// or if no song is currently playing.
        /// </summary>
        public void PauseSong()
        {
            if (_currentSong != null && !_isMusicPaused)
            {
                if (Enabled) MediaPlayer.Pause();
                _isMusicPaused = true;
            }
        }
        /// <summary>
        /// Resumes the currently paused song. This is a no-op if the song is not paused,
        /// or if no song is currently playing.
        /// </summary>
        public void ResumeSong()
        {
            if (_currentSong != null && _isMusicPaused)
            {
                if (Enabled) MediaPlayer.Resume();
                _isMusicPaused = false;
            }
        }
        /// <summary>
        /// Stops the currently playing song. This is a no-op if no song is currently playing.
        /// </summary>
        public void StopSong()
        {
            if (_currentSong != null && MediaPlayer.State != MediaState.Stopped)
            {
                MediaPlayer.Stop();
                _isMusicPaused = false;
            }
        }
        /// <summary>
        /// Smoothly transition between two volumes.
        /// </summary>
        /// <param name="targetVolume">Target volume, 0.0f to 1.0f</param>
        /// <param name="duration">Length of volume transition</param>
        public void FadeSong(float targetVolume, TimeSpan duration)
        {
            if (duration <= TimeSpan.Zero)
            {
                throw new ArgumentException("Duration must be a positive value");
            }

            _fadeEffect = new MusicFadeEffect(MediaPlayer.Volume, targetVolume, duration);
            _isFading = true;
        }
        /// <summary>
        /// Stop the current fade.
        /// </summary>
        /// <param name="option">Options for setting the music volume</param>
        public void CancelFade(FadeCancelOptions option)
        {
            if (_isFading)
            {
                switch (option)
                {
                    case FadeCancelOptions.Source: MediaPlayer.Volume = _fadeEffect.SourceVolume; break;
                    case FadeCancelOptions.Target: MediaPlayer.Volume = _fadeEffect.TargetVolume; break;
                }

                _isFading = false;
            }
        }
        /// <summary>
        /// Plays the sound of the given name.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        public void PlaySound(string soundName)
        {
            PlaySound(soundName, 1.0f, 0.0f, 0.0f);
        }
        /// <summary>
        /// Plays the sound of the given name at the given volume.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        public void PlaySound(string soundName, float volume)
        {
            PlaySound(soundName, volume, 0.0f, 0.0f);
        }
        /// <summary>
        /// Plays the sound of the given name with the given parameters.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        /// <param name="pitch">Pitch, -1.0f (down one octave) to 1.0f (up one octave)</param>
        /// <param name="pan">Pan, -1.0f (full left) to 1.0f (full right)</param>
        public void PlaySound(string soundName, float volume, float pitch, float pan)
        {
            SoundEffect sound;

            if (!_sounds.TryGetValue(soundName, out sound))
            {
                throw new ArgumentException(string.Format("Sound '{0}' not found", soundName));
            }

            int index = GetAvailableSoundIndex();

            if (index != -1)
            {
                _playingSounds[index] = sound.CreateInstance();
                _playingSounds[index].Volume = volume;
                _playingSounds[index].Pitch = pitch;
                _playingSounds[index].Pan = pan;
                _playingSounds[index].Play();

                if (!Enabled)
                {
                    _playingSounds[index].Pause();
                }
            }
        }
        /// <summary>
        /// Stops all currently playing sounds.
        /// </summary>
        public void StopAllSounds()
        {
            for (int i = 0; i < _playingSounds.Length; ++i)
            {
                if (_playingSounds[i] != null)
                {
                    _playingSounds[i].Stop();
                    _playingSounds[i].Dispose();
                    _playingSounds[i] = null;
                }
            }
        }
        /// <summary>
        /// Called per loop unless Enabled is set to false.
        /// </summary>
        /// <param name="gameTime">Time elapsed since last frame</param>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < _playingSounds.Length; ++i)
            {
                if (_playingSounds[i] != null && _playingSounds[i].State == SoundState.Stopped)
                {
                    _playingSounds[i].Dispose();
                    _playingSounds[i] = null;
                }
            }

            if (_currentSong != null && MediaPlayer.State == MediaState.Stopped)
            {
                _currentSong = null;
                CurrentSong = null;
                _isMusicPaused = false;
            }

            if (_isFading && !_isMusicPaused)
            {
                if (_currentSong != null && MediaPlayer.State == MediaState.Playing)
                {
                    if (_fadeEffect.Update(gameTime.ElapsedGameTime))
                    {
                        _isFading = false;
                    }

                    MediaPlayer.Volume = _fadeEffect.GetVolume();
                }
                else
                {
                    _isFading = false;
                }
            }

            base.Update(gameTime);
        }
        // Pauses all music and sound if disabled, resumes if enabled.
        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (Enabled)
            {
                for (int i = 0; i < _playingSounds.Length; ++i)
                {
                    if (_playingSounds[i] != null && _playingSounds[i].State == SoundState.Paused)
                    {
                        _playingSounds[i].Resume();
                    }
                }

                if (!_isMusicPaused)
                {
                    MediaPlayer.Resume();
                }
            }
            else
            {
                for (int i = 0; i < _playingSounds.Length; ++i)
                {
                    if (_playingSounds[i] != null && _playingSounds[i].State == SoundState.Playing)
                    {
                        _playingSounds[i].Pause();
                    }
                }

                MediaPlayer.Pause();
            }

            base.OnEnabledChanged(sender, args);
        }
        // Acquires an open sound slot.
        private int GetAvailableSoundIndex()
        {
            for (int i = 0; i < _playingSounds.Length; ++i)
            {
                if (_playingSounds[i] == null)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}