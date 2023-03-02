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
  [Header("Stats")]
  public float fireRate;
  public float shotDamage;
  public int magSize;
  public float reloadTime;
  [Header("Recoil")]
  public float upRecoil;
  public float backRecoil;
  public float recoilRecovery;
  public float shotSpread;
  [Header("Effects")]
  public bool upRecoilAnim;
  public int shotColor;
  [SerializeField] public List<Attachment> attachments;
}
