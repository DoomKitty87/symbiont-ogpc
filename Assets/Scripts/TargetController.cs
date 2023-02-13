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
}
