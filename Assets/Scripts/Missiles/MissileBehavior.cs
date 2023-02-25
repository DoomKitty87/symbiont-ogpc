using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MissileBehavior : MonoBehaviour
{
  [SerializeField] private GameObject _missileExplodeParticlePrefab;
  [SerializeField] private float _damage;
  [SerializeField] private float _movementSpeed;
  private Transform _playerTransform;
  private Rigidbody _rigidBody;
  void Start() {
    // We gotta find a better way to do this 
    _playerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    _rigidBody = gameObject.GetComponent<Rigidbody>();
    if (_playerTransform == null){
      Debug.LogError("MissileBehavior: Player Transform is null!");
      Destroy(gameObject);
    }
    if (_missileExplodeParticlePrefab == null){
      Debug.LogError("MissileBehavior: Particle Prefab is null!");
      Destroy(gameObject);
    }
    Physics.IgnoreCollision(gameObject.GetComponent<MeshCollider>(), transform.parent.gameObject.GetComponent<MeshCollider>());
    transform.LookAt(_playerTransform);
  }
  void Update() {
    transform.LookAt(_playerTransform);
  }
  void FixedUpdate() {
    _rigidBody.AddForce(transform.forward * _movementSpeed);
  }
  void OnCollisionEnter(Collision collider) {
    if (!collider.gameObject.CompareTag("Player")) {
      return;
    }
    DamageGameObject(collider.gameObject);
    SpawnParticleEffects();
    Destroy(this.gameObject);
  }
  void DamageGameObject(GameObject obj) {
    HealthManager healthManager = obj.GetComponent<HealthManager>();
    healthManager.Damage(_damage);
  }
  void SpawnParticleEffects() {
    GameObject emitter = Instantiate(_missileExplodeParticlePrefab, transform.position, Quaternion.identity);
    emitter.GetComponent<ParticleSystem>().Play();
  }
}
