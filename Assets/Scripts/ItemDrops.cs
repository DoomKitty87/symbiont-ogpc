using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrops : MonoBehaviour
{

  private string[] possibleDrops = new string[] {"Li-Ion Battery", "Accelerator"};
  private PersistentData dataContainer;

  void Start()
  {
      dataContainer = GameObject.FindGameObjectWithTag("Data").GetComponent<PersistentData>();
  }

  public void RollForItem() {
    if (Random.Range(1, 100) % 9 != 0) return;
    int drop = Random.Range(0, possibleDrops.Length);
    if (dataContainer.unlockedAttachments.Contains(drop)) return;
    dataContainer.unlockedAttachments.Add(drop);
  }
}
