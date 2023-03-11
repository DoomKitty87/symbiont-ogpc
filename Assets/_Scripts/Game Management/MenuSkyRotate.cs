using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSkyRotate : MonoBehaviour
{
  void Update() {
    GetComponent<Skybox>().material.SetFloat("_Rotation", Time.unscaledTime * 0.4f);
  }
}
