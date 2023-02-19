using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayHealth : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField][Tooltip("This is optional.")] private TextMeshProUGUI _textElement;
    [SerializeField][Tooltip("This is optional.")] private Slider _healthSlider;


    [Header("Tweening Values")]
    [SerializeField] private AnimationCurve _easeCurve;
    [SerializeField] private float _easeDuration;

    // Debounce
    private bool _tweeningHealth;

    private void Start()
    {
        if (_textElement == null & _healthSlider == null)
        {
            Debug.LogWarning("DisplayHealth: Both UI Elements are null. Are you missing a reference?");
        }
    }
    private void Update()
    {

    }
    public void OnHealthChanged(float health, float maxHealth)
    {
        if (_healthSlider != null)
        {
            StartCoroutine(TweenSlider(_healthSlider, health / maxHealth, _easeDuration));
        }
    }
    private IEnumerator TweenSlider(Slider slider, float targetValue, float duration)
    {
        float timeElapsed = 0;
        float initSliderValue = slider.value;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
            t = _easeCurve.Evaluate(t); 
            slider.value = Mathf.Lerp(initSliderValue, targetValue, t);
            yield return null;
        }
        slider.value = targetValue;
    }
    private IEnumerator TweenTextValue(TextMeshProUGUI text, float targetValue, float duration)
    {
        float timeElapsed = 0;
        // float initSliderValue = slider.value;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
            t = _easeCurve.Evaluate(t); 
            // slider.value = Mathf.Lerp(initSliderValue, targetValue, t);
            yield return null;
        }

    }
}

    

