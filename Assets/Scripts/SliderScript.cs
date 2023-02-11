using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SliderScript : MonoBehaviour
{

	[SerializeField] private GameObject valueText;

	public void Update() {
		ReplaceTextValue();
	}

	private void ReplaceTextValue() {
		valueText.GetComponent<TextMeshProUGUI>().text = System.Math.Round(GetSliderValue(), 2).ToString() + ExtraText(System.Math.Round(GetSliderValue(), 2));
	}

	private string ExtraText(double value) {
		if (value == 0 || value == 1) {
			return ".00";
		} else if (value == 0.1 || value == 0.2 || value == 0.3 || value == 0.4 || value == 0.5 || value == 0.6 || value == 0.7 || value == 0.8 || value == 0.9) {
			// REALLY DUMB Need to fix this
			return "0";
		} else return null;
	}

	private float GetSliderValue() {
		return GetComponent<Slider>().value;
	}

}
