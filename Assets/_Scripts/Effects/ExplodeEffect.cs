using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplodeEffect : MonoBehaviour
{
  // TODO: Assign main and shape ParticleSystem references to explodeFX's main and shape modules
  // NOTE: Once this is finished, hook up to target HealthManager and remove code from gun controller to test;
  // will update SampleScene as well to finalize

  [Header("Particles")]
  [SerializeField] private GameObject _explodeFXPrefab;
  [SerializeField] private GameObject _fragmentFXPrefab;
  [SerializeField] private GameObject _explosionAudioPrefab;

  [Header("Explosion Settings")]
  [Tooltip("The amount of time it takes for this object to scale to (0, 0, 0) from its current scale")]
  [SerializeField] private float _scalingDuration = 0.12f;
  [SerializeField] private float _initialParticleSizeMultiplier = 0.25f;
  
  [SerializeField] private List<GameObject> _DestroyOnEffectComplete;

  private void Start()
  {
    if (_explodeFXPrefab == null) {
      Debug.LogError("ExpldoeEffect: ExplodeFX Particle Prefab is null!");
    }
    if (_explodeFXPrefab.TryGetComponent<ParticleSystem>(out _) == false) {
      Debug.LogError("ExplodeEffect: ExplodeFX Particle Prefab doesn't contain a particle system!");
    }
    if (_fragmentFXPrefab.TryGetComponent<ParticleSystem>(out _) == false) {
      Debug.LogError("ExplodeEffect: FragmentFX Particle Prefab doesn't contain a particle system!");
    }
  }
  public void StartEffect() {
    StartCoroutine(ExplodeTarget());
  }
  private IEnumerator ExplodeTarget() {
    float timer = 0f;
    MeshRenderer targetMesh = gameObject.GetComponent<MeshRenderer>();
    Vector3 initScale = transform.localScale;
    while (timer < _scalingDuration) {
      transform.localScale = Vector3.Lerp(initScale, new Vector3(0, 0, 0), Mathf.SmoothStep(0f, 1f, timer / _scalingDuration));
      timer += Time.deltaTime;
      yield return null;
    }
    transform.localScale = Vector3.zero;
    GameObject explodeFXInstance = Instantiate(_explodeFXPrefab, transform.position, Quaternion.identity);
    GameObject fragmentFXInstance = Instantiate(_fragmentFXPrefab, transform.position, Quaternion.identity);
    ParticleSystem explodeFX = explodeFXInstance.GetComponent<ParticleSystem>();
    ParticleSystem fragmentFX = fragmentFXInstance.GetComponent<ParticleSystem>();

    // Apparently the properties inside the modules are only settable through a specific class,
    // and not from the top level ParticleSystem class, so we have to create them here.
    ParticleSystem.MainModule explodeFXMain = explodeFX.main;
    ParticleSystem.ShapeModule explodeFXShape = explodeFX.shape;

    explodeFXMain.startSizeMultiplier = _initialParticleSizeMultiplier;
    explodeFXShape.mesh = gameObject.GetComponent<MeshFilter>().mesh;
    explodeFXShape.scale = transform.localScale;
    explodeFX.Play();
    fragmentFX.Play();
    GameObject audioPrefabInstance = Instantiate(_explosionAudioPrefab, transform.position, Quaternion.identity);
    AudioSource prefabAudioSource = audioPrefabInstance.GetComponent<AudioSource>();
    prefabAudioSource.Play();
    yield return new WaitForSeconds(prefabAudioSource.clip.length);
    Destroy(explodeFXInstance);
    Destroy(fragmentFXInstance);
    Destroy(audioPrefabInstance);
    for (int i = 0; i < _DestroyOnEffectComplete.Count; i++) {
      Destroy(_DestroyOnEffectComplete[i]);
    }
  }
}
