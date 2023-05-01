using UnityEngine;

namespace Enemy {

	public class Waypoints : MonoBehaviour {
		public Vector3[] points;

		private GameObject parentObject;

		public Vector3 this[int index] {
			get { return points[index]; }
			set { points[index] = value; }
		}

		/// <summary>
		/// Returns the number of waypoints
		/// </summary>
		public int Length {
			get { return points.Length; }
		}

		public int GetNextWaypointIndex(int index, int moveDirection) {
			int nextIndex = index + moveDirection;
			if (nextIndex < points.Length) { 
				return nextIndex; 
			} else { 
				return 0;
			}
		}

		private void Start() {
			if (transform.parent.tag == "Room") parentObject = transform.parent.gameObject;
			else if (transform.parent.parent.tag == "Room") parentObject = transform.parent.parent.gameObject;

			MovePoints();
		}

		private void MovePoints() {
			for (int i = 0; i < points.Length; i++) {
				points[i] += parentObject.transform.position;
			}
		}
	}

}
