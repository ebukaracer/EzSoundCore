# EzSoundCore
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-blue)](http://makeapullrequest.com) [![License: MIT](https://img.shields.io/badge/License-MIT-blue)](https://ebukaracer.github.io/ebukaracer/md/LICENSE.html)

**EzSoundCore** is a Unity package that provides a simple and efficient way to manage sound effects and music playback in your game. 

 [View in DocFx](https://ebukaracer.github.io/EzSoundCore)
 
## Features  
- Easy-to-use methods for playing, muting, and enabling/disabling audio sources.  
- Support for audio mixer snapshots.  
- Editor utilities for importing and removing package elements.

## Installation
 *In unity editor inside package manager:*
- Hit `(+)`, choose `Add package from Git URL`(Unity 2019.4+)
- Paste the `URL` for this package inside the box: https://github.com/ebukaracer/EzSoundCore.git#upm
- Hit `Add`
- If you're using assembly definition in your project, be sure to add this package's reference under: `Assembly Definition References` or check out [this](https://ebukaracer.github.io/ebukaracer/md/SETUPGUIDE.html)

## Quick Usage
```csharp
using Racer.EzSoundCore.Core;
using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    [SerializeField] private AudioClip sfxClip;
    [SerializeField] private AudioClip musicClip;

    private void Start()
    {
        // Play sound effect
        SoundCore.Instance.PlaySfx(sfxClip);

        // Play music
        SoundCore.Instance.PlayMusic();

        // Mute all audio sources
        SoundCore.Instance.MuteAllSources(true);

        // Unmute all audio sources
        SoundCore.Instance.MuteAllSources(false);
    }
}
```

## Samples and Best Practices
After installation, use the menu option: `Racer > EzSoundCore > Import Elements` to import the prebuilt elements(prefabs) of this package, which will speed up your workflow. In the case of any updates to newer versions, use the menu option: `Racer > EzSoundCore > Import Elements(Force)`. 

Optionally import this package's demo from the package manager's `Samples` tab.

To remove this package completely(leaving no trace), navigate to: `Racer > EzSoundCore > Remove package`

## [Contributing](https://ebukaracer.github.io/ebukaracer/md/CONTRIBUTING.html) 
Contributions are welcome! Please open an issue or submit a pull request.