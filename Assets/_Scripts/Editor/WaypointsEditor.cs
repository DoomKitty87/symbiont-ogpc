using UnityEngine;
using UnityEditor;

// Stolen gratefully from https://www.youtube.com/watch?v=MV4dMYhoV8c

namespace Enemy.Editor {
	[CustomEditor(typeof(Waypoints))]
	public class WayPointsEditor : UnityEditor.Editor {
		#region Fields
		private readonly float _dashSize = 4f;
		private Waypoints _waypoints;
		private EnemyMovement _enemyMovement;

		private int[] _segmentIndices;
		private Vector3[] _lines;
		private SerializedProperty _pointsProperty;

		private Color _pointOnLineColor = Color.green;
		private GUISkin _inspectorSkin;
		private GUISkin _sceneSkin;
		#endregion Fields

		private void OnEnable() {
			_inspectorSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
			_sceneSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
			Tools.hidden = true;

			_waypoints = (Waypoints)target;
			_pointsProperty = serializedObject.FindProperty("points");

			_enemyMovement = _waypoints.GetComponent<EnemyMovement>();

			CreateSegments();
			CreateLines();
		}

		private void OnDisable() {
			Tools.hidden = false; 
		}

		public override void OnInspectorGUI() {
			Event currentEvent = Event.current;

			GUILayout.Space(10f);

			if (_waypoints.Length == 0) {
				if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Plus"), GUILayout.ExpandWidth(true))) {
					_pointsProperty.InsertArrayElementAtIndex(0);
					serializedObject.ApplyModifiedProperties();
					_waypoints[0] = SceneView.lastActiveSceneView.camera.transform.position +
						(SceneView.lastActiveSceneView.camera.transform.forward * 1.5f);
					CreateSegments();
					CreateLines();
				}
			}

			for (int i = 0; i < _waypoints.Length; i++) {
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button(i.ToString(), _inspectorSkin.button, GUILayout.ExpandWidth(false))) {
					SceneView.lastActiveSceneView.LookAt(_waypoints[i]);
				}

				EditorGUI.BeginChangeCheck();
				Vector3 value = EditorGUILayout.Vector3Field(string.Empty, _waypoints[i], GUILayout.ExpandWidth(true));
				if (EditorGUI.EndChangeCheck()) {
					_waypoints[i] = value;
					Undo.RecordObject(_waypoints, "Moved Waypoint.");
					SceneView.RepaintAll();
					EditorUtility.SetDirty(_waypoints);
				}

				if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Plus"), GUILayout.ExpandWidth(false))) {
					_pointsProperty.InsertArrayElementAtIndex(i + 1);
					serializedObject.ApplyModifiedProperties();

					Vector3 midPoint;
					if (_waypoints.Length >= 2 && i + 1 > _waypoints.Length - 1) {
						Vector3 direction = (_waypoints[i] - _waypoints[i - 1]).normalized;
						midPoint = _waypoints[i] + direction;
					} else if (i + 1 >= _waypoints.Length - 1 && i != 0) {
						Vector3 previousPointDifference = _waypoints[i] - _waypoints[i - 1];
						midPoint = _waypoints[i] + previousPointDifference.normalized;
					} else if (i + 1 >= _waypoints.Length - 1) {
						midPoint = _waypoints[i] + Vector3.right;
					} else {
						midPoint = (_waypoints[i] + _waypoints[i + 1]) * 0.5f;
					}
					_waypoints[i + 1] = midPoint;
					CreateSegments();
					CreateLines();
				}

				if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Minus"), GUILayout.ExpandWidth(false))) {
					_pointsProperty.DeleteArrayElementAtIndex(i);
					serializedObject.ApplyModifiedProperties();
					CreateSegments();
					CreateLines();
					i--;
				}
				EditorGUILayout.EndHorizontal();
			}

			GUILayout.Space(10f);

			if (_waypoints.Length >= 2) {
				if (_waypoints.Length >= 2) {
					GUILayout.Label("Hold Ctrl to add intermediate points");
				}
				GUILayout.Space(10f);
			}


			if (_waypoints.Length > 0) {
				if (currentEvent.modifiers != EventModifiers.Shift) {
					GUILayout.Label("Hold Shift to clear points");
				} else {
					if (GUILayout.Button("Clear Points", GUILayout.ExpandWidth(true))) {
							for (int i = 0; i < _waypoints.Length; i++) {
							_pointsProperty.DeleteArrayElementAtIndex(0);
						}
						serializedObject.ApplyModifiedProperties();
						CreateSegments();
						CreateLines();
					}
				}
			}
		}

		
		private void OnSceneGUI() {
			// Recreates segments if undo or redo
			if (Event.current.type == EventType.ValidateCommand && Event.current.commandName.Equals("UndoRedoPerformed")) {
				CreateSegments();
				CreateLines();
			}

			// Position Handles
			for (int i = 0; i < _waypoints.Length; i++) {
				Handles.Label(_waypoints[i] + Vector3.down * 0.0f, i.ToString(), _sceneSkin.textField);
				EditorGUI.BeginChangeCheck();
				Vector3 newPosition = Handles.PositionHandle(_waypoints[i], Quaternion.identity);
				if (EditorGUI.EndChangeCheck()) {
					Undo.RecordObject(_waypoints, "Moved Waypoint");
					_waypoints[i] = newPosition;

					EditorUtility.SetDirty(_waypoints);
					CreateSegments();
					CreateLines();
				}
			}

			DrawConnectLines();
			DrawClosestPointOnLines();
		}


		private void DrawConnectLines() {
			if (_waypoints.Length >= 2) {
				Handles.DrawDottedLines(_waypoints.points, _segmentIndices, _dashSize);
			}
		}

		private void CreateSegments() {
			if (_waypoints.Length < 2)
				return;

			_segmentIndices = new int [_waypoints.Length * 2];
			int index = 0;
			for (int start = 0; start < _segmentIndices.Length - 2; start+= 2) {
				_segmentIndices[start] = index;
				index++;
				_segmentIndices[start + 1] = index;
			}
			if (_enemyMovement.ShouldLoop) {
				_segmentIndices[^2] = _waypoints.Length - 1;
				_segmentIndices[^1] = 0;
			}
		}

		private void CreateLines() {
			_lines = new Vector3[_waypoints.Length + 1];
			System.Array.Copy(_waypoints.points, _lines, _waypoints.Length);
			_lines[^1] = _lines[0];
		}

		private void DrawClosestPointOnLines() {
			Event currentEvent = Event.current;
			if (currentEvent.modifiers == EventModifiers.Control && _waypoints.Length >= 2) {
				Vector3 pointPosition = HandleUtility.ClosestPointToPolyLine(_lines);
				Color previousColor = Handles.color;
				Handles.color = _pointOnLineColor;
				Handles.DrawSolidDisc(pointPosition, Camera.current.transform.forward, HandleUtility.GetHandleSize(pointPosition) / 5);
				Handles.color = previousColor;
				HandleUtility.Repaint();

				if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0) {
					GUIUtility.hotControl = 0;
					int index = GetIndexOfClosestLine(pointPosition);
					_pointsProperty.InsertArrayElementAtIndex(index);
					serializedObject.ApplyModifiedProperties();
					_waypoints[index] = pointPosition;
					CreateSegments();
					CreateLines();
					currentEvent.Use();
				}
			}
		}

		private int GetIndexOfClosestLine(Vector3 pointPosition) {
			float distance = float.MaxValue;
			int index = 0;
			for (int i = 0; i < _lines.Length - 1; i++) {
				float currentDistance = HandleUtility.DistancePointLine(pointPosition, _lines[i], _lines[i + 1]);
				if (currentDistance < distance) {
					distance = currentDistance;
					index = i;
				}
			}
			return index + 1;
		}
	}
}