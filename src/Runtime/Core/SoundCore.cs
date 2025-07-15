using System.Collections.Generic;
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
        // @formatter:off
        [Header("SECTION 1")]
        [SerializeField] private AudioSource[] sfxSources;
        [SerializeField] private AudioSource[] musicSources;
        [SerializeField] private AudioMixerSnapshot[] snapshots;
        // @formatter:on

        [field: Header("SECTION 2")]
        [field: SerializeField]
        public List<AudioClip> AllAudioClips { get; internal set; } = new();

        [SerializeField, HideInInspector] private string audioClipsPath;


        // --- SFX ---
        /// <summary>
        /// Plays a sound effect using the specified audio clip.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        /// <param name="volumeScale">The volume scale to apply to the audio clip.</param>
        /// <param name="audioSrcIndex">The index of the audio source to use.</param>
        public void PlaySfx(AudioClip clip, float volumeScale = 1f, int audioSrcIndex = 0)
        {
            var source = sfxSources[audioSrcIndex];

            if (source.enabled && clip)
                source.PlayOneShot(clip, volumeScale);
        }

        /// <summary>
        /// Plays a sound effect using the specified clip index from the list of all audio clips.
        /// </summary>
        /// <param name="clipIndex">The index of the audio clip to play.</param>
        /// <param name="volumeScale">The volume scale to apply to the audio clip.</param>
        /// <param name="audioSrcIndex">The index of the audio source to use.</param>
        public void PlaySfx(int clipIndex, float volumeScale = 1f, int audioSrcIndex = 0)
        {
            var source = sfxSources[audioSrcIndex];

            if (!source.enabled || !IsValidIndex(clipIndex)) return;

            var clip = AllAudioClips[clipIndex];
            if (clip)
                source.PlayOneShot(clip, volumeScale);
        }

        // --- Music ---
        /// <summary>
        /// Plays music using the specified audio clip.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        /// <param name="audioSrcIndex">The index of the audio source to use.</param>
        public void PlayMusic(AudioClip clip, int audioSrcIndex = 0)
        {
            var source = musicSources[audioSrcIndex];

            if (!source.enabled || !clip) return;

            source.clip = clip;
            source.Play();
        }

        /// <summary>
        /// Plays music using the specified clip index from the list of all audio clips.
        /// </summary>
        /// <param name="clipIndex">The index of the audio clip to play.</param>
        /// <param name="audioSrcIndex">The index of the audio source to use.</param>
        public void PlayMusic(int clipIndex, int audioSrcIndex = 0)
        {
            var source = musicSources[audioSrcIndex];
            if (!source.enabled || !IsValidIndex(clipIndex)) return;

            var clip = AllAudioClips[clipIndex];

            if (!clip) return;

            source.clip = clip;
            source.Play();
        }

        // --- Utility ---
        /// <summary>
        /// Gets the audio source for sound effects.
        /// </summary>
        /// <param name="index">The index of the audio source to get.</param>
        /// <returns>The audio source for sound effects.</returns>
        public AudioSource GetSfxSrc(int index = 0) => sfxSources[index];

        /// <summary>
        /// Gets the audio source for music.
        /// </summary>
        /// <param name="index">The index of the audio source to get.</param>
        /// <returns>The audio source for music.</returns>
        public AudioSource GetMusicSrc(int index = 0) => musicSources[index];

        /// <summary>
        /// Mutes the sound effects audio source.
        /// </summary>
        /// <param name="mute">Whether to mute the audio source.</param>
        /// <param name="index">The index of the audio source to mute.</param>
        public void MuteSfxSrc(bool mute, int index = 0) => sfxSources[index].mute = mute;

        /// <summary>
        /// Mutes the music audio source.
        /// </summary>
        /// <param name="mute">Whether to mute the audio source.</param>
        /// <param name="index">The index of the audio source to mute.</param>
        public void MuteMusicSrc(bool mute, int index = 0) => musicSources[index].mute = mute;

        /// <summary>
        /// Enables the sound effects audio source.
        /// </summary>
        /// <param name="enable">Whether to enable the audio source.</param>
        /// <param name="index">The index of the audio source to enable.</param>
        public void EnableSfxSrc(bool enable, int index = 0) => sfxSources[index].enabled = enable;

        /// <summary>
        /// Enables the music audio source.
        /// </summary>
        /// <param name="enable">Whether to enable the audio source.</param>
        /// <param name="index">The index of the audio source to enable.</param>
        public void EnableMusicSrc(bool enable, int index = 0) => musicSources[index].enabled = enable;

        /// <summary>
        /// Mutes all audio sources.
        /// </summary>
        /// <param name="mute">Whether to mute the audio sources.</param>
        /// <param name="index">The index of the audio source to mute.</param>
        /// <param name="ignoreIndex">Whether to ignore the index and mute all sources.</param>
        public void MuteAllSources(bool mute, int index = 0, bool ignoreIndex = false)
        {
            if (ignoreIndex)
            {
                foreach (var s in sfxSources) s.mute = mute;
                foreach (var m in musicSources) m.mute = mute;
            }
            else
            {
                MuteSfxSrc(mute, index);
                MuteMusicSrc(mute, index);
            }
        }

        /// <summary>
        /// Enables all audio sources.
        /// </summary>
        /// <param name="enable">Whether to enable the audio sources.</param>
        /// <param name="index">The index of the audio source to enable.</param>
        /// <param name="ignoreIndex">Whether to ignore the index and enable all sources.</param>
        public void EnableAllSources(bool enable, int index = 0, bool ignoreIndex = false)
        {
            if (ignoreIndex)
            {
                foreach (var s in sfxSources) s.enabled = enable;
                foreach (var m in musicSources) m.enabled = enable;
            }
            else
            {
                EnableSfxSrc(enable, index);
                EnableMusicSrc(enable, index);
            }
        }

        // --- Mixer ---
        /// <summary>
        /// Mutes the audio mixer using the specified snapshot index.
        /// </summary>
        /// <param name="snapshotIndex">The index of the snapshot to use.</param>
        /// <param name="timeToReach">The time to reach the snapshot.</param>
        public void MuteAudioMixer(int snapshotIndex, float timeToReach = 0.1f) =>
            GetSnapShot(snapshotIndex)?.TransitionTo(timeToReach);

        /// <summary>
        /// Unmutes the audio mixer using the specified snapshot index.
        /// </summary>
        /// <param name="snapshotIndex">The index of the snapshot to use.</param>
        /// <param name="timeToReach">The time to reach the snapshot.</param>
        public void UnMuteAudioMixer(int snapshotIndex, float timeToReach = 0.1f) =>
            GetSnapShot(snapshotIndex)?.TransitionTo(timeToReach);

        /// <summary>
        /// Gets the audio mixer snapshot at the specified index.
        /// </summary>
        /// <param name="index">The index of the snapshot to get.</param>
        /// <returns>The audio mixer snapshot.</returns>
        public AudioMixerSnapshot GetSnapShot(int index)
        {
            if (index >= 0 && index < snapshots.Length)
                return snapshots[index];

            Debug.LogWarning($"Snapshot index {index} is out of bounds.");
            return null;
        }

        // --- Helper ---
        private bool IsValidIndex(int index) =>
            index >= 0 && index < AllAudioClips.Count;
    }
}