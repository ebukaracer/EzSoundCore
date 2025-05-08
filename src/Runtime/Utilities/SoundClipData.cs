using System;
using UnityEngine;

namespace Racer.EzSoundCore.Utilities
{
    [Serializable]
    public class SoundClipData
    {
        public AudioClip clip;
        public string id;

        public SoundClipData(string id, AudioClip clip)
        {
            this.id = id;
            this.clip = clip;
        }
    }
}