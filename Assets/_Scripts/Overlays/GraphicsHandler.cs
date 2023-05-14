using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GraphicsHandler : MonoBehaviour
{

  private int _lastAA, _lastAniso, _lastShadows, _lastVsync, _lastRefreshRate, _lastFullscreen;
  private float _lastBrightness;

  private void Start() {
    if (PlayerPrefs.HasKey("GRAPHICS_MSAA_SCALE")) {
      QualitySettings.antiAliasing = PlayerPrefs.GetInt("GRAPHICS_MSAA_SCALE"); //0-Off, 1-2x, 2-4x, 3-8x
      _lastAA = PlayerPrefs.GetInt("GRAPHICS_MSAA_SCALE");
    }

    if (PlayerPrefs.HasKey("GRAPHICS_ANISOTROPIC")) {
      if (PlayerPrefs.GetInt("GRAPHICS_ANISOTROPIC") == 1) QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
      else QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
      _lastAniso = PlayerPrefs.GetInt("GRAPHICS_ANISOTROPIC");
    }

    if (PlayerPrefs.HasKey("GRAPHICS_SHADOWS_ENABLED")) {
      if (PlayerPrefs.GetInt("GRAPHICS_SHADOWS_ENABLED") == 1) QualitySettings.shadows = UnityEngine.ShadowQuality.All;
      else QualitySettings.shadows = UnityEngine.ShadowQuality.Disable;
      _lastShadows = PlayerPrefs.GetInt("GRAPHICS_SHADOWS_ENABLED");
    }

    if (PlayerPrefs.HasKey("GRAPHICS_VSYNC")) {
      if (PlayerPrefs.GetInt("GRAPHICS_VSYNC") == 1) QualitySettings.vSyncCount = 1;
      else QualitySettings.vSyncCount = 0;
      _lastVsync = PlayerPrefs.GetInt("GRAPHICS_VSYNC");
    }

    if (PlayerPrefs.HasKey("GRAPHICS_REFRESH_RATE")) {
      Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreenMode, PlayerPrefs.GetInt("GRAPHICS_REFRESH_RATE"));
      _lastRefreshRate = PlayerPrefs.GetInt("GRAPHICS_REFRESH_RATE");
    }

    if (PlayerPrefs.HasKey("GRAPHICS_FULLSCREEN")) {
      if (PlayerPrefs.GetInt("GRAPHICS_FULLSCREEN") == 1) Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.ExclusiveFullScreen, Screen.currentResolution.refreshRate);
      else Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.Windowed, Screen.currentResolution.refreshRate);
      _lastFullscreen = PlayerPrefs.GetInt("GRAPHICS_FULLSCREEN");
    }
    
    if (PlayerPrefs.HasKey("GRAPHICS_BRIGHTNESS")) {
      VolumeProfile volumeProfile = GameObject.FindGameObjectWithTag("Post Processing").GetComponent<Volume>().profile;
      LiftGammaGain lgg;
      volumeProfile.TryGet(out lgg);
      lgg.gamma.value = new Vector4(1, 1, 1, PlayerPrefs.GetFloat("GRAPHICS_BRIGHTNESS") - 1);
      _lastBrightness = PlayerPrefs.GetFloat("GRAPHICS_BRIGHTNESS");
    }
  }

  private void Update() {
    UpdateGraphics();
  }

  private void UpdateGraphics() {
    if (_lastAA != PlayerPrefs.GetInt("GRAPHICS_MSAA_SCALE")) {
      QualitySettings.antiAliasing = PlayerPrefs.GetInt("GRAPHICS_MSAA_SCALE");
      _lastAA = PlayerPrefs.GetInt("GRAPHICS_MSAA_SCALE");
    }

    if (_lastAniso != PlayerPrefs.GetInt("GRAPHICS_ANISOTROPIC")) {
      if (PlayerPrefs.GetInt("GRAPHICS_ANISOTROPIC") == 1) QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
      else QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
      _lastAniso = PlayerPrefs.GetInt("GRAPHICS_ANISOTROPIC");
    }

    if (_lastShadows != PlayerPrefs.GetInt("GRAPHICS_SHADOWS_ENABLED")) {
      if (PlayerPrefs.GetInt("GRAPHICS_SHADOWS_ENABLED") == 1) QualitySettings.shadows = UnityEngine.ShadowQuality.All;
      else QualitySettings.shadows = UnityEngine.ShadowQuality.Disable;
      _lastShadows = PlayerPrefs.GetInt("GRAPHICS_SHADOWS_ENABLED");
    }

    if (_lastVsync != PlayerPrefs.GetInt("GRAPHICS_VSYNC")) {
      if (PlayerPrefs.GetInt("GRAPHICS_VSYNC") == 1) QualitySettings.vSyncCount = 1;
      else QualitySettings.vSyncCount = 0;
      _lastVsync = PlayerPrefs.GetInt("GRAPHICS_VSYNC");
    }

    if (_lastRefreshRate != PlayerPrefs.GetInt("GRAPHICS_REFRESH_RATE")) {
      Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreenMode, PlayerPrefs.GetInt("GRAPHICS_REFRESH_RATE"));
      _lastRefreshRate = PlayerPrefs.GetInt("GRAPHICS_REFRESH_RATE");
    }

    if (_lastFullscreen != PlayerPrefs.GetInt("GRAPHICS_FULLSCREEN")) {
      if (PlayerPrefs.GetInt("GRAPHICS_FULLSCREEN") == 1) Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.ExclusiveFullScreen, Screen.currentResolution.refreshRate);
      else Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.Windowed, Screen.currentResolution.refreshRate);
      _lastFullscreen = PlayerPrefs.GetInt("GRAPHICS_FULLSCREEN");
    }

    if (_lastBrightness != PlayerPrefs.GetFloat("GRAPHICS_BRIGHTNESS")) {
      VolumeProfile volumeProfile = GameObject.FindGameObjectWithTag("Post Processing").GetComponent<Volume>().profile;
      LiftGammaGain lgg;
      volumeProfile.TryGet(out lgg);
      lgg.gamma.value = new Vector4(1, 1, 1, PlayerPrefs.GetFloat("GRAPHICS_BRIGHTNESS"));
      _lastBrightness = PlayerPrefs.GetFloat("GRAPHICS_BRIGHTNESS");
    }
  }
}