using Racer.EzSoundCore.Core;
using UnityEngine;

namespace Racer.EzSoundCore.Utilities
{
    /// <summary>
    /// SfxPlayer is a component that plays a sound effect using the SoundCore singleton.
    /// </summary>
    public class SfxPlayer : MonoBehaviour
    {
        /// <summary>
        /// The audio clip to be played.
        /// </summary>
        [SerializeField] private AudioClip clip;

        
        /// <summary>
        /// Plays the assigned audio clip using the SoundCore instance.
        /// </summary>
        public void Play()
        {
            SoundCore.Instance.PlaySfx(clip);
        }
    }
}