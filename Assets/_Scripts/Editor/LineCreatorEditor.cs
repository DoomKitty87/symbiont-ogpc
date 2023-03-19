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
	 }

	// Creates all point and line handles shown in inspector
	public void OnSceneGUI() {
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
	}

	// The function that causes custom text to show in inspector
	public override void OnInspectorGUI() {
		serializedObject.Update();

		LineCreator lineCreator = (LineCreator)target;
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("lineType"));
		EditorGUILayout.Space();

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
				break;
		}

		EditorGUILayout.PropertyField(serializedObject.FindProperty("lockYMovement"));

		serializedObject.ApplyModifiedProperties();
	}

	// Used for both Linear_Bounce and Linear_Loop
	// Draws handles
	private void DrawLinearPoints() {
		LineCreator lineCreator = (LineCreator)target;

		for (int i = 0; i < lineCreator.numberOfLinearPoints; ++i) {

			EditorGUI.BeginChangeCheck();

			Handles.color = lineCreator.anchorHandleColor;
			Vector3 newLocation = Handles.FreeMoveHandle(lineCreator.linearPoints[i], Quaternion.identity, lineCreator.handleSize, Vector3.zero, Handles.CircleHandleCap);

			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(lineCreator, "Update location");
				if (lineCreator.lockYMovement) lineCreator.linearPoints[i] = new Vector3(newLocation.x, lineCreator.linearPoints[i].y, newLocation.z);
				else lineCreator.linearPoints[i] = newLocation;
			}
		}
	}

	// Used for both Bezier_Bounce and Bezier_Loop
	// Draws handles
	private void DrawBezierPoints() {
		LineCreator lineCreator = (LineCreator)target;

		// Draws Bezier Anchor Points
		for (int i = 0; i < lineCreator.numberOfBezierAnchorPoints; ++i) {

			EditorGUI.BeginChangeCheck();

			Handles.color = lineCreator.anchorHandleColor;
			Vector3 newLocation = Handles.FreeMoveHandle(lineCreator.bezierAnchorPoints[i], Quaternion.identity, lineCreator.handleSize, Vector3.zero, Handles.CircleHandleCap);

			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(lineCreator, "Update location");
				if (lineCreator.lockYMovement) lineCreator.bezierAnchorPoints[i] = new Vector3(newLocation.x, lineCreator.bezierAnchorPoints[i].y, newLocation.z);
				else lineCreator.bezierAnchorPoints[i] = newLocation;
			}
		}

		// Draws Bezier Swing Points
		Handles.color = lineCreator.swingHandleColor;
		for (int i = 0; i < lineCreator.bezierSwingPoints.Count; ++i) {

			EditorGUI.BeginChangeCheck();

			Vector3 newLocation = Handles.FreeMoveHandle(lineCreator.bezierSwingPoints[i], Quaternion.identity, lineCreator.handleSize, Vector3.zero, Handles.CircleHandleCap);

			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(lineCreator, "Update location");
				if (lineCreator.lockYMovement) lineCreator.bezierSwingPoints[i] = new Vector3(newLocation.x, lineCreator.bezierSwingPoints[i].y, newLocation.z);
				else lineCreator.bezierSwingPoints[i] = newLocation;
			}
		}
	}

	// Connects each element in linearPoints with a line EXECEPT first and last element
	private void DrawLinearBounce() {
		LineCreator lineCreator = (LineCreator)target;

		Handles.color = lineCreator.travelLineColor;
		for (int i = 0; i < lineCreator.numberOfLinearPoints; ++i) {
			if (i != 0) {
				Handles.DrawLine(lineCreator.linearPoints[i], lineCreator.linearPoints[i - 1], lineCreator.lineWidth);
			}
		}
	}

	// Connects each element in linearPOints with a line INCLUDING first and last element
	private void DrawLinearLoop() {
		LineCreator lineCreator = (LineCreator)target;

		Handles.color = lineCreator.travelLineColor;
		for (int i = 0; i < lineCreator.numberOfLinearPoints; ++i) {
			if (i != 0) {
				Handles.DrawLine(lineCreator.linearPoints[i], lineCreator.linearPoints[i - 1], lineCreator.lineWidth);
			} else {
				Handles.DrawLine(lineCreator.linearPoints[i], lineCreator.linearPoints[^1], lineCreator.lineWidth);
			}
		}
	}

	// Draws all lines assosiated with Bezier Bounce
	private void DrawBezierBounce() {
		LineCreator lineCreator = (LineCreator)target;

		// Draws straight lines indicating order
		Handles.color = lineCreator.swingLineColor;
		for (int i = 1; i < lineCreator.numberOfBezierAnchorPoints; i++) {
			Handles.DrawLine(lineCreator.bezierAnchorPoints[i], lineCreator.bezierSwingPoints[(i * 2) - 1], lineCreator.lineWidth);
			Handles.DrawLine(lineCreator.bezierSwingPoints[(i * 2) - 1], lineCreator.bezierSwingPoints[(i * 2) - 2], lineCreator.lineWidth);
			Handles.DrawLine(lineCreator.bezierSwingPoints[(i * 2) - 2], lineCreator.bezierAnchorPoints[i - 1], lineCreator.lineWidth);
		}

		// Draws bezier lines from each anchor point in respect to swing points
		Handles.color = lineCreator.travelLineColor;
		for (int i = 1; i < lineCreator.numberOfBezierAnchorPoints; i++) {
			Handles.DrawBezier(lineCreator.bezierAnchorPoints[i], lineCreator.bezierAnchorPoints[i - 1], lineCreator.bezierSwingPoints[(i * 2) - 1], lineCreator.bezierSwingPoints[(i * 2) - 2], lineCreator.travelLineColor, null, lineCreator.lineWidth);
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
				Handles.DrawLine(lineCreator.bezierAnchorPoints[i], lineCreator.bezierSwingPoints[^1], lineCreator.lineWidth);
				Handles.DrawLine(lineCreator.bezierSwingPoints[^1], lineCreator.bezierSwingPoints[^2], lineCreator.lineWidth);
				Handles.DrawLine(lineCreator.bezierSwingPoints[^2], lineCreator.bezierAnchorPoints[^1], lineCreator.lineWidth);
			} else {
				Handles.DrawLine(lineCreator.bezierAnchorPoints[i], lineCreator.bezierSwingPoints[(i * 2) - 1], lineCreator.lineWidth);
				Handles.DrawLine(lineCreator.bezierSwingPoints[(i * 2) - 1], lineCreator.bezierSwingPoints[(i * 2) - 2], lineCreator.lineWidth);
				Handles.DrawLine(lineCreator.bezierSwingPoints[(i * 2) - 2], lineCreator.bezierAnchorPoints[i - 1], lineCreator.lineWidth);
			}
		}

		// Draws bezier lines from each anchor point in respect to swing points
		for (int i = 0; i < lineCreator.numberOfBezierAnchorPoints; i++) {
			if (i == 0) Handles.DrawBezier(lineCreator.bezierAnchorPoints[i], lineCreator.bezierAnchorPoints[^1], lineCreator.bezierSwingPoints[^1], lineCreator.bezierSwingPoints[^2], lineCreator.travelLineColor, null, lineCreator.lineWidth);
			else Handles.DrawBezier(lineCreator.bezierAnchorPoints[i], lineCreator.bezierAnchorPoints[i - 1], lineCreator.bezierSwingPoints[(i * 2) - 1], lineCreator.bezierSwingPoints[(i * 2) - 2], lineCreator.travelLineColor, null, lineCreator.lineWidth);
		}
	}
}