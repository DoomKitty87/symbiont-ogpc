using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{

  private RaycastHit hit;
  private float tileWidth;
  private float generatedDistance;
  private float turnSeverity;
  private float hillSeverity;
  private float cycleOffset;
  private float lastZ;
  private float heightIncreaseLast;
  private float heightIncreaseCurr;
  private float trickleDownLast;
  private float trickleDownNext;
  private float goingFrom;
  private float goingTo;
  private int xCycles;
  private int turnDuration;
  private int turnedLast;
  private int hillLast;
  private int hillDuration;
  private GameObject[,] landInstances;
  private float[] cachedPositions;
  private bool turning;
  private bool hill;

  [SerializeField] private GameObject tilePrefab;
  [SerializeField] private int genRange;
  [SerializeField] private int forwardRange;
  [SerializeField] private int turnDelay;
  [SerializeField] private int hillDelay;
  [SerializeField] private float turnProbability;
  [SerializeField] private float hillProbability;
  [SerializeField] private float minHillAmplitude;
  [SerializeField] private float maxHillAmplitude;
  [SerializeField] private int minHillLength;

  void Start() {
    tileWidth = tilePrefab.GetComponent<Renderer>().bounds.size.x;
    landInstances = new GameObject[forwardRange * 2 + 1, genRange * 2 + 1];
    cachedPositions = new float[forwardRange + 1];
    InitialGeneration();
  }

  public float GetGenRange() {
    return genRange;
  }

  void Update() {
    if (transform.position.x - generatedDistance > tileWidth) UpdateTerrain();
    transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(goingFrom, goingTo, (transform.position.x % 10) / 10));
  }

  void FixedUpdate() {
    transform.position += new Vector3(10f * Time.deltaTime, 0, 0);
    if (Physics.Raycast(transform.position, -transform.up, out hit)) {
      if (Mathf.Abs(Mathf.Abs(hit.point.y - transform.position.y) - 5) > 0.5f) {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, hit.point.y + 5, 0.025f), transform.position.z);
      }
    }
  }

  private void InitialGeneration() {
    for (int i = 0; i < forwardRange + 1; i++) {
      cachedPositions[i] = transform.position.z;
    }
    for (int x = 0; x < forwardRange * 2 + 1; x++) {
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
    hillLast++;
    if (turnedLast > turnDelay) {
      if (Random.value <= turnProbability) {
        turnSeverity = Random.Range(-tileWidth, tileWidth);
        turnDuration = Random.Range(1, turnDelay);
        turning = true;
        turnedLast = 0;
      }
    }
    if (turning) cycleOffset = lastZ + turnSeverity;

    if (hillLast > hillDelay) {
      if (Random.value <= hillProbability) {
        hillSeverity = Random.Range(minHillAmplitude, maxHillAmplitude);
        hillDuration = Random.Range(minHillLength, hillDelay);
        hill = true;
        hillLast = 0;
      }
    }

    heightIncreaseLast = heightIncreaseCurr;
    if (hill) {
      if (hillDuration % 2 == 0) hillDuration -= 1;
      if (hillLast > hillDuration) {
        hill = false;
      }
      else if (hillLast < hillDuration / 2f) {
        heightIncreaseCurr = Mathf.SmoothStep(1, hillSeverity, (hillLast + 1) / ((hillDuration + 1) / 2f));
      }
      else if (hillLast > hillDuration / 2f) {
        heightIncreaseCurr = Mathf.SmoothStep(hillSeverity, 1, (hillLast + 1 - ((hillDuration + 1) / 2)) / ((hillDuration + 1) / 2f));
      }
    }
    else {
      heightIncreaseCurr = 1;
    }

    for (int z = 0; z < genRange * 2 + 1; z++) {
      landInstances[xCycles, z].transform.position += new Vector3((forwardRange * 2 + 1) * tileWidth, 0, cycleOffset);
      landInstances[xCycles, z].GetComponent<TileFromNoise>().GenerateTile(heightIncreaseLast, heightIncreaseCurr);
    }
    lastZ = cycleOffset;
    trickleDownLast = cycleOffset;
    for (int i = cachedPositions.Length - 1; i >= 0; i--) {
      trickleDownNext = cachedPositions[i];
      cachedPositions[i] = trickleDownLast;
      trickleDownLast = trickleDownNext;
    }
    goingFrom = goingTo;
    goingTo = cachedPositions[0];
    generatedDistance += tileWidth;
    xCycles++;
    if (xCycles >= forwardRange * 2 + 1) xCycles = 0;
  }
}
