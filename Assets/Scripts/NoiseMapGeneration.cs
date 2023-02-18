using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.noise;

[System.Serializable]
public class Wave
{

  public float seed;
  public float frequency;
  public float amplitude;
}

public class NoiseMapGeneration : MonoBehaviour
{

  public float[,] GenerateCellularNoise(int depth, int width, float scale, float offsetX, float offsetZ, Wave[] waves) {
    float[,] noiseMap = new float[depth, width];

    for (int zIndex = 0; zIndex < depth; zIndex++) {
      for (int xIndex = 0; xIndex < width; xIndex++) {
        float sampleX = (xIndex + offsetX) / scale;
        float sampleZ = (zIndex + offsetZ) / scale;

        float noise = 0f;
        float normalization = 0f;
        foreach (Wave wave in waves) {
          noise += wave.amplitude * cellular(new Vector2(sampleX * wave.frequency + wave.seed, sampleZ * wave.frequency + wave.seed)).x;
          normalization += wave.amplitude;
        }
        noise /= normalization;

        noiseMap[zIndex, xIndex] = noise;
      }
    }
    return noiseMap;
  }

  public float[,] GeneratePerlinNoise(int depth, int width, float scale, float offsetX, float offsetZ, Wave[] waves) {
    float[,] noiseMap = new float[depth, width];

    for (int zIndex = 0; zIndex < depth; zIndex++) {
      for (int xIndex = 0; xIndex < width; xIndex++) {
        float sampleX = (xIndex + offsetX) / scale;
        float sampleZ = (zIndex + offsetZ) / scale;

        float noise = 0f;
        float normalization = 0f;
        foreach (Wave wave in waves) {
          noise += wave.amplitude * Mathf.PerlinNoise(sampleX * wave.frequency + wave.seed, sampleZ * wave.frequency + wave.seed);
          normalization += wave.amplitude;
        }
        noise /= normalization;

        noiseMap[zIndex, xIndex] = noise;
      }
    }
    return noiseMap;
  }
}
