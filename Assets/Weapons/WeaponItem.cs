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
  [Header("WeaponEffects requires a empty GameObject named 'MuzzlePosition'\nin this prefab's heirarchy.")]
  public GameObject gunPrefab;
  [Header("This moves the WeaponContainer in WeaponRenderer. \nMove the WeaponContainer to help set this offset.")]
  public Vector3 gunOffset;
  public Vector3 gunRotationOffset;
  [Header("Description")]
  public string manufacturer;
  public string modelName;
  public string nickName;
  [Header("Fire Type")]
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
  [Header("[Optional]")]
  [Header("Burst Stats")]
  public int shotsPerBurst;
  public float secondsBetweenBurstShots;
  [Header("Charge Stats")]
  public float minShotDamage;
  public float chargeTimeSeconds;
  [Header("Animation")]
  [Header("Place the new animations in the AnimatiorOverride; \na template is provided in the Weapons/Animations folder.")]
  public AnimatorOverrideController animatorOverride;
  [Header("Sounds")]
  public AudioClip shotSound;
  public AudioClip shotSoundNearEmpty;
  public AudioClip emptyFireSound;
  public AudioClip reloadStartSound;
  public AudioClip reloadEndSound;
  public AudioClip equipSound;
  [Header("Effects & Particles")]
  [Header("Laser Effect")]
  public Vector3 effectPositionOffset;
  [ColorUsage(true, true)] public Color laserScaleUpColor; 
  [ColorUsage(true, true)] public Color laserScaleDownColor; 
  public float durationLaserScaleUp = 0.08f;
  public float durationLaserScaleDown = 0.1f;
  [ColorUsage(true, true)] public Color laserEmissionColor;
  [Header("Other Effects")]
  public GameObject muzzleFlashEffectPrefab;
  public GameObject hitEffectPrefab;
}
