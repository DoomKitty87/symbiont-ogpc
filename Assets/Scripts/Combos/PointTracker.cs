using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using Cinemachine;

public class PointTracker : MonoBehaviour
{

  [SerializeField] private float comboTime;
  [SerializeField] private GameObject HUD;
  [SerializeField] private GameObject WorldSpaceHUD;
  [SerializeField] private TextMeshProUGUI score;

  private float points;
  private float timer;
  private float comboLength;
  private float init;
  private TextMeshProUGUI combo;
  private Volume postProcessing;
  private Bloom bloom;
  private CinemachineVirtualCamera vcam;
  private CinemachineBasicMultiChannelPerlin noise;
  private Vector3 initScale;
  private GameObject comboParticles;

  //Functions to return variables (to prevent intentional or unintentional access of points variables)
  public float GetPoints() {
    return points;
  }

  public float GetCombo() {
    return comboLength;
  }

  //Changes color of score counter when it goes up in a combo
  private IEnumerator ScoreFX() {
    float fxtimer = 0f;
    float flashIn = 0.05f;
    float flashOut = 0.35f;
    while (fxtimer < flashIn) {
      score.color = Color.Lerp(Color.white, Color.green, fxtimer / flashIn);
      fxtimer += Time.deltaTime;
      yield return null;
    }
    fxtimer = 0f;
    while (fxtimer < flashOut) {
      score.color = Color.Lerp(Color.yellow, Color.white, fxtimer / flashOut);
      fxtimer += Time.deltaTime;
      yield return null;
    }
    score.color = Color.white;
  }

  //Flashes combo counter on screen, along with increasing bloom and shaking camera
  private IEnumerator FlashFX() {
    float fxtimer = 0f;
    float flashIn = 0.1f;
    float flashOut = 0.2f;
    initScale = combo.gameObject.transform.localScale;
    combo.gameObject.SetActive(true);
    combo.color = Color.clear;
    combo.gameObject.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-15, 15));
    Color targetColor = Color.clear;
    switch (Random.Range(0, 3)) {
      case 0:
        targetColor = Color.red;
        break;
      case 1:
        targetColor = Color.blue;
        break;
      case 2:
        targetColor = Color.yellow;
        break;
      case 3:
        targetColor = Color.green;
        break;
    }
    while (fxtimer < flashIn) {
      bloom.intensity.value = Mathf.Lerp(init, init * Mathf.Min(comboLength, 5), fxtimer / flashIn);
      combo.color = Color.Lerp(Color.clear, targetColor * 25, fxtimer / flashIn);
      combo.gameObject.transform.localScale = Vector3.Lerp(initScale, initScale * 3f, fxtimer / flashIn);
      noise.m_AmplitudeGain = Mathf.Lerp(0, 5, fxtimer / flashIn);
      fxtimer += Time.deltaTime;
      yield return null;
    }
    fxtimer = 0f;
    while (fxtimer < flashOut) {
      bloom.intensity.value = Mathf.Lerp(init * Mathf.Min(comboLength, 5), init, fxtimer / flashOut);
      combo.color = Color.Lerp(targetColor * 25, Color.clear, fxtimer / flashOut);
      noise.m_AmplitudeGain = Mathf.Lerp(5, 0, fxtimer / flashOut);
      fxtimer += Time.deltaTime;
      yield return null;
    }
    bloom.intensity.value = init;
    noise.m_AmplitudeGain = 0f;
    fxtimer = 0f;
    while (fxtimer < 0.1f) {
      combo.gameObject.transform.localScale = Vector3.Lerp(initScale * 3, Vector3.zero, fxtimer / 0.1f);
      fxtimer += Time.deltaTime;
      yield return null;
    }
    if (comboLength >= 10) comboParticles.GetComponent<ParticleSystem>().Play();
    combo.gameObject.SetActive(false);
    combo.gameObject.transform.localScale = initScale;
    combo.color = Color.clear;
  }

  //Used to communicate that a target was destroyed
  public void DestroyedTarget(GameObject target) {
    float basePoints = float.Parse(target.name.Substring(6, 1)) * 100;
    if (comboLength > 0) {
      StopCoroutine("ScoreFX");
      StopCoroutine("FlashFX");
      combo.gameObject.SetActive(false);
      bloom.intensity.value = init;
      combo.gameObject.transform.localScale = initScale;
      noise.m_AmplitudeGain = 0f;
      StartCoroutine(ScoreFX());
      StartCoroutine(FlashFX());
    }
    comboLength += 1;
    combo.text = "Combo x" + comboLength.ToString();
    basePoints = Mathf.Pow(basePoints, (1 + (((float)5 / 8) * Mathf.Log(comboLength, 10))));
    points += (int) basePoints;
    timer = 0f;
    score.text = points.ToString();
  }
  
  private void Start() {
    combo = WorldSpaceHUD.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    initScale = combo.gameObject.transform.localScale;
    postProcessing = GameObject.FindGameObjectWithTag("Post Processing").GetComponent<Volume>();
    postProcessing.profile.TryGet(out bloom);
    vcam = GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>();
    noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    comboParticles = combo.transform.GetChild(0).gameObject;
    init = bloom.intensity.value;
  }

  private void Update() {
    timer += Time.deltaTime;
    if (timer > comboTime) comboLength = 0;
  }
}