using Racer.EzSoundCore.Utilities;
using UnityEngine;
using UnityEngine.Audio;

namespace Racer.EzSoundCore.Core
{
    /// <summary>
    /// Singleton-persistent class that manages sound effects and music playback in the game.
    /// Two audio-sources are used for sound effects and music.
    /// <remarks>
    /// Ensure <see cref="SoundCore"/> prefab or a gameobject containing this class is present in the scene.
    /// </remarks>
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class SoundCore : SingletonPattern.SingletonPersistent<SoundCore>
    {
        [SerializeField,
         Tooltip("Assign an audio-source component that will be used for playing sound effects. " +
                 "Leave the audio-clip's field empty as the audio-source will be used for playing one-shot audio-clips")]
        private AudioSource sfxSource;

        [SerializeField,
         Tooltip("Assign an audio-source component that will be used for playing music. " +
                 "Assign a background music to the audio-clip's field or leave empty in case you intend to load it dynamically")]
        private AudioSource musicSource;

        [Space(10)]
        [SerializeField,
         Tooltip("Handy if snapshots are being used in your project, assign them here.")]
        private AudioMixerSnapshot[] snapshots;


        /// <summary>
        /// Plays a sound effect.
        /// </summary>
        /// <param name="clip">The audio-clip to play.</param>
        /// <param name="volumeScale">The scale of the volume (default is 1).</param>
        public void PlaySfx(AudioClip clip, float volumeScale = 1)
        {
            if (sfxSource.enabled)
                sfxSource.PlayOneShot(clip, volumeScale);
        }

        /// <summary>
        /// Plays the music assigned to the music's audio-source.
        /// </summary>
        public void PlayMusic()
        {
            if (musicSource.enabled)
                musicSource.Play();
        }

        /// <summary>
        /// Gets the music's audio-source.
        /// </summary>
        /// <returns>The music's audio-source.</returns>
        public AudioSource GetMusicSrc() => musicSource;

        /// <summary>
        /// Gets the sound effects' audio-source.
        /// </summary>
        /// <returns>The sound effects' audio-source.</returns>
        public AudioSource GetSfxSrc() => sfxSource;

        /// <summary>
        /// Mutes or unmutes the sound effects' audio-source.
        /// </summary>
        /// <param name="mute">True to mute, false to unmute.</param>
        public void MuteSfxSrc(bool mute) => sfxSource.mute = mute;

        /// <summary>
        /// Mutes or unmutes the music's audio-source.
        /// </summary>
        /// <param name="mute">True to mute, false to unmute.</param>
        public void MuteMusicSrc(bool mute) => musicSource.mute = mute;

        /// <summary>
        /// Mutes or unmutes all audio-sources.
        /// </summary>
        public void MuteAllSources(bool mute)
        {
            MuteSfxSrc(mute);
            MuteMusicSrc(mute);
        }

        /// <summary>
        /// Enables or disables all audio-sources.
        /// </summary>
        public void EnableAllSources(bool isEnabled)
        {
            EnableMusicSrc(isEnabled);
            EnableSfxSrc(isEnabled);
        }

        /// <summary>
        /// Enables or disables the music's audio-source.
        /// </summary>
        /// <param name="isEnabled">True to enable, false to disable.</param>
        public void EnableMusicSrc(bool isEnabled) => musicSource.enabled = isEnabled;

        /// <summary>
        /// Enables or disables the sound effects' audio-source.
        /// </summary>
        /// <param name="isEnabled">True to enable, false to disable.</param>
        public void EnableSfxSrc(bool isEnabled) => sfxSource.enabled = isEnabled;

        /// <summary>
        /// Mutes the audio-mixer by transitioning to a specified snapshot.
        /// </summary>
        /// <param name="snapShotIndex">The index of the snapshot to transition to.</param>
        /// <param name="timeToReach">The time to reach the snapshot (default is 0.1f).</param>
        public void MuteAudioMixer(int snapShotIndex, float timeToReach = .1f)
        {
            GetSnapShot(snapShotIndex).TransitionTo(timeToReach);
        }

        /// <summary>
        /// Unmutes the audio-mixer by transitioning to a specified snapshot.
        /// </summary>
        /// <param name="snapShotIndex">The index of the snapshot to transition to.</param>
        /// <param name="timeToReach">The time to reach the snapshot (default is 0.1f).</param>
        public void UnMuteAudioMixer(int snapShotIndex, float timeToReach = .1f)
        {
            GetSnapShot(snapShotIndex).TransitionTo(timeToReach);
        }

        /// <summary>
        /// Gets the audio-mixer's snapshot at the specified index.
        /// </summary>
        /// <param name="snapShotIndex">The index of the snapshot.</param>
        /// <returns>The audio-mixer snapshot.</returns>
        public AudioMixerSnapshot GetSnapShot(int snapShotIndex)
        {
            return snapshots[snapShotIndex];
        }
    }
}