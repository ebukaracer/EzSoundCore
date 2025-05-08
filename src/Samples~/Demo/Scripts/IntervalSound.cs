using System.Collections;
using Racer.EzSoundCore.Core;
using Racer.EzSoundCore.Utilities;
using UnityEngine;

namespace Racer.EzSoundCore.Samples
{
    internal class IntervalSound : MonoBehaviour
    {
        private WaitForSeconds _delay;

        private void Start()
        {
            _delay = new WaitForSeconds(2.5f);
            StartCoroutine(PlayIntervalSound());
        }

        private IEnumerator PlayIntervalSound()
        {
            while (true)
            {
                yield return _delay;
                
                // Playing using a predefined ClipId
                SoundCore.Instance.PlaySfx(ClipId._131660__bertrof__game_sound_correct);
            }
        }
    }
}