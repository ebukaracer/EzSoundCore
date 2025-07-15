# EzSoundCore
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-blue)](http://makeapullrequest.com) [![License: MIT](https://img.shields.io/badge/License-MIT-blue)](https://ebukaracer.github.io/ebukaracer/md/LICENSE.html)

**EzSoundCore** is a Unity package that provides a simple and efficient way to manage sound effects and music playback in your game. 

 [View in DocFx](https://ebukaracer.github.io/EzSoundCore)
 
## Features
- Easy-to-use methods for playing sounds  
- Provides a quick and easy way to manage multiple audio sources
- Support for audio mixer snapshots
- Flexible utility for generating unique ID from audio-clips

## Installation
_Inside the Unity Editor using the Package Manager:_
- Click the **(+)** button in the Package Manager and select **"Add package from Git URL"** (requires Unity 2019.4 or later).
-  Paste the Git URL of this package into the input box: https://github.com/ebukaracer/EzSoundCore.git#upm
-  Click **Add** to install the package.
-  If your project uses **Assembly Definitions**, make sure to add a reference to this package under **Assembly Definition References**. 
    - For more help, see [this guide](https://ebukaracer.github.io/ebukaracer/md/SETUPGUIDE.html).

## Setup
After installation, use the menu option:  `Racer > EzSoundCore > Import Elements`\
This will import the prebuilt elements (prefabs) included in the package to help you get started quickly and streamline your workflow.

## Quick Usage
- Ensure the `SoundCore` prefab or a GameObject containing the `SoundCore` component is present in the scene.
- Use `SoundCore.Instance` to access the singleton instance of the class.
- The `ClipID` enum is auto-generated. If you intend to play sound using that approach, first import the elements of this package, use the `SoundCore` prefab to generate it after adding audio clips to the list.

```csharp
using UnityEngine;
using Racer.EzSoundCore.Core;

public class ExampleUsage : MonoBehaviour
{
    [SerializeField] private AudioClip sfxClip;
    [SerializeField] private AudioClip musicClip;

    private void Start()
    {
        // Play a sound effect
        SoundCore.Instance.PlaySfx(sfxClip, volumeScale: 0.8f);

        // Play music
        SoundCore.Instance.PlayMusic(musicClip);

        // Mute all audio sources
        SoundCore.Instance.MuteAllSources(mute: true, ignoreIndex: true);

	// Play a sound effect using ClipID(if generated)  
	SoundCore.Instance.PlaySfx(ClipID.Explosion, volumeScale: 0.8f);  
  
	// Play music using ClipID (if generated) 
	SoundCore.Instance.PlayMusic(ClipID.BackgroundMusic);

        // Enable all audio sources
        SoundCore.Instance.EnableAllSources(enable: true, ignoreIndex: true);

        // Transition to a specific audio mixer snapshot
        SoundCore.Instance.MuteAudioMixer(snapshotIndex: 0, timeToReach: 0.5f);
    }
}
```

## Samples and Best Practices
- In the case of any updates to newer versions, use the menu option: `Racer > EzSoundCore > Import Elements(Force)` 
- Optionally import this package's demo from the package manager's `Samples` tab.
- To remove this package completely(leaving no trace), navigate to: `Racer > EzSoundCore > Remove package`

## [Contributing](https://ebukaracer.github.io/ebukaracer/md/CONTRIBUTING.html) 
Contributions are welcome! Please open an issue or submit a pull request.