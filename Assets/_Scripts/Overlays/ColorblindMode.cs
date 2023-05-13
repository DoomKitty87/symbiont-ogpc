using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorblindMode : MonoBehaviour
{

  private int _lastMode = 0;

  private void Update() {
    UpdateColorBlindMode();
  }

  private void UpdateColorBlindMode() {
    if (!PlayerPrefs.HasKey("COLORBLIND_MODE")) return;
    if (_lastMode == PlayerPrefs.GetInt("COLORBLIND_MODE")) return;
    VolumeProfile volumeProfile = GameObject.FindGameObjectWithTag("Post Processing").GetComponent<Volume>().profile;
    ChannelMixer channelMixer;
    volumeProfile.TryGet(out channelMixer);
    switch(PlayerPrefs.GetInt("COLORBLIND_MODE")) {
      case 0:
        channelMixer.redOutRedIn.overrideState = false;
        channelMixer.greenOutGreenIn.overrideState = false;
        channelMixer.blueOutBlueIn.overrideState = false;
        break;
      case 1:
        //Red-Green (Protanopia)
        channelMixer.redOutRedIn.overrideState = true;
        channelMixer.greenOutGreenIn.overrideState = true;
        channelMixer.blueOutBlueIn.overrideState = false;
        channelMixer.redOutRedIn.value = 150;
        channelMixer.greenOutGreenIn.value = 50;
        break;
      case 2:
        //Blue-Yellow (Tritanopia)
        channelMixer.redOutRedIn.overrideState = true;
        channelMixer.greenOutGreenIn.overrideState = true;
        channelMixer.blueOutBlueIn.overrideState = true;
        channelMixer.blueOutBlueIn.value = 150;
        channelMixer.redOutRedIn.value = 75;
        channelMixer.greenOutGreenIn.value = 75;
        break;
    }
    _lastMode = PlayerPrefs.GetInt("COLORBLIND_MODE");
  }
}