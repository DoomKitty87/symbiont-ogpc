using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Toggle))]
public class SwitchSpriteOnToggle : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private Toggle _toggle;
  [SerializeField] private Image _image;
  [SerializeField] private Sprite _onSprite, _offSprite;

  private void Start() {
    if (_toggle == null) {
      _toggle = gameObject.GetComponent<Toggle>();
    }
    if (_image == null) {
      Debug.LogError("SwitchSpriteOnToggle: Image is null!");
    }
    if (_onSprite == null) {
      Debug.LogError("SwitchSpriteOnToggle: On Sprite is null!");
    }
    if (_offSprite == null) {
      Debug.LogError("SwitchSpriteOnToggle: Off Sprite is null!");
    }
    _toggle.onValueChanged.AddListener(SwitchSprites);
  }

 public void SwitchSprites(bool _) {
    if (_toggle.isOn) {
      _image.sprite = _onSprite;
    }
    else {
      _image.sprite = _offSprite;
    }
 }
}
