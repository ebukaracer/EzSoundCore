using Racer.EzSoundCore.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Racer.EzSoundCore.Samples
{
    internal class SoundToggle : MonoBehaviour
    {
        [SerializeField] private Text toggleText;

        
        public void Toggle()
        {
            if (toggleText.text == "Sound: On")
            {
                toggleText.text = "Sound: Off";
                SoundCore.Instance.MuteAllSources(true);
            }
            else
            {
                toggleText.text = "Sound: On";
                SoundCore.Instance.MuteAllSources(false);
            }
        }
    }
}