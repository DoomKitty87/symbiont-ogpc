using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplodeEffect : MonoBehaviour
{
  // TODO: Assign main and shape ParticleSystem references to explodeFX's main and shape modules
  // NOTE: Once this is finished, hook up to target HealthManager and remove code from gun controller to test;
  // will update SampleScene as well to finalize

  private GameObject explodeFX;
  private GameObject fragmentFX;
  [SerializeField] private GameObject explosionAudioPrefab;

  private ParticleSystem.MainModule main;
  private ParticleSystem.ShapeModule sh;
  
  public UnityEvent _OnEffectComplete;

  private void Start()
  {
    main = explodeFX.GetComponent<ParticleSystem>().main;
    sh = explodeFX.GetComponent<ParticleSystem>().shape;
  }
  public void StartEffect() {
    StartCoroutine(ExplodeTarget());
  }
  private IEnumerator ExplodeTarget() {
    float timer = 0f;
    float explodeTime = 0.12f;
    MeshRenderer targetMesh = gameObject.GetComponent<MeshRenderer>();
    Vector3 initScale = gameObject.transform.localScale;
    while (timer < explodeTime) {
      gameObject.transform.localScale = Vector3.Lerp(initScale, new Vector3(0, 0, 0), Mathf.SmoothStep(0f, 1f, timer / explodeTime));
      timer += Time.deltaTime;
      yield return null;
    }
    sh.mesh = gameObject.GetComponent<MeshFilter>().mesh;
    sh.scale = transform.localScale;
    main.startSizeMultiplier = 0.25f;
    explodeFX.transform.position = transform.position;
    fragmentFX.transform.position = explodeFX.transform.position;
    explodeFX.GetComponent<ParticleSystem>().Play();
    fragmentFX.GetComponent<ParticleSystem>().Play();
    GetComponent<ItemDrops>().RollForItem();
    GameObject tmp = Instantiate(explosionAudioPrefab, transform.position, Quaternion.identity);
    Destroy(gameObject);
    tmp.GetComponent<AudioSource>().Play();
    yield return new WaitForSeconds(tmp.GetComponent<AudioSource>().clip.length);
    Destroy(tmp);
  }
}
