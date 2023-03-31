using System.Collections.Generic;
using UnityEngine;

// Uses LineCreatorEditor script located in _Scripts\Editor folder
// So that it is not loaded into any builds

public class LineCreator : MonoBehaviour {

	public LineType lineType;
	public enum LineType {
		Linear_Bounce,
		Linear_Loop,
		Bezier_Bounce,
		Bezier_Loop,
		Stationary
	}

	// Variables realated to Editor Script
	// Must be public in order for custom inspector to work
	public GameObject referenceTransform;

	public bool unlockYMovement;

	public Color travelLineColor = Color.green;
	public Color swingLineColor = Color.black;
	public Color anchorHandleColor = Color.green;
	public Color swingHandleColor = Color.black;

	[Range(0, 1)] public float handleSize = 0.5f;
	[Range(0, 10)] public float lineWidth = 5f;

	public int numberOfLinearPoints;
	public List<Vector3> linearPoints;

	public int numberOfBezierAnchorPoints;
	public List<Vector3> bezierAnchorPoints;
	public List<Vector3> bezierSwingPoints;

	// Moves each point proportional to the Enemy's localPosition
	private void CheckPointsRelativePosition(LineType lineType) {
		switch (lineType) {
			case LineType.Linear_Bounce:
				for (int i = 0; i < linearPoints.Count; i++) {
					linearPoints[i] += transform.localPosition;
				}
				break;
			case LineType.Linear_Loop:
				for (int i = 0; i < linearPoints.Count; i++) {
					linearPoints[i] += transform.localPosition;
				}
				break;
			case LineType.Bezier_Bounce:
			for (int i = 0; i < bezierAnchorPoints.Count; i++) {
				bezierAnchorPoints[i] += transform.localPosition;
			}
			for (int i = 0; i < bezierSwingPoints.Count; i++) {
				bezierSwingPoints[i] += transform.localPosition;
			}
				break;
			case LineType.Bezier_Loop:
			for (int i = 0; i < bezierAnchorPoints.Count; i++) {
				bezierAnchorPoints[i] += transform.localPosition;
			}
			for (int i = 0; i < bezierSwingPoints.Count; i++) {
				bezierSwingPoints[i] += transform.localPosition;
			}
				break;
			case LineType.Stationary:
				break;
		}
	}

	// Switches on which LineType currently selected
	public void CreatePoints() {
		switch (lineType) {
			case LineType.Linear_Bounce:
				CreateLinearPoints();
				break;
			case LineType.Linear_Loop:
				CreateLinearPoints();
				break;
			case LineType.Bezier_Bounce:
				CreateBezierBouncePoints();
				break;
			case LineType.Bezier_Loop:
				CreateBezierLoopPoints();
				break;
			case LineType.Stationary:
				break;
		}
	}

	// Sets number of points in linearPoints equal to numberOfLinearPoints
	private void CreateLinearPoints() {
		if (linearPoints.Count < numberOfLinearPoints) {
			while (linearPoints.Count < numberOfLinearPoints) {
				if (linearPoints.Count != 0) linearPoints.Add(linearPoints[^1]);
				else linearPoints.Add(Vector3.zero);
			}
		} else if (linearPoints.Count > numberOfLinearPoints) {
			while (linearPoints.Count > numberOfLinearPoints) {
				linearPoints.RemoveAt(linearPoints.Count - 1);
			}
		}
	}

	// Sets number of points in bezierAnchorPoints equal to numberOfBezierAnchorPoints
	// And sets number of points in bezierSwingPoints
	private void CreateBezierBouncePoints() {
		// Handles bezierAnchorPoints
		if (bezierAnchorPoints.Count < numberOfBezierAnchorPoints) {
			while (bezierAnchorPoints.Count < numberOfBezierAnchorPoints) {
				if (bezierAnchorPoints.Count != 0) bezierAnchorPoints.Add(bezierAnchorPoints[^1]);
				else bezierAnchorPoints.Add(Vector3.zero);
			}
		} else if (bezierAnchorPoints.Count > numberOfBezierAnchorPoints) {
			while (bezierAnchorPoints.Count > numberOfBezierAnchorPoints) {
				bezierAnchorPoints.RemoveAt(bezierAnchorPoints.Count - 1);
			}
		}

		// Cleares bezierSwingPoints in there are no anchor points
		if (numberOfBezierAnchorPoints == 0 || numberOfBezierAnchorPoints == 1) {
			bezierSwingPoints.Clear();
			return;
		}
		int numberOfSwingPoints = (numberOfBezierAnchorPoints - 1) * 2;

		// Handles bezierSwingPoints
		if (bezierSwingPoints.Count < numberOfSwingPoints) {
			while (bezierSwingPoints.Count < numberOfSwingPoints) {
				if (bezierSwingPoints.Count != 0) bezierSwingPoints.Add(bezierAnchorPoints[(int)Mathf.Ceil(bezierSwingPoints.Count / 2)]);
				else bezierSwingPoints.Add(Vector3.zero);
			}
		} else if (bezierSwingPoints.Count > numberOfSwingPoints) {
			while (bezierSwingPoints.Count > numberOfSwingPoints) {
				bezierSwingPoints.RemoveAt(bezierSwingPoints.Count - 1);
			}
		}
	}

	// Sets number of points in bezierAnchorPoints equal to numberOfBezierAnchorPoints
	// And sets number of points in bezierSwingPoints
	private void CreateBezierLoopPoints() {
		// Handles bezierAnchorPoints
		if (bezierAnchorPoints.Count < numberOfBezierAnchorPoints) {
			while (bezierAnchorPoints.Count < numberOfBezierAnchorPoints) {
				if (bezierAnchorPoints.Count != 0) bezierAnchorPoints.Add(bezierAnchorPoints[^1]);
				else bezierAnchorPoints.Add(Vector3.zero);
			}
		} else if (bezierAnchorPoints.Count > numberOfBezierAnchorPoints) {
			while (bezierAnchorPoints.Count > numberOfBezierAnchorPoints) {
				bezierAnchorPoints.RemoveAt(bezierAnchorPoints.Count - 1);
			}
		}

		if (numberOfBezierAnchorPoints == 0 || numberOfBezierAnchorPoints == 1) {
			bezierSwingPoints.Clear();
			return;
		}

		int numberOfSwingPoints = (numberOfBezierAnchorPoints) * 2;

		// Cleares bezierSwingPoints in there are no anchor points
		if (bezierSwingPoints.Count < numberOfSwingPoints) {
			while (bezierSwingPoints.Count < numberOfSwingPoints) {
				if (bezierSwingPoints.Count != 0) bezierSwingPoints.Add(bezierAnchorPoints[(int)Mathf.Ceil(bezierSwingPoints.Count / 2)]);
				else bezierSwingPoints.Add(Vector3.zero);
			}
		} else if (bezierSwingPoints.Count > numberOfSwingPoints) {
			while (bezierSwingPoints.Count > numberOfSwingPoints) {
				bezierSwingPoints.RemoveAt(bezierSwingPoints.Count - 1);
			}
		}
	}
}