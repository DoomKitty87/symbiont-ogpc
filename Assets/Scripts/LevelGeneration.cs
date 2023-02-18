using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelGeneration : MonoBehaviour
{
  
  //Both values are in Map Tiles
  [SerializeField] private int mapWidth, mapDepth;
  [SerializeField] private GameObject tilePrefab;

  void Start() {
    GenerateMap();
  }

  void GenerateMap() {
    Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
    int tileWidth = (int)tileSize.x;
    int tileDepth = (int)tileSize.z;

    for (int xTileIndex = 0; xTileIndex < mapWidth; xTileIndex++) {
      for (int zTileIndex = 0; zTileIndex < mapDepth; zTileIndex++) {
        Vector3 tilePosition = new Vector3(transform.position.x + xTileIndex * tileWidth,
          transform.position.y, transform.position.z + zTileIndex * tileDepth);

        GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
      }
    }
  }
}
