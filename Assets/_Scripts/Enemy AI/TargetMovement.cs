using System.Collections;
using UnityEngine;

// Script that moves object along a path
// Requires lineCreator Component

[RequireComponent(typeof(LineCreator))]
public class TargetMovement : MonoBehaviour {
	
	LineCreator lineCreator;

	[SerializeField] private float targetSpeed;		
	[SerializeField] int indexOfStartingPoint;
	[SerializeField] private float timeToStop;


	// Useful variables for finding next object in list
	private int _targetPositionIndex;
	private Vector3 _targetPosition;

	private int _previousPositionIndex;
	private Vector3 _previousPosition;

	private void Awake() {
		lineCreator = GetComponent<LineCreator>();
	}

	private void Start() {
		// Sets Index variables
		_targetPositionIndex = indexOfStartingPoint;
		_previousPositionIndex = indexOfStartingPoint - 1;

		MoveTarget();

		transform.position = _previousPosition;
	}

	private void MoveTarget() {
		switch (lineCreator.lineType) {
			case LineCreator.LineType.Linear_Bounce: StartupCheck(true);  UpdateTargetPosition(1, true); StartCoroutine(LinearMoveBounce(1)); break; 
			case LineCreator.LineType.Linear_Loop: StartupCheck(true);  UpdateTargetPosition(1, true); StartCoroutine(LinearMoveLoop(1)); break;
			case LineCreator.LineType.Bezier_Bounce: StartupCheck(false); UpdateTargetPosition(1, false); StartCoroutine(BezierMoveBounce(1)); break;
			case LineCreator.LineType.Bezier_Loop: StartupCheck(false); UpdateTargetPosition(1, false); StartCoroutine(BezierMoveLoop(1)); break;
			case LineCreator.LineType.Stationary: break;
		}
	}

	// Moves character along Linear Lines
	private IEnumerator LinearMoveBounce(int moveDirection) {

		float _localTime = 0;
		
		// Continue until reaching next _targetPosition
		while (Vector3.Distance(transform.position, _targetPosition) > 0.01f) {

			_localTime += Time.deltaTime * targetSpeed;

			Vector3 targetPosition = Vector3.Lerp(_previousPosition, _targetPosition, _localTime / Vector3.Distance(_previousPosition, _targetPosition));

			transform.position = targetPosition;
			
			yield return null;
		}
		transform.position = _targetPosition; // For small corrections

		// If first or last index, wait for time, then reverse
		if (_targetPositionIndex == lineCreator.linearPoints.Count - 1 || _targetPositionIndex == 0) {
			yield return new WaitForSeconds(timeToStop);

			UpdateTargetPosition(-moveDirection, true);
			StartCoroutine(LinearMoveBounce(-moveDirection));
		} else {
			UpdateTargetPosition(moveDirection, true);
			StartCoroutine(LinearMoveBounce(moveDirection));
		}
	}

	// Moves character along Linear Lines
	private IEnumerator LinearMoveLoop(int moveDirection) {

		float _localTime = 0;

		// Continue until reaching next _targetPosition
		while (Vector3.Distance(transform.position, _targetPosition) > 0.1f) {
			_localTime += Time.deltaTime * targetSpeed;

			Vector3 targetPosition = Vector3.Lerp(_previousPosition, _targetPosition, _localTime / Vector3.Distance(_previousPosition, _targetPosition));

			transform.position = targetPosition;

			yield return null;
		}

		transform.position = _targetPosition; // For small corrections

		UpdateTargetPosition(moveDirection, true);
		StartCoroutine(LinearMoveLoop(moveDirection));
	}

	// Moves character along Bezier Lines
	private IEnumerator BezierMoveBounce(int moveDirection) {
		float _localTime = 0;

		// Points from lineCreator script
		Vector3 a = _previousPosition;
		Vector3 b;
		Vector3 c;
		Vector3 d = _targetPosition;

		// Dumb thing but I have a headache so a if/else statement will work fine
		if (moveDirection == 1) {
			b = lineCreator.bezierSwingPoints[_previousPositionIndex * 2];
			c = lineCreator.bezierSwingPoints[_previousPositionIndex * 2 + moveDirection];
		} else {
			b = lineCreator.bezierSwingPoints[_previousPositionIndex * 2 - moveDirection];
			c = lineCreator.bezierSwingPoints[_previousPositionIndex * 2 - 2 * moveDirection]	;
		}

		// Continue until reaching next _targetPosition
		while (Vector3.Distance(transform.position, _targetPosition) > 0.01f) {
			_localTime += Time.deltaTime * targetSpeed;

			float t = _localTime / Vector3.Distance(_previousPosition, _targetPosition);

			// The actual Bezier Line math
			Vector3 ab = Vector3.Lerp(a, b, t);
			Vector3 bc = Vector3.Lerp(b, c, t);
			Vector3 cd = Vector3.Lerp(c, d, t);
			Vector3 abc = Vector3.Lerp(ab, bc, t);
			Vector3 bcd = Vector3.Lerp(bc, cd, t);
			Vector3 abcd = Vector3.Lerp(abc, bcd, t);

			// Debug Lines to see what is actually happening (looks quite cool)
			// Debug.DrawLine(a, b);
			// Debug.DrawLine(b, c);
			// Debug.DrawLine(c, d);
			// Debug.DrawLine(ab, bc);
			// Debug.DrawLine(bc, cd);
			// Debug.DrawLine(abc, bcd);

			Vector3 targetPosition = abcd;

			transform.position = targetPosition;

			yield return null;
		}
		transform.position = _targetPosition; // Small corrections

		// If If first or last index, wait for time, the flip
		if (_targetPositionIndex == lineCreator.bezierAnchorPoints.Count - 1 || _targetPositionIndex == 0) {
			yield return new WaitForSeconds(0.5f);
			UpdateTargetPosition(-moveDirection, false);
			StartCoroutine(BezierMoveBounce(-moveDirection));
		} else {
			UpdateTargetPosition(moveDirection, false);
			StartCoroutine(BezierMoveBounce(moveDirection));
		}

	}

	// Move character along Bezier Lines
	private IEnumerator BezierMoveLoop(int moveDirection) {
		float _localTime = 0;

		// Points from lineCreator script
		Vector3 a = _previousPosition;
		Vector3 b = lineCreator.bezierSwingPoints[_previousPositionIndex * 2];
		Vector3 c = lineCreator.bezierSwingPoints[_previousPositionIndex * 2 + moveDirection];
		Vector3 d = _targetPosition;

		// Continue until reach next _targetLocation
		while (Vector3.Distance(transform.position, _targetPosition) > 0.01f) {
			_localTime += Time.deltaTime * targetSpeed;

			float t = _localTime / Vector3.Distance(_previousPosition, _targetPosition);

			// The actual Bezier Line math
			Vector3 ab = Vector3.Lerp(a, b, t);
			Vector3 bc = Vector3.Lerp(b, c, t);
			Vector3 cd = Vector3.Lerp(c, d, t);
			Vector3 abc = Vector3.Lerp(ab, bc, t);
			Vector3 bcd = Vector3.Lerp(bc, cd, t);
			Vector3 abcd = Vector3.Lerp(abc, bcd, t);

			// Debug Lines to see what is actually happening (looks quite cool)
			// Debug.DrawLine(a, b);
			// Debug.DrawLine(b, c);
			// Debug.DrawLine(c, d);
			// Debug.DrawLine(ab, bc);
			// Debug.DrawLine(bc, cd);
			// Debug.DrawLine(abc, bcd);

			Vector3 targetPosition = abcd;

			transform.position = targetPosition;

			yield return null;
		}
		transform.position = _targetPosition; // Small correction

		UpdateTargetPosition(moveDirection, false);
		StartCoroutine(BezierMoveLoop(moveDirection));
	}

	// Is used to update references to _targetPosition, _targetPositionIndex, _previousPosition, _previousPositionIndex
	// Takes in a value (1 or -1) to indicate which direction List should move
	// Bool isLinear is used to choose between lineCreator.linearPoints and lineCreator.bezierAnchorPoints
	private void UpdateTargetPosition(int value, bool isLinear) {
		_previousPositionIndex = _targetPositionIndex;
		_targetPositionIndex += value;

		if (isLinear) {
			if (_targetPositionIndex > lineCreator.linearPoints.Count - 1) _targetPositionIndex = 0;
			else if (_targetPositionIndex < 0) _targetPositionIndex = lineCreator.linearPoints.Count - 1;

			_targetPosition = lineCreator.linearPoints[_targetPositionIndex];
			_previousPosition = lineCreator.linearPoints[_previousPositionIndex];
		} else {
			if (_targetPositionIndex > lineCreator.bezierAnchorPoints.Count - 1) _targetPositionIndex = 0;
			else if (_targetPositionIndex < 0) _targetPositionIndex = lineCreator.bezierAnchorPoints.Count - 1;

			_targetPosition = lineCreator.bezierAnchorPoints[_targetPositionIndex];
			_previousPosition = lineCreator.bezierAnchorPoints[_previousPositionIndex];
		}
	}

	private void StartupCheck(bool isLinear) {
		if (isLinear) {
			if (lineCreator.linearPoints.Count <= 0) {

				Debug.LogError("Enemy " + gameObject.name + " LineCreator contains no accessable points. If you wish to have a stationary target, pick Stationary as Line Type."); ;
				this.enabled = false;

			}
		} else {
			if (lineCreator.bezierAnchorPoints.Count <= 0) {
				Debug.LogError("Enemy " + gameObject.name + " LineCreator contains no accessable points. If you wish to have a stationary target, pick Stationary as Line Type."); ;
				this.enabled = false;
			}
		}
	}
}
