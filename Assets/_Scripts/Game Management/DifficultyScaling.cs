using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyScaling : MonoBehaviour
{

    private VehicleMovement vehicleScript;

    public float difficultyScale;

    void Start() {
        vehicleScript = GameObject.FindGameObjectWithTag("Player").GetComponent<VehicleMovement>();
    }

    void Update() {
        difficultyScale = vehicleScript.GetDistance() / 1000 + 1;
        print(difficultyScale);
    }
}
