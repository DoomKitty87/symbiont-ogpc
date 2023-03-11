using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairConfig : MonoBehaviour
{
  [SerializeField] 

  // These fields are going to be public so that a settings menu can be made to change them
  [Header("Center Dot")]
  public bool _useCenterDot;
  public GameObject _centerDotElement;
  public CenterDotType _centerDotType;
  public enum CenterDotType { Circle, Square }
  public Sprite _centerDotCircleSprite;
  public Sprite _centerDotSquareSprite;
  [ColorUsage(true, false)] public Color _centerDotColor;
  public float _centerDotSize;
  [Header("Center Dot Outline")]
  public bool _useCenterDotOutline;
  public Outline _centerDotOutlineElement;
  [ColorUsage(true, false)] public Color _centerDotOutlineColor;
  [Range(0, 1.5f)] public float _centerDotOutlineThickness;
  [Header("Crosshair")]
  public bool _useCrosshair;
  public GameObject _crosshairElementTop;
  public GameObject _crosshairElementBottom;
  public GameObject _crosshairElementLeft;
  public GameObject _crosshairElementRight;
  public CrosshairType _crosshairType;
  public enum CrosshairType { Double, Triple, Quad }
  [ColorUsage(true, false)] public Color _crosshairColor;
  public float _crosshairLength;
  public float _crosshairThickness;
  public float _crosshairBaseOffset;
  [Header("Crosshair Outline")]
  public bool _useCrosshairOutline;
  public Outline _crosshairOutlineElementTop;
  public Outline _crosshairOutlineElementBottom;
  public Outline _crosshairOutlineElementLeft;
  public Outline _crosshairOutlineElementRight;

  [ColorUsage(true, false)] public Color _crosshairOutlineColor;
  [Range(0, 1.5f)] public float _crosshairOutlineThickness;
  [Header("Crosshair Dynamic Offset")]
  public bool _useDynamicOffset;
  public float _dynamicOffset;
  public float _dynamicOffsetMultiplier;
  public float _dynamicOffsetMax;
  
  private void OnValidate() {
      ApplySettings();
  }

  // Start is called before the first frame update
  private void Start() {
    ApplySettings();
  }

  public void SetDynamicOffset(float dynamicOffset) {
    _dynamicOffset = dynamicOffset;
  }

  private void LateUpdate() {
    if (_useDynamicOffset) {
      UpdatePositionsByDynamicOffset();
    }
  }

  private void ApplySettings() {
    UpdateActivation();
    UpdateColors();
    UpdateSize();
    UpdatePosition();
  }

  private void UpdatePositionsByDynamicOffset() {
    Vector2 crosshairTopPosition = new Vector2(0, _crosshairBaseOffset + _crosshairLength / 2);
    Vector2 crosshairBottomPosition = new Vector2(0, -(_crosshairBaseOffset + _crosshairLength / 2));
    Vector2 crosshairLeftPosition = new Vector2(-(_crosshairBaseOffset + _crosshairLength / 2), 0);
    Vector2 crosshairRightPosition = new Vector2(_crosshairBaseOffset + _crosshairLength / 2, 0);

    float dynamicOffset = _dynamicOffset * _dynamicOffsetMultiplier;
    if (dynamicOffset > _dynamicOffsetMax) {
      dynamicOffset = _dynamicOffsetMax;
    }

    _crosshairElementTop.GetComponent<RectTransform>().anchoredPosition = crosshairTopPosition + new Vector2(0, dynamicOffset);
    _crosshairElementBottom.GetComponent<RectTransform>().anchoredPosition = crosshairBottomPosition + new Vector2(0, -dynamicOffset);
    _crosshairElementLeft.GetComponent<RectTransform>().anchoredPosition = crosshairLeftPosition + new Vector2(-dynamicOffset, 0);
    _crosshairElementRight.GetComponent<RectTransform>().anchoredPosition = crosshairRightPosition + new Vector2(dynamicOffset, 0);
  }

  private void UpdateActivation() {
    if (_useCenterDot) {
      _centerDotElement.SetActive(true);
    } 
    else {
      _centerDotElement.SetActive(false);
    }

    if (_useCrosshair) {
      if (_crosshairType == CrosshairType.Double) {
        _crosshairElementTop.SetActive(false); // 
        _crosshairElementBottom.SetActive(false); // 
        _crosshairElementLeft.SetActive(true);
        _crosshairElementRight.SetActive(true);
      }
      else if (_crosshairType == CrosshairType.Triple) {
        _crosshairElementTop.SetActive(false); // 
        _crosshairElementBottom.SetActive(true);
        _crosshairElementLeft.SetActive(true);
        _crosshairElementRight.SetActive(true);
      } 
      else if (_crosshairType == CrosshairType.Quad) {
        _crosshairElementTop.SetActive(true);
        _crosshairElementBottom.SetActive(true);
        _crosshairElementLeft.SetActive(true);
        _crosshairElementRight.SetActive(true);
      }
    } 
    else {
      _crosshairElementTop.SetActive(false);
      _crosshairElementBottom.SetActive(false);
      _crosshairElementLeft.SetActive(false);
      _crosshairElementRight.SetActive(false);
    }

    // Outlines
    if (_useCenterDotOutline) {
      _centerDotOutlineElement.enabled = true;
    } 
    else {
      _centerDotOutlineElement.enabled = false;
    }

    if (_useCrosshairOutline) {
      _crosshairOutlineElementTop.enabled = true;
      _crosshairOutlineElementBottom.enabled = true;
      _crosshairOutlineElementLeft.enabled = true;
      _crosshairOutlineElementRight.enabled = true;
    } 
    else {
      _crosshairOutlineElementTop.enabled = false;
      _crosshairOutlineElementBottom.enabled = false;
      _crosshairOutlineElementLeft.enabled = false;
      _crosshairOutlineElementRight.enabled = false;
    }
  }

  private void UpdateType() {
    if (_centerDotType == CenterDotType.Circle) {
      _centerDotElement.GetComponent<Image>().sprite = _centerDotCircleSprite;
    } 
    else if (_centerDotType == CenterDotType.Square) {
      _centerDotElement.GetComponent<Image>().sprite = _centerDotSquareSprite;
    }
  }

  private void UpdateColors() {
    // Center Dot
    _centerDotElement.GetComponent<Image>().color = _centerDotColor;
    _centerDotOutlineElement.effectColor = _centerDotOutlineColor;

    // Crosshair
    _crosshairElementTop.GetComponent<Image>().color = _crosshairColor;
    _crosshairElementBottom.GetComponent<Image>().color = _crosshairColor;
    _crosshairElementLeft.GetComponent<Image>().color = _crosshairColor;
    _crosshairElementRight.GetComponent<Image>().color = _crosshairColor;
    _crosshairOutlineElementTop.effectColor = _crosshairOutlineColor;
    _crosshairOutlineElementBottom.effectColor = _crosshairOutlineColor;
    _crosshairOutlineElementLeft.effectColor = _crosshairOutlineColor;
    _crosshairOutlineElementRight.effectColor = _crosshairOutlineColor;
  }

  private void UpdateSize() {
    // Center Dot
    _centerDotElement.GetComponent<RectTransform>().sizeDelta = new Vector2(_centerDotSize, _centerDotSize);

    // Center Dot Outline
    _centerDotOutlineElement.effectDistance = new Vector2(_centerDotOutlineThickness, _centerDotOutlineThickness);

    // Crosshair
    _crosshairElementTop.GetComponent<RectTransform>().sizeDelta = new Vector2(_crosshairThickness, _crosshairLength);
    _crosshairElementBottom.GetComponent<RectTransform>().sizeDelta = new Vector2(_crosshairThickness, _crosshairLength);
    _crosshairElementLeft.GetComponent<RectTransform>().sizeDelta = new Vector2(_crosshairLength, _crosshairThickness);
    _crosshairElementRight.GetComponent<RectTransform>().sizeDelta = new Vector2(_crosshairLength, _crosshairThickness);

    // Crosshair Outline
    _crosshairOutlineElementTop.effectDistance = new Vector2(_crosshairOutlineThickness, _crosshairOutlineThickness);
  }

  private void UpdatePosition() {
    // Crosshair
    _crosshairElementTop.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, _crosshairBaseOffset + _crosshairLength / 2);
    _crosshairElementBottom.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(_crosshairBaseOffset + _crosshairLength / 2));
    _crosshairElementLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(_crosshairBaseOffset + _crosshairLength / 2), 0);
    _crosshairElementRight.GetComponent<RectTransform>().anchoredPosition = new Vector2(_crosshairBaseOffset + _crosshairLength / 2, 0);
  }
}
