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
        while (timer < 0.15f) {
            transform.localScale = Vector3.Lerp(init, init * 0.8f, timer / 0.15f);
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        while (timer < 0.25f) {
            transform.localScale = Vector3.Lerp(init * 0.8f, init, timer / 0.25f);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = init;
    }
}
