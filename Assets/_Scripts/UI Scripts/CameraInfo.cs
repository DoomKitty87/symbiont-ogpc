using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraInfo : MonoBehaviour
{

  [SerializeField] private GameObject _cameraVolume;

  private void Start() {
    VolumeProfile profile = _cameraVolume.GetComponent<Volume>().profile;
    ColorAdjustments clr;
    profile.TryGet(out clr);
    clr.hueShift.value = GameObject.FindGameObjectWithTag("Persistent").GetComponent<FloorManager>().GetHueShift();
  }
}