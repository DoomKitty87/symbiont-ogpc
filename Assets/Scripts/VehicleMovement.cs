using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{

  private RaycastHit hit;
  private float tileWidth;
  private float generatedDistance;

  [SerializeField] private GameObject tilePrefab;
  [SerializeField] private float genRange;

  void Start() {
    tileWidth = tilePrefab.GetComponent<Renderer>().bounds.size.x;
    generatedDistance = -tileWidth;
  }

  public float GetGenRange() {
    return genRange;
  }

  void Update() {
    if (transform.position.x - generatedDistance > tileWidth * -genRange) LoadNewTerrain();
  }

  void FixedUpdate() {
    transform.position += new Vector3(10f * Time.deltaTime, 0, 0);
    Physics.Raycast(transform.position, -transform.up, out hit);
    if (Mathf.Abs(Mathf.Abs(hit.point.y - transform.position.y) - 5) > 0.5f) {
      transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, hit.point.y + 5, 0.1f), transform.position.z);
    }
  }

  private void LoadNewTerrain() {
    float xIndex = generatedDistance + tileWidth;
    Instantiate(tilePrefab, new Vector3(xIndex, 0, 0), Quaternion.identity);
    for (int n = 0; n < genRange; n++) {
      Instantiate(tilePrefab, new Vector3(xIndex, 0, (n + 1) * tileWidth), Quaternion.identity);
      Instantiate(tilePrefab, new Vector3(xIndex, 0, (n + 1) * -tileWidth), Quaternion.identity);
    }
    generatedDistance = xIndex;
  }
}
