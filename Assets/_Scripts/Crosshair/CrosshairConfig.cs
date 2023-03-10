using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairConfig : MonoBehaviour
{
  [SerializeField] 

  // These fields are going to be public so that a settings menu can be made to change them
  [Header("Center Dot")]
  public GameObject _centerDotElement;
  public bool _useCenterDot;
  public CenterDotType _centerDotType;
  public enum CenterDotType { Dot, Circle, Square }
  [ColorUsage(true, false)] public Color _centerDotColor;
  public float _centerDotSize;
  [Header("Center Dot Outline")]
  public bool _useCenterDotOutline;
  [ColorUsage(true, false)] public Color _centerDotOutlineColor;
  public float _centerDotOutlineThickness;
  [Header("Crosshair")]
  public GameObject _crosshairElementTop;
  public GameObject _crosshairElementBottom;
  public GameObject _crosshairElementLeft;
  public GameObject _crosshairElementRight;
  public bool _useCrosshair;
  public CrosshairType _crosshairType;
  public enum CrosshairType { Double, Triple, Quad }
  [ColorUsage(true, false)] public Color _crosshairColor;
  public float _crosshairLength;
  public float _crosshairThickness;
  public float _crosshairOffset;
  [Header("Crosshair Outline")]
  public bool _useCrosshairOutline;
  [ColorUsage(true, false)] public Color _crosshairOutlineColor;
  public float _crosshairOutlineThickness;
  [Header("Crosshair Dynamic Offset")]
  public bool _useDynamicOffset;
  public float _dynamicOffsetMultiplier;
  


  // Start is called before the first frame update
  private void Start() {
    ApplySettings();
  }


  public void ApplySettings() {

  }

  private void UpdateActivation() {
    if (_useCenterDot) {
      _centerDotElement.SetActive(true);
    } else {
      _centerDotElement.SetActive(false);
    }
    if (_crosshairType == CrosshairType.Double) {
      _crosshairElementTop.SetActive(false);
      _crosshairElementBottom.SetActive(false);
      _crosshairElementLeft.SetActive(true);
      _crosshairElementRight.SetActive(true);
    } else if (_crosshairType == CrosshairType.Triple) {
      _crosshairElementTop.SetActive(false);
      _crosshairElementBottom.SetActive(true);
      _crosshairElementLeft.SetActive(true);
      _crosshairElementRight.SetActive(true);
    } else if (_crosshairType == CrosshairType.Quad) {
      _crosshairElementTop.SetActive(true);
      _crosshairElementBottom.SetActive(true);
      _crosshairElementLeft.SetActive(true);
      _crosshairElementRight.SetActive(true);
    }
  }

}
