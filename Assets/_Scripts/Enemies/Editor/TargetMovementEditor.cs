using UnityEditor;

// Tells Unity to use this Editor class with the EnemyStats component.
[CustomEditor(typeof(TargetMovement))]
public class TargetMovementEditor : Editor
{
    // The various categories the editor will display the variables in
    public enum DisplayCategory
    {
        Linear_Bounce, Linear_Loop, Circular_Bounce, Circular_Loop
    }
    // The enum field that will determine what variables to display in the Inspector
    public DisplayCategory categoryToDisplay;

    // The function that makes the custom editor work
    public override void OnInspectorGUI()
    {
        // Display the enum popup in the inspector
        categoryToDisplay = (DisplayCategory) EditorGUILayout.EnumPopup("Display", categoryToDisplay);

        // Create a space to separate this enum popup from the other variables 
        EditorGUILayout.Space(); 

        switch (categoryToDisplay) {
            case DisplayCategory.Linear_Bounce:
                DisplayLinearBounceInfo();
                break;
            case DisplayCategory.Linear_Loop:
                DisplayLinearLoopInfo();
                break;
            case DisplayCategory.Circular_Bounce:
                DisplayCircularBounceInfo();
                break;
            case DisplayCategory.Circular_Loop:
                DisplayCircularLoopInfo();
                break;
        }

        // Save all changes made on the Inspector
        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayLinearBounceInfo() {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("health"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defense"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("movementSpeed"));
    }

    private void DisplayLinearLoopInfo() {
        
    }

    private void DisplayCircularBounceInfo() {

    }

    private void DisplayCircularLoopInfo() {
        
    }
}