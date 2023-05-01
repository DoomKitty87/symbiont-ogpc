using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderReturnedGameObject : MonoBehaviour
{
  // One line, but still needed because GameObjects that contain colliders might not contain the "main" scripts.
  // Every raycast function that is dependent on "main" scripts should check for this.
  public GameObject _returnedGameObject;
}
