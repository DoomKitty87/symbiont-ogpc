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
  [SerializeField] private AnimationCurve heightCurve;
  [SerializeField] private GameObject targetPrefab;

  private Transform playerVehicle;
  private float tileWidth;

  void Awake(){
    playerVehicle = GameObject.FindGameObjectWithTag("Player").transform;
    if (Mathf.Abs(playerVehicle.position.z - transform.position.z) > tileWidth) Destroy(meshCollider);
    GenerateTile();
    tileWidth = GetComponent<Renderer>().bounds.size.x;
  }

  public void GenerateTile() {
    Vector3[] meshVertices = meshFilter.mesh.vertices;
    int tileDepth = (int)Mathf.Sqrt(meshVertices.Length);
    int tileWidth = tileDepth;

    float offsetX = -gameObject.transform.position.x;
    float offsetZ = -gameObject.transform.position.z;

    float[,] heightMap = noiseMapGeneration.CellularNoiseJobs(tileDepth, tileWidth, mapScale, offsetX, offsetZ, waves, amplitude, frequency, seed);

    //Texture2D tileTexture = BuildTexture(heightMap);
    //tileRenderer.material.mainTexture = tileTexture;
    UpdateMeshVertices(heightMap);
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
        meshVertices[vertexIndex] = new Vector3(vertex.x, heightCurve.Evaluate(height) * heightMultiplier, vertex.z);
        vertexIndex++;
      }
    }
    meshFilter.mesh.vertices = meshVertices;
    meshFilter.mesh.RecalculateBounds();
    meshFilter.mesh.RecalculateNormals();
    if (meshCollider != null) meshCollider.sharedMesh = meshFilter.mesh;
  }

  public void GenerateTargets(float[,] heightMap) {
    int tileDepth = heightMap.GetLength(0);
    int tileWidth = heightMap.GetLength(1);

    Vector3[] meshVertices = meshFilter.mesh.vertices;

    int vertexIndex = 0;
    for (int zIndex = 0; zIndex < tileDepth; zIndex++) {
      for (int xIndex = 0; xIndex < tileWidth; xIndex++) {
        float height = heightMap[zIndex, xIndex];
        Vector3 vertex = meshVertices[vertexIndex];
        if (Random.value > 0.999f) Instantiate(targetPrefab, transform.position + new Vector3(vertex.x, heightCurve.Evaluate(height) * heightMultiplier + 5, vertex.z), Quaternion.identity, transform);
        vertexIndex++;
      }
    }
  }
}
