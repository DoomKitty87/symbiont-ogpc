using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
  // TODO: Will update this to include minutes per in game day, maybe representation of in game time? 
  private float dayProgression;

  [SerializeField] private Transform skyAxis;
  [SerializeField] private float timePerDay;

  void Start() {

  }

  void Update() {
    dayProgression += Time.deltaTime;

    skyAxis.rotation = Quaternion.Euler(dayProgression / timePerDay * 360, skyAxis.rotation.y, skyAxis.rotation.z);

    if (dayProgression >= timePerDay) dayProgression = 0;
  }
}