using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{

    public float health;

    private float[] targetHealth = new float[] {5f, 7f, 9f, 12f};
    void Start()
    {
        health = targetHealth[(int)float.Parse(gameObject.name.Substring(6, 1)) - 1];
    }

    public void IsHit() {
        StopCoroutine("Bounce");
        StartCoroutine(Bounce());
    }

    private IEnumerator Bounce() {
        Vector3 init = transform.localScale;
        float timer = 0f;
        while (timer < 0.075f) {
            transform.localScale = Vector3.Lerp(init, init * 0.9f, timer / 0.075f);
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        while (timer < 0.1f) {
            transform.localScale = Vector3.Lerp(init * 0.9f, init, timer / 0.1f);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = init;
    }
}
