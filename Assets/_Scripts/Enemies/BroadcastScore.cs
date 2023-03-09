using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BroadcastScore : MonoBehaviour
{
  [Header("This requires a GameObject with tag 'Handler' containing ScoreTracker")]
  [SerializeField] private float _pointsWorth;

  public void BroadcastPoints() {
    GameObject.FindGameObjectWithTag("Handler").GetComponent<ScoreTracker>().AddPoints(_pointsWorth);
  }
}
