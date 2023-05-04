using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class SliderScriptElliot : MonoBehaviour
{
   [Header("Setup")]
   [SerializeField] private float minValue;
   [SerializeField] private float maxValue;
   [SerializeField] private float _defaultValue;
   [SerializeField] private string _settingsKey;


 [Header("References")]
 [SerializeField] private Slider _slider;
   [SerializeField] private TextMeshProUGUI _valueText;
   [SerializeField] private Button _button;
   [SerializeField] private TMP_InputField _inputField;




   [Header("Layout")]
   [Tooltip("How text should be displayed.")]
   [SerializeField] private string layoutType;
  
   private void Start() {
       _slider.onValueChanged.AddListener((v) => {
           _valueText.text = v.ToString(layoutType);
           PlayerPrefs.SetFloat(_settingsKey, v);
       });


       _slider.minValue = minValue;
       _slider.maxValue = maxValue;
       _slider.value = _defaultValue; // Remove this with the relevant TODO


       // TODO:
       // Have slider value fetch playerPrefs value on start and assign slider.value to playerPrefs.value
       // If playerPrefs value is null set value to _defaultValue
       // This should also fix bug with value text not updating initially




       _button.onClick.AddListener(() => ShowInputField());
       _inputField.onSubmit.AddListener(EnterInputSlider);


       _inputField.gameObject.SetActive(false);
   }


   private void ShowInputField() {
       _valueText.gameObject.SetActive(false);
       _inputField.gameObject.SetActive(true);


       _inputField.text = _slider.value.ToString(layoutType);


       _inputField.Select();
   }


   private void EnterInputSlider(string result) {


       float updateNum = Mathf.Clamp(float.Parse(result), _slider.minValue, _slider.maxValue);
       updateNum = Mathf.Round(updateNum * 100f) / 100f;


       _slider.value = updateNum;
       _valueText.gameObject.SetActive(true);
       _inputField.gameObject.SetActive(false);
       return;
   }  
}




