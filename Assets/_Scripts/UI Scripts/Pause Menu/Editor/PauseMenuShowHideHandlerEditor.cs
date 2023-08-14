using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[CustomEditor(typeof(PauseMenuShowHideHandler))]
public class PauseMenuShowHideHandlerEditor : Editor
{
  public override void OnInspectorGUI() {
    PauseMenuShowHideHandler pauseMenuShowHideHandler = (PauseMenuShowHideHandler)target;
    base.OnInspectorGUI();
    if (GUILayout.Button("Reset Fade Values")) {
      pauseMenuShowHideHandler.ResetCanvasGroupValues();
    }
  }
}
