using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MissileBehavior : MonoBehaviour
{
  [SerializeField] private GameObject _missileExplodeParticlePrefab;
  [SerializeField] private float _damage;
  [SerializeField] private float _movementSpeed;
  private GameObject _player;
  private Transform _playerTransform;
  private Vector3 _lastPlayerPosition;

  private Rigidbody _rigidBody;


  void Start() {
    _player = GameObject.FindGameObjectWithTag("Player");
    _playerTransform = _player.transform;
    _lastPlayerPosition = _playerTransform.position;
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
    transform.LookAt(_playerTransform.position + (_playerTransform.position - _lastPlayerPosition));
  }

  void Update() {
    transform.LookAt(_playerTransform.position + (_playerTransform.position - _lastPlayerPosition));
    _lastPlayerPosition = _playerTransform.position;
  }

  void FixedUpdate() {
    _rigidBody.AddForce(transform.forward * _movementSpeed);
  }

  void OnCollisionEnter(Collision collider) {
    if (!collider.gameObject.CompareTag("Player")) {
      return;
    }
    collider.gameObject.GetComponent<HealthManager>().Damage(_damage);
    SpawnParticleEffects();
    Destroy(this.gameObject);
  }

  void SpawnParticleEffects() {
    GameObject emitter = Instantiate(_missileExplodeParticlePrefab, transform.position, Quaternion.identity);
    emitter.GetComponent<ParticleSystem>().Play();
  }
}
