using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyButton : MonoBehaviour
{

  [SerializeField] private GameObject[] _sliders, _toggles, _carousels;

  public void ApplySettings() {
    foreach (GameObject slider in _sliders) {
      slider.GetComponent<SliderScript>().Apply();
    }
    foreach (GameObject toggle in _toggles) {
      toggle.GetComponent<CheckboxScript>().Apply();
    }
    foreach (GameObject carousel in _carousels) {
      carousel.GetComponent<CarouselScript>().Apply();
    }
  }
}