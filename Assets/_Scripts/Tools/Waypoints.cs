using UnityEngine;

namespace Enemy {

	public class Waypoints : MonoBehaviour {
		public Vector3[] points;

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

			// TODO:
			// Code for detecting and dealing with collisions
		}
	}

}
