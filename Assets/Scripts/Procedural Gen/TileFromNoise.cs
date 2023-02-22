using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainType
{

  public string name;
  public float height;
  public Color color;
}

public class TileFromNoise : MonoBehaviour
{

  //[SerializeField] private Wave[] wavesStruct;
  [SerializeField] private TerrainType[] terrainTypes;
  [SerializeField] private NoiseMapGeneration noiseMapGeneration;
  [SerializeField] private MeshRenderer tileRenderer;
  [SerializeField] private MeshFilter meshFilter;
  [SerializeField] private MeshCollider meshCollider;
  [SerializeField] private float mapScale;
  [SerializeField] private float heightMultiplier;
  [SerializeField] private float amplitude;
  [SerializeField] private float frequency;
  [SerializeField] private float waves;
  [SerializeField] private float seed;
  [SerializeField] private float targetDensity;
  [SerializeField] private float buildingDensity;
  [SerializeField] private AnimationCurve heightCurve;
  [SerializeField] private GameObject targetPrefab;
  [SerializeField] private GameObject structurePrefab;
  [SerializeField] private LayerMask spawnedLayer;
  [SerializeField] private LayerMask groundLayer;

  private Transform playerVehicle;
  private float tileSize;
  private float distanceFromPlayer;
  private float heightIncStart;
  private float heightIncEnd;
  private LayerMask targetSpawnMask;

  void Awake(){
    targetSpawnMask = spawnedLayer | groundLayer;
    playerVehicle = GameObject.FindGameObjectWithTag("Player").transform;
    distanceFromPlayer = Mathf.Abs(playerVehicle.position.z - transform.position.z);
    tileSize = GetComponent<Renderer>().bounds.size.x;
    if (distanceFromPlayer > tileSize) Destroy(meshCollider);
    GenerateTile(1, 1);
  }

  public void GenerateTile(float heightIncreaseStart, float heigthIncreaseEnd) {
    Vector3[] meshVertices = meshFilter.mesh.vertices;
    int tileDepth = (int)Mathf.Sqrt(meshVertices.Length);
    int tileWidth = tileDepth;

    heightIncEnd = heightIncreaseStart;
    heightIncStart = heigthIncreaseEnd;

    float offsetX = -gameObject.transform.position.x;
    float offsetZ = -gameObject.transform.position.z;
    float[,] heightMap = noiseMapGeneration.CellularNoiseJobs(tileDepth, tileWidth, mapScale, offsetX, offsetZ, waves, amplitude, frequency, seed);
    //Texture2D tileTexture = BuildTexture(heightMap);
    //tileRenderer.material.mainTexture = tileTexture;
    UpdateMeshVertices(heightMap);
    if (distanceFromPlayer == tileSize * 3) GenerateStructures(heightMap, 1);
    if (distanceFromPlayer == tileSize * 5) GenerateStructures(heightMap, 2);
    if (distanceFromPlayer == tileSize) GenerateTargets(heightMap);
    //if (Mathf.Abs(playerVehicle.position.z - transform.position.z) < tileWidth) GenerateTargets(heightMap);
  }

  private TerrainType ChooseTerrainType(float height) {
    foreach (TerrainType terrainType in terrainTypes) {
      if (height < terrainType.height) {
        return terrainType;
      }
    }
    return terrainTypes[terrainTypes.Length -1];
  }

  private Texture2D BuildTexture(float[,] heightMap) {
    int tileDepth = heightMap.GetLength(0);
    int tileWidth = heightMap.GetLength(1);

    Color[] colorMap = new Color[tileDepth * tileWidth];
    for (int zIndex = 0; zIndex < tileDepth; zIndex++) {
      for (int xIndex = 0; xIndex < tileWidth; xIndex++) {
        int colorIndex = zIndex * tileWidth + xIndex;
        float height = heightMap[zIndex, xIndex];

        TerrainType terrainType = ChooseTerrainType(height);
        colorMap[colorIndex] = terrainType.color;
      }
    }
    Texture2D tileTexture = new Texture2D(tileWidth, tileDepth);
    tileTexture.wrapMode = TextureWrapMode.Clamp;
    tileTexture.SetPixels(colorMap);
    tileTexture.Apply();

    return tileTexture;
  }

  private void UpdateMeshVertices(float[,] heightMap) {
    int tileDepth = heightMap.GetLength(0);
    int tileWidth = heightMap.GetLength(1);

    Vector3[] meshVertices = meshFilter.mesh.vertices;

    int vertexIndex = 0;
    for (int zIndex = 0; zIndex < tileDepth; zIndex++) {
      for (int xIndex = 0; xIndex < tileWidth; xIndex++) {
        float height = heightMap[zIndex, xIndex];

        Vector3 vertex = meshVertices[vertexIndex];
        meshVertices[vertexIndex] = new Vector3(vertex.x, heightCurve.Evaluate(height) * heightMultiplier + Mathf.Lerp(heightIncStart, heightIncEnd, ((float)xIndex) / (tileWidth - 1)), vertex.z);
        vertexIndex++;
      }
    }
    meshFilter.mesh.vertices = meshVertices;
    meshFilter.mesh.RecalculateBounds();
    meshFilter.mesh.RecalculateNormals();
    if (meshCollider != null) meshCollider.sharedMesh = meshFilter.mesh;
  }

  private void GenerateStructures(float[,] heightMap, float buildingScale) {
    foreach (Transform child in transform) {
      Destroy(child.gameObject);
    }
    int tileDepth = heightMap.GetLength(0);

    Vector3[] meshVertices = meshFilter.mesh.vertices;

    int vertexIndex = 0;
    for (int zIndex = 0; zIndex < tileDepth; zIndex++) {
      for (int xIndex = 0; xIndex < tileDepth; xIndex++) {
        float height = heightMap[zIndex, xIndex];
        Vector3 vertex = meshVertices[vertexIndex];
        if (Random.value > 1 - buildingDensity / 100) {
          Transform tmp = Instantiate(structurePrefab, transform.position + vertex, Quaternion.identity, transform).transform;
          tmp.rotation = Quaternion.Euler(new Vector3(-90, Random.Range(0, 360), 0));
          tmp.localScale *= buildingScale;
        }
        vertexIndex++;
      }
    }
  }

  private void GenerateTargets(float[,] heightMap) {
    float targetCount = targetDensity;
    if (targetCount < 1) targetCount = (Random.value <= targetCount) ? 1 : 0;
    for (int i = 0; i < targetCount; i++) {
      RaycastHit hit;
      Physics.Raycast(transform.position + new Vector3(Random.Range(-tileSize / 2, tileSize / 2), 250, Random.Range(-tileSize / 2, tileSize / 2)), Vector3.down, out hit);
      Vector3 instPos = hit.point + new Vector3(0, 5, 0);
      Collider[] collidersOverlapped = new Collider[1];
      if (Physics.OverlapBoxNonAlloc(instPos, targetPrefab.GetComponent<Renderer>().bounds.size / 2, collidersOverlapped, Quaternion.identity, targetSpawnMask) == 0) {
        Instantiate(targetPrefab, instPos, Quaternion.identity, transform);
      }
    }
  }
}
