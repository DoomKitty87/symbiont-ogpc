using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairConfig : MonoBehaviour
{
  // These fields are going to be public so that a settings menu can be made to change them
  
  [Header("Center Dot")]
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
  public bool _useCrosshair;
  public CrosshairType _crosshairType;
  public enum CrosshairType { Double, Triple, Quad }
  [ColorUsage(true, false)] public Color _crosshairColor;
  public float _crosshairLength;
  public float _crosshairThickness;
  [Header("Crosshair Outline")]
  public bool _useCrosshairOutline;
  [ColorUsage(true, false)] public Color _crosshairOutlineColor;
  public float _crosshairOutlineThickness;
  [Header("Crosshair Spread")]
  public float _baseCrosshairSpread;
  public float _maxCrosshairSpread;


    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
