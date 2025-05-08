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
After installation, use the menu option:  `Racer > EzSoundCore > Import Elements` to will import the prebuilt elements (prefabs) included in the package to help you get started quickly and streamline your workflow.

## Quick Usage
After you have imported this package's **Elements**, locate **SoundCore** prefab and add it to your desired scene. 

```csharp
using Racer.EzSoundCore.Core;
using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    [SerializeField] private AudioClip sfxClip;

    private void Start()
    {
        // Play sound via an assigned clip in the inspector
        SoundCore.Instance.PlaySfx(sfxClip);

	// Alternatively, play sound via a generated enum ID from the clip 
	SoundCore.Instance.PlaySfx(ClipID.mysfxclip);

        // Play music, the music should have been assigned in the audio source's clip field
        SoundCore.Instance.PlayMusic();

        // Mute all audio sources
        SoundCore.Instance.MuteAllSources(true);

        // Unmute all audio sources
        SoundCore.Instance.MuteAllSources(false);
    }
}
```

## Samples and Best Practices
- In the case of any updates to newer versions, use the menu option: `Racer > EzSoundCore > Import Elements(Force)` 
- Optionally import this package's demo from the package manager's `Samples` tab.
- To remove this package completely(leaving no trace), navigate to: `Racer > EzSoundCore > Remove package`

## [Contributing](https://ebukaracer.github.io/ebukaracer/md/CONTRIBUTING.html) 
Contributions are welcome! Please open an issue or submit a pull request.