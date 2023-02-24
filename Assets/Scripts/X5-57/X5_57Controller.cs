using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class X5_57Controller : MonoBehaviour
{


  // public GameObject player;
  // public float maxCharge;
  // public float maxRange;
  // public float shieldMaxHealth;
  // public float speed;
  // public float attackPower;
  // public float attackRate;
  // public float healPower;
  // public float healRate;
  // public float currentHealthTankAmount;
  // public float currentChargeAmount; 
  // public string currentMode;
  // public GameObject focusedTarget;

  // ParticleSystem.MainModule main = explodeFX.GetComponent<ParticleSystem>().main;
  // ParticleSystem.ShapeModule sh = explodeFX.GetComponent<ParticleSystem>().shape;

  // void Update() {
  //   switch (currentMode) {
  //     case "Attack":
  //       AttackMode();
  //       break;
  //     case "Heal":
  //       HealMode();
  //       break;
  //     case "ConservePower":
  //       ConservePowerMode();
  //       break;
  //     case "ProtectOffensive":
  //       ProtectModeOffensive();
  //       break;
  //     case "ProtectDefensive":
  //       ProtectModeDefensive();
  //       break;
  //   }
  // }

  // void ChangeMode(string selectedMode) {
  //   currentMode = selectedMode;
  // }

  // void AttackMode() {
  //   // this mode has the bot shoot at the closest targets/turrets/etc. 
  //   if (!focusedTarget) {
  //     float distance;
  //     float bestDistance = Mathf.Infinity;
  //     GameObject bestTarget;
  //     Collider[] targets = Physics.OverlapSphere(transform.position, maxRange);
  //     if (!targets.Any()) {
  //       MoveToTarget(player, 2, 2, 2);
  //       return;
  //     } else {
  //       foreach (Collider col in targets) {
  //         distance = Vector3.Distance(transform.position, col.transform.position);
  //         if (col.gameObject.tag == "Target" && distance < bestDistance) {
  //           bestTarget = col.gameObject;
  //           bestDistance = distance;
  //         }
  //       }
  //     }
  //   }
  //   TurnToObject(focusedTarget);
  //   RaycastHit hit;
  //   if (Physics.Raycast(transform.position, transform.forward, out hit)) {
  //     if (hit.collider.gameObject.CompareTag("Target")) HitTarget(hit);
  //   }
  // }

  // void HealMode() {
  //   // this mode gives the player health periodically.
  // }

  // void ConservePowerMode() {
  //   // this mode has the bot get as far away as possible in order to stay alive.
  // }

  // void ProtectModeOffensive() {
  //   // this mode has the bot shoot down missiles coming the player's direction.
  // }

  // void ProtectModeDefensive() {
  //   // this mode has the bot block the player with a shield.
  // }

  // private IEnumerator TurnToObject(GameObject focus) {
  //   float elapsedTime = 0f;
  //   float waitTime = 1f;      
  //   Vector3 relativePos = focus.transform.position - transform.position;
  //   Quaternion toRotation = Quaternion.LookRotation(relativePos);
  //   while (elapsedTime < waitTime) {
  //     transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, elapsedTime / waitTime);
  //     elapsedTime += Time.deltaTime;
  //     yield return null;
  //   }
  // }

  // private IEnumerator MoveToTarget(GameObject focus, int proxX, int proxY, int proxZ) {
  //   float elapsedTime = 0f;
  //   float waitTime = 1f;      
  //   Vector3 targetLocation = new Vector3(focus.transform.position.x + proxX, player.transform.position.y + proxY, player.transform.position.z + proxZ);
  //   while (elapsedTime < waitTime) {
  //     transform.position = Vector3.Lerp(transform.position, targetLocation, elapsedTime / waitTime);
  //     elapsedTime += Time.deltaTime;
  //     yield return null;
  //   }
  // }

  // public void HitTarget(RaycastHit hit) {
  //   hit.collider.gameObject.GetComponent<HealthManager>().Damage(attackPower);
  //   if (hit.collider.gameObject.GetComponent<HealthManager>()._currentHealth >= 0) {
  //     return;
  //   }
  //   player.GetComponent<ScoreTracker>().DestroyedTarget(hit.collider.gameObject);
  //   StartCoroutine(ExplodeTarget(hit.collider.gameObject));
  //   focusedTarget = null;
  //   MoveToTarget(player, 2, 2, 2);
  // }

  // private IEnumerator ExplodeTarget(GameObject target) {
  //   float timer = 0f;
  //   float explodeTime = 0.12f;
  //   MeshRenderer targetMesh = target.GetComponent<MeshRenderer>();
  //   Vector3 initScale = target.transform.localScale;
  //   while (timer < explodeTime) {
  //     target.transform.localScale = Vector3.Lerp(initScale, new Vector3(0, 0, 0), Mathf.SmoothStep(0f, 1f, timer / explodeTime));
  //     timer += Time.deltaTime;
  //     yield return null;
  //   }
  //   sh.mesh = target.GetComponent<MeshFilter>().mesh;
  //   sh.scale = target.transform.localScale;
  //   main.startSizeMultiplier = 0.25f;
  //   explodeFX.transform.position = target.transform.position;
  //   fragmentFX.transform.position = explodeFX.transform.position;
  //   explodeFX.GetComponent<ParticleSystem>().Play();
  //   fragmentFX.GetComponent<ParticleSystem>().Play();
  //   GetComponent<ItemDrops>().RollForItem();
  //   GameObject tmp = Instantiate(explosionAudioPrefab, target.transform.position, Quaternion.identity);
  //   Destroy(target);
  //   tmp.GetComponent<AudioSource>().Play();
  //   yield return new WaitForSeconds(tmp.GetComponent<AudioSource>().clip.length);
  //   Destroy(tmp);
  // }
}