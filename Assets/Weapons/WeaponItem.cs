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
  // Since we're implementing weapon switching, there will need to be a way to 
  // get the current ammoCount from the equipped guns, otherwise players will
  // just switch weapons to get more ammo.
  public int magSize;
  public float reloadTimeSeconds;
  [Header("Camera Recoil")]
  public float verticalRecoil;
  public float horizontalRecoilDeviation;
  public float recoilSnapiness;
  public float recoilRecoverySpeed;
  public float zCameraShake;
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
  [Header("Sounds")]
  public AudioClip shotSound;
  public AudioClip shotSoundNearEmpty;
  public AudioClip emptyFireSound;
  public AudioClip reloadStartSound;
  public AudioClip reloadEndSound;
  public AudioClip equipSound;
  [Header("Effects & Particles")]
  [ColorUsage(true, true)] public Color shotColor;
  [SerializeField] public List<Attachment> attachments;
}
