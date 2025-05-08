using System;
using System.Collections.Generic;
using System.Linq;
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
    [DefaultExecutionOrder(-100), DisallowMultipleComponent]
    public class SoundCore : SingletonPattern.SingletonPersistent<SoundCore>
    {
        private Dictionary<string, SoundClipData> _clipIdLookup;

        [Header("SECTION 1")]
        [SerializeField,
         Tooltip("Assign an audio-source components for playing sound effects. " +
                 "Leave the audio-clip's field empty as the audio-source will be used for playing one-shot audio-clips")]
        private AudioSource[] sfxSources;

        [SerializeField,
         Tooltip("Assign an audio-source components for playing music. " +
                 "You may also assign a background music to the audio-clip's field or leave empty in case you intend to load it dynamically")]
        private AudioSource[] musicSources;

        [SerializeField,
         Tooltip("Handy if snapshots are being used in your project, assign them here.")]
        private AudioMixerSnapshot[] snapshots;

        [field: Space(5), Header("SECTION 2")]
        [field: SerializeField,
                Tooltip("Assign your audio-clips here")]
        public List<AudioClip> AllAudioClips { get; internal set; } = new();

        [field: SerializeField,
                Tooltip("Stores the mapping of audio-clip IDs to their respective sound clip data.")]
        public List<SoundClipData> MappedClipsData { get; private set; } = new();

        // Section 3
        [SerializeField, HideInInspector] private string audioClipsPath;


        protected override void Awake()
        {
            base.Awake();
            _clipIdLookup = MappedClipsData.ToDictionary(data => data.id, data => data);
        }

        /// <summary>
        /// Plays a sound effect using the specified audio clip and volume scale.
        /// </summary>
        /// <param name="clip">The audio-clip to play.</param>
        /// <param name="volumeScale">The scale of the volume (default is 1).</param>
        /// <param name="audioSrcIndex">The index of the sound effects audio-source to use.</param>
        public void PlaySfx(AudioClip clip, float volumeScale = 1, int audioSrcIndex = 0)
        {
            var sfxSource = sfxSources[audioSrcIndex];

            if (sfxSource.enabled)
                sfxSource.PlayOneShot(clip, volumeScale);
        }

        /// <summary>
        /// Plays a sound effect associated with the specified clip ID using the designated audio source.
        /// </summary>
        /// <param name="clipId">The ID of the clip to play.</param>
        /// <param name="volumeScale">The scale of the volume (default is 1).</param>
        /// <param name="audioSrcIndex">The index of the sound effects audio-source to use (default is 0).</param>
        public void PlaySfx(Enum clipId, float volumeScale = 1, int audioSrcIndex = 0)
        {
            var sfxSource = sfxSources[audioSrcIndex];

            if (!sfxSource.enabled) return;

            if (_clipIdLookup.TryGetValue(clipId.ToString(), out var soundClipData) && soundClipData.clip)
                sfxSource.PlayOneShot(soundClipData.clip, volumeScale);
            else
                Debug.LogError($"{nameof(SoundClipData)} for {clipId} not assigned/initialized properly.");
        }

        /// <summary>
        /// Plays the music assigned to the specified music audio-source.
        /// </summary>
        /// <param name="audioSrcIndex">The index of the music audio-source to use (default is 0).</param>
        public void PlayMusic(int audioSrcIndex = 0)
        {
            var musicSource = musicSources[audioSrcIndex];

            if (musicSource.enabled && !musicSource.isPlaying)
                musicSource.Play();
        }

        /// <summary>
        /// Plays the music associated with the specified clip ID using the designated audio source.
        /// </summary>
        /// <param name="clipId">The ID of the clip to play.</param>
        /// <param name="audioSrcIndex">The index of the music audio-source to use (default is 0).</param>
        public void PlayMusic(Enum clipId, int audioSrcIndex = 0)
        {
            var musicSource = musicSources[audioSrcIndex];

            if (!musicSource.enabled) return;

            if (_clipIdLookup.TryGetValue(clipId.ToString(), out var soundClipData) && soundClipData.clip)
            {
                musicSource.clip = soundClipData.clip;
                musicSource.Play();
            }
            else
                Debug.LogError($"{nameof(SoundClipData)} for {clipId} not assigned/initialized properly.");
        }

        /// <summary>
        /// Gets the music audio-source at the specified index.
        /// </summary>
        /// <param name="audioSrcIndex">The index of the music audio-source to retrieve (default is 0).</param>
        /// <returns>The music audio-source.</returns>
        public AudioSource GetMusicSrc(int audioSrcIndex = 0) => musicSources[audioSrcIndex];

        /// <summary>
        /// Gets the sound effects audio-source at the specified index.
        /// </summary>
        /// <param name="audioSrcIndex">The index of the sound effects audio-source to retrieve (default is 0).</param>
        /// <returns>The sound effects audio-source.</returns>
        public AudioSource GetSfxSrc(int audioSrcIndex = 0) => sfxSources[audioSrcIndex];

        /// <summary>
        /// Mutes or unmutes the sound effects audio-source at the specified index.
        /// </summary>
        /// <param name="mute">True to mute, false to unmute.</param>
        /// <param name="audioSrcIndex">The index of the sound effects audio-source to mute/unmute (default is 0).</param>
        public void MuteSfxSrc(bool mute, int audioSrcIndex = 0) => sfxSources[audioSrcIndex].mute = mute;

        /// <summary>
        /// Mutes or unmutes the music audio-source at the specified index.
        /// </summary>
        /// <param name="mute">True to mute, false to unmute.</param>
        /// <param name="audioSrcIndex">The index of the music audio-source to mute/unmute (default is 0).</param>
        public void MuteMusicSrc(bool mute, int audioSrcIndex = 0) => musicSources[audioSrcIndex].mute = mute;

        /// <summary>
        /// Mutes or unmutes all audio-sources (both sound effects and music) at the specified index.
        /// </summary>
        /// <param name="mute">True to mute all sources, false to unmute.</param>
        /// <param name="index">The index of the music & sound effects audio-sources to mute/unmute (default is 0).</param>
        /// <param name="ignoreIndex">If true, all audio-sources will be muted/un-muted regardless of the index.</param>
        public void MuteAllSources(bool mute, int index = 0, bool ignoreIndex = false)
        {
            if (ignoreIndex)
            {
                foreach (var sfxSource in sfxSources)
                    sfxSource.mute = mute;

                foreach (var musicSource in musicSources)
                    musicSource.mute = mute;

                return;
            }

            MuteSfxSrc(mute, index);
            MuteMusicSrc(mute, index);
        }

        /// <summary>
        /// Enables or disables all audio-sources (both sound effects and music) at the specified index.
        /// </summary>
        /// <param name="isEnabled">True to enable all sources, false to disable.</param>
        /// <param name="index">The index of the music & sound effects audio-sources to enable/disable (default is 0).</param>
        /// <param name="ignoreIndex">If true, all audio-sources will be enabled/disabled regardless of the index.</param>
        public void EnableAllSources(bool isEnabled, int index = 0, bool ignoreIndex = false)
        {
            if (ignoreIndex)
            {
                foreach (var sfxSource in sfxSources)
                    sfxSource.enabled = isEnabled;

                foreach (var musicSource in musicSources)
                    musicSource.enabled = isEnabled;

                return;
            }

            EnableSfxSrc(isEnabled, index);
            EnableMusicSrc(isEnabled, index);
        }

        /// <summary>
        /// Enables or disables the music audio-source at the specified index.
        /// </summary>
        /// <param name="isEnabled">True to enable, false to disable.</param>
        /// <param name="audioSrcIndex">The index of the music audio-source to enable/disable (default is 0).</param>
        public void EnableMusicSrc(bool isEnabled, int audioSrcIndex = 0) =>
            musicSources[audioSrcIndex].enabled = isEnabled;

        /// <summary>
        /// Enables or disables the sound effects audio-source at the specified index.
        /// </summary>
        /// <param name="isEnabled">True to enable, false to disable.</param>
        /// <param name="audioSrcIndex">The index of the sound effects audio-source to enable/disable (default is 0).</param>
        public void EnableSfxSrc(bool isEnabled, int audioSrcIndex = 0) =>
            sfxSources[audioSrcIndex].enabled = isEnabled;

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
        /// Gets the audio-mixer snapshot at the specified index.
        /// </summary>
        /// <param name="snapShotIndex">The index of the snapshot to retrieve.</param>
        /// <returns>The audio-mixer snapshot.</returns>
        public AudioMixerSnapshot GetSnapShot(int snapShotIndex)
        {
            return snapshots[snapShotIndex];
        }
    }
}