using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.noise;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

[System.Serializable]
public struct Wave
{

  public float seed;
  public float frequency;
  public float amplitude;
}

public class NoiseMapGeneration : MonoBehaviour
{

  [BurstCompile]
  public struct CellularGenerationJob : IJobParallelFor
  {

    public int depth;
    public int width;
    public float scale;
    public float offsetX;
    public float offsetZ;
    public float waves;
    public float amplitude;
    public float frequency;
    public float seed;

    [WriteOnly]
    public NativeArray<float> Result;

    public void Execute(int index) {
      float sampleX = (index / width + offsetX) / scale;
      float sampleZ = (index % width + offsetZ) / scale;

      float normalization = 0f;
      float noise = 0f;

      
      for (int n = 0; n < waves; n++) {
        noise += amplitude * cellular(new Vector2(sampleX * frequency + seed, sampleZ * frequency + seed)).x;
        normalization += amplitude;
      }
      
      noise /= normalization;

      Result[index] = noise;
    }
  }

  public float[,] ConvertNativeToArray(Vector2 dimensions, NativeArray<float> jobResult) {
    float[,] output = new float[(int)dimensions.x, (int)dimensions.y];
    for (var y = 0; y < dimensions.y; y++) {
      for (var x = 0; x < dimensions.x; x++) {
        output[x, y] = jobResult[y * (int)dimensions.x + x];
      }
    }
    jobResult.Dispose();
    return output;
  }

  public float[,] CellularNoiseJobs(int depth, int width, float scale, float offsetX, float offsetZ, float waves, float amplitude, float frequency, float seed) {
    var jobResult = new NativeArray<float>(width * depth, Allocator.TempJob);

    var job = new CellularGenerationJob() {
      depth = depth,
      width = width,
      scale = scale,
      offsetX = offsetX,
      offsetZ = offsetZ,
      waves = waves,
      amplitude = amplitude,
      frequency = frequency,
      seed = seed,
      Result = jobResult
    };

    var handle = job.Schedule(jobResult.Length, 32);
    handle.Complete();
    return ConvertNativeToArray(new Vector2(depth, width), jobResult);
  }

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
