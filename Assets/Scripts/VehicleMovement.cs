using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{

  private RaycastHit hit;
  private float tileWidth;
  private float generatedDistance;
  private int xCycles;
  private GameObject[,] landInstances;

  [SerializeField] private GameObject tilePrefab;
  [SerializeField] private int genRange;

  void Start() {
    tileWidth = tilePrefab.GetComponent<Renderer>().bounds.size.x;
    landInstances = new GameObject[genRange * 2 + 1, genRange * 2 + 1];
    InitialGeneration();
  }

  public float GetGenRange() {
    return genRange;
  }

  void Update() {
    if (transform.position.x - generatedDistance > tileWidth) UpdateTerrain();
  }

  void FixedUpdate() {
    transform.position += new Vector3(10f * Time.deltaTime, 0, 0);
    Physics.Raycast(transform.position, -transform.up, out hit);
    if (Mathf.Abs(Mathf.Abs(hit.point.y - transform.position.y) - 5) > 0.5f) {
      transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, hit.point.y + 5, 0.1f), transform.position.z);
    }
  }

  private void InitialGeneration() {
    for (int x = 0; x < genRange * 2 + 1; x++) {
      for (int z = 0; z < genRange; z++) {
        landInstances[x, z] = Instantiate(tilePrefab, new Vector3((x - genRange) * tileWidth, 0, z * tileWidth), Quaternion.identity);
      }
      for (int z = 0; z < genRange; z++) {
        landInstances[x, z + genRange + 1] = Instantiate(tilePrefab, new Vector3((x - genRange) * tileWidth, 0, -z * tileWidth), Quaternion.identity);
      }
      landInstances[x, genRange] = Instantiate(tilePrefab, new Vector3((x - genRange) * tileWidth, 0, 0), Quaternion.identity);
    }
  }

  private void UpdateTerrain() {
    for (int z = 0; z < genRange * 2 + 1; z++) {
      landInstances[xCycles, z].transform.position += new Vector3((genRange * 2 + 1) * tileWidth, 0, 0);
      landInstances[xCycles, z].GetComponent<TileFromNoise>().GenerateTile();
    }
    generatedDistance += tileWidth;
    xCycles++;
    if (xCycles > genRange * 2 + 1) xCycles = 0;
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
