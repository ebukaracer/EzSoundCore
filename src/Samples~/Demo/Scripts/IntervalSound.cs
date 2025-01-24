using System.Collections;
using Racer.EzSoundCore.Core;
using UnityEngine;

namespace Racer.EzSoundCore.Samples
{
    internal class IntervalSound : MonoBehaviour
    {
        private WaitForSeconds _delay;

        [SerializeField] private AudioClip clip;

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
                SoundCore.Instance.PlaySfx(clip);
            }
        }
    }
}