using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraCosmetic : MonoBehaviour
{

  [SerializeField] private TextMeshProUGUI camID, timestamp;

  private void Awake() {
    camID.text = "Camera " + GetRandCamID();
  }

  private void Update() {
    timestamp.text = System.DateTime.UtcNow.ToString();
  }

  private string GetRandCamID() {
    string[] letters = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
    string returning = "";
    for (int i = 0; i < Random.Range(2, 4); i++) {
      returning += letters[Random.Range(0, letters.Length)];
    }
    for (int i = 0; i < Random.Range(4, 6); i++) {
      returning += Random.Range(0, 10).ToString();
    }
    return returning;
  }
}