using Enemy;
using UnityEngine;

public class EnemyCollisionDetection : MonoBehaviour
{
  TargetMovement _targetMovement;

	private void Awake() {
		_targetMovement = transform.parent.GetComponent<TargetMovement>();
	}

	private void OnCollisionEnter(Collision collision) {
		_targetMovement.CollisionDetected(collision);
	}
}
