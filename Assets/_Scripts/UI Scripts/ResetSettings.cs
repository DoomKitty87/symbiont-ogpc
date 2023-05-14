using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSettings : MonoBehaviour
{

  [SerializeField] private GameObject[] _sliders, _toggles, _carousels;

  public void Reset() {
    foreach (GameObject sl in _sliders) {
      sl.GetComponent<SliderScript>().ResetValue();
    }
    foreach (GameObject tg in _toggles) {
      tg.GetComponent<CheckboxScript>().ResetValue();
    }
    foreach (GameObject cr in _carousels) {
      cr.GetComponent<CarouselScript>().ResetValue();
    }
  }
}