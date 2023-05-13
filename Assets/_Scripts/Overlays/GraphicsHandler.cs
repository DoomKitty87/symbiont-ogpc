using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GraphicsHandler : MonoBehaviour
{

  private RenderPipelineAsset _renderAsset;
  private int _lastAA, _lastAniso, _lastShadows, _lastVsync;

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
      if (PlayerPrefs.GetInt("GRAPHICS_SHADOWS_ENABLED") == 1) QualitySettings.shadows = ShadowQuality.All;
      else QualitySettings.shadows = ShadowQuality.Disable;
      _lastShadows = PlayerPrefs.GetInt("GRAPHICS_SHADOWS_ENABLED");
    }

    if (PlayerPrefs.HasKey("GRAPHICS_VSYNC")) {
      if (PlayerPrefs.GetInt("GRAPHICS_VSYNC") == 1) QualitySettings.vSyncCount = 1;
      else QualitySettings.vSyncCount = 0;
      _lastVsync = PlayerPrefs.GetInt("GRAPHICS_VSYNC");
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
      if (PlayerPrefs.GetInt("GRAPHICS_SHADOWS_ENABLED") == 1) QualitySettings.shadows = ShadowQuality.All;
      else QualitySettings.shadows = ShadowQuality.Disable;
      _lastShadows = PlayerPrefs.GetInt("GRAPHICS_SHADOWS_ENABLED");
    }

    if (_lastVsync != PlayerPrefs.GetInt("GRAPHICS_VSYNC")) {
      if (PlayerPrefs.GetInt("GRAPHICS_VSYNC") == 1) QualitySettings.vSyncCount = 1;
      else QualitySettings.vSyncCount = 0;
      _lastVsync = PlayerPrefs.GetInt("GRAPHICS_VSYNC");
    }
  }
}