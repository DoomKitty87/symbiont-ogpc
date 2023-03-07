using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Scriptable Objects/Weapon Item")]
public class WeaponItem : ScriptableObject
{
  [Header("Identification")]
  public new string name; // Throws error: 'WeaponItem.name' hides inherited member 'Object.name'. Use the new keyword if hiding was intended.
  public int id;
  [Header("Model")]
  [SerializeField] public GameObject gunPrefab;
  [SerializeField] public Vector3 gunOffset;
  [Header("Description")]
  public string manufacturer;
  public string modelName;
  public string nickName;
  [Header("Type")]
  public FireType fireType;
  public enum FireType {
    SemiAuto,
    Burst,
    FullAuto,
    Charge,
  }
  [Header("Global Stats")]
  public float maxShotDamage;
  public float fireDelaySeconds;
  public int magSize;
  public float reloadTimeSeconds;
  [Header("Vertical Recoil")]
  public float verticalRecoilDegrees;
  public float verticalRecoilRecovery;
  public float backRecoil;
  [Header("Shot Spread")]
  public float minShotSpreadDegrees;
  public float maxShotSpreadDegrees;
  public float shotSpreadFireInaccuracyDegrees;
  public float shotSpreadRecovery;
  [Header("Burst Stats")]
  public int shotsPerBurst;
  public float secondsBetweenBurstShots;
  [Header("Charge Stats")]
  public float minShotDamage;
  public float chargeTimeSeconds;
  [Header("Effects")]
  public bool upRecoilAnim;
  [ColorUsage(true, true)] public Color shotColor;
  [SerializeField] public List<Attachment> attachments;
}
