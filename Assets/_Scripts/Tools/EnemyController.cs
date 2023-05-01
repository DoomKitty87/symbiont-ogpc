using Enemy;
using UnityEngine;
using UnityEngine.Assertions;

[SelectionBase]
[RequireComponent(typeof(Waypoints))]
public class EnemyController : MonoBehaviour {
	public Enemy.Waypoints waypoints;

	[SerializeField] private int _initialPoint;

	private int _waypointIndex;

	private void Awake() {
		Assert.IsNotNull(waypoints, "No waypoints assigned to EnemyController");
		Assert.IsTrue(_initialPoint < 0 || _initialPoint >= waypoints.points.Length, string.Format("<color=red>{0}</color> invalid _initalPoint value", _initialPoint));
	}

	private void Start() {
		_waypointIndex = waypoints.GetNextWaypointIndex(_initialPoint, 0);

		// TODO:
		// Make enemy start moving towards first waypoint
	}

	private void Update() {
		if (Vector3.Distance(gameObject.transform.position, waypoints[_waypointIndex]) <= 0.1f) {
			// TODO:
			// Start moving towards next waypoint
		}
	}
}