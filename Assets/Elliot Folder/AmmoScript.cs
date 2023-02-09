using UnityEngine;
using UnityEngine.UI;

public class AmmoScript : MonoBehaviour {
    Color32 defaultColor;
    Image image;
    [HideInInspector] public int maxAmmo;
    [HideInInspector] public int currAmmo;

    float colorCoefficient;

    [SerializeField] Color32 targetColor;

    private void Start() {
        image = GetComponent<Image>();
        defaultColor = image.color;
    }

    private void Update() {
        if (currAmmo != 0 && maxAmmo != 0) {
            colorCoefficient = ((maxAmmo/currAmmo) * (255 / maxAmmo));
        }
        Debug.Log(colorCoefficient / 255);
        image.color = Color.Lerp(defaultColor, targetColor, colorCoefficient / 255);
    }
}
