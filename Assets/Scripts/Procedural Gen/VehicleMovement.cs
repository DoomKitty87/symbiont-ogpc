using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{

  private RaycastHit hit;
  private float tileWidth;
  private float generatedDistance;
  private float turnSeverity;
  private float cycleOffset;
  private float lastZ;
  private float heightIncreaseLast;
  private float heightIncreaseCurr;
  private int xCycles;
  private int turnDuration;
  private int turnedLast;
  private GameObject[,] landInstances;
  private bool turning;

  [SerializeField] private GameObject tilePrefab;
  [SerializeField] private int genRange;
  [SerializeField] private int forwardRange;
  [SerializeField] private int turnDelay;
  [SerializeField] private float turnProbability;

  void Start() {
    tileWidth = tilePrefab.GetComponent<Renderer>().bounds.size.x;
    landInstances = new GameObject[forwardRange * 2 + 1, genRange * 2 + 1];
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
    for (int x = 0; x < forwardRange * 2 + 1; x++) {
      print(new Vector3((x - forwardRange) * tileWidth, 0, 0));
      for (int z = 0; z < genRange; z++) {
        landInstances[x, z] = Instantiate(tilePrefab, new Vector3((x - forwardRange) * tileWidth, 0, z * tileWidth), Quaternion.identity);
      }
      for (int z = 0; z < genRange; z++) {
        landInstances[x, z + genRange + 1] = Instantiate(tilePrefab, new Vector3((x - forwardRange) * tileWidth, 0, -z * tileWidth), Quaternion.identity);
      }
      landInstances[x, genRange] = Instantiate(tilePrefab, new Vector3((x - forwardRange) * tileWidth, 0, 0), Quaternion.identity);
    }
    heightIncreaseCurr = 1;
  }

  private void UpdateTerrain() {
    turnedLast++;
    if (turnedLast > turnDelay) {
      if (Random.value <= turnProbability) {
        turnSeverity = Random.Range(-tileWidth, tileWidth);
        turnDuration = Random.Range(1, turnDelay);
        turning = true;
        turnedLast = 0;
      }
    }
    if (turning) cycleOffset = lastZ + turnSeverity;
    heightIncreaseLast = heightIncreaseCurr;
    heightIncreaseCurr = Random.Range(1, 1.5f);
    for (int z = 0; z < genRange * 2 + 1; z++) {
      landInstances[xCycles, z].transform.position += new Vector3((forwardRange * 2 + 1) * tileWidth, 0, cycleOffset);
      landInstances[xCycles, z].GetComponent<TileFromNoise>().GenerateTile(heightIncreaseLast, heightIncreaseCurr);
    }
    lastZ = cycleOffset;
    generatedDistance += tileWidth;
    xCycles++;
    if (xCycles >= forwardRange * 2 + 1) xCycles = 0;
  }
}
