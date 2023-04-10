/*
using UnityEngine;
using UnityEditor;

namespace Enemy.Editor {
	[CustomEditor(typeof(TargetMovement))]
	public class TargetMovementEditor : UnityEditor.Editor {
		#region Fields
		private TargetMovement _targetMovement;

		private GUISkin _inspectorSkin;
		private GUISkin _sceneSkin;
		#endregion Fields

		private void OnEnable() {
			_inspectorSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
			_sceneSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
			Tools.hidden = true;

			_targetMovement = (TargetMovement)target;
		}

		private void OnDisable() {
			Tools.hidden = false;
		}

		public override void OnInspectorGUI() {
			Event currentEvent = Event.current;

			GUILayout.Space(10f);

			EditorGUI.BeginChangeCheck();

			float value = EditorGUILayout.PropertyField(serializedObject.FindProperty("_speed"));

			GUILayout.Space(10f);

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_startingIndex"));

			EditorGUI.EndChangeCheck();

			GUILayout.Space(10f);


		}
	}
}
*/
