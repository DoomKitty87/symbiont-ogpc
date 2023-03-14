using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainType
{

  public string name;
  public float distance;
  public Color color;
  public GameObject structureType;
  public GameObject groundDeco;
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
  [SerializeField] private float bonusDensity;
  [SerializeField] private AnimationCurve heightCurve;
  [SerializeField] private LayerMask spawnedLayer;
  [SerializeField] private LayerMask groundLayer;
  [SerializeField] private int targetSpawnOffset;
  [SerializeField] private int buildingSpawnOffset;
  [SerializeField] private int buildingCycles;
  [SerializeField] private GameObject[] targets;
  [SerializeField] private GameObject[] shootingTargets;
  [SerializeField] private GameObject[] runningTargets;

  private DifficultyScaling diffScaleScript;
  private Transform playerVehicle;
  private float tileSize;
  private float distanceFromPlayer;
  private float heightIncStart;
  private float heightIncEnd;
  private LayerMask targetSpawnMask;
  private float[] offsets = {-0.5f, 0, 0.5f};

  void Awake() {
    diffScaleScript = GameObject.FindGameObjectWithTag("Manager").GetComponent<DifficultyScaling>();
    targetSpawnMask = spawnedLayer | groundLayer;
    playerVehicle = GameObject.FindGameObjectWithTag("Player").transform;
    distanceFromPlayer = Mathf.Abs(playerVehicle.position.z - transform.position.z);
    tileSize = GetComponent<Renderer>().bounds.size.x;
    if (distanceFromPlayer > tileSize) Destroy(meshCollider);
    GenerateTile(1, 1);
  }

  public void GenerateTile(float heightIncreaseStart, float heightIncreaseEnd) {
    Vector3[] meshVertices = meshFilter.mesh.vertices;
    int tileDepth = (int)Mathf.Sqrt(meshVertices.Length);
    int tileWidth = tileDepth;

    heightIncEnd = heightIncreaseStart;
    heightIncStart = heightIncreaseEnd;

    float offsetX = -gameObject.transform.position.x;
    float offsetZ = -gameObject.transform.position.z;
    float[,] heightMap = noiseMapGeneration.CellularNoiseJobs(tileDepth, tileWidth, mapScale, offsetX, offsetZ, waves, amplitude, frequency, seed);
    UpdateMeshVertices(heightMap);
    Texture2D tileTexture = BuildTexture(heightMap);
    tileRenderer.material.mainTexture = tileTexture;
    for (int n = 0; n < buildingCycles; n++) {
      if (distanceFromPlayer == tileSize * (buildingSpawnOffset + (n * 2))) GenerateStructures(1 + n);
    }
    if (distanceFromPlayer < buildingSpawnOffset * tileSize) GenerateBonus();
    if (distanceFromPlayer == tileSize * targetSpawnOffset) GenerateTargets(heightMap);
  }

  private TerrainType[] ChooseTerrainType(float distance) {
    for (int i = 0; i < terrainTypes.Length; i++) {
      if (distance < terrainTypes[i].distance) {
        return new TerrainType[] {terrainTypes[(i > 0) ? i - 1 : 0], terrainTypes[i]};
      }
    }
    return new TerrainType[] {terrainTypes[0], terrainTypes[0]};
  }

  private Texture2D BuildTexture(float[,] heightMap) {
    int tileDepth = heightMap.GetLength(0);
    int tileWidth = heightMap.GetLength(1);

    Color[] colorMap = new Color[tileDepth * tileWidth];
    
    Vector3[] meshVertices = meshFilter.mesh.vertices;
    for (int vertexIndex = 0; vertexIndex < meshVertices.Length; vertexIndex++) {
      int colorIndex = vertexIndex;
      float distance = meshVertices[vertexIndex].x + transform.position.x;

      TerrainType[] terrainType = ChooseTerrainType(distance);
      float terrainInterval = terrainType[1].distance - terrainType[0].distance;
      colorMap[colorIndex] = Color.Lerp(terrainType[0].color, terrainType[1].color, (distance % terrainInterval - (terrainInterval * 0.75f)) / (terrainInterval * 0.25f));
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

  private void GenerateStructures(float buildingScale) {
    foreach (Transform child in transform) {
      Destroy(child.gameObject);
    }
    Vector3[] meshVertices = meshFilter.mesh.vertices;
    TerrainType[] options = ChooseTerrainType(transform.position.x);
    float terrainInterval = options[1].distance - options[0].distance;
    for (int i = 0; i < Random.Range(buildingDensity, buildingDensity * 2); i++) {
      Vector3 vertex = meshVertices[Random.Range(0, meshVertices.Length)];
      Transform tmp = Instantiate(Random.value < ((transform.position.x % terrainInterval - (terrainInterval * 0.75f)) / (terrainInterval * 0.25f)) ? options[1].structureType : options[0].structureType, transform.position + vertex, Quaternion.identity, transform).transform;
      tmp.rotation = Quaternion.Euler(new Vector3(-90, Random.Range(0, 360), 0));
      tmp.localScale *= Random.Range(buildingScale * 0.6f, buildingScale * 1.4f);
    }
  }

  private void GenerateTargets(float[,] heightMap) {
    float targetCount = targetDensity;
    if (targetCount < 1) targetCount = (Random.value <= targetCount) ? 1 : 0;
    for (int i = 0; i < targetCount; i++) {
      RaycastHit hit;
      Physics.Raycast(transform.position + new Vector3(tileSize * offsets[Random.Range(0, 2)], 250, tileSize * offsets[Random.Range(0, 2)]), Vector3.down, out hit);
      Vector3 instPos;
      instPos = new Vector3(hit.point.x, Mathf.Floor(hit.point.y / 5) * 5, hit.point.z);
      if (instPos.y - hit.point.y < 7) {
        while (instPos.y - hit.point.y < 7) {
          instPos += new Vector3(0, 5, 0);
        }
      }
      Collider[] collidersOverlapped = new Collider[1];
      if (Physics.OverlapBoxNonAlloc(instPos, targets[0].GetComponent<Renderer>().bounds.size / 2, collidersOverlapped, Quaternion.identity, targetSpawnMask) == 0) {
        float randVal = Random.value;
        if (randVal < 0.1f * diffScaleScript.difficultyScale) {
          Instantiate(shootingTargets[(int)(Random.value * Mathf.Min(diffScaleScript.difficultyScale, targets.Length - 1))], instPos, Quaternion.identity, transform).GetComponent<HealthManager>()._maxHealth *= Mathf.Pow(1.05f, diffScaleScript.difficultyScale);
        }
        else if (randVal < 0.2f * diffScaleScript.difficultyScale) {
          GameObject tmp = Instantiate(runningTargets[(int)(Random.value * Mathf.Min(diffScaleScript.difficultyScale, targets.Length - 1))], instPos, Quaternion.identity, transform);
          tmp.GetComponent<HealthManager>()._maxHealth *= Mathf.Pow(1.05f, diffScaleScript.difficultyScale);
          tmp.GetComponent<TargetMovement>().intelligence = Random.Range(5f, 10f);
        }
        else {
          Instantiate(targets[(int)(Random.value * Mathf.Min(diffScaleScript.difficultyScale, targets.Length - 1))], instPos, Quaternion.identity, transform).GetComponent<HealthManager>()._maxHealth *= Mathf.Pow(1.05f, diffScaleScript.difficultyScale);
        }
      }
    }
  }

  private void GenerateBonus() {
    Vector3[] meshVertices = meshFilter.mesh.vertices;
    TerrainType[] options = ChooseTerrainType(transform.position.x);
        float terrainInterval = options[1].distance - options[0].distance;
    for (int i = 0; i < Random.Range(bonusDensity, bonusDensity * 2); i++) {
      Vector3 vertex = meshVertices[Random.Range(0, meshVertices.Length)];
      Instantiate(Random.value < ((transform.position.x % terrainInterval - (terrainInterval * 0.75f)) / (terrainInterval * 0.25f)) ? options[1].groundDeco : options[0].groundDeco, transform.position + vertex, Random.rotation, transform);
    }
  }
}
