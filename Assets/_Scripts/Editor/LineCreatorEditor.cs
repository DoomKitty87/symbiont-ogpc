using UnityEditor;
using UnityEngine;

// Custom Editor of LineCreator
// Renderers paths

[CustomEditor(typeof(LineCreator))]
public class LineCreatorEditor : Editor
{

	private LineCreator _lineCreator;

	// References target script, required to make things work
	 private void OnEnable() {
		_lineCreator = (LineCreator)target;
			if (_lineCreator != null) {
				_lineCreator.CreatePoints();
			}

			if (_lineCreator.referenceTransform == null) _lineCreator.referenceTransform = _lineCreator.gameObject;
	 }

	// Creates all point and line handles shown in inspector
	public void OnSceneGUI() {

		EditorGUI.BeginChangeCheck();

		_lineCreator.CreatePoints();

		switch (_lineCreator.lineType) {
			case LineCreator.LineType.Linear_Bounce:
				DrawLinearPoints();
				DrawLinearBounce();
				break;
			case LineCreator.LineType.Linear_Loop:
				DrawLinearPoints();
				DrawLinearLoop();
				break;
			case LineCreator.LineType.Bezier_Bounce:
				DrawBezierPoints();
				DrawBezierBounce();
				break;
			case LineCreator.LineType.Bezier_Loop:
				DrawBezierPoints();
				DrawBezierLoop();
				break;
		}

		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(target, "Update location");
			PrefabUtility.RecordPrefabInstancePropertyModifications(_lineCreator);
		}
	}

	// The function that causes custom text to show in inspector
	public override void OnInspectorGUI() {
		serializedObject.Update();

		EditorGUI.BeginChangeCheck();

		if (_lineCreator.referenceTransform == null) _lineCreator.referenceTransform = _lineCreator.gameObject;

		LineCreator lineCreator = (LineCreator)target;
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("lineType"));
		EditorGUILayout.Space();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("referenceTransform"));

		switch (lineCreator.lineType) {
			case LineCreator.LineType.Linear_Bounce:
				lineCreator.numberOfLinearPoints = EditorGUILayout.DelayedIntField("Number of Points", lineCreator.numberOfLinearPoints);
				EditorGUILayout.HelpBox("Lowering this value will delete the last Vector3 in the list", MessageType.Warning);
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("linearPoints"));
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("handleSize"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("anchorHandleColor"));
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("lineWidth"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("travelLineColor"));
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("unlockYMovement"));
				break;

			case LineCreator.LineType.Linear_Loop:
				lineCreator.numberOfLinearPoints = EditorGUILayout.DelayedIntField("Number of Points", lineCreator.numberOfLinearPoints);
				EditorGUILayout.HelpBox("Lowering this value will delete the last Vector3 in the list", MessageType.Warning);
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("linearPoints"));
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("handleSize"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("anchorHandleColor"));
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("lineWidth"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("travelLineColor"));
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("unlockYMovement"));
				break;

			case LineCreator.LineType.Bezier_Bounce:
				lineCreator.numberOfBezierAnchorPoints = EditorGUILayout.DelayedIntField("Number of Points", lineCreator.numberOfBezierAnchorPoints);
				EditorGUILayout.HelpBox("Lowering this value will delete the last Vector3 in the list", MessageType.Warning);
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("bezierAnchorPoints"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("bezierSwingPoints"));
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("handleSize"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("anchorHandleColor"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("swingHandleColor"));
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("lineWidth"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("travelLineColor"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("swingLineColor"));
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("unlockYMovement"));
				break;
			case LineCreator.LineType.Bezier_Loop:
				lineCreator.numberOfBezierAnchorPoints = EditorGUILayout.DelayedIntField("Number of Points", lineCreator.numberOfBezierAnchorPoints);
				EditorGUILayout.HelpBox("Lowering this value will delete the last Vector3 in the list", MessageType.Warning);
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("bezierAnchorPoints"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("bezierSwingPoints"));
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("handleSize"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("anchorHandleColor"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("swingHandleColor"));
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("lineWidth"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("travelLineColor"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("swingLineColor"));
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("unlockYMovement"));
				break;
			case LineCreator.LineType.Stationary:
				EditorGUILayout.Space();
				break;
		}
		
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(target, "Update location");
			PrefabUtility.RecordPrefabInstancePropertyModifications(_lineCreator);
		}

		serializedObject.ApplyModifiedProperties();
	}

	// Used for both Linear_Bounce and Linear_Loop
	// Draws handles
	private void DrawLinearPoints() {
		LineCreator lineCreator = (LineCreator)target;

		for (int i = 0; i < lineCreator.numberOfLinearPoints; ++i) {

			if (i == lineCreator.gameObject.GetComponent<TargetMovement>().indexOfStartingPoint) {
				Handles.color = Color.black;
				Handles.CapFunction capFunction = Handles.CircleHandleCap;
			} else {
				Handles.color = lineCreator.anchorHandleColor;
				Handles.CapFunction capFunction = Handles.CircleHandleCap;
			}

			Vector3 handlesVisuals = Handles.FreeMoveHandle(lineCreator.linearPoints[i] + lineCreator.referenceTransform.transform.position, Quaternion.identity, lineCreator.handleSize, Vector3.zero, Handles.CircleHandleCap);
			Vector3 newLocation = handlesVisuals - lineCreator.referenceTransform.transform.position;

			if (!lineCreator.unlockYMovement) lineCreator.linearPoints[i] = new Vector3(newLocation.x, lineCreator.linearPoints[i].y, newLocation.z);
			else lineCreator.linearPoints[i] = newLocation;
		}
	}

	// Used for both Bezier_Bounce and Bezier_Loop
	// Draws handles
	private void DrawBezierPoints() {
		LineCreator lineCreator = (LineCreator)target;

		// Draws Bezier Anchor Points
		for (int i = 0; i < lineCreator.numberOfBezierAnchorPoints; ++i) {

			Handles.color = lineCreator.anchorHandleColor;
			Vector3 handlesVisuals = Handles.FreeMoveHandle(lineCreator.bezierAnchorPoints[i] + lineCreator.referenceTransform.transform.position, Quaternion.identity, lineCreator.handleSize, Vector3.zero, Handles.CircleHandleCap);
			Vector3 newLocation = handlesVisuals - lineCreator.referenceTransform.transform.position;

			if (!lineCreator.unlockYMovement) lineCreator.bezierAnchorPoints[i] = new Vector3(newLocation.x, lineCreator.bezierAnchorPoints[i].y, newLocation.z);
			else lineCreator.bezierAnchorPoints[i] = newLocation;

		}

		// Draws Bezier Swing Points
		Handles.color = lineCreator.swingHandleColor;
		for (int i = 0; i < lineCreator.bezierSwingPoints.Count; ++i) {

			Vector3 handlesVisuals = Handles.FreeMoveHandle(lineCreator.bezierSwingPoints[i] + lineCreator.referenceTransform.transform.position, Quaternion.identity, lineCreator.handleSize, Vector3.zero, Handles.CircleHandleCap);
			Vector3 newLocation = handlesVisuals - lineCreator.referenceTransform.transform.position;

			if (!lineCreator.unlockYMovement) lineCreator.bezierSwingPoints[i] = new Vector3(newLocation.x, lineCreator.bezierSwingPoints[i].y, newLocation.z);
			else lineCreator.bezierSwingPoints[i] = newLocation;
		}
	}

	// Connects each element in linearPoints with a line EXECEPT first and last element
	private void DrawLinearBounce() {
		LineCreator lineCreator = (LineCreator)target;

		Handles.color = lineCreator.travelLineColor;
		for (int i = 0; i < lineCreator.numberOfLinearPoints; ++i) {
			if (i != 0) {
				Handles.DrawLine(lineCreator.linearPoints[i] + lineCreator.referenceTransform.transform.position, lineCreator.linearPoints[i - 1] + lineCreator.referenceTransform.transform.position, lineCreator.lineWidth);
			}
		}
	}

	// Connects each element in linearPoints with a line INCLUDING first and last element
	private void DrawLinearLoop() {
		LineCreator lineCreator = (LineCreator)target;

		Handles.color = lineCreator.travelLineColor;
		for (int i = 0; i < lineCreator.numberOfLinearPoints; ++i) {
			if (i != 0) {
				Handles.DrawLine(lineCreator.linearPoints[i] + lineCreator.referenceTransform.transform.position, lineCreator.linearPoints[i - 1] + lineCreator.referenceTransform.transform.position, lineCreator.lineWidth);
			} else {
				Handles.DrawLine(lineCreator.linearPoints[i] + lineCreator.referenceTransform.transform.position, lineCreator.linearPoints[^1] + lineCreator.referenceTransform.transform.position, lineCreator.lineWidth);
			}
		}
	}

	// Draws all lines assosiated with Bezier Bounce
	private void DrawBezierBounce() {
		LineCreator lineCreator = (LineCreator)target;

		// Draws straight lines indicating order
		Handles.color = lineCreator.swingLineColor;
		for (int i = 1; i < lineCreator.numberOfBezierAnchorPoints; i++) {
			Handles.DrawLine(lineCreator.bezierAnchorPoints[i] + lineCreator.referenceTransform.transform.position, lineCreator.bezierSwingPoints[(i * 2) - 1] + lineCreator.referenceTransform.transform.position, lineCreator.lineWidth);
			Handles.DrawLine(lineCreator.bezierSwingPoints[(i * 2) - 1] + lineCreator.referenceTransform.transform.position, lineCreator.bezierSwingPoints[(i * 2) - 2] + lineCreator.referenceTransform.transform.position, lineCreator.lineWidth);
			Handles.DrawLine(lineCreator.bezierSwingPoints[(i * 2) - 2] + lineCreator.referenceTransform.transform.position, lineCreator.bezierAnchorPoints[i - 1] + lineCreator.referenceTransform.transform.position, lineCreator.lineWidth);
		}

		// Draws bezier lines from each anchor point in respect to swing points
		Handles.color = lineCreator.travelLineColor;
		for (int i = 1; i < lineCreator.numberOfBezierAnchorPoints; i++) {
			Handles.DrawBezier(lineCreator.bezierAnchorPoints[i] + lineCreator.referenceTransform.transform.position, lineCreator.bezierAnchorPoints[i - 1] + lineCreator.referenceTransform.transform.position, lineCreator.bezierSwingPoints[(i * 2) - 1] + lineCreator.referenceTransform.transform.position, lineCreator.bezierSwingPoints[(i * 2) - 2] + lineCreator.referenceTransform.transform.position, lineCreator.travelLineColor, null, lineCreator.lineWidth);
		}
	}

	// Draws all lines assosiated with Bezier Loop
	private void DrawBezierLoop() {
		LineCreator lineCreator = (LineCreator)target;

		// Draws straight lines indicating order
		Handles.color = lineCreator.swingLineColor;
		for (int i = 0; i < lineCreator.numberOfBezierAnchorPoints; i++) {
			if (lineCreator.numberOfBezierAnchorPoints == 0 || lineCreator.numberOfBezierAnchorPoints == 1) return;
			if (i == 0) {
				Handles.DrawLine(lineCreator.bezierAnchorPoints[i] + lineCreator.referenceTransform.transform.position, lineCreator.bezierSwingPoints[^1] + lineCreator.referenceTransform.transform.position, lineCreator.lineWidth);
				Handles.DrawLine(lineCreator.bezierSwingPoints[^1] + lineCreator.referenceTransform.transform.position, lineCreator.bezierSwingPoints[^2] + lineCreator.referenceTransform.transform.position, lineCreator.lineWidth);
				Handles.DrawLine(lineCreator.bezierSwingPoints[^2] + lineCreator.referenceTransform.transform.position, lineCreator.bezierAnchorPoints[^1] + lineCreator.referenceTransform.transform.position, lineCreator.lineWidth);
			} else {
				Handles.DrawLine(lineCreator.bezierAnchorPoints[i] + lineCreator.referenceTransform.transform.position, lineCreator.bezierSwingPoints[(i * 2) - 1] + lineCreator.referenceTransform.transform.position, lineCreator.lineWidth);
				Handles.DrawLine(lineCreator.bezierSwingPoints[(i * 2) - 1] + lineCreator.referenceTransform.transform.position, lineCreator.bezierSwingPoints[(i * 2) - 2] + lineCreator.referenceTransform.transform.position, lineCreator.lineWidth);
				Handles.DrawLine(lineCreator.bezierSwingPoints[(i * 2) - 2] + lineCreator.referenceTransform.transform.position, lineCreator.bezierAnchorPoints[i - 1] + lineCreator.referenceTransform.transform.position, lineCreator.lineWidth);
			}
		}

		// Draws bezier lines from each anchor point in respect to swing points
		for (int i = 0; i < lineCreator.numberOfBezierAnchorPoints; i++) {
			if (i == 0) Handles.DrawBezier(lineCreator.bezierAnchorPoints[i] + lineCreator.referenceTransform.transform.position, lineCreator.bezierAnchorPoints[^1] + lineCreator.referenceTransform.transform.position, lineCreator.bezierSwingPoints[^1] + lineCreator.referenceTransform.transform.position, lineCreator.bezierSwingPoints[^2] + lineCreator.referenceTransform.transform.position, lineCreator.travelLineColor, null, lineCreator.lineWidth);
			else Handles.DrawBezier(lineCreator.bezierAnchorPoints[i] + lineCreator.referenceTransform.transform.position, lineCreator.bezierAnchorPoints[i - 1] + lineCreator.referenceTransform.transform.position, lineCreator.bezierSwingPoints[(i * 2) - 1] + lineCreator.referenceTransform.transform.position, lineCreator.bezierSwingPoints[(i * 2) - 2] + lineCreator.referenceTransform.transform.position, lineCreator.travelLineColor, null, lineCreator.lineWidth);
		}
	}
}