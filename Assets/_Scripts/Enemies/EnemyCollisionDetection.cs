using Enemy;
using UnityEngine;

public class EnemyCollisionDetection : MonoBehaviour
{
  EnemyMovement _enemyMovement;

	private void Awake() {
		_enemyMovement = transform.parent.GetComponent<EnemyMovement>();
	}

	private void OnCollisionEnter(Collision collision) {
		_enemyMovement.CollisionDetected(collision);
	}
}
