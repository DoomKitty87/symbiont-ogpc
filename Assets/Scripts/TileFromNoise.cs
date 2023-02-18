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

  [SerializeField] private Wave[] waves;
  [SerializeField] private TerrainType[] terrainTypes;
  [SerializeField] private NoiseMapGeneration noiseMapGeneration;
  [SerializeField] private MeshRenderer tileRenderer;
  [SerializeField] private MeshFilter meshFilter;
  [SerializeField] private MeshCollider meshCollider;
  [SerializeField] private float mapScale;
  [SerializeField] private float heightMultiplier;
  [SerializeField] private AnimationCurve heightCurve;

  private Transform playerVehicle;
  private float tileWidth;
  private float genRange;

  void Start(){
    GenerateTile();
    playerVehicle = GameObject.FindGameObjectWithTag("Player").transform;
    tileWidth = GetComponent<Renderer>().bounds.size.x;
    genRange = playerVehicle.gameObject.GetComponent<VehicleMovement>().GetGenRange();
  }

  void Update() {
    if (playerVehicle.position.x - transform.position.x > tileWidth * genRange) Destroy(gameObject);
  }

  private void GenerateTile() {
    Vector3[] meshVertices = meshFilter.mesh.vertices;
    int tileDepth = (int)Mathf.Sqrt(meshVertices.Length);
    int tileWidth = tileDepth;

    float offsetX = -gameObject.transform.position.x;
    float offsetZ = -gameObject.transform.position.z;

    float[,] heightMap = noiseMapGeneration.GenerateCellularNoise(tileDepth, tileWidth, mapScale, offsetX, offsetZ, waves);

    Texture2D tileTexture = BuildTexture(heightMap);
    tileRenderer.material.mainTexture = tileTexture;
    UpdateMeshVertices(heightMap);
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
    meshCollider.sharedMesh = meshFilter.mesh;
  }
}
