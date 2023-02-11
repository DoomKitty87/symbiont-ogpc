using UnityEngine;
using UnityEngine.UI;

public class AmmoScript : MonoBehaviour 
{

  [SerializeField] Color32 targetColor;

  [HideInInspector] public int maxAmmo;
  [HideInInspector] public int currAmmo;

  private float colorCoefficient;
  private Color32 defaultColor;
  private Image image;

  private void Start() {
    image = GetComponent<Image>();
    defaultColor = image.color;
  }

  private void Update() {
    if (currAmmo != 0 && maxAmmo != 0) {
			// If you aren't dividing by zero, exponentially raise colorCoefficient based on how small currAmmo is in respect to maxAmmo
			colorCoefficient = ((maxAmmo/currAmmo) * (255 / maxAmmo));
    }
    image.color = Color.Lerp(defaultColor, targetColor, colorCoefficient / 255);
  }
}
